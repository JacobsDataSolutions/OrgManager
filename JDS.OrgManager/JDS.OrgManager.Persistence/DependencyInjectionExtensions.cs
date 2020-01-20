// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.DbQueryFacades;
using JDS.OrgManager.Persistence.DbContexts;
using JDS.OrgManager.Persistence.DbQueryFacades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JDS.OrgManager.Persistence
{
    public static class DependencyInjectionExtensions
    {
        #region Public Methods

        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrgManagerDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ApplicationDatabase")));

            services.AddScoped<IOrgManagerDbContext>(provider => provider.GetService<OrgManagerDbContext>());
            services.AddScoped<IOrgManagerDbQueryFacade, DapperDbQueryFacade>();

            return services;
        }

        #endregion
    }
}