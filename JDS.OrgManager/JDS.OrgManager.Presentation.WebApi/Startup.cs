// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using IdentityServer4.Services;
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
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

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

            // JDS TODO: add in custom middleware

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core, see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // JDS TODO: add in logging/serilog.

            services.AddScoped<DummyDataInserter>();

            // Add caching.
            services.AddSingleton<IDistributedCache, MemoryDistributedCache>();

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

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, AppIdentityDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            // JDS - add in custom profile service.
            services.AddTransient<IProfileService, CustomProfileService>();

            services.AddControllersWithViews();
            services.AddRazorPages();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }
    }
}