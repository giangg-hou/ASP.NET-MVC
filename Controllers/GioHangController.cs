using BTL002.Data;
using BTL002.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BTL002.Controllers
{
    public class GioHangController : Controller
    {
        private readonly BookStoreDbContext _db;

        // Constructor với Dependency Injection
        public GioHangController(BookStoreDbContext db)
        {
            _db = db;
        }

        // GET: GioHang
        public IActionResult Index()
        {
            int userId = GetCurrentUserId();
            var gioHangs = _db.GioHangs
                .Include(g => g.Sach)
                .Where(g => g.MaNguoiDung == userId)
                .ToList();

            decimal tongTien = gioHangs.Sum(g => g.Sach.GiaBan * g.SoLuong);
            ViewBag.TongTien = tongTien;

            return View(gioHangs);
        }

        // POST: GioHang/ThemVaoGio
        [HttpPost]
        public IActionResult ThemVaoGio(int maSach, int soLuong = 1)
        {
            int userId = GetCurrentUserId();

            var gioHang = _db.GioHangs
                .FirstOrDefault(g => g.MaNguoiDung == userId && g.MaSach == maSach);

            if (gioHang != null)
            {
                // Cập nhật số lượng nếu đã có trong giỏ
                gioHang.SoLuong += soLuong;
            }
            else
            {
                // Thêm mới vào giỏ hàng
                gioHang = new GioHang
                {
                    MaNguoiDung = userId,
                    MaSach = maSach,
                    SoLuong = soLuong,
                    NgayThem = DateTime.Now.ToString("yyyy-MM-dd")
                };
                _db.GioHangs.Add(gioHang);
            }

            _db.SaveChanges();
            TempData["Success"] = "Đã thêm sách vào giỏ hàng";

            return RedirectToAction(nameof(Index));
        }

        // POST: GioHang/CapNhatSoLuong
        [HttpPost]
        public IActionResult CapNhatSoLuong(int maGioHang, int soLuong)
        {
            var gioHang = _db.GioHangs.Find(maGioHang);

            if (gioHang != null && gioHang.MaNguoiDung == GetCurrentUserId())
            {
                if (soLuong > 0)
                {
                    gioHang.SoLuong = soLuong;
                    _db.SaveChanges();
                    TempData["Success"] = "Đã cập nhật số lượng";
                }
                else
                {
                    // Nếu số lượng <= 0 thì xóa khỏi giỏ
                    _db.GioHangs.Remove(gioHang);
                    _db.SaveChanges();
                    TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: GioHang/Xoa
        [HttpPost]
        public IActionResult Xoa(int maGioHang)
        {
            var gioHang = _db.GioHangs.Find(maGioHang);

            if (gioHang != null && gioHang.MaNguoiDung == GetCurrentUserId())
            {
                _db.GioHangs.Remove(gioHang);
                _db.SaveChanges();
                TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: GioHang/XoaTatCa
        [HttpPost]
        public IActionResult XoaTatCa()
        {
            int userId = GetCurrentUserId();
            var gioHangs = _db.GioHangs
                .Where(g => g.MaNguoiDung == userId)
                .ToList();

            if (gioHangs.Any())
            {
                _db.GioHangs.RemoveRange(gioHangs);
                _db.SaveChanges();
                TempData["Success"] = "Đã xóa toàn bộ giỏ hàng";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: GioHang/GetCartCount (AJAX)
        [HttpGet]
        public IActionResult GetCartCount()
        {
            int userId = GetCurrentUserId();
            int count = _db.GioHangs
                .Where(g => g.MaNguoiDung == userId)
                .Sum(g => g.SoLuong);

            return Json(new { count });
        }

        private int GetCurrentUserId()
        {
            // Lấy UserId từ Claims (ASP.NET Core Identity)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return 0; // Hoặc throw exception nếu không tìm thấy
        }
    }
}
