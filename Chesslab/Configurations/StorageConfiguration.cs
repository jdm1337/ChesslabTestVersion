using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Chesslab.Configurations
{
    public class StorageConfiguration
    {
        public string DefaultPath { get; set; }
        public string UserInfo { get; set; }
        public string Article { get; set; }
        private IConfiguration _configuration;
        public StorageConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            DefaultPath = _configuration["LocalStorage:DefaultPath"];
            UserInfo = _configuration["LocalStorage:UserInfo"];
            Article = _configuration["LocalStorage:Article"];
        }



    }
}
