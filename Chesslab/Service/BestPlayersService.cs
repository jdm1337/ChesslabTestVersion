using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Chesslab.Service
{

    public class BestPlayersService
    {
        private readonly static string SiteUrl = "https://shahmaty.info/top-fide/";
        public BestPlayersService()
        {
        }

        public async Task<string> CallUrl()
        {
            HttpClient httpClient = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            httpClient.DefaultRequestHeaders.Accept.Clear();
            var response = httpClient.GetStringAsync(SiteUrl);
            return await response;
        }

    }
}
