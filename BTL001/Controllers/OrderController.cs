using BTL001.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using YourNamespace.Data;
using YourNamespace.Models;

namespace YourNamespace.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Checkout()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var cart = _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.UserId == userId)
                .ToList();

            var vm = new ViewModel.CartViewModel { Items = cart };
            return View(vm);
        }

        [HttpPost]
        public IActionResult PlaceOrder(string ShippingAddress, string PaymentMethod)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var cart = _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.UserId == userId)
                .ToList();

            if (!cart.Any()) return RedirectToAction("Index", "Cart");

            var order = new Order
            {
                UserId = userId.Value,
                OrderDate = DateTime.Now,
                Status = "Đang xử lý",
                TotalAmount = cart.Sum(i => i.UnitPrice * i.Quantity)
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cart)
            {
                _context.OrderDetails.Add(new OrderDetail
                {
                    OrderId = order.OrderId,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            _context.CartItems.RemoveRange(cart);
            _context.SaveChanges();

            return RedirectToAction("History");
        }

        public IActionResult History()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Book)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }
    }
}
