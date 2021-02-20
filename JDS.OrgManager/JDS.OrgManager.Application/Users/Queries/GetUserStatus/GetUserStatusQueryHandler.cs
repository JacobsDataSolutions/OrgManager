// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Queries;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Users.Queries.GetUserStatus
{
    public class GetUserStatusQuery : IRequest<UserStatusViewModel>, ICacheableQuery
    {
        public int AspNetUsersId { get; set; }

        public bool BypassCache { get; set; }

        public string CacheKey => $"{nameof(GetUserStatusQuery)}-{AspNetUsersId}";

        public bool RefreshCachedEntry { get; set; }

        public bool ReplaceCachedEntry { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public int? TenantId { get; set; }

        public class GetUserStatusQueryHandler : IRequestHandler<GetUserStatusQuery, UserStatusViewModel>
        {
            private readonly IApplicationReadDbFacade facade;

            public GetUserStatusQueryHandler(IApplicationReadDbFacade facade) => this.facade = facade ?? throw new ArgumentNullException(nameof(facade));

            public async Task<UserStatusViewModel> Handle(GetUserStatusQuery request, CancellationToken cancellationToken)
            {
                var userStatus = await facade.QueryFirstOrDefaultAsync<UserStatusViewModel>(@"
						SELECT TOP 1
                            u.IsCustomer,
                            c.Id CustomerId,
	                        CASE WHEN c.Id IS NULL THEN 0 ELSE 1 END HasProvidedCustomerInformation,
	                        CASE WHEN e.Id IS NULL THEN 0 ELSE 1 END HasProvidedEmployeeInformation
                        FROM AspNetUsers u WITH(NOLOCK)
                        LEFT JOIN Customers c WITH(NOLOCK) ON c.AspNetUsersId = u.Id
                        LEFT JOIN Employees e WITH(NOLOCK) ON e.AspNetUsersId = u.Id AND (@TenantId IS NULL OR e.TenantId = @TenantId)
                        WHERE u.Id = @AspNetUsersId
                    ", request, cancellationToken: cancellationToken);
                userStatus.AuthorizedTenants = (await facade.QueryAsync<int>("SELECT TenantId FROM TenantAspNetUsers t WITH(NOLOCK) WHERE AspNetUsersId = @AspNetUsersId", request, cancellationToken: cancellationToken)).ToArray();
                return userStatus;
            }
        }
    }
}