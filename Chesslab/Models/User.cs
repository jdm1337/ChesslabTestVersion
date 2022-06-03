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
        
        public int TaskRating { get; set; }
        public DateTime RegisterDate { get; set; }
        [DefaultValue(1.0)]
        public Decimal Level { get; set; }
        
        public string Status { get; set; }
        public string Location { get; set; }
        public string About { get; set; }
        // also images
        public string Avatar { get; set;}
    }
}
