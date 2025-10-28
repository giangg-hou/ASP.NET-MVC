using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YourNamespace.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Slug { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
