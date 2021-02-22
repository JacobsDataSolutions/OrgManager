using JDS.OrgManager.Application.HumanResources.TimeOff;
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
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<PaidTimeOffRequestValidationResult>> ValidateRequestedPaidTimeOffHours(ValidateRequestedPaidTimeOffHoursViewModel request) => Ok(await mediator.Send(new ValidateRequestedPaidTimeOffHoursQuery { ValidationRequest = request }));

        [HttpGet("[action]")]
        public async Task<ActionResult<PaidTimeOffRequestViewModel[]>> GetPaidTimeOffRequestsForEmployee(int? employeeId, int tenantId) => Ok(await mediator.Send(new GetPaidTimeOffRequestsForEmployeeQuery { AspNetUsersId = GetAspNetUsersId(), EmployeeId = employeeId, TenantId = tenantId }));

        [HttpGet("[action]")]
        public async Task<ActionResult<PaidTimeOffRequestViewModel[]>> GetPaidTimeOffRequestsForTenant(int tenantId) => Ok(await mediator.Send(new GetPaidTimeOffRequestsForTenantQuery { TenantId = tenantId }));
    }
}
