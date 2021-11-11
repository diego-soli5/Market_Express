using Market_Express.Application.DTOs.Order;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.Order;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Order
{
    public class MyOrdersViewModel
    {
        public OrderStatsDTO OrderStats { get; set; }
        public List<RecentOrderDTO> RecentOrders { get; set; }
        public List<OrderDTO> Orders { get; set; }
        public MyOrdersQueryFilter Filters { get; set; }
        public Metadata Metadata { get; set; }
    }
}
