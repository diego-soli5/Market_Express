using AutoMapper;
using Market_Express.Application.DTOs.Cart;
using Market_Express.Domain.CustomEntities.Cart;

namespace Market_Express.Infrastructure.Mappings
{
    public class CartMappings : Profile
    {
        public CartMappings()
        {
            CreateMap<CartBillingDetails, CartBillingDetailsDTO>()
                .ReverseMap();
        }
    }
}
