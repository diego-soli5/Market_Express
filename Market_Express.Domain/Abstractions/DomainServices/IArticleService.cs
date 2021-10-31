using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.Article;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IArticleService
    {
        PagedList<Article> GetAll(ArticleIndexQueryFilter filters);
    }
}
