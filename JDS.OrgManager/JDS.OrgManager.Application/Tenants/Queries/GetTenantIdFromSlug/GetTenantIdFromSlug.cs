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

namespace JDS.OrgManager.Application.Tenants.Queries.GetTenantIdFromSlug
{
    public class GetTenantIdFromSlugQuery : IRequest<int>, ICacheableQuery
    {
        public bool BypassCache { get; set; }

        public string CacheKey => $"{nameof(GetTenantIdFromSlugQuery)}-{Slug}";

        public bool RefreshCachedEntry { get; set; }

        public bool ReplaceCachedEntry { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public string Slug { get; set; } = default!;

        public class GetTenantIdFromSlugQueryHandler : IRequestHandler<GetTenantIdFromSlugQuery, int>
        {
            private readonly IApplicationReadDbFacade facade;

            public GetTenantIdFromSlugQueryHandler(IApplicationReadDbFacade facade) => this.facade = facade ?? throw new ArgumentNullException(nameof(facade));

            public async Task<int> Handle(GetTenantIdFromSlugQuery request, CancellationToken cancellationToken)
            {
                var id = await facade.QueryFirstOrDefaultAsync<int?>(@"SELECT TOP 1 Id FROM Tenants WITH(NOLOCK) WHERE Slug = @Slug", request, default!, cancellationToken);
                if (id == null)
                {
                    throw new NotFoundException($"Tenant not found for slug '{request.Slug}'.");
                }
                return (int)id;
            }
        }
    }
}