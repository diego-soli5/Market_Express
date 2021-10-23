﻿using Market_Express.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<(int, int)> GetArticleDetails(Guid categoryId);
    }
}