using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chesslab.Models;
using Chesslab.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chesslab.Service
{
    public class ArticleService
    {
        private readonly ApplicationContext _appContext;
        private readonly LocalStorageService _localStorageService;
        private readonly UserManager<User> _userManager;

        private static int PageSize = 5;
        public ArticleService(ApplicationContext appContext, LocalStorageService localStorageService, UserManager<User> userManager)
        {
            _appContext = appContext;
            _localStorageService = localStorageService;
            _userManager = userManager;
        }

        public async Task<ArticleViewModel> GetStdArticles(int page)
        {
            var allArticles = _appContext.articles.Where(article => article.Id > 0);
             return await ArticleViewModelBuilder(allArticles, page);
        }

        public async Task<ArticleViewModel> GetByParamsArticles(ArticleViewModel articleViewModel, int page)
        {
            Console.WriteLine(articleViewModel.ArticleSearchViewModel.ChosenCategory);
            Console.WriteLine(articleViewModel.ArticleSearchViewModel.ChosenPeriod);
            List<int> defineChosenPeriodValue = await articleViewModel.ArticleSearchViewModel.DefineChosenPeriodValue();
            var allArticles =  _appContext.articles
                .Where(article =>
                    (defineChosenPeriodValue[0] <= article.PublishDate.Year) &&
                    (article.PublishDate.Year <= defineChosenPeriodValue[1]))
                .Where(article => article.Categories == articleViewModel.ArticleSearchViewModel.ChosenCategory);
            return await ArticleViewModelBuilder(allArticles, page);
        }


        public async Task<ArticleViewModel> ArticleViewModelBuilder(IQueryable<Article> allArticles, int page)
        {
            var countArticles = await allArticles.CountAsync();
            var currentArticles = await allArticles.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(countArticles, page, PageSize);
            
            
            return new
                ArticleViewModel()
            {
                PageViewModel = pageViewModel,
                articles = currentArticles
            };

        }

        public async Task<bool> SaveArticle(PubReadArticleViewModel viewModel)
        {
            Article article = await ArticleBuilder(viewModel);
            bool downloaded = await _localStorageService.UploadArticle(article, viewModel);
            return downloaded;


        }

        public async Task<Article> ArticleBuilder(PubReadArticleViewModel viewModel)
        {
            Article article = new Article()
            {
                Postname = viewModel.PostName,
                AuthorId = viewModel.Article.AuthorId,
                PublishDate = DateTime.Now,
                Categories = viewModel.Article.Categories,
                AuthorName = viewModel.Article.AuthorName
            };
            return article;
        }

    }
    
}
