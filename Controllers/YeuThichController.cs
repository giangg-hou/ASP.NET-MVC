using BTL002.Data;
using BTL002.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BTL002.Controllers
{
    [Authorize] // bắt buộc phải đăng nhập mới xem danh sách yêu thích
    public class YeuThichController : Controller
    {
        private readonly BookStoreDbContext _db;

        public YeuThichController(BookStoreDbContext db)
        {
            _db = db;
        }

        // GET: YeuThich
        public IActionResult Index()
        {
            int userId = GetCurrentUserId();
            var yeuThichs = _db.YeuThichs
                .Include(y => y.Sach)
                    .ThenInclude(s => s.TacGia)
                .Include(y => y.Sach)
                    .ThenInclude(s => s.DanhMuc)
                .Where(y => y.MaNguoiDung == userId)
                .OrderByDescending(y => y.NgayThem)
                .ToList();

            return View(yeuThichs);
        }

        // POST: YeuThich/Them
        [HttpPost]
        public IActionResult Them(int maSach)
        {
            int userId = GetCurrentUserId();

            var yeuThich = _db.YeuThichs
                .FirstOrDefault(y => y.MaNguoiDung == userId && y.MaSach == maSach);

            if (yeuThich == null)
            {
                yeuThich = new YeuThich
                {
                    MaNguoiDung = userId,
                    MaSach = maSach,
                    NgayThem = DateTime.Now.ToString("yyyy-MM-dd")
                };

                _db.YeuThichs.Add(yeuThich);
                _db.SaveChanges();

                TempData["Success"] = "Đã thêm vào danh sách yêu thích";
            }

            return RedirectToAction("Details", "Sach", new { id = maSach });
        }

        // POST: YeuThich/Xoa
        [HttpPost]
        public IActionResult Xoa(int maSach)
        {
            int userId = GetCurrentUserId();

            var yeuThich = _db.YeuThichs
                .FirstOrDefault(y => y.MaNguoiDung == userId && y.MaSach == maSach);

            if (yeuThich != null)
            {
                _db.YeuThichs.Remove(yeuThich);
                _db.SaveChanges();
                TempData["Success"] = "Đã xóa khỏi danh sách yêu thích";
            }

            return RedirectToAction(nameof(Index));
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out int userId))
                return userId;

            return 0;
        }
    }
}
