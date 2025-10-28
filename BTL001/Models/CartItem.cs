using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL001.Models
{
    // CartItem có thể lưu tạm trong DB hoặc session. Nếu lưu DB, gắn UserId; nếu dùng session, giữ lớp ở model để bind.
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public int Quantity { get; set; } = 1;

        [Column(TypeName = "numeric(18,2)")]
        public decimal UnitPrice { get; set; } // lưu giá tại thời điểm thêm vào giỏ

        // Nếu lưu theo user:
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        // Nếu dùng session, bạn sẽ serialize CartItem mà không cần DB
    }
}
