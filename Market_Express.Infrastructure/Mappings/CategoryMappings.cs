using AutoMapper;
using Market_Express.Application.DTOs.Category;
using Market_Express.Domain.CustomEntities.Category;
using Market_Express.Domain.Entities;

namespace Market_Express.Infrastructure.Mappings
{
    public class CategoryMappings : Profile
    {
        public CategoryMappings()
        {
            CreateMap<Category, CategoryDTO>()
                .ReverseMap();

            CreateMap<Category, CategoryCreateDTO>()
                .ReverseMap();

            CreateMap<Category, CategoryUpdateDTO>()
                .ReverseMap();

            CreateMap<CategoryForSearch, CategorySearchDTO>()
               .ReverseMap();
        }
    }
}
