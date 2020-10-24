// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.WebApi.Controllers
{
    public abstract class CqrsControllerBase : ControllerBase
    {
        protected IMediator Mediator { get; }

        protected CqrsControllerBase(IMediator mediator)
        {
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected UserClaims GetUserClaims() => User.GetUserClaims();

        protected async Task<ActionResult<T>> WithAuthenticatedEmployeeUserClaimsDo<T>(Func<UserClaims, Task<T>> func, int tenantId)
        {
            var userClaims = User.GetUserClaims();
            if (userClaims == null)
            {
                return Unauthorized();
            }
            var employeeId = userClaims.GetEmployeeId(tenantId);
            return await WithAuthenticatedUserClaimsDo(func, tenantId, employeeId);
        }

        protected async Task<ActionResult<T>> WithAuthenticatedUserClaimsDo<T>(Func<UserClaims, Task<T>> func, int? tenantId = null, int? employeeId = null)
        {
            var userClaims = User.GetUserClaims();
            if (
                // Ensure user is authenticated.
                userClaims == null ||
                // Ensure user is authenticated and associated with the specified tenant.
                (tenantId != null && !userClaims.TenantEmployees.Any(t => t.TenantId == tenantId)) ||
                // Ensure user is authenticated and associated with the specified tenant and employee.
                (tenantId != null && employeeId != null && !userClaims.TenantEmployees.Any(t => t.TenantId == tenantId && t.EmployeeId == employeeId)))
            {
                return Unauthorized();
            }
            return Ok(await func(userClaims));
        }
    }
}