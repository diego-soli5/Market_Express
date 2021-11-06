﻿using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.Home;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IArticleRepository : IGenericRepository<Article>
    {
        IEnumerable<Article> GetAllActiveWithCategoryAsigned();
        Task<SQLServerPagedList<Article>> GetAllForSearch(HomeSearchQueryFilter filters);
        Task<List<Article>> GetMostPopular(int? take = null);
    }
}
