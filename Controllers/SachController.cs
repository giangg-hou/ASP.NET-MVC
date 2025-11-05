using BTL002.Data;
using BTL002.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BTL002.Controllers
{
    public class SachController : Controller
    {
        private readonly BookStoreDbContext _db;
        private readonly IWebHostEnvironment _env;

        // Inject DbContext và IWebHostEnvironment
        public SachController(BookStoreDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: Sach
        public IActionResult Index(string search, int? danhMuc, int? tacGia, bool? trangThai)
        {
            var sachs = _db.Sachs
                .Include(s => s.TacGia)
                .Include(s => s.NhaXuatBan)
                .Include(s => s.DanhMuc)
                .AsQueryable();

            // Nếu không phải Admin, chỉ hiển thị sách đã duyệt
            if (!User.IsInRole("Admin"))
            {
                sachs = sachs.Where(s => s.TrangThai == true);
            }
            else if (trangThai.HasValue)
            {
                // Admin có thể lọc theo trạng thái
                sachs = sachs.Where(s => s.TrangThai == trangThai.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                sachs = sachs.Where(s => s.TenSach.Contains(search));
            }

            if (danhMuc.HasValue)
            {
                sachs = sachs.Where(s => s.MaDanhMuc == danhMuc.Value);
            }

            if (tacGia.HasValue)
            {
                sachs = sachs.Where(s => s.MaTacGia == tacGia.Value);
            }

            ViewBag.DanhMucs = new SelectList(_db.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewBag.TacGias = new SelectList(_db.TacGias, "MaTacGia", "TenTacGia");

            return View(sachs.ToList());
        }

        // GET: Sach/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var sach = _db.Sachs
                .Include(s => s.TacGia)
                .Include(s => s.NhaXuatBan)
                .Include(s => s.DanhMuc)
                .Include(s => s.DanhGias)
                    .ThenInclude(d => d.NguoiDung)
                .FirstOrDefault(s => s.MaSach == id);

            if (sach == null)
                return NotFound();

            // Nếu sách chưa duyệt và user không phải Admin thì không cho xem
            if (!sach.TrangThai && !User.IsInRole("Admin"))
            {
                return NotFound();
            }

            return View(sach);
        }

        // GET: Sach/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        // POST: Sach/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sach sach, IFormFile uploadImage)
        {
            // Loại bỏ validation cho các navigation properties
            ModelState.Remove("TacGia");
            ModelState.Remove("NhaXuatBan");
            ModelState.Remove("DanhMuc");
            ModelState.Remove("GioHangs");
            ModelState.Remove("ChiTietDonHangs");
            ModelState.Remove("DanhGias");
            ModelState.Remove("YeuThichs");
            ModelState.Remove("NgayTao");
            ModelState.Remove("HinhAnh");

            // Debug: Log giá trị nhận được
            System.Diagnostics.Debug.WriteLine($"TenSach: {sach.TenSach}");
            System.Diagnostics.Debug.WriteLine($"MaTacGia: {sach.MaTacGia}");
            System.Diagnostics.Debug.WriteLine($"MaNhaXuatBan: {sach.MaNhaXuatBan}");
            System.Diagnostics.Debug.WriteLine($"MaDanhMuc: {sach.MaDanhMuc}");
            System.Diagnostics.Debug.WriteLine($"GiaBan: {sach.GiaBan}");
            System.Diagnostics.Debug.WriteLine($"SoLuongTon: {sach.SoLuongTon}");
            System.Diagnostics.Debug.WriteLine($"NamXuatBan: {sach.NamXuatBan}");

            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý upload hình ảnh
                    if (uploadImage != null && uploadImage.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "books");

                        // Tạo thư mục nếu chưa có
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadImage.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await uploadImage.CopyToAsync(fileStream);
                        }

                        sach.HinhAnh = "/images/books/" + uniqueFileName;
                    }
                    else if (string.IsNullOrEmpty(sach.HinhAnh))
                    {
                        // Nếu không có ảnh, dùng ảnh mặc định
                        sach.HinhAnh = "/images/no-image.jpg";
                    }

                    // Set ngày tạo
                    sach.NgayTao = DateTime.Now.ToString("yyyy-MM-dd");

                    // Mặc định trạng thái là chờ duyệt (false)
                    // Admin có thể chọn duyệt ngay khi tạo

                    _db.Sachs.Add(sach);
                    await _db.SaveChangesAsync();

                    TempData["Success"] = "Thêm sách thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError("", "Chi tiết: " + ex.InnerException.Message);
                    }
                }
            }
            else
            {
                // Log các lỗi validation để debug
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine("Validation Error: " + error.ErrorMessage);
                }
            }

            LoadDropdowns(sach);
            return View(sach);
        }

        // GET: Sach/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var sach = _db.Sachs.Find(id);

            if (sach == null)
                return NotFound();

            LoadDropdowns(sach);
            return View(sach);
        }

        // POST: Sach/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Sach sach, IFormFile ImageFile)
        {
            // Loại bỏ validation cho các navigation properties
            ModelState.Remove("TacGia");
            ModelState.Remove("NhaXuatBan");
            ModelState.Remove("DanhMuc");
            ModelState.Remove("GioHangs");
            ModelState.Remove("ChiTietDonHangs");
            ModelState.Remove("DanhGias");
            ModelState.Remove("YeuThichs");
            ModelState.Remove("NgayTao");

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy thông tin sách cũ từ DB
                    var sachCu = await _db.Sachs.AsNoTracking().FirstOrDefaultAsync(s => s.MaSach == sach.MaSach);

                    if (sachCu == null)
                    {
                        return NotFound();
                    }

                    // Xử lý upload hình ảnh mới
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "books");

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }

                        sach.HinhAnh = "/images/books/" + uniqueFileName;

                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(sachCu.HinhAnh) && sachCu.HinhAnh != "/images/no-image.jpg")
                        {
                            var oldImagePath = Path.Combine(_env.WebRootPath, sachCu.HinhAnh.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                    }
                    else
                    {
                        // Giữ nguyên ảnh cũ
                        sach.HinhAnh = sachCu.HinhAnh;
                    }

                    // Giữ nguyên ngày tạo
                    sach.NgayTao = sachCu.NgayTao;

                    _db.Entry(sach).State = EntityState.Modified;
                    await _db.SaveChangesAsync();

                    TempData["Success"] = "Cập nhật sách thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SachExists(sach.MaSach))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                }
            }

            LoadDropdowns(sach);
            return View(sach);
        }

        // GET: Sach/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var sach = _db.Sachs
                .Include(s => s.TacGia)
                .Include(s => s.NhaXuatBan)
                .Include(s => s.DanhMuc)
                .FirstOrDefault(s => s.MaSach == id);

            if (sach == null)
                return NotFound();

            return View(sach);
        }

        // POST: Sach/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sach = await _db.Sachs.FindAsync(id);

            if (sach != null)
            {
                // Xóa ảnh nếu có
                if (!string.IsNullOrEmpty(sach.HinhAnh) && sach.HinhAnh != "/images/no-image.jpg")
                {
                    var imagePath = Path.Combine(_env.WebRootPath, sach.HinhAnh.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _db.Sachs.Remove(sach);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Xóa sách thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Sach/DuyetSach/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DuyetSach(int id)
        {
            var sach = await _db.Sachs.FindAsync(id);

            if (sach == null)
            {
                return NotFound();
            }

            sach.TrangThai = true;
            await _db.SaveChangesAsync();

            TempData["Success"] = "Đã duyệt sách: " + sach.TenSach;
            return RedirectToAction(nameof(Index));
        }

        // POST: Sach/HuyDuyetSach/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyDuyetSach(int id)
        {
            var sach = await _db.Sachs.FindAsync(id);

            if (sach == null)
            {
                return NotFound();
            }

            sach.TrangThai = false;
            await _db.SaveChangesAsync();

            TempData["Success"] = "Đã hủy duyệt sách: " + sach.TenSach;
            return RedirectToAction(nameof(Index));
        }

        // Helper: Load dropdown lists
        private void LoadDropdowns(Sach sach = null)
        {
            ViewBag.MaDanhMuc = new SelectList(_db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sach?.MaDanhMuc);
            ViewBag.MaTacGia = new SelectList(_db.TacGias, "MaTacGia", "TenTacGia", sach?.MaTacGia);
            ViewBag.MaNhaXuatBan = new SelectList(_db.NhaXuatBans, "MaNhaXuatBan", "TenNhaXuatBan", sach?.MaNhaXuatBan);

            // Thêm các ViewBag với tên khác cho Edit view
            ViewBag.DanhMucList = ViewBag.MaDanhMuc;
            ViewBag.TacGiaList = ViewBag.MaTacGia;
            ViewBag.NhaXuatBanList = ViewBag.MaNhaXuatBan;
        }

        // Helper: Check if Sach exists
        private bool SachExists(int id)
        {
            return _db.Sachs.Any(e => e.MaSach == id);
        }
    }
}