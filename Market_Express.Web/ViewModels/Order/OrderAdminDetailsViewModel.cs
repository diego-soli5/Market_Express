using Market_Express.Application.DTOs.Order;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Order
{
    public class OrderAdminDetailsViewModel
    {
        public OrderDTO Order { get; set; }
        public List<OrderArticleDetailDTO> Details { get; set; }
    }
}
