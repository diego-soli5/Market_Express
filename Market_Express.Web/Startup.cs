using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Market_Express.Infrastructure.Extensions;
using Market_Express.Infrastructure.Mappings;

namespace Market_Express.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddRepositories();

            services.AddApplicationServices();

            services.AddInfrastructureServices();

            services.AddDomainServices();

            services.AddValidations();

            services.AddOptions(Configuration);

            services.AddAzureClients(Configuration);

            services.AddDbContext(Configuration);

            services.AddAntiforgery(setup => setup.HeaderName = "X-Anti-Forgery-Token");

            services.AddAppAuthentication();

            services.AddAutoMapper(typeof(AppMappings));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //app.UseExceptionHandler("/Error/Handle");
            }
            else
            {
                app.UseExceptionHandler("/Error/Handle");
               
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
