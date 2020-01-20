// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Common.Currencies;
using JDS.OrgManager.Application.Common.PaidTimeOffPolicies;
using JDS.OrgManager.Domain.Common.Finance;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.System.Commands.SeedInitialData
{
    public class InitialDataSeeder
    {
        #region Private Fields

        // Hard-coded for the time being.
        private const int TenantId = 1;

        private readonly IOrgManagerDbContext context;

        private readonly ILogger logger;

        #endregion

        #region Public Constructors

        public InitialDataSeeder(IOrgManagerDbContext context, ILogger logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Public Methods

        public async Task SeedAllAsync(CancellationToken cancellationToken)
        {
            if (context.PaidTimeOffPolicies.Any())
            {
                logger.LogInformation("Database has already been initialized with basic data. Nothing to do.");
                return;
            }

            await SeedPtoPoliciesAsync();
            await SeedCurrenciesAsync();
            await context.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Private Methods

        private async Task SeedCurrenciesAsync()
        {
            await context.Currencies.AddRangeAsync(from c in Currency.GetAll() select new CurrencyEntity { Code = c.Code });
        }

        private async Task SeedPtoPoliciesAsync()
        {
            await context.PaidTimeOffPolicies.AddRangeAsync(new PaidTimeOffPolicyEntity[]
            {
                new PaidTimeOffPolicyEntity { TenantId = TenantId, EmployeeLevel = 1, Name = "Standard 1", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 10.0m },
                new PaidTimeOffPolicyEntity { TenantId = TenantId, EmployeeLevel = 2, Name = "Standard 2", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 10.6666m },
                new PaidTimeOffPolicyEntity { TenantId = TenantId, EmployeeLevel = 3, Name = "Standard 3", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 11.3333m },
                new PaidTimeOffPolicyEntity { TenantId = TenantId, EmployeeLevel = 4, Name = "Standard 4", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 12.0m },
                new PaidTimeOffPolicyEntity { TenantId = TenantId, EmployeeLevel = 5, Name = "Standard 5", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 12.6666m },
                new PaidTimeOffPolicyEntity { TenantId = TenantId, EmployeeLevel = 6, Name = "Standard 6", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 13.3333m },
                new PaidTimeOffPolicyEntity { TenantId = TenantId, EmployeeLevel = 7, Name = "Standard 7", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 14.0m },
                new PaidTimeOffPolicyEntity { TenantId = TenantId, EmployeeLevel = 8, Name = "Standard 8", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 14.6666m },
                new PaidTimeOffPolicyEntity { TenantId = TenantId, EmployeeLevel = 8, Name = "Unlimited 8", IsDefaultForEmployeeLevel = false, AllowsUnlimitedPto = true },
                new PaidTimeOffPolicyEntity { TenantId = TenantId, EmployeeLevel = 9, Name = "Standard 9", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 15.3333m },
                new PaidTimeOffPolicyEntity { TenantId = TenantId, EmployeeLevel = 9, Name = "Unlimited 9", IsDefaultForEmployeeLevel = false, AllowsUnlimitedPto = true },
                new PaidTimeOffPolicyEntity { TenantId = TenantId, EmployeeLevel = 10, Name = "Standard 10", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 16.0m },
                new PaidTimeOffPolicyEntity { TenantId = TenantId, EmployeeLevel = 10, Name = "Unlimited 10", IsDefaultForEmployeeLevel = false, AllowsUnlimitedPto = true },
            });
        }

        #endregion
    }
}