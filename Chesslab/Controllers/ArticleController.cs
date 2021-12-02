﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chesslab.Models;
using Chesslab.Service;
using Chesslab.ViewModels;

namespace Chesslab.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ArticleService _articleService;
        public ArticleController(ArticleService articleService, ApplicationContext appContext)
        {
            _articleService = articleService;
        }

        public async Task<IActionResult> Index(int page=1)
        {
            ArticleViewModel stdArticleViewModel = await _articleService.GetStdArticles(page);
            return View(stdArticleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SearchArticle()
        {
            return null;
            
        }

        
    }
}