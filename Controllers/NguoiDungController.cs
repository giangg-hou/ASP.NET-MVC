using BTL002.Data;
using BTL002.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BTL002.Controllers
{
    public class NguoiDungController : Controller
    {
        private readonly BookStoreDbContext _db;

        public NguoiDungController(BookStoreDbContext db)
        {
            _db = db;
        }

        // GET: NguoiDung
        public async Task<IActionResult> Index()
        {
            var nguoiDungs = await _db.NguoiDungs
                .Include(n => n.DonHangs)
                .Include(n => n.DanhGias)
                .Include(n => n.YeuThichs)
                .OrderByDescending(n => n.MaNguoiDung)
                .ToListAsync();

            return View(nguoiDungs);
        }

        // GET: NguoiDung/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var nguoiDung = await _db.NguoiDungs
                .Include(n => n.DonHangs)
                .Include(n => n.DanhGias)
                .Include(n => n.YeuThichs)
                .Include(n => n.GioHangs)
                .FirstOrDefaultAsync(m => m.MaNguoiDung == id);

            if (nguoiDung == null)
                return NotFound();

            return View(nguoiDung);
        }

        // GET: NguoiDung/Create
        public IActionResult Create()
        {
            ViewData["VaiTroList"] = new SelectList(Enum.GetValues(typeof(VaiTro)));
            ViewData["TrangThaiList"] = new SelectList(Enum.GetValues(typeof(TrangThaiTaiKhoan)));

            return View();
        }

        // POST: NguoiDung/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NguoiDungCreateVM model)
        {
            // Check email trùng
            bool emailTonTai = await _db.NguoiDungs.AnyAsync(x => x.Email == model.Email);

            if (emailTonTai)
                ModelState.AddModelError("Email", "Email này đã được sử dụng");

            if (!ModelState.IsValid)
            {
                ViewData["VaiTroList"] = new SelectList(Enum.GetValues(typeof(VaiTro)), model.VaiTro);
                ViewData["TrangThaiList"] = new SelectList(Enum.GetValues(typeof(TrangThaiTaiKhoan)), model.TrangThai);
                return View(model);
            }

            // Map ViewModel → Entity
            var user = new NguoiDung
            {
                HoTen = model.HoTen,
                Email = model.Email,
                MatKhau = HashPassword(model.MatKhau),
                SoDienThoai = model.SoDienThoai,
                DiaChi = model.DiaChi,
                VaiTro = model.VaiTro,
                TrangThai = model.TrangThai,

                // Tự generate giá trị không nhập
                NgayDangKy = DateTime.Now.ToString("dd/MM/yyyy"),
                VerifyKey = GenerateVerifyKey(),
                IsVerified = false
            };

            _db.Add(user);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thêm người dùng thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: NguoiDung/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _db.NguoiDungs.FindAsync(id);
            if (user == null) return NotFound();

            var vm = new NguoiDungEditVM
            {
                MaNguoiDung = user.MaNguoiDung,
                HoTen = user.HoTen,
                Email = user.Email,
                SoDienThoai = user.SoDienThoai,
                DiaChi = user.DiaChi,
                VaiTro = user.VaiTro,
                TrangThai = user.TrangThai
            };

            ViewData["VaiTroList"] = new SelectList(Enum.GetValues(typeof(VaiTro)), vm.VaiTro);
            ViewData["TrangThaiList"] = new SelectList(Enum.GetValues(typeof(TrangThaiTaiKhoan)), vm.TrangThai);

            return View(vm);
        }


        // POST: NguoiDung/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NguoiDungEditVM model)
        {
            if (id != model.MaNguoiDung)
                return NotFound();

            var user = await _db.NguoiDungs.FindAsync(id);
            if (user == null)
                return NotFound();

            // Check trùng email
            bool emailTrung = await _db.NguoiDungs
                .AnyAsync(n => n.Email == model.Email && n.MaNguoiDung != id);

            if (emailTrung)
                ModelState.AddModelError("Email", "Email này đã được sử dụng");

            if (!ModelState.IsValid)
            {
                ViewData["VaiTroList"] = new SelectList(Enum.GetValues(typeof(VaiTro)), model.VaiTro);
                ViewData["TrangThaiList"] = new SelectList(Enum.GetValues(typeof(TrangThaiTaiKhoan)), model.TrangThai);
                return View(model);
            }

            // Update dữ liệu
            user.HoTen = model.HoTen;
            user.Email = model.Email;
            user.SoDienThoai = model.SoDienThoai;
            user.DiaChi = model.DiaChi;
            user.VaiTro = model.VaiTro;
            user.TrangThai = model.TrangThai;

            // KHÔNG update mat_khau
            // KHÔNG update VerifyKey
            // KHÔNG update IsVerified
            // KHÔNG update NgayDangKy
            // KHÔNG update navigation properties

            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật người dùng thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: NguoiDung/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var nguoiDung = await _db.NguoiDungs
                .Include(n => n.DonHangs)
                .Include(n => n.DanhGias)
                .Include(n => n.YeuThichs)
                .FirstOrDefaultAsync(m => m.MaNguoiDung == id);

            if (nguoiDung == null)
                return NotFound();

            return View(nguoiDung);
        }

        // POST: NguoiDung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nguoiDung = await _db.NguoiDungs
                .Include(n => n.DonHangs)
                .Include(n => n.DanhGias)
                .Include(n => n.YeuThichs)
                .Include(n => n.GioHangs)
                .FirstOrDefaultAsync(n => n.MaNguoiDung == id);

            if (nguoiDung == null)
                return NotFound();

            // Kiểm tra ràng buộc trước khi xóa
            if (nguoiDung.DonHangs?.Any() == true)
            {
                TempData["ErrorMessage"] = "Không thể xóa người dùng này vì đã có đơn hàng!";
                return RedirectToAction(nameof(Delete), new { id });
            }

            // Xóa các dữ liệu liên quan trước
            if (nguoiDung.GioHangs?.Any() == true)
                _db.GioHangs.RemoveRange(nguoiDung.GioHangs);

            if (nguoiDung.DanhGias?.Any() == true)
                _db.DanhGias.RemoveRange(nguoiDung.DanhGias);

            if (nguoiDung.YeuThichs?.Any() == true)
                _db.YeuThichs.RemoveRange(nguoiDung.YeuThichs);

            _db.NguoiDungs.Remove(nguoiDung);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa người dùng thành công!";
            return RedirectToAction(nameof(Index));
        }

        // POST: NguoiDung/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var nguoiDung = await _db.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
                return NotFound();

            nguoiDung.TrangThai = nguoiDung.TrangThai == TrangThaiTaiKhoan.KichHoat
                ? TrangThaiTaiKhoan.TuChoi
                : TrangThaiTaiKhoan.KichHoat;

            _db.Update(nguoiDung);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã {(nguoiDung.TrangThai == TrangThaiTaiKhoan.KichHoat ? "mở khóa" : "khóa")} tài khoản!";
            return RedirectToAction(nameof(Index));
        }

        // POST: NguoiDung/VerifyUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyUser(int id)
        {
            var nguoiDung = await _db.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
                return NotFound();

            nguoiDung.IsVerified = true;
            _db.Update(nguoiDung);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xác thực tài khoản thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: NguoiDung/ChangePassword/5
        public async Task<IActionResult> ChangePassword(int? id)
        {
            if (id == null)
                return NotFound();

            var nguoiDung = await _db.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
                return NotFound();

            return View(nguoiDung);
        }

        // POST: NguoiDung/ChangePassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(int id, string newPassword, string confirmPassword)
        {
            var nguoiDung = await _db.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                ModelState.AddModelError("", "Mật khẩu mới không được để trống");
                return View(nguoiDung);
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("", "Mật khẩu xác nhận không khớp");
                return View(nguoiDung);
            }

            if (newPassword.Length < 6)
            {
                ModelState.AddModelError("", "Mật khẩu phải có ít nhất 6 ký tự");
                return View(nguoiDung);
            }

            nguoiDung.MatKhau = HashPassword(newPassword);
            _db.Update(nguoiDung);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction(nameof(Index));
        }


        // GET: NguoiDung/Search
        public IActionResult Search()
        {
            ViewData["VaiTroList"] = new SelectList(Enum.GetValues(typeof(VaiTro)));
            return View();
        }

        // POST: NguoiDung/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string p1, string p2)
        {
            ViewData["VaiTroList"] = new SelectList(Enum.GetValues(typeof(VaiTro)));
            ViewData["P1"] = p1;
            ViewData["P2"] = p2;

            // Validate p2
            if (string.IsNullOrWhiteSpace(p2))
            {
                ModelState.AddModelError("p2", "Tham số p2 không được để trống");
                return View();
            }

            if (p2.Length < 3)
            {
                ModelState.AddModelError("p2", "Tham số p2 phải có ít nhất 3 ký tự");
                return View();
            }

            if (p2.Any(char.IsDigit))
            {
                ModelState.AddModelError("p2", "Tham số p2 không được chứa chữ số");
                return View();
            }

            // Tìm kiếm người dùng
            var query = _db.NguoiDungs
                .Include(n => n.DonHangs)
                .Include(n => n.DanhGias)
                .Include(n => n.YeuThichs)
                .AsQueryable();

            // p1: Tìm theo email
            if (!string.IsNullOrWhiteSpace(p1))
            {
                query = query.Where(n => n.Email.Contains(p1));
            }

            // p2: Tìm theo họ tên
            query = query.Where(n => n.HoTen.Contains(p2));

            var results = await query
                .OrderByDescending(n => n.MaNguoiDung)
                .ToListAsync();

            ViewData["SearchPerformed"] = true;
            ViewData["ResultCount"] = results.Count;

            return View(results);
        }



        private bool NguoiDungExists(int id)
        {
            return _db.NguoiDungs.Any(e => e.MaNguoiDung == id);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private string GenerateVerifyKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
