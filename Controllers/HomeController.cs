using BTL002.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL002.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookStoreDbContext _db;

        // Inject DbContext qua constructor
        public HomeController(BookStoreDbContext db)
        {
            _db = db;
        }

        // GET: /
        public IActionResult Index()
        {
            var sachMoi = _db.Sachs
                .Include(s => s.TacGia)
                .Include(s => s.NhaXuatBan)
                .Include(s => s.DanhMuc)
                .OrderByDescending(s => s.NgayTao)
                .Take(8)
                .ToList();

            return View(sachMoi);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
