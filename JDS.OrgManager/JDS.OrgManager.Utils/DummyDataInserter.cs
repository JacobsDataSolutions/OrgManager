// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application;
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.System;
using JDS.OrgManager.Application.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Utils
{
    public class DummyDataInserter
    {
        private static readonly Random random = new Random();

        private readonly IApplicationWriteDbContext context;

        private readonly IApplicationWriteDbFacade facade;

        public DummyDataInserter(IApplicationWriteDbContext context, IApplicationWriteDbFacade facade)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
        }

        public async Task InsertDummyDataAsync()
        {
            var c = 0;

            var connection = context.Connection;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            using var transaction = await context.Database.BeginTransactionAsync();
            var sqlTransaction = transaction.GetDbTransaction();

            async Task<int> createTestUser(int userNum)
            {
                var sql = @$"IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE UserName = 'TEST-EMPLOYEE{userNum}@ORG-MANAGER.COM')
INSERT [dbo].[AspNetUsers] ([UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsCustomer]) VALUES (N'TEST-EMPLOYEE{userNum}@ORG-MANAGER.COM', N'TEST-EMPLOYEE{userNum}@ORG-MANAGER.COM', N'TEST-EMPLOYEE{userNum}@ORG-MANAGER.COM', N'TEST-EMPLOYEE{userNum}@ORG-MANAGER.COM', 1, N'AQAAAAEAACcQAAAAEEEeWPvxgc0pa7boxO1GvxzQKedhDNkI0aVCwaws/52ehWp8Wple22rf+zcXp3hhQA==', N'2QEPCZBRJ6NF6JKJ446RBKVZXH7SXZ6X', N'f6d7885b-0ef3-4db3-a913-72871353dd65', NULL, 0, 0, NULL, 1, 0, 0)
SELECT TOP 1 ISNULL(SCOPE_IDENTITY(), Id) FROM AspNetUsers WITH(NOLOCK) WHERE UserName = 'TEST-EMPLOYEE{userNum}@ORG-MANAGER.COM'";
                return await facade.QueryFirstOrDefaultAsync<int>(sql, null, sqlTransaction);
            }

            var tenantIds = await facade.QueryAsync<int>(@"SELECT Id FROM Tenants WITH(NOLOCK) WHERE Id > 1", transaction: sqlTransaction);
            await facade.SetIdentitySeedAsync(nameof(IApplicationWriteDbContext.Employees), ApplicationLayerConstants.SystemSeedStartValue, sqlTransaction);
            var ptoPolicies = await context.PaidTimeOffPolicies.ToArrayAsync();
            foreach (var tenantId in tenantIds)
            {
                var employeeCount = random.Next(5) + 5;
                List<int> userIds = new();
                for (var i = 0; i < employeeCount; i++)
                {
                    ++c;
                    userIds.Add(await createTestUser(c));
                }
                var employees = (from aspNetUsersId in userIds select DummyData.GenerateRandomEmployeeEntity(tenantId, aspNetUsersId, ptoPolicies)).ToList();
                await context.Employees.AddRangeAsync(employees);
                await context.SaveChangesAsync();

                await context.TenantAspNetUsers.AddRangeAsync(from aspNetUsersId in userIds select new TenantAspNetUserEntity { AspNetUsersId = aspNetUsersId, TenantId = tenantId });
                await context.SaveChangesAsync();

                // Create org hierarchy.
                var groups = (
                    from emp in employees
                    group emp by emp.EmployeeLevel into grouped
                    orderby grouped.Key descending
                    select grouped).ToArray();
                for (var i = 0; i < groups.Length - 1; i++)
                {
                    foreach (var (man, manId) in groups[i].Zip(Enumerable.Range(1, 999)))
                    {
                        foreach (var (sub, subId) in groups[i + 1].Zip(Enumerable.Range(1, 999)))
                        {
                            if (subId % manId == 0)
                            {
                                context.EmployeeManagers.Add(new EmployeeManagerEntity { EmployeeId = sub.Id, ManagerId = man.Id, TenantId = tenantId });
                            }
                        }
                    }
                }
                await context.SaveChangesAsync();
            }
            await facade.SetIdentitySeedAsync(nameof(IApplicationWriteDbContext.Employees), ApplicationLayerConstants.ProductionSeedStartValue, sqlTransaction);
            await transaction.CommitAsync();
        }
    }
}