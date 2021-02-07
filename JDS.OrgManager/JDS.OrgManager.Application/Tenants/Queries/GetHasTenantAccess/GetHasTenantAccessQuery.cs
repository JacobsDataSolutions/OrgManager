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
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Tenants.Queries.GetHasTenantAccess
{
    public class GetHasTenantAccessQuery : IRequest<bool>, ICacheableQuery
    {
        public int AspNetUsersId { get; set; }

        public bool BypassCache { get; set; }

        public string CacheKey => $"{nameof(GetHasTenantAccessQuery)}-{AspNetUsersId}-{TenantId}";

        public bool RefreshCachedEntry { get; set; }

        public bool ReplaceCachedEntry { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public int TenantId { get; set; }

        public class GetHasTenantAccessQueryHandler : IRequestHandler<GetHasTenantAccessQuery, bool>
        {
            private readonly IApplicationReadDbFacade facade;

            public GetHasTenantAccessQueryHandler(IApplicationReadDbFacade facade)
            {
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
            }

            public async Task<bool> Handle(GetHasTenantAccessQuery request, CancellationToken cancellationToken)
            {
                var sql = @"SELECT TOP 1 TenantId FROM TenantAspNetUsers WITH(NOLOCK) WHERE TenantId = @TenantId AND AspNetUsersId = @AspNetUsersId";
                return (await facade.QueryFirstOrDefaultAsync<int?>(sql, request)) != null;
            }
        }
    }
}