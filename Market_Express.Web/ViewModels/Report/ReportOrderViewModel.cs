using Market_Express.Application.DTOs.Order;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.Report;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Report
{
    public class ReportOrderViewModel
    {
        public List<OrderDTO> Orders { get; set; }
        public ReportOrderQueryFilter Filters { get; set; }
        public Metadata Metadata { get; set; }
    }
}
