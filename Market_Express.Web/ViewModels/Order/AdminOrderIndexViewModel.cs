using Market_Express.Application.DTOs.Order;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.Order;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Order
{
    public class AdminOrderIndexViewModel
    {
        public List<OrderDTO> Orders { get; set; }
        public List<RecentOrderDTO> RecentOrders { get; set; }
        public Metadata Metadata { get; set; }
        public AdminOrderQueryFilter Filters { get; set; }
        public OrderStatsDTO OrderStats { get; set; }
    }
}
