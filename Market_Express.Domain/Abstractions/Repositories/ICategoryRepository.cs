﻿using Market_Express.Domain.CustomEntities.Category;
using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<(int, int)> GetArticleDetails(Guid categoryId);
        IQueryable<Category> GetAllActive();
        Task<List<CategoryForSearch>> GetAllAvailableForSearch();
        Task<List<Category>> GetMostPopular(int? take = null);
    }
}
