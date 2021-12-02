using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chesslab.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Postname { get; set; }

        public string? AuthorId { get; set; }
        public User user { get; set; }

        public DateTime PublishDate { get; set; }

        public string LinkStorage { get; set; }
        public string Categories { get; set; }
    }
}
