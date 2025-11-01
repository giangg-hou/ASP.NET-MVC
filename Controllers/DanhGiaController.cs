using BTL002.Data;
using BTL002.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BTL002.Controllers
{
    public class DanhGiaController : Controller
    {
        private readonly BookStoreDbContext _db;

        // Constructor với Dependency Injection
        public DanhGiaController(BookStoreDbContext db)
        {
            _db = db;
        }

        // POST: DanhGia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int maSach, int diemDanhGia, string noiDung)
        {
            int userId = GetCurrentUserId();

            // Kiểm tra đã mua sách chưa
            bool daMuaSach = _db.ChiTietDonHangs
                .Any(c => c.MaSach == maSach &&
                     c.DonHang.MaNguoiDung == userId &&
                     c.DonHang.TrangThai == TrangThaiDonHang.DaGiao);

            if (!daMuaSach)
            {
                TempData["Error"] = "Bạn cần mua sách trước khi đánh giá";
                return RedirectToAction("Details", "Sach", new { id = maSach });
            }

            // Kiểm tra đã đánh giá chưa
            var danhGiaCu = _db.DanhGias.FirstOrDefault(d => d.MaSach == maSach && d.MaNguoiDung == userId);

            if (danhGiaCu != null)
            {
                // Cập nhật đánh giá cũ
                danhGiaCu.DiemDanhGia = diemDanhGia;
                danhGiaCu.NoiDung = noiDung;
                danhGiaCu.NgayDanhGia = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                // Tạo đánh giá mới
                var danhGia = new DanhGia
                {
                    MaSach = maSach,
                    MaNguoiDung = userId,
                    DiemDanhGia = diemDanhGia,
                    NoiDung = noiDung,
                    NgayDanhGia = DateTime.Now.ToString("yyyy-MM-dd")
                };
                _db.DanhGias.Add(danhGia);
            }

            _db.SaveChanges();
            return RedirectToAction("Details", "Sach", new { id = maSach });
        }

        // POST: DanhGia/Delete
        [HttpPost]
        public IActionResult Delete(int maDanhGia)
        {
            int userId = GetCurrentUserId();
            var danhGia = _db.DanhGias.FirstOrDefault(d => d.MaDanhGia == maDanhGia && d.MaNguoiDung == userId);

            if (danhGia != null)
            {
                int maSach = danhGia.MaSach;
                _db.DanhGias.Remove(danhGia);
                _db.SaveChanges();
                return RedirectToAction("Details", "Sach", new { id = maSach });
            }

            return RedirectToAction("Index", "Home");
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
