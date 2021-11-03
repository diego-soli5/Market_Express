using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class ArticleRepository : GenericRepository<Article>, IArticleRepository
    {
        public ArticleRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }

        public IEnumerable<Article> GetAllActiveWithCategoryAsigned()
        {
            return _dbEntity.Where(article => article.Status == EntityStatus.ACTIVADO && article.CategoryId != null)
                            .AsEnumerable();
        }
    }
}
