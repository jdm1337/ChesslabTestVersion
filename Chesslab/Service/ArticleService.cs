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
            
            List<int> defineChosenPeriodValue = await articleViewModel.ArticleSearchViewModel.DefineChosenPeriodValue();
            var allArticles =   _appContext.articles
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

        public async Task<bool> SaveArticle(PubArticleViewModel viewModel)
        {
            Article article = await ArticleBuilder(viewModel);
            bool downloaded = await _localStorageService.UploadArticle(article, viewModel);
            return downloaded;


        }

        public async Task<Article> ArticleBuilder(PubArticleViewModel viewModel)
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

        public async Task<ReadArticleViewModel> ReadArticleViewModelBuilder(int articleId)
        {
            var currentArticle = await _appContext.articles.FindAsync(articleId);
            //searching first 7 articles which have the same category with readable article
            var recommendedArticles = await _appContext.articles
                .Where(article => article.Categories == currentArticle.Categories)
                .Where(article=> article.Id!= articleId)
                .Take(7)
                .ToListAsync();

            return new ReadArticleViewModel()
            {
                Article = currentArticle,
                RecommendedArticles = recommendedArticles,
                Content = await _localStorageService.DownloadArticle(currentArticle)
            };
        }

        public async Task<List<Article>> GetRecentArticles()
        {
            var countOfRecentArticles = 3;
            var allArticles = await _appContext.articles.ToListAsync();
            //take a few last articles from all list
            var lastenArticle = allArticles.Skip(allArticles.Count - countOfRecentArticles).ToList();
            return lastenArticle;

        }
    }
    
}
