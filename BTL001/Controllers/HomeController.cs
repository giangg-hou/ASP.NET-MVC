using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using YourNamespace.Data;

namespace YourNamespace.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var latestBooks = _context.Books
                .OrderByDescending(b => b.BookId)
                .Take(8)
                .ToList();

            ViewBag.LatestBooks = latestBooks;
            return View();
        }
    }
}
