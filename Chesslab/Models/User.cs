using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Chesslab.Models
{
    public class User: IdentityUser
    {
        public string NickName { get; set; }
        [DefaultValue(1000)]
        public int TaskRating { get; set; }
        public DateTime RegisterDate { get; set; }
        [DefaultValue(1)]
        public Decimal Level { get; set; }
    }
}
