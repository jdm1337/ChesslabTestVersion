using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chesslab.ViewModels
{
    public class ProfileViewModel
    {
        public string NickName { get; set; }
        public int TaskRating { get; set; }

        public string Location { get; set;}
        public decimal Level { get; set; }

        public string About { get; set; }

        public string Status { get; set; }
    }
}
