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

        // Inject DbContext
        public SachController(BookStoreDbContext db)
        {
            _db = db;
        }

        // GET: Sach
        public IActionResult Index(string search, int? danhMuc, int? tacGia)
        {
            var sachs = _db.Sachs
                .Include(s => s.TacGia)
                .Include(s => s.NhaXuatBan)
                .Include(s => s.DanhMuc)
                .AsQueryable();

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
        public IActionResult Create(Sach sach)
        {
            if (ModelState.IsValid)
            {
                sach.NgayTao = DateTime.Now.ToString("yyyy-MM-dd");
                _db.Sachs.Add(sach);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
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
        public IActionResult Edit(Sach sach)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(sach).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
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
        public IActionResult DeleteConfirmed(int id)
        {
            var sach = _db.Sachs.Find(id);

            if (sach != null)
            {
                _db.Sachs.Remove(sach);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper: Load dropdown lists
        private void LoadDropdowns(Sach sach = null)
        {
            ViewBag.MaDanhMuc = new SelectList(_db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sach?.MaDanhMuc);
            ViewBag.MaTacGia = new SelectList(_db.TacGias, "MaTacGia", "TenTacGia", sach?.MaTacGia);
            ViewBag.MaNhaXuatBan = new SelectList(_db.NhaXuatBans, "MaNhaXuatBan", "TenNhaXuatBan", sach?.MaNhaXuatBan);
        }
    }
}
