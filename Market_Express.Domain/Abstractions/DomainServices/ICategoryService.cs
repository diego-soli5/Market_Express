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
        Task<BusisnessResult> Create(Category category, IFormFile image, Guid userId);
        Task<BusisnessResult> ChangeStatus(Guid categoryId, Guid userId);
    }
}
