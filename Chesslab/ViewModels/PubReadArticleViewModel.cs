using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Chesslab.Models;

namespace Chesslab.ViewModels
{
    public class PubArticleViewModel
    {
        [Required(ErrorMessage = "Введите название статьи")]
        [MaxLength(50, ErrorMessage = "Максимальное количество символов для названия статьи 50 символов")]
        public string PostName { get; set; }
        [Required(ErrorMessage = "Статья должна содержать текст")]
        public string Content { get; set; }
        public Article Article {get; set; }
    }
}
