using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.Article;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IArticleService
    {
        PagedList<Article> GetAll(ArticleIndexQueryFilter filters, bool includeCategory = false);
        Task<Article> GetById(Guid id);
        Task<BusisnessResult> Create(Article article, IFormFile image, Guid currentUserId);
        Task<BusisnessResult> Edit(Article article, IFormFile image, Guid currentUserId);
        Task<BusisnessResult> ChangeStatus(Guid articleId, Guid currentUserId);
    }
}
