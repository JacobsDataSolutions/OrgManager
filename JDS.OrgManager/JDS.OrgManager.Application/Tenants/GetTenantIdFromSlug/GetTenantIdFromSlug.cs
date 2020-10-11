using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Tenants.GetTenantIdFromSlug
{
    public class GetTenantIdFromSlugQuery : IRequest<int>, ICacheableQuery
    {
        public bool BypassCache { get; set; }

        public string CacheKey => $"{nameof(GetTenantIdFromSlugQuery)}-{Slug}";

        public bool RefreshCachedEntry { get; set; }

        public bool ReplaceCachedEntry { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public string Slug { get; set; }

        public class GetTenantIdFromSlugQueryHandler : IRequestHandler<GetTenantIdFromSlugQuery, int>
        {
            private readonly IApplicationReadDbFacade facade;

            public GetTenantIdFromSlugQueryHandler(IApplicationReadDbFacade facade)
            {
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
            }

            public async Task<int> Handle(GetTenantIdFromSlugQuery request, CancellationToken cancellationToken)
            {
                var id = await facade.QueryFirstOrDefaultAsync<int?>(@"SELECT TOP 1 Id FROM Tenants WITH(NOLOCK) WHERE Slug = @Slug", request, null, cancellationToken);
                if (id == null)
                {
                    throw new NotFoundException($"Tenant not found for slug '{request.Slug}'.");
                }
                return (int)id;
            }
        }
    }
}
