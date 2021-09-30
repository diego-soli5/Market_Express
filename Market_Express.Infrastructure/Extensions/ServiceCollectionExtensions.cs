using Market_Express.Application.Services;
using Market_Express.CrossCutting.Options;
using Market_Express.Domain.Abstractions.ApplicationServices;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Abstractions.Validations;
using Market_Express.Domain.EntityValidations;
using Market_Express.Domain.Services;
using Market_Express.Infrastructure.Data;
using Market_Express.Infrastructure.Data.Repositories;
using Market_Express.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Market_Express.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static void AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAccountService), typeof(AccountService));

            services.AddScoped(typeof(ICartService), typeof(CartService));
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(ISystemService), typeof(SystemService));
        }

        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAuthenticationService), typeof(AuthenticationService));

            services.AddScoped(typeof(IPasswordService), typeof(PasswordService));
        }

        public static void AddValidations(this IServiceCollection services)
        {
            services.AddTransient(typeof(IAppUserValidations), typeof(AppUserValidations));

            services.AddTransient(typeof(IClientValidations), typeof(ClientValidations));

            services.AddTransient(typeof(IArticleValidations), typeof(ArticleValidations));
        }

        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationOptions>(configuration.GetSection("Options:AuthenticationOptions"));

            services.Configure<PasswordOptions>(configuration.GetSection("Options:PasswordOptions"));
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MARKET_EXPRESSContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Local_Desa"));
            });
        }

        public static void AddAppAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
                {
                    config.Cookie.Name = "App.Auth";
                    config.LoginPath = "/Account/Login";
                    config.AccessDeniedPath = "/Account/Unauthorizedv";
                    config.LogoutPath = "/Account/Logout";
                    config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                });
        }
    }
}
