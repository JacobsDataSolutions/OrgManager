// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Customers.Queries.GetTenantsForCustomer;
using JDS.OrgManager.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.WebApi.Middleware
{
    public class CustomerAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomerAuthorizeAttribute() : base(typeof(CustomerAuthorizeFilter))
        {
            Arguments = new object[] { };
        }

        private class CustomerAuthorizeFilter : IAsyncActionFilter
        {
            private readonly IMediator mediator;

            public CustomerAuthorizeFilter(IMediator mediator)
            {
                this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var user = context.HttpContext.User;
                var userClaims = user.GetUserClaims();
                if (userClaims != null)
                {
                    if (userClaims.IsCustomer)
                    {
                        // No tenant arguments.
                        if (!context.ActionArguments.Any())
                        {
                            await next();
                            return;
                        }

                        // Check for tenant access.
                        var tenantId = GetTenantId(context.ActionArguments.First().Value);
                        if (tenantId == null || (await mediator.Send(new GetTenantsForCustomerQuery { AspNetUsersId = userClaims.AspNetUsersId })).Any(t => t.Id == tenantId))
                        {
                            // User is authorized. Proceed.
                            await next();
                            return;
                        }
                    }
                    context.Result = new ForbidResult();
                    return;
                }
                context.Result = new UnauthorizedResult();
            }

            private int? GetTenantId(object actionArgument)
            {
                if (actionArgument.GetType() == typeof(int?))
                {
                    return (int?)actionArgument;
                }
                else if (actionArgument is int tenantId)
                {
                    return tenantId;
                }
                else
                {
                    dynamic model = actionArgument;
                    int? tenantIdProperty;
                    try
                    {
                        tenantIdProperty = model.Id;
                        return tenantIdProperty;
                    }
                    catch (RuntimeBinderException)
                    {
                        try
                        {
                            tenantIdProperty = model.TenantId;
                            return tenantIdProperty;
                        }
                        catch (RuntimeBinderException)
                        { }
                    }
                }
                return null;
            }
        }
    }
}