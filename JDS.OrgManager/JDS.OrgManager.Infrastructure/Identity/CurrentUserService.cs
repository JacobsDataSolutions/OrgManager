// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using IdentityServer4.Extensions;
using JDS.OrgManager.Application.Abstractions.Identity;
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Infrastructure.Http;
using System.Linq;
using System.Security.Claims;

namespace JDS.OrgManager.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        public bool IsAuthenticated => MyHttpContext.Current?.User != null ? MyHttpContext.Current.User.IsAuthenticated() : false;

        public string UserId
        {
            get
            {
                var user = MyHttpContext.Current?.User;
                if (user != null)
                {
                    // ASP.NET Core user Id.
                    var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    if (userIdClaim != null)
                    {
                        var id = userIdClaim.Value;
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            if (id.Length > Lengths.CreatedUpdatedBy)
                            {
                                return id.Substring(0, Lengths.CreatedUpdatedBy);
                            }
                            return id;
                        }
                    }
                }
                return "SYSTEM";
            }
        }
    }
}