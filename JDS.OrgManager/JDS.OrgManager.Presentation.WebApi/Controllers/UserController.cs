// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Users.Queries.GetUserStatus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CqrsControllerBase
    {
        private static readonly TimeSpan userStatusSlidingExpiration = TimeSpan.FromMinutes(5.0);

        private readonly IMediator mediator;

        public UserController(IMediator mediator) => this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<UserStatusViewModel>> GetUserStatus(int? tenantId) => Ok(await mediator.Send(new GetUserStatusQuery { AspNetUsersId = GetAspNetUsersId(), TenantId = tenantId, SlidingExpiration = userStatusSlidingExpiration }));
    }
}