// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Customers;
using JDS.OrgManager.Application.Customers.Commands.AddOrUpdateCustomer;
using JDS.OrgManager.Application.Customers.Commands.AddOrUpdateTenant;
using JDS.OrgManager.Application.Customers.Commands.DeleteTenant;
using JDS.OrgManager.Application.Customers.Commands.ProvisionTenant;
using JDS.OrgManager.Application.Customers.Queries.GetCustomer;
using JDS.OrgManager.Application.Customers.Queries.GetCustomerId;
using JDS.OrgManager.Application.Customers.Queries.GetNewAssignmentKey;
using JDS.OrgManager.Application.Customers.Queries.GetTenantsForCustomer;
using JDS.OrgManager.Application.Tenants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : CqrsControllerBase
    {
        private readonly IMediator mediator;

        public CustomerController(IMediator mediator) => this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerViewModel>> AddOrUpdateCustomer([FromBody] CustomerViewModel customer)
        {
            var userId = GetAspNetUsersId();
            return Ok(await mediator.Send(new AddOrUpdateCustomerCommand { AspNetUsersId = userId, Customer = customer }));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<TenantViewModel>> AddOrUpdateTenant([FromBody] TenantViewModel tenant)
        {
            var isNewTenant = tenant.Id == 0;
            tenant = await mediator.Send(new AddOrUpdateTenantCommand { AspNetUsersId = GetAspNetUsersId(), Tenant = tenant });

            if (isNewTenant)
            {
                await mediator.Send(new ProvisionTenantCommand { TenantId = tenant.Id });
            }

            return Ok(tenant);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<DeleteTenantViewModel>> DeleteTenant([FromBody] DeleteTenantViewModel deleteTenant)
        {
            return Ok(await mediator.Send(new DeleteTenantCommand { DeleteTenant = deleteTenant }));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<CustomerViewModel>> GetCustomer() => Ok(await mediator.Send(new GetCustomerQuery() { AspNetUsersId = GetAspNetUsersId() }));

        [HttpGet("[action]")]
        public async Task<ActionResult<int?>> GetCustomerId() => Ok(await mediator.Send(new GetCustomerIdQuery() { AspNetUsersId = GetAspNetUsersId() }));

        [HttpGet("[action]")]
        public async Task<ActionResult<Guid>> GetNewAssignmentKey() => Ok(await mediator.Send(new GetNewAssignmentKeyQuery() { AspNetUsersId = GetAspNetUsersId() }));

        [HttpGet("[action]")]
        public async Task<ActionResult<TenantViewModel[]>> GetTenantsForCustomer() => Ok(await mediator.Send(new GetTenantsForCustomerQuery() { AspNetUsersId = GetAspNetUsersId() }));
    }
}