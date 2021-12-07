using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chesslab.Models;

namespace Chesslab.ViewModels
{
    public class ReadArticleViewModel
    {
        public Article Article { get; set; }
        public List<Article> RecommendedArticles { get; set; }

        public string Content { get; set; }
    }
}
