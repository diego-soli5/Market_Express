using Market_Express.Domain.CustomEntities.Article;
using Market_Express.Domain.CustomEntities.Client;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.Report;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IReportService
    {
        IQueryable<Order> GetOrdersForReport(ReportOrderQueryFilter filters);
        PagedList<Order> GetOrdersPaginated(ReportOrderQueryFilter filters);
        Task<SQLServerPagedList<ArticleForReport>> GetMostSoldArticlesPaginated(ReportArticleQueryFilter filters);
        Task<List<ArticleForReport>> GetMostSoldArticles(ReportArticleQueryFilter filters);
        Task<SQLServerPagedList<ClientForReport>> GetClientsStatsPaginated(ReportClientQueryFilter filters);
        Task<List<ClientForReport>> GetClientsStats(ReportClientQueryFilter filters);
    }
}
