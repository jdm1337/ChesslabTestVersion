using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chesslab.Models;

namespace Chesslab.ViewModels
{
    public class ArticleViewModel
    {
        public List<Article> articles { get; set;}
        public PageViewModel PageViewModel { get; set;}

        public ArticleSearchViewModel ArticleSearchViewModel { get; set; }

    }
}
