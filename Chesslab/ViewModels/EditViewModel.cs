using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Chesslab.Models;

namespace Chesslab.ViewModels
{
    public class EditViewModel
    {
        [RegularExpression(@"[a-zA-Z]\w{5,14}", ErrorMessage = "Неккоректное имя пользователя")]
        [Display(Name = "Имя пользователя")]
        public string NickName { get; set; }
        
        public string About { get; set;}

        [MaxLength(50, ErrorMessage ="Неккоректное местожительство")]
        public string Location { get; set;}

        public IFormFile UploadedAvatar { get; set;}

        public User UserView { get; set; }
    }
}
