using Market_Express.Domain.CustomEntities.Article;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IReportRepository
    {
        Task<SQLServerPagedList<ArticleForReport>> GetMostSoldArticlesPaginated(ReportArticleQueryFilter filters);
        Task<List<ArticleForReport>> GetMostSoldArticles(ReportArticleQueryFilter filters);
    }
}
