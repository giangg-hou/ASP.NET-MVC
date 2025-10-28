using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL001.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; }

        [MaxLength(250)]
        public string Bio { get; set; }

        [MaxLength(250)]
        public string ProfileImageUrl { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
