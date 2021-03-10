// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Customers.Commands.ProvisionTenant;
using JDS.OrgManager.Application.System.Commands.ClearTables;
using JDS.OrgManager.Application.System.Commands.SeedInitialData;
using JDS.OrgManager.Application.Tenants;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.System
{
    public class DataInitializerService
    {
        private readonly IMediator mediator;

        public DataInitializerService(IMediator mediator) => this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        public async Task InitializeDataForSystemAsync()
        {
            var tenants = new[] { new TenantViewModel { Name = "TEMPLATE TENANT--DO NOT MODIFY", Id = 1, Slug = "template-tenant", AssignmentKey = Guid.NewGuid() } }
                .Concat(from n in Enumerable.Range(10, 3) select new TenantViewModel { Name = $"Test Tenant {n}", Id = n, Slug = $"test-tenant-{n}", AssignmentKey = Guid.NewGuid() });

            await mediator.Send(new ClearTablesCommand());

            await mediator.Send(new SeedInitialDataCommand { Tenants = tenants });

            foreach (var tenant in tenants)
            {
                await mediator.Send(new ProvisionTenantCommand { TenantId = tenant.Id });
            }
        }
    }
}