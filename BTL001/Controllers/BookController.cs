using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using YourNamespace.Data;

namespace YourNamespace.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        public BookController(AppDbContext context)
        {
            _context = context;
        }

        // Danh sách tất cả sách
        public IActionResult Index()
        {
            var books = _context.Books.Include(b => b.Author).Include(b => b.Category).ToList();
            return View(books);
        }

        // Chi tiết sách
        public IActionResult Details(int id)
        {
            var book = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefault(b => b.BookId == id);

            if (book == null) return NotFound();
            return View(book);
        }

        // AJAX Search
        public IActionResult Search(string q)
        {
            if (string.IsNullOrEmpty(q)) return Content("");

            var results = _context.Books
                .Where(b => b.Title.ToLower().Contains(q.ToLower()))
                .Select(b => new
                {
                    b.BookId,
                    b.Title,
                    b.Price,
                    b.ImageUrl
                }).Take(10).ToList();

            string html = "";
            foreach (var b in results)
            {
                html += $"<div class='p-2 border-bottom'><a href='/Book/Details/{b.BookId}'>{b.Title}</a> - {b.Price:N0} đ</div>";
            }

            return Content(html, "text/html");
        }
    }
}
