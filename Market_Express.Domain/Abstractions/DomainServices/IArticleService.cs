using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.Article;
using System;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IArticleService
    {
        PagedList<Article> GetAll(ArticleIndexQueryFilter filters, bool includeCategory = false);
        Task<Article> GetById(Guid id);
        Task<BusisnessResult> ChangeStatus(Guid articleId, Guid currentUserId);
    }
}
