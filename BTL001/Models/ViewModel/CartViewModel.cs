using BTL001.Models;
using System.Collections.Generic;

namespace YourNamespace.Models.ViewModel
{
    public class CartViewModel
    {
        public IEnumerable<CartItem> Items { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Total => SubTotal + ShippingFee;
    }
}
