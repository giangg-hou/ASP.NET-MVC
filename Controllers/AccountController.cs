using BTL002.Data;
using BTL002.Models;
using BTL002.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BTL002.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookStoreDbContext _db;

        public AccountController(BookStoreDbContext db)
        {
            _db = db;
        }

        // GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_db.NguoiDungs.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng");
                return View(model);
            }

            if (model.Role == VaiTro.Admin)
            {
                ModelState.AddModelError("Role", "Không thể đăng ký tài khoản Admin.");
                return View(model);
            }

            // Seller thì chuyển sang trạng thái Pending
            var status = model.Role switch
            {
                VaiTro.KhachHang => TrangThaiTaiKhoan.KichHoat,  // buyer -> active immediately
                VaiTro.NguoiBan => TrangThaiTaiKhoan.DangCho,    // seller -> pending
                _ => TrangThaiTaiKhoan.KichHoat
            };

            var user = new NguoiDung
            {
                HoTen = model.HoTen,
                Email = model.Email,
                MatKhau = HashPassword(model.MatKhau),
                SoDienThoai = model.SoDienThoai,
                DiaChi = model.DiaChi,
                VaiTro = model.Role,
                TrangThai = status,
                VerifyKey = GenerateVerifyKey(),
                NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd")
            };

            _db.NguoiDungs.Add(user);
            await _db.SaveChangesAsync();

            if (model.Role == VaiTro.NguoiBan)
            {
                TempData["Success"] = "Đăng ký thành công! Vui lòng chờ admin duyệt tài khoản người bán.";
            }
            else
            {
                TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            }
            return RedirectToAction(nameof(Login));
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var user = _db.NguoiDungs.FirstOrDefault(u => u.Email == model.Email);

            if (user == null || !VerifyPassword(model.MatKhau, user.MatKhau))
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng");
                return View(model);
            }

            // Kiểm tra trạng thái tài khoản như Identity
            if (user.TrangThai != TrangThaiTaiKhoan.KichHoat)
            {
                ModelState.AddModelError("", "Tài khoản của bạn chưa được duyệt hoặc đã bị từ chối.");
                return View(model);
            }

            // Tạo Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.MaNguoiDung.ToString()),
                new Claim(ClaimTypes.Name, user.HoTen),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.VaiTro.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                authProperties);

            TempData["Success"] = $"Chào mừng {user.HoTen}!";

            // Redirect theo Role giống Identity
            switch (user.VaiTro)
            {
                case VaiTro.Admin:
                    return RedirectToAction("Index", "Admin");

                case VaiTro.NguoiBan:
                    return RedirectToAction("Index", "Home");

                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        // POST: Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Success"] = "Đã đăng xuất thành công";
            return RedirectToAction("Index", "Home");
        }

        // GET: Profile
        [Authorize]
        public IActionResult Profile()
        {
            var id = GetUserId();
            var user = _db.NguoiDungs.Find(id);

            if (user == null)
                return NotFound();

            var model = new ProfileViewModel
            {
                HoTen = user.HoTen,
                Email = user.Email,
                SoDienThoai = user.SoDienThoai,
                DiaChi = user.DiaChi
            };

            return View(model);
        }

        // POST: Profile
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var id = GetUserId();
            var user = _db.NguoiDungs.Find(id);

            if (user == null)
                return NotFound();

            user.HoTen = model.HoTen;
            user.SoDienThoai = model.SoDienThoai;
            user.DiaChi = model.DiaChi;

            await _db.SaveChangesAsync();

            TempData["Success"] = "Cập nhật thông tin thành công";
            return RedirectToAction(nameof(Profile));
        }

        // GET: ChangePassword
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: ChangePassword
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var id = GetUserId();
            var user = _db.NguoiDungs.Find(id);

            if (user == null)
                return NotFound();

            if (!VerifyPassword(model.MatKhauCu, user.MatKhau))
            {
                ModelState.AddModelError("MatKhauCu", "Mật khẩu cũ không đúng");
                return View(model);
            }

            user.MatKhau = HashPassword(model.MatKhauMoi);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Đổi mật khẩu thành công";
            return RedirectToAction(nameof(Profile));
        }

        public IActionResult AccessDenied() => View();

        // Helpers
        private int GetUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(id, out var uid) ? uid : 0;
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string input, string hash)
        {
            return HashPassword(input) == hash;
        }

        /// Tạo mã VerifyKey ngẫu nhiên gồm 10 ký tự (chữ hoa + số)
        private string GenerateVerifyKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var verifyKey = new char[10];

            for (int i = 0; i < 10; i++)
            {
                verifyKey[i] = chars[random.Next(chars.Length)];
            }

            return new string(verifyKey);
        }
    }
}
