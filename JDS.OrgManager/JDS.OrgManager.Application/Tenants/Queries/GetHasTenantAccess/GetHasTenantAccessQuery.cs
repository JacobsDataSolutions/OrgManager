using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace JDS.OrgManager.Application.Tenants.Queries.GetHasTenantAccess
{
    public class GetHasTenantAccessQuery : IRequest<bool>, ICacheableQuery
    {
        public int AspNetUsersId { get; set; }

        public int TenantId { get; set; }

        public bool BypassCache { get; set; }

        public string CacheKey => $"{nameof(GetHasTenantAccessQuery)}-{AspNetUsersId}-{TenantId}";

        public bool RefreshCachedEntry { get; set; }

        public bool ReplaceCachedEntry { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

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
