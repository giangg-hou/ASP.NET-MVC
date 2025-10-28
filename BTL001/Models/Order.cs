using BTL001.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL001.Models
{
    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5,
        Refunded = 6
    }

    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [MaxLength(250)]
        public string ShippingAddress { get; set; }

        [MaxLength(100)]
        public string PaymentMethod { get; set; } // e.g., COD, VNPay,...

        [Column(TypeName = "numeric(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "numeric(18,2)")]
        public decimal ShippingFee { get; set; }

        [Column(TypeName = "numeric(18,2)")]
        public decimal TotalAmount { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
