using BTL002.Data;
using BTL002.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Net;

namespace BTL002.Controllers
{
    public class DonHangController : Controller
    {
        private readonly BookStoreDbContext _db;

        // Constructor với Dependency Injection
        public DonHangController(BookStoreDbContext db)
        {
            _db = db;
        }

        // GET: DonHang
        public IActionResult Index()
        {
            int userId = GetCurrentUserId();
            var donHangs = _db.DonHangs
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(c => c.Sach)
                .Where(d => d.MaNguoiDung == userId)
                .OrderByDescending(d => d.NgayDatHang)
                .ToList();

            return View(donHangs);
        }

        // GET: DonHang/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            int userId = GetCurrentUserId();
            var donHang = _db.DonHangs
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(c => c.Sach)
                .Include(d => d.NguoiDung)
                .FirstOrDefault(d => d.MaDonHang == id && d.MaNguoiDung == userId);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // GET: DonHang/ThanhToan
        public IActionResult ThanhToan()
        {
            int userId = GetCurrentUserId();
            var gioHangs = _db.GioHangs
                .Include(g => g.Sach)
                .Where(g => g.MaNguoiDung == userId)
                .ToList();

            if (!gioHangs.Any())
            {
                return RedirectToAction("Index", "GioHang");
            }

            decimal tongTien = gioHangs.Sum(g => g.Sach.GiaBan * g.SoLuong);
            ViewBag.TongTien = tongTien;
            ViewBag.GioHangs = gioHangs;

            return View(new DonHang());
        }

        // POST: DonHang/ThanhToan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThanhToan(DonHang donHang)
        {
            int userId = GetCurrentUserId();
            var gioHangs = _db.GioHangs
                .Include(g => g.Sach)
                .Where(g => g.MaNguoiDung == userId)
                .ToList();

            if (!gioHangs.Any())
            {
                return RedirectToAction("Index", "GioHang");
            }

            // Tạo đơn hàng
            donHang.MaNguoiDung = userId;
            donHang.TongTien = gioHangs.Sum(g => g.Sach.GiaBan * g.SoLuong);
            donHang.TrangThai = TrangThaiDonHang.ChoXacNhan;
            donHang.NgayDatHang = DateTime.Now.ToString("yyyy-MM-dd");

            _db.DonHangs.Add(donHang);
            _db.SaveChanges();

            // Tạo chi tiết đơn hàng
            foreach (var item in gioHangs)
            {
                var chiTiet = new ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaSach = item.MaSach,
                    SoLuong = item.SoLuong,
                    GiaBan = item.Sach.GiaBan,
                    ThanhTien = item.Sach.GiaBan * item.SoLuong
                };

                _db.ChiTietDonHangs.Add(chiTiet);

                // Cập nhật tồn kho
                var sach = _db.Sachs.Find(item.MaSach);
                if (sach != null)
                {
                    sach.SoLuongTon -= item.SoLuong;
                }
            }

            // Xóa giỏ hàng
            _db.GioHangs.RemoveRange(gioHangs);
            _db.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = donHang.MaDonHang });
        }

        // POST: DonHang/HuyDonHang
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HuyDonHang(int maDonHang)
        {
            int userId = GetCurrentUserId();
            var donHang = _db.DonHangs.FirstOrDefault(d => d.MaDonHang == maDonHang && d.MaNguoiDung == userId);

            if (donHang != null && donHang.TrangThai == TrangThaiDonHang.ChoXacNhan)
            {
                donHang.TrangThai = TrangThaiDonHang.DaHuy;

                // Hoàn lại tồn kho
                var chiTiets = _db.ChiTietDonHangs.Where(c => c.MaDonHang == maDonHang).ToList();
                foreach (var chiTiet in chiTiets)
                {
                    var sach = _db.Sachs.Find(chiTiet.MaSach);
                    if (sach != null)
                    {
                        sach.SoLuongTon += chiTiet.SoLuong;
                    }
                }

                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Details), new { id = maDonHang });
        }

        // GET: DonHang/QuanLy (Dành cho Admin)
        [Authorize(Roles = "Admin")]
        public IActionResult QuanLy(TrangThaiDonHang? trangThai)
        {
            var donHangs = _db.DonHangs
                .Include(d => d.NguoiDung)
                .AsQueryable();

            if (trangThai.HasValue)
            {
                donHangs = donHangs.Where(d => d.TrangThai == trangThai.Value);
            }

            return View(donHangs.OrderByDescending(d => d.NgayDatHang).ToList());
        }

        // POST: DonHang/CapNhatTrangThai (Dành cho Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult CapNhatTrangThai(int maDonHang, TrangThaiDonHang trangThai)
        {
            var donHang = _db.DonHangs.Find(maDonHang);
            if (donHang != null)
            {
                donHang.TrangThai = trangThai;
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(QuanLy));
        }

        private int GetCurrentUserId()
        {
            // Lấy UserId từ Claims (ASP.NET Core Identity)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return 0;
        }
    }
}
