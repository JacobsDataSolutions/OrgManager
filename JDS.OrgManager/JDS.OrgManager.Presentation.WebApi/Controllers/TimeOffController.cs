// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.HumanResources.TimeOff;
using JDS.OrgManager.Application.HumanResources.TimeOff.Commands.SubmitNewPaidTimeOffRequest;
using JDS.OrgManager.Application.HumanResources.TimeOff.Queries.GetPaidTimeOffPolicyDetail;
using JDS.OrgManager.Application.HumanResources.TimeOff.Queries.GetPaidTimeOffPolicyList;
using JDS.OrgManager.Application.HumanResources.TimeOff.Queries.GetPaidTimeOffRequestsForEmployee;
using JDS.OrgManager.Application.HumanResources.TimeOff.Queries.GetPaidTimeOffRequestsForTenant;
using JDS.OrgManager.Application.HumanResources.TimeOff.Queries.ValidateRequestedPaidTimeOffHours;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
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
    public class TimeOffController : CqrsControllerBase
    {
        private readonly IMediator mediator;

        public TimeOffController(IMediator mediator) => this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet("[action]")]
        public async Task<ActionResult<GetPaidTimeOffPolicyDetailViewModel>> GetPaidTimeOffPolicyDetail(int id, int tenantId) => Ok(await mediator.Send(new GetPaidTimeOffPolicyDetailQuery { Id = id, TenantId = tenantId }));

        [HttpGet("[action]")]
        public async Task<ActionResult<GetPaidTimeOffPolicyListViewModel[]>> GetPaidTimeOffPolicyList(int tenantId) => Ok(await mediator.Send(new GetPaidTimeOffPolicyListQuery { TenantId = tenantId }));

        [HttpGet("[action]")]
        public async Task<ActionResult<PaidTimeOffRequestViewModel[]>> GetPaidTimeOffRequestsForEmployee(int? employeeId, int tenantId) => Ok(await mediator.Send(new GetPaidTimeOffRequestsForEmployeeQuery { AspNetUsersId = GetAspNetUsersId(), EmployeeId = employeeId, TenantId = tenantId }));

        [HttpGet("[action]")]
        public async Task<ActionResult<PaidTimeOffRequestViewModel[]>> GetPaidTimeOffRequestsForTenant(int tenantId) => Ok(await mediator.Send(new GetPaidTimeOffRequestsForTenantQuery { TenantId = tenantId }));

        [HttpPost("[action]")]
        public async Task<ActionResult<SubmitNewPaidTimeOffRequestViewModel>> SubmitNewPaidTimeOffRequest([FromBody] SubmitNewPaidTimeOffRequestViewModel request) => Ok(await mediator.Send(new SubmitNewPaidTimeOffRequestCommand { AspNetUsersId = GetAspNetUsersId(), PaidTimeOffRequest = request }));

        [HttpPost("[action]")]
        public async Task<ActionResult<PaidTimeOffRequestValidationResult>> ValidateRequestedPaidTimeOffHours([FromBody] ValidateRequestedPaidTimeOffHoursViewModel request) => Ok(await mediator.Send(new ValidateRequestedPaidTimeOffHoursQuery { AspNetUsersId = GetAspNetUsersId(), ValidationRequest = request }));
    }
}