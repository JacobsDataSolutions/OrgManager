// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain;
using JDS.OrgManager.Infrastructure.Identity;
using JDS.OrgManager.Persistence.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.WebApi
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                // Perform additional initialization of the domain layer.
                serviceProvider.WireUpDomainEventHandlers();
                serviceProvider.WireUpDateTimeService();

                try
                {
                    // Identity tables must be created first because app user tables will have foreign keys to them.
                    var identityContext = serviceProvider.GetRequiredService<AppIdentityDbContext>();
                    identityContext.Database.Migrate();

                    var applicationDbContext = serviceProvider.GetRequiredService<ApplicationWriteDbContext>();
                    applicationDbContext.Database.Migrate();

                    var dbUpdater = serviceProvider.GetRequiredService<DatabaseUpdater>();
                    await dbUpdater.UpdateDatabaseAsync(true);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or initializing the database.");
                }
            }
            host.Run();
        }
    }
}