using Market_Express.Domain.CustomEntities.Category;
using Market_Express.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();
        Task<List<CategoryForSearch>> GetAllAvailableForSearch();
        IEnumerable<Category> GetAllAvailable();
        Task<Category> GetById(Guid categoryId);
        Task<(int, int)> GetArticleDetails(Guid categoryId);
        Task<BusisnessResult> Create(Category category, IFormFile image, Guid userId);
        Task<BusisnessResult> Edit(Category category, IFormFile image, Guid userId);
        Task<BusisnessResult> ChangeStatus(Guid categoryId, Guid userId);
    }
}
