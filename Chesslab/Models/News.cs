using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient.DataClassification;

namespace Chesslab.Models
{
    public class News
    {
        public int Id { get; set; }
        public string PostName { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
