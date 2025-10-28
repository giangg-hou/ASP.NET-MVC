using BTL001.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "numeric(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "numeric(18,2)")]
        public decimal LineTotal { get; set; } // = UnitPrice * Quantity
    }
}
