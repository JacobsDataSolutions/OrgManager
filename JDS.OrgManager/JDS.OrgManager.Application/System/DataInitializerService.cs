using JDS.OrgManager.Application.Customers.ProvisionTenant;
using JDS.OrgManager.Application.System.Commands.ClearTables;
using JDS.OrgManager.Application.System.Commands.SeedInitialData;
using JDS.OrgManager.Application.Tenants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.System
{
    public class DataInitializerService
    {
        private readonly IMediator mediator;

        public DataInitializerService(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task InitializeDataForSystemAsync()
        {
            var tenants = new[] { new TenantViewModel { Name = "TEMPLATE TENANT--DO NOT MODIFY", Id = 1, Slug = "template-tenant", AssignmentKey = Guid.NewGuid() } }
                .Concat(from n in Enumerable.Range(10, 3) select new TenantViewModel { Name = $"Test Tenant {n}", Id = n, Slug = $"test-tenant-{n}", AssignmentKey = Guid.NewGuid() });

            await mediator.Send(new ClearTablesCommand());

            await mediator.Send(new SeedInitialDataCommand { Tenants = tenants });

            foreach (var tenant in tenants)
            {
                await mediator.Send(new ProvisionTenantCommand { TenantId = (int)tenant.Id });
            }
        }
    }
}
