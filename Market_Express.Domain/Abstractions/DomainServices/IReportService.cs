using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.Report;
using System.Collections.Generic;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IReportService
    {
        IEnumerable<Order> GetOrdersForReport(ReportOrderQueryFilter filters);
        PagedList<Order> GetOrdersPaginated(ReportOrderQueryFilter filters);
    }
}
