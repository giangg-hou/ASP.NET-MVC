using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using YourNamespace.Data;
using YourNamespace.Models;

namespace YourNamespace.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalBooks = _context.Books.Count();
            ViewBag.TotalOrders = _context.Orders.Count();
            ViewBag.Revenue = _context.Orders.Sum(o => o.TotalAmount);
            return View();
        }

        public IActionResult Books()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
            return Ok();
        }

        public IActionResult Users()
        {
            return View(_context.Users.ToList());
        }

        public IActionResult Orders()
        {
            var orders = _context.Orders.Include(o => o.User).ToList();
            return View(orders);
        }
    }
}
