// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Common.Currencies;
using JDS.OrgManager.Application.Customers;
using JDS.OrgManager.Application.Tenants;
using JDS.OrgManager.Domain.Common.Finance;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.System.Commands.SeedInitialData
{
    public class InitialDataSeeder
    {
        private const string SystemUserName = "TEST-CUSTOMER@ORGMANAGER.COM";

        private readonly IApplicationWriteDbContext context;

        private readonly IApplicationWriteDbFacade facade;

        private readonly ILogger logger;

        // Once again, if only one or two mappers needed, then there's no reason to inject in the universal mapper.
        private readonly IViewModelToDbEntityMapper<TenantViewModel, TenantEntity> tenantViewModelToDbEntityMapper;

        public InitialDataSeeder(IApplicationWriteDbContext context, IApplicationWriteDbFacade facade, ILogger logger, IViewModelToDbEntityMapper<TenantViewModel, TenantEntity> tenantViewModelToDbEntityMapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.tenantViewModelToDbEntityMapper = tenantViewModelToDbEntityMapper ?? throw new ArgumentNullException(nameof(tenantViewModelToDbEntityMapper));
        }

        public async Task SeedAllAsync(IEnumerable<TenantViewModel>? tenants = null)
        {
            tenants ??= Enumerable.Empty<TenantViewModel>();

            if ((await facade.QueryFirstOrDefaultAsync<int?>("SELECT TOP 1 Id FROM Customers WITH(NOLOCK)")) != null)
            {
                logger.LogInformation("Database has already been initialized with basic data. Nothing to do.");
                return;
            }

            using var transaction = await context.Database.BeginTransactionAsync();
            var sqlTransaction = transaction.GetDbTransaction();

            await facade.SetIdentitySeedForAllTablesAsync(ApplicationLayerConstants.SystemSeedStartValue, sqlTransaction);
            await SeedCurrenciesAsync();
            await CreateTestCompanyUserAsync(sqlTransaction);
            var customer = await SeedDefaultCustomerAsync(sqlTransaction);
            if (customer != null)
            {
                await CreateDefaultTenantsAsync(sqlTransaction, customer, tenants);
            }
            await facade.SetIdentitySeedForAllTablesAsync(ApplicationLayerConstants.ProductionSeedStartValue, sqlTransaction);
            await transaction.CommitAsync();
        }

        private async Task CreateDefaultTenantsAsync(DbTransaction sqlTransaction, CustomerEntity customer, IEnumerable<TenantViewModel> tenants)
        {
            await facade.TurnOffIdentityIncrementAsync(nameof(IApplicationWriteDbContext.Tenants), sqlTransaction);
            foreach (var tenant in tenants.Select(t => tenantViewModelToDbEntityMapper.Map(t)))
            {
                tenant.CustomerId = customer.Id;
                await context.Tenants.AddAsync(tenant);
                await context.TenantAspNetUsers.AddAsync(new TenantAspNetUserEntity { AspNetUsersId = customer.AspNetUsersId, TenantId = tenant.Id });
            }
            await context.SaveChangesAsync();
            await facade.TurnOnIdentityIncrementAsync(nameof(IApplicationWriteDbContext.Tenants), sqlTransaction);
        }

        private async Task CreateTestCompanyUserAsync(DbTransaction sqlTransaction)
        {
            var sql = @$"IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE UserName = '{SystemUserName}')
INSERT [dbo].[AspNetUsers] ([UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsCustomer]) VALUES (N'{SystemUserName}', N'{SystemUserName}', N'{SystemUserName}', N'{SystemUserName}', 1, N'AQAAAAEAACcQAAAAEEEeWPvxgc0pa7boxO1GvxzQKedhDNkI0aVCwaws/52ehWp8Wple22rf+zcXp3hhQA==', N'2QEPCZBRJ6NF6JKJ446RBKVZXH7SXZ6X', N'f6d7885b-0ef3-4db3-a913-72871353dd65', NULL, 0, 0, NULL, 1, 0, 1)
";
            await facade.ExecuteAsync(sql, null, sqlTransaction);
        }

        private async Task SeedCurrenciesAsync()
        {
            await context.Currencies.AddRangeAsync(from c in Currency.GetAll() select new CurrencyEntity { Code = c.Code });
            await context.SaveChangesAsync();
        }

        private async Task<CustomerEntity?> SeedDefaultCustomerAsync(DbTransaction sqlTransaction)
        {
            var corpUserId = await facade.QueryFirstOrDefaultAsync<int?>($"SELECT TOP 1 Id FROM AspNetUsers WHERE NormalizedEmail = '{SystemUserName}'", transaction: sqlTransaction);
            if (corpUserId == null)
            {
                return null;
            }

            var customer = new CustomerEntity
            {
                AspNetUsersId = (int)corpUserId,
                CompanyName = "TEST COMPANY",
                FirstName = "TEST",
                LastName = "TEST",
                Address1 = "12345 TEST ST",
                City = "CHICAGO",
                State = "IL",
                ZipCode = "60606",
                CurrencyCode = "USD"
            };
            await context.Customers.AddAsync(customer);

            await context.SaveChangesAsync();

            return customer;
        }
    }
}