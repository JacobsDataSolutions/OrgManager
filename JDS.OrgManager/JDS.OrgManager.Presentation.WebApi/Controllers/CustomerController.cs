// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Customers.Commands.AddOrUpdateTenant;
using JDS.OrgManager.Application.Customers.Commands.DeleteTenant;
using JDS.OrgManager.Application.Customers.Commands.ProvisionTenant;
using JDS.OrgManager.Application.Customers.Queries.GetNewAssignmentKey;
using JDS.OrgManager.Application.Customers.Queries.GetTenantsForCustomer;
using JDS.OrgManager.Application.Tenants;
using JDS.OrgManager.Presentation.WebApi.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.WebApi.Controllers
{
    [Authorize]
    [CustomerAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : CqrsControllerBase
    {
        public CustomerController(IMediator mediator) : base(mediator)
        { }

        [HttpPost("[action]")]
        public async Task<ActionResult<TenantViewModel>> AddOrUpdateTenant([FromBody] TenantViewModel tenant)
        {
            var isNewTenant = tenant.Id == null;

            var userClaims = GetUserClaims();
            tenant = await Mediator.Send(new AddOrUpdateTenantCommand { AspNetUsersId = userClaims.AspNetUsersId, Tenant = tenant });
            if (tenant.Id == null)
            {
                throw new InvalidOperationException();
            }

            if (isNewTenant)
            {
                await Mediator.Send(new ProvisionTenantCommand { TenantId = (int)tenant.Id });
            }

            return tenant;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<DeleteTenantViewModel>> DeleteTenant([FromBody] DeleteTenantViewModel deleteTenant)
        {
            return await Mediator.Send(new DeleteTenantCommand { DeleteTenant = deleteTenant });
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Guid>> GetNewAssignmentKey()
        {
            var userClaims = GetUserClaims();
            return await Mediator.Send(new GetNewAssignmentKeyQuery() { AspNetUsersId = userClaims.AspNetUsersId });
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<TenantViewModel[]>> GetTenantsForCustomer()
        {
            var userClaims = GetUserClaims();
            return await Mediator.Send(new GetTenantsForCustomerQuery() { AspNetUsersId = userClaims.AspNetUsersId });
        }
    }
}