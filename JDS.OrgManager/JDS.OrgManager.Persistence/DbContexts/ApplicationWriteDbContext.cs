// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.Identity;
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Common.Currencies;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Common.PaidTimeOffPolicies;
using JDS.OrgManager.Application.Customers;
using JDS.OrgManager.Application.Tenants;
using JDS.OrgManager.Common.Abstractions.DateTimes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Persistence.DbContexts
{
    public class ApplicationWriteDbContext : DbContext, IApplicationWriteDbContext
    {
        private readonly ICurrentUserService currentUserService;

        private readonly IDateTimeService dateTimeService;

        public IDbConnection Connection => Database.GetDbConnection();

        public DbSet<CurrencyEntity> Currencies { get; set; }

        public DbSet<CustomerEntity> Customers { get; set; }

        public DbSet<EmployeeManagerEntity> EmployeeManagers { get; set; }

        public DbSet<EmployeeEntity> Employees { get; set; }

        public bool HasChanges => ChangeTracker.HasChanges();

        public DbSet<PaidTimeOffPolicyEntity> PaidTimeOffPolicies { get; set; }

        public DbSet<TenantAspNetUserEntity> TenantAspNetUsers { get; set; }

        public DbSet<TenantEntity> Tenants { get; set; }

        public ApplicationWriteDbContext(DbContextOptions<ApplicationWriteDbContext> options)
                                                    : base(options)
        { }

        public ApplicationWriteDbContext(
            DbContextOptions<ApplicationWriteDbContext> options,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService)
            : base(options)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableDbEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = currentUserService.UserId;
                        entry.Entity.CreatedUtc = dateTimeService.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = currentUserService.UserId;
                        entry.Entity.LastModifiedUtc = dateTimeService.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationWriteDbContext).Assembly);
    }
}