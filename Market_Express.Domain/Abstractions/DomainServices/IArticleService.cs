using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.Article;
using Market_Express.Domain.QueryFilter.Home;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IArticleService
    {
        PagedList<Article> GetAll(ArticleIndexQueryFilter filters, bool includeCategory = false);
        IEnumerable<Article> GetAllForSearch(HomeSearchQueryFilter filters);
        IEnumerable<Article> GetAllActive(int? max = null);
        Task<Article> GetById(Guid id, bool includeCategory = false);
        Task<BusisnessResult> Create(Article article, IFormFile image, Guid currentUserId);
        Task<BusisnessResult> Edit(Article article, IFormFile image, Guid currentUserId);
        Task<BusisnessResult> ChangeStatus(Guid articleId, bool enableCategory, Guid currentUserId);
        Task<BusisnessResult> SetCategory(Guid articleId, Guid categoryId, Guid currentUserId);
    }
}
