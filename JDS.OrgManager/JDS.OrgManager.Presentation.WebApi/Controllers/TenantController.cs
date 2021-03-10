// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Tenants;
using JDS.OrgManager.Application.Tenants.Queries.GetTenant;
using JDS.OrgManager.Application.Tenants.Queries.GetTenantIdFromAssignmentKey;
using JDS.OrgManager.Application.Tenants.Queries.GetTenantIdFromSlug;
using JDS.OrgManager.Application.Tenants.Queries.GetUserHasTenantAccess;
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
    public class TenantController : CqrsControllerBase
    {
        private readonly IMediator mediator;

        public TenantController(IMediator mediator) => this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet("[action]")]
        public async Task<ActionResult<bool>> GetHasTenantAccess(int tenantId) => Ok(await mediator.Send(new GetUserHasTenantAccessQuery { AspNetUsersId = GetAspNetUsersId(), TenantId = tenantId }));

        [HttpGet("[action]")]
        public async Task<ActionResult<TenantViewModel>> GetTenant(int tenantId) => Ok(await mediator.Send(new GetTenantQuery() { TenantId = tenantId }));

        [HttpGet("[action]")]
        public async Task<ActionResult<int>> GetTenantIdFromAssignmentKey(Guid assignmentKey) => Ok(await mediator.Send(new GetTenantIdFromAssignmentKeyQuery() { AssignmentKey = assignmentKey }));

        [HttpGet("[action]")]
        public async Task<ActionResult<int>> GetTenantIdFromSlug(string slug) => Ok(await mediator.Send(new GetTenantIdFromSlugQuery() { Slug = slug }));
    }
}