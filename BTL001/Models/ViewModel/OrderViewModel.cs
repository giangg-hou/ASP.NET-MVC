using System.Collections.Generic;

namespace YourNamespace.Models.ViewModel
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
