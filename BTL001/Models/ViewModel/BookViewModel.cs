using BTL001.Models;
using System.Collections.Generic;
using YourNamespace.Models;

namespace YourNamespace.Models.ViewModel
{
    public class BookViewModel
    {
        public Book Book { get; set; }
        public IEnumerable<Book> RelatedBooks { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Author> Authors { get; set; }

        // For pagination / search
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    }
}
