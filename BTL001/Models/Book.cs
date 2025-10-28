using BTL001.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required, MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string ShortDescription { get; set; }

        public string Description { get; set; }

        [Required]
        [Column(TypeName = "numeric(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "numeric(18,2)")]
        public decimal? DiscountPrice { get; set; }

        public int StockQuantity { get; set; } = 0;

        [MaxLength(200)]
        public string ISBN { get; set; }

        [MaxLength(250)]
        public string ImageUrl { get; set; }

        public DateTime PublishedDate { get; set; }

        // Foreign keys
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }

        // Navigation properties
        public virtual Category Category { get; set; }
        public virtual Author Author { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
