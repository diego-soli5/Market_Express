using AutoMapper;
using Market_Express.Application.DTOs.Article;
using Market_Express.Application.DTOs.System;
using Market_Express.Domain.CustomEntities.Article;
using Market_Express.Domain.Entities;

namespace Market_Express.Infrastructure.Mappings
{
    public class ArticleMappings : Profile
    {
        public ArticleMappings()
        {
            CreateMap<Article, ArticuloSyncDTO>()
                .ReverseMap();

            CreateMap<ArticleForReport, ArticleForReportDTO>()
                .ForPath(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForPath(dest => dest.BarCode, opt => opt.MapFrom(src => src.BarCode))
                .ForPath(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForPath(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForPath(dest => dest.SoldUnitsCount, opt => opt.MapFrom(src => src.SoldUnitsCount))
                .ForPath(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ReverseMap();

            CreateMap<Article, ArticleDTO>()
                .ForPath(dest => dest.Category.Id, opt => opt.MapFrom(src => src.Category.Id))
                .ForPath(dest => dest.Category.Name, opt => opt.MapFrom(src => src.Category.Name))
                .ForPath(dest => dest.Category.Description, opt => opt.MapFrom(src => src.Category.Description))
                .ForPath(dest => dest.Category.Status, opt => opt.MapFrom(src => src.Category.Status))
                .ReverseMap();

            CreateMap<Article, ArticleEditDTO>()
                .ForPath(dest => dest.Category.Id, opt => opt.MapFrom(src => src.Category.Id))
                .ForPath(dest => dest.Category.Name, opt => opt.MapFrom(src => src.Category.Name))
                .ForPath(dest => dest.Category.Description, opt => opt.MapFrom(src => src.Category.Description))
                .ForPath(dest => dest.Category.Status, opt => opt.MapFrom(src => src.Category.Status))
                .ReverseMap();

            CreateMap<Article, ArticleCreateDTO>()
                .ReverseMap();

            CreateMap<ArticleToAddInCart, ArticleToAddInCartDTO>()
               .ReverseMap();

            CreateMap<ArticleForCartDetails, ArticleForCartDetailsDTO>()
                .ForPath(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ReverseMap();
        }
    }
}
