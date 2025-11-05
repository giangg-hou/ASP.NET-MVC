using BTL002.Data;
using BTL002.Models;
using BTL002.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly BookStoreDbContext _db;

    public AdminController(BookStoreDbContext db)
    {
        _db = db;
    }

    // Danh sách yêu cầu Seller đang chờ duyệt
    public async Task<IActionResult> Index()
    {
        var pendingRequests = await _db.NguoiDungs
            .Where(u => u.VaiTro == VaiTro.NguoiBan && u.TrangThai == TrangThaiTaiKhoan.DangCho)
            .Select(u => new SellerRequestViewModel
            {
                UserId = u.MaNguoiDung.ToString(),
                FullName = u.HoTen,
                Email = u.Email,
                Phone = u.SoDienThoai,
                Address = u.DiaChi,
                CreatedAt = u.NgayDangKy,
                Status = u.TrangThai
            })
            .ToListAsync();

        return View(pendingRequests);
    }

    // DUYỆT seller
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveSellerRequest(string userId)
    {
        if (!int.TryParse(userId, out int uid))
            return RedirectToAction("Index");

        var user = await _db.NguoiDungs.FindAsync(uid);

        if (user != null && user.VaiTro == VaiTro.NguoiBan)
        {
            user.TrangThai = TrangThaiTaiKhoan.KichHoat;
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã duyệt tài khoản người bán: " + user.HoTen;
        }

        return RedirectToAction("Index");
    }

    // TỪ CHỐI seller
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectSellerRequest(string userId)
    {
        if (!int.TryParse(userId, out int uid))
            return RedirectToAction("Index");

        var user = await _db.NguoiDungs.FindAsync(uid);

        if (user != null && user.VaiTro == VaiTro.NguoiBan)
        {
            user.TrangThai = TrangThaiTaiKhoan.TuChoi;
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã từ chối tài khoản người bán: " + user.HoTen;
        }

        return RedirectToAction("Index");
    }

    // Quản lý toàn bộ user
    public async Task<IActionResult> ManageUsers()
    {
        var users = await _db.NguoiDungs
            .Where(u => u.VaiTro != VaiTro.Admin)
            .ToListAsync();

        return View(users);
    }

    // Nâng cấp KH -> Seller (gửi yêu cầu Pending)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpgradeToSeller(string userId)
    {
        if (!int.TryParse(userId, out int uid))
            return RedirectToAction("ManageUsers");

        var user = await _db.NguoiDungs.FindAsync(uid);

        if (user != null && user.VaiTro == VaiTro.KhachHang)
        {
            user.VaiTro = VaiTro.NguoiBan;
            user.TrangThai = TrangThaiTaiKhoan.DangCho;
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Đã gửi yêu cầu nâng cấp tài khoản " + user.HoTen + " lên người bán.";
        }

        return RedirectToAction("ManageUsers");
    }
}
