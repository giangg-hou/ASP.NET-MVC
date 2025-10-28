using BTL001.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using YourNamespace.Data;
using YourNamespace.Models;

namespace YourNamespace.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // Hiển thị giỏ hàng
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var items = _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.UserId == userId)
                .ToList();

            return View(items);
        }

        // AJAX thêm vào giỏ
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Json(new { count = 0 });

            var book = _context.Books.Find(id);
            if (book == null) return NotFound();

            var item = _context.CartItems.FirstOrDefault(c => c.BookId == id && c.UserId == userId);
            if (item != null)
                item.Quantity++;
            else
                _context.CartItems.Add(new CartItem
                {
                    BookId = id,
                    Quantity = 1,
                    UnitPrice = book.Price,
                    UserId = userId.Value
                });

            _context.SaveChanges();

            int count = _context.CartItems.Count(c => c.UserId == userId);
            return Json(new { count });
        }

        // Cập nhật số lượng
        [HttpPost]
        public IActionResult UpdateQuantity(int id, int qty)
        {
            var item = _context.CartItems.Find(id);
            if (item != null)
            {
                item.Quantity = qty;
                _context.SaveChanges();
            }
            return Ok();
        }

        // Xóa item
        [HttpPost]
        public IActionResult Remove(int id)
        {
            var item = _context.CartItems.Find(id);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }

            int? userId = HttpContext.Session.GetInt32("UserId");
            int count = _context.CartItems.Count(c => c.UserId == userId);
            return Json(new { count });
        }
    }
}
