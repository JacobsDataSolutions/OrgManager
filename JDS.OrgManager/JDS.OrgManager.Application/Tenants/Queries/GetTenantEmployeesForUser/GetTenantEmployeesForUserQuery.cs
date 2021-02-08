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

namespace JDS.OrgManager.Application.Tenants.Queries.GetTenantEmployeesForUser
{
    public class GetTenantEmployeesForUserQuery : IRequest<TenantEmployeeIdentityModel[]>, ICacheableQuery
    {
        public int AspNetUsersId { get; set; }

        public bool BypassCache { get; set; }

        public string CacheKey => $"{nameof(GetTenantEmployeesForUserQuery)}-{AspNetUsersId}";

        public bool RefreshCachedEntry { get; set; }

        public bool ReplaceCachedEntry { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public class GetTenantEmployeesForUserQueryHandler : IRequestHandler<GetTenantEmployeesForUserQuery, TenantEmployeeIdentityModel[]>
        {
            private readonly IApplicationReadDbFacade facade;

            public GetTenantEmployeesForUserQueryHandler(IApplicationReadDbFacade facade) => this.facade = facade ?? throw new ArgumentNullException(nameof(facade));

            public async Task<TenantEmployeeIdentityModel[]> Handle(GetTenantEmployeesForUserQuery request, CancellationToken cancellationToken)
            {
                var sql = @"
                    SELECT t.TenantId, e.Id AS EmployeeId FROM TenantAspNetUsers t WITH(NOLOCK)
                    LEFT JOIN Employees e WITH(NOLOCK) ON t.TenantId = e.TenantId AND t.AspNetUsersId = e.AspNetUsersId
                    WHERE t.AspNetUsersId = @AspNetUsersId";

                var employeesForUserByTenant =
                    await facade.QueryAsync<TenantEmployeeIdentityModel>(sql, request, cancellationToken: cancellationToken);
                return (
                    from e in employeesForUserByTenant
                    group e by e.TenantId into grouped
                    select grouped.First()).ToArray();
            }
        }
    }
}