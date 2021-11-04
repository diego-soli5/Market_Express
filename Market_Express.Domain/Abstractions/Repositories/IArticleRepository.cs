using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.Home;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IArticleRepository : IGenericRepository<Article>
    {
        IEnumerable<Article> GetAllActiveWithCategoryAsigned();
        Task<List<Article>> GetAllForSearch(HomeSearchQueryFilter filters);
    }
}
