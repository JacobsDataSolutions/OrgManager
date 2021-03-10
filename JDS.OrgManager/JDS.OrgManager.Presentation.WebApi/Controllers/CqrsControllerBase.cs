// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace JDS.OrgManager.Presentation.WebApi.Controllers
{
    public abstract class CqrsControllerBase : ControllerBase
    {
        public CqrsControllerBase()
        {
        }

        protected int GetAspNetUsersId() =>
            GetClaimsValue(ClaimTypes.NameIdentifier, value => int.TryParse(value, out var id) ? id : throw new AuthorizationException("Can't retrieve user ID for anonymous user."));

        private T GetClaimsValue<T>(string claimName, Func<string, T> parseClaimValue)
        {
            var claim = User.FindFirst(claimName);
            if (claim != null)
            {
                return parseClaimValue(claim.Value);
            }
            return default;
        }
    }
}