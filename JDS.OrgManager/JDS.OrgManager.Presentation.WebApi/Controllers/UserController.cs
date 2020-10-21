using JDS.OrgManager.Application.Users.GetUserStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CqrsControllerBase
    {
        private static readonly TimeSpan userStatusSlidingExpiration = TimeSpan.FromMinutes(5.0);

        public UserController(IMediator mediator) : base(mediator)
        { }

        [HttpGet("[action]")]
        public async Task<ActionResult<UserStatusViewModel>> GetUserStatus(int? tenantId = null) =>
            await WithAuthenticatedUserClaimsDo(async userClaims =>
            {
                return await Mediator.Send(new GetUserStatusQuery { AspNetUsersId = userClaims.AspNetUsersId, TenantId = tenantId, SlidingExpiration = userStatusSlidingExpiration });
            });
    }
}
