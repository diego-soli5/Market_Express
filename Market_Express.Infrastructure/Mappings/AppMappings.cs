using AutoMapper;
using Market_Express.Application.DTOs.Account;
using Market_Express.Application.DTOs.Address;
using Market_Express.Application.DTOs.AppUser;
using Market_Express.Application.DTOs.Article;
using Market_Express.Application.DTOs.BinnacleAccess;
using Market_Express.Application.DTOs.BinnacleMovement;
using Market_Express.Application.DTOs.Cart;
using Market_Express.Application.DTOs.Category;
using Market_Express.Application.DTOs.Client;
using Market_Express.Application.DTOs.Order;
using Market_Express.Application.DTOs.Permission;
using Market_Express.Application.DTOs.Role;
using Market_Express.Application.DTOs.Slider;
using Market_Express.Application.DTOs.System;
using Market_Express.Domain.CustomEntities.Article;
using Market_Express.Domain.CustomEntities.Cart;
using Market_Express.Domain.CustomEntities.Category;
using Market_Express.Domain.CustomEntities.Client;
using Market_Express.Domain.CustomEntities.Order;
using Market_Express.Domain.CustomEntities.Role;
using Market_Express.Domain.Entities;

namespace Market_Express.Infrastructure.Mappings
{
    public class AppMappings : Profile
    {
        public AppMappings()
        {
            //CreateArticleMappings();
            //CreateClientMappings();
            //CreateAppUserMappings();
            //CreateAddressMappings();
            //CreateSliderMappings();
            //CreateCategoryMappings();
            //CreateRoleMappings();
            //CreatePermissionMappings();
            //CreateCartMappings();
            //CreateOrderMappings();
            //CreateBinnacleAccessMappings();
            //CreateBinnacleMovementMappings();
        }

        private void CreateBinnacleMovementMappings()
        {
            CreateMap<BinnacleMovement, BinnacleMovementDTO>()
                .ReverseMap();
        }

        private void CreateBinnacleAccessMappings()
        {
            CreateMap<BinnacleAccess, BinnacleAccessDTO>()
                .ReverseMap();
        }

        private void CreateOrderMappings()
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

        private void CreateCartMappings()
        {
            CreateMap<CartBillingDetails, CartBillingDetailsDTO>()
                .ReverseMap();
        }

        private void CreateArticleMappings()
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

        private void CreateClientMappings()
        {
            CreateMap<Client, ClientDTO>()
                .ReverseMap();

            CreateMap<ClientForReport, ClientForReportDTO>()
                .ForPath(dest => dest.AppUser, opt => opt.MapFrom(src => src.AppUser))
                .ReverseMap();

            CreateMap<Client, ClienteSyncDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AppUser.Name))
                .ForMember(dest => dest.Identification, opt => opt.MapFrom(src => src.AppUser.Identification))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.Phone))
                .ForMember(dest => dest.ClientCode, opt => opt.MapFrom(src => src.ClientCode))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }

        private void CreateAppUserMappings()
        {
            CreateMap<AppUser, LoginRequestDTO>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();

            CreateMap<AppUser, AppUserProfileDTO>()
                .ReverseMap();

            CreateMap<AppUser, AppUserDTO>()
                .ReverseMap();

            CreateMap<AppUser, AppUserEditDTO>()
                .ReverseMap();

            CreateMap<AppUser, AppUserDetailsDTO>()
                .ReverseMap();
        }

        private void CreateAddressMappings()
        {
            CreateMap<Address, AddressDTO>()
                .ReverseMap();
        }

        private void CreateSliderMappings()
        {
            CreateMap<Slider, SliderDTO>()
                .ReverseMap();

            CreateMap<Slider, SliderCreateDTO>()
                .ReverseMap();

            CreateMap<Slider, SliderUpdateDTO>()
                .ReverseMap();
        }

        private void CreateCategoryMappings()
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

        private void CreateRoleMappings()
        {
            CreateMap<Role, RoleDTO>()
                .ReverseMap();

            CreateMap<RoleWithPermissions, RoleDTO>()
                .ReverseMap();
        }

        private void CreatePermissionMappings()
        {
            CreateMap<Permission, PermissionDTO>()
                .ReverseMap();
        }
    }
}
