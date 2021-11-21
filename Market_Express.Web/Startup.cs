using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Market_Express.Infrastructure.Extensions;
using Market_Express.Infrastructure.Mappings;
using Market_Express.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.InteropServices;

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
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(CheckPasswordFilter));
            });

            services.Configure<MvcViewOptions>(options =>
            {
                options.HtmlHelperOptions.CheckBoxHiddenInputRenderMode = CheckBoxHiddenInputRenderMode.None;
            });

            services.AddRepositories();

            services.AddApplicationServices();

            services.AddInfrastructureServices();

            services.AddDomainServices();

            services.AddValidations();

            services.ConfigureOptions(Configuration);

            services.ConfigureAzureClients(Configuration);

            services.ConfigureDbContext(Configuration);

            services.AddAntiforgery(setup => setup.HeaderName = "X-Anti-Forgery-Token");

            services.ConfigureAuthentication();

            services.AddAutoMapper(typeof(AppMappings));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            //Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "../WkHTMLToPDF");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Rotativa.AspNetCore.RotativaConfiguration.Setup(env.ContentRootPath, "../WkHTMLToPDF/Windows");
            else
                Rotativa.AspNetCore.RotativaConfiguration.Setup(env.ContentRootPath, "../WkHTMLToPDF/Linux");
        }
    }
}
