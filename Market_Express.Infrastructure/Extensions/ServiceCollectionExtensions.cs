using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Infrastructure.Data;
using Market_Express.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Market_Express.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MARKET_EXPRESSContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Local_Desa"));
            });
        }
    }
}
