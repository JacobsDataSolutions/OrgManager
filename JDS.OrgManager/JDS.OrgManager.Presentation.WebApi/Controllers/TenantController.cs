using JDS.OrgManager.Application.Tenants.Queries.GetHasTenantAccess;
using JDS.OrgManager.Application.Tenants.Queries.GetTenantIdFromAssignmentKey;
using JDS.OrgManager.Application.Tenants.Queries.GetTenantIdFromSlug;
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
    public class TenantController : CqrsControllerBase
    {
        public TenantController(IMediator mediator) : base(mediator)
        { }

        [HttpGet("[action]")]
        public async Task<ActionResult<bool>> GetHasTenantAccess(int tenantId) =>
            await WithAuthenticatedUserClaimsDo(async userClaims =>
            {
                return userClaims.AuthorizedTenantIds.Contains(tenantId) || await Mediator.Send(new GetHasTenantAccessQuery() { AspNetUsersId = userClaims.AspNetUsersId, TenantId = tenantId });
            });

        [HttpGet("[action]")]
        public async Task<ActionResult<int>> GetTenantIdFromAssignmentKey(Guid assignmentKey) => Ok(await Mediator.Send(new GetTenantIdFromAssignmentKeyQuery() { AssignmentKey = assignmentKey }));

        [HttpGet("[action]")]
        public async Task<ActionResult<int>> GetTenantIdFromSlug(string slug) => Ok(await Mediator.Send(new GetTenantIdFromSlugQuery() { Slug = slug }));
    }
}
