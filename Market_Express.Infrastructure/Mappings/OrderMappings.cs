using AutoMapper;
using Market_Express.Application.DTOs.Order;
using Market_Express.Domain.CustomEntities.Order;
using Market_Express.Domain.Entities;

namespace Market_Express.Infrastructure.Mappings
{
    public class OrderMappings : Profile
    {
        public OrderMappings()
        {
            CreateMap<OrderStats, OrderStatsDTO>()
                .ReverseMap();

            CreateMap<RecentOrder, RecentOrderDTO>()
                .ReverseMap();

            CreateMap<Order, OrderDTO>()
                .ReverseMap();

            CreateMap<OrderArticleDetail, OrderArticleDetailDTO>()
                .ReverseMap();
        }
    }
}
