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
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JDS.OrgManager.Utils
{
    public class DummyDataInserter
    {
        private const string TestUserName = "TEST-EMPLOYEE@ORGMANAGER.COM";

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
            var connection = context.Connection;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                var sqlTransaction = transaction.GetDbTransaction();

                var createTestUserSql = @$"IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE UserName = '{TestUserName}')
INSERT [dbo].[AspNetUsers] ([UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsCustomer]) VALUES (N'{TestUserName}', N'{TestUserName}', N'{TestUserName}', N'{TestUserName}', 1, N'AQAAAAEAACcQAAAAEEEeWPvxgc0pa7boxO1GvxzQKedhDNkI0aVCwaws/52ehWp8Wple22rf+zcXp3hhQA==', N'2QEPCZBRJ6NF6JKJ446RBKVZXH7SXZ6X', N'f6d7885b-0ef3-4db3-a913-72871353dd65', NULL, 0, 0, NULL, 1, 0, 1)
";
                await facade.ExecuteAsync(createTestUserSql, null, sqlTransaction);

                var aspNetUsersId = await facade.QueryFirstOrDefaultAsync<int>(@$"SELECT TOP 1 Id FROM AspNetUsers WITH(NOLOCK) WHERE NormalizedUserName = '{TestUserName}'", transaction: sqlTransaction);
                var tenantIds = await facade.QueryAsync<int>(@"SELECT Id FROM Tenants WITH(NOLOCK) WHERE Id > 1", transaction: sqlTransaction);
                await facade.SetIdentitySeedAsync(nameof(IApplicationWriteDbContext.Employees), ApplicationLayerConstants.SystemSeedStartValue, sqlTransaction);
                var ptoPolicies = await context.PaidTimeOffPolicies.ToArrayAsync();
                foreach (var tenantId in tenantIds)
                {
                    var employees = (from n in Enumerable.Range(1, random.Next(5) + 5) select DummyData.GenerateRandomEmployeeEntity(tenantId, aspNetUsersId, ptoPolicies)).ToList();
                    await context.Employees.AddRangeAsync(employees);
                    await context.SaveChangesAsync();

                    await context.TenantAspNetUsers.AddAsync(new TenantAspNetUserEntity { AspNetUsersId = aspNetUsersId, TenantId = tenantId });
                    await context.SaveChangesAsync();

                    // Create org hierarchy.
                    var groups = (
                        from emp in employees
                        group emp by emp.EmployeeLevel into grouped
                        orderby grouped.Key descending
                        select grouped).ToArray();
                    for (var i = 0; i < groups.Length - 1; i++)
                    {
                        foreach (var man in groups[i].Zip(Enumerable.Range(1, 999)))
                        {
                            foreach (var sub in groups[i + 1].Zip(Enumerable.Range(1, 999)))
                            {
                                if (sub.Second % man.Second == 0)
                                {
                                    context.EmployeeManagers.Add(new EmployeeManagerEntity { EmployeeId = sub.First.Id, ManagerId = man.First.Id, TenantId = tenantId });
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
}