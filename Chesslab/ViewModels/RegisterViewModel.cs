using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chesslab.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [RegularExpression(@"[a-zA-Z]\w{5,14}", ErrorMessage = "Неккоректное имя пользователя")]
        [Display (Name = "Имя пользователя")]
        public string NickName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }


    }
}
