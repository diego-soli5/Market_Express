using Market_Express.Domain.CustomEntities.Article;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.Home;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IArticleRepository : IGenericRepository<Article>
    {
        IEnumerable<Article> GetAllActiveWithCategoryAsigned();
        Task<SQLServerPagedList<ArticleToAddInCart>> GetAllForSearch(HomeSearchQueryFilter filters, Guid? userId);
        Task<List<Article>> GetMostPopular(int? take = null);
        Task<List<ArticleForCartDetails>> GetAllForCartDetails(Guid userId);
    }
}
