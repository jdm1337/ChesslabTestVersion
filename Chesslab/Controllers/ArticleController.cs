using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chesslab.Models;
using Chesslab.Service;
using Chesslab.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Chesslab.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ArticleService _articleService;
        private readonly UserManager<User> _userManager;
        public ArticleController(ArticleService articleService, ApplicationContext appContext, UserManager<User> userManager)
        {
            _articleService = articleService;
            _userManager = userManager;

        }

        public async Task<IActionResult> Index(int page=1)
        {
            var stdArticleViewModel = await _articleService.GetStdArticles(page);
            return View(stdArticleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ArticleViewModel articleViewModel, int page=1)
        {
            Console.WriteLine(1);
            Console.WriteLine(articleViewModel);
            var byParamsArticles = await _articleService.GetByParamsArticles(articleViewModel, page);
            return View(byParamsArticles);
        }
        [Authorize]
        public async Task<IActionResult> Write()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Publish(PubReadArticleViewModel viewModel)
        {
            var authorUser = await _userManager.GetUserAsync(User);
            viewModel.Article.AuthorName = authorUser.NickName;
            await _articleService.SaveArticle(viewModel);
            return RedirectToAction("Index", "Article");
        }

        [HttpGet]
        public async Task<IActionResult> View(int articleId)
        {
            return View();
        }



    }
}
