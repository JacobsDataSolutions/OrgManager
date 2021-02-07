// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Common.TimeOff;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Customers.Commands.ProvisionTenant
{
    public class ProvisionTenantCommand : IRequest<int>
    {
        public int TenantId { get; set; }

        public class ProvisionTenantCommandHandler : IRequestHandler<ProvisionTenantCommand, int>
        {
            private readonly IApplicationWriteDbContext context;

            public ProvisionTenantCommandHandler(
                IApplicationWriteDbContext context)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public async Task<int> Handle(ProvisionTenantCommand request, CancellationToken cancellationToken)
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var sqlTransaction = transaction.GetDbTransaction();

                    await context.PaidTimeOffPolicies.AddRangeAsync(new[]
                    {
                        new PaidTimeOffPolicyEntity { TenantId = request.TenantId, EmployeeLevel = 1, Name = "Standard 1", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 10.0m },
                        new PaidTimeOffPolicyEntity { TenantId = request.TenantId, EmployeeLevel = 2, Name = "Standard 2", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 10.6666m },
                        new PaidTimeOffPolicyEntity { TenantId = request.TenantId, EmployeeLevel = 3, Name = "Standard 3", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 11.3333m },
                        new PaidTimeOffPolicyEntity { TenantId = request.TenantId, EmployeeLevel = 4, Name = "Standard 4", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 12.0m },
                        new PaidTimeOffPolicyEntity { TenantId = request.TenantId, EmployeeLevel = 5, Name = "Standard 5", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 12.6666m },
                        new PaidTimeOffPolicyEntity { TenantId = request.TenantId, EmployeeLevel = 6, Name = "Standard 6", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 13.3333m },
                        new PaidTimeOffPolicyEntity { TenantId = request.TenantId, EmployeeLevel = 7, Name = "Standard 7", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 14.0m },
                        new PaidTimeOffPolicyEntity { TenantId = request.TenantId, EmployeeLevel = 8, Name = "Standard 8", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 14.6666m },
                        new PaidTimeOffPolicyEntity { TenantId = request.TenantId, EmployeeLevel = 8, Name = "Unlimited 8", IsDefaultForEmployeeLevel = false, AllowsUnlimitedPto = true },
                        new PaidTimeOffPolicyEntity { TenantId = request.TenantId, EmployeeLevel = 9, Name = "Standard 9", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 15.3333m },
                        new PaidTimeOffPolicyEntity { TenantId = request.TenantId, EmployeeLevel = 9, Name = "Unlimited 9", IsDefaultForEmployeeLevel = false, AllowsUnlimitedPto = true },
                        new PaidTimeOffPolicyEntity { TenantId = request.TenantId, EmployeeLevel = 10, Name = "Standard 10", IsDefaultForEmployeeLevel = true, MaxPtoHours = 320.0m, PtoAccrualRate = 16.0m },
                        new PaidTimeOffPolicyEntity { TenantId = request.TenantId, EmployeeLevel = 10, Name = "Unlimited 10", IsDefaultForEmployeeLevel = false, AllowsUnlimitedPto = true },
                    });

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return request.TenantId;
                }
            }
        }
    }
}