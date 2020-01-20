// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.Identity;
using JDS.OrgManager.Application.Common.Currencies;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Common.PaidTimeOffPolicies;
using JDS.OrgManager.Application.Models;
using JDS.OrgManager.Common.Abstractions.Dates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Persistence.DbContexts
{
    public class OrgManagerDbContext : DbContext, IOrgManagerDbContext
    {
        #region Private Fields

        private readonly ICurrentUserService currentUserService;

        private readonly IDateTimeService dateTimeService;

        #endregion

        #region Public Properties + Indexers

        public DbSet<CurrencyEntity> Currencies { get; set; }

        public DbSet<EmployeeManagerEntity> EmployeeManagers { get; set; }

        public DbSet<EmployeeEntity> Employees { get; set; }

        public bool HasChanges => ChangeTracker.HasChanges();

        public DbSet<PaidTimeOffPolicyEntity> PaidTimeOffPolicies { get; set; }

        #endregion

        #region Public Constructors

        public OrgManagerDbContext(DbContextOptions<OrgManagerDbContext> options)
                                                    : base(options)
        { }

        public OrgManagerDbContext(
            DbContextOptions<OrgManagerDbContext> options,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService)
            : base(options)
        {
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
        }

        #endregion

        #region Public Methods

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
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

        #endregion

        #region Protected Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrgManagerDbContext).Assembly);

        #endregion
    }
}