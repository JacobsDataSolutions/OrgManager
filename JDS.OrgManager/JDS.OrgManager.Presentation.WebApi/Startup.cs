using JDS.OrgManager.Application;
using JDS.OrgManager.Domain;
using JDS.OrgManager.Domain.HumanResources.Advanced;
using JDS.OrgManager.Infrastructure;
using JDS.OrgManager.Infrastructure.Identity;
using JDS.OrgManager.Persistence;
using JDS.OrgManager.Utils;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace JDS.OrgManager.Presentation.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // JDS TODO: add in logging/serilog.

            services.AddScoped<DummyDataInserter>();

            // Add caching.
            services.AddSingleton<IDistributedCache, MemoryDistributedCache>();

            // TODO: configure Serilog.

            // Add mediatR and associated types.
            services.AddMediatR((from t in new[] { typeof(DomainLayer), typeof(ApplicationLayer) } select t.Assembly).ToArray());

            services.AddDomainLayer();
            services.AddDomainLayerAdvanced();
            services.AddApplicationLayer(addValidation: true, addRequestLogging: true, useReadThroughCachingForQueries: true);
            services.AddInfrastructureLayer();
            services.AddPersistenceLayer(Configuration);
            services.AddScoped<DatabaseUpdater>();

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString(InfrastructureLayerConstants.TenantMasterDatabaseConnectionStringName)));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, AppIdentityDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddControllersWithViews();
            services.AddRazorPages();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // JDS. Custom exception handling.
            app.UseExceptionHandler(app2 => app2.UseCustomErrors(env));

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
