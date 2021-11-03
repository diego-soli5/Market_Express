using Market_Express.Domain.Entities;
using System.Collections.Generic;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IArticleRepository : IGenericRepository<Article>
    {
        IEnumerable<Article> GetAllActiveWithCategoryAsigned();
    }
}
