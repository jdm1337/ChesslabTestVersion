using Chesslab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Chesslab.Service;
using Chesslab.ViewModels;

namespace Chesslab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BestPlayersService _bestPlayersService;
        private readonly ArticleService _articleService;

        public HomeController(ILogger<HomeController> logger, BestPlayersService bestPlayersService, ArticleService articleService)
        {
            _logger = logger;
            _bestPlayersService = bestPlayersService;
            _articleService = articleService;
        }

        public async Task<IActionResult> Index()
        {
            HomeIndexViewModel homeIndexViewModel = new HomeIndexViewModel()
            {
                RecentArticles = await _articleService.GetRecentArticles()
            };
            return View(homeIndexViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
