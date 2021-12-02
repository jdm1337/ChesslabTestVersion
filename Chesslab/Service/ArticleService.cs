using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chesslab.Models;
using Chesslab.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Chesslab.Service
{
    public class ArticleService
    {
        private readonly ApplicationContext _appContext;

        private static int PageSize = 5;
        public ArticleService(ApplicationContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<ArticleViewModel> GetStdArticles(int page)
        {
            var allArticles = _appContext.articles.Where(article => article.Id > 0);
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
    }
}
