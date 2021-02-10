using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Tenants.Queries.GetUserHasTenantAccess
{
    public class GetUserHasTenantAccessQuery : IRequest<bool>, ICacheableQuery
    {
        public int AspNetUsersId { get; set; }

        public int TenantId { get; set; }

        public bool BypassCache { get; set; }

        public string CacheKey => nameof(GetUserHasTenantAccessQuery);

        public bool RefreshCachedEntry { get; set; }

        public bool ReplaceCachedEntry { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public class GetIsTenantOwnedByCustomerQueryHandler : IRequestHandler<GetUserHasTenantAccessQuery, bool>
        {
            private readonly IApplicationReadDbFacade queryFacade;

            public GetIsTenantOwnedByCustomerQueryHandler(IApplicationReadDbFacade queryFacade) => this.queryFacade = queryFacade ?? throw new ArgumentNullException(nameof(queryFacade));

            public Task<bool> Handle(GetUserHasTenantAccessQuery request, CancellationToken cancellationToken) =>
                queryFacade.QuerySingleAsync<bool>(@"
SELECT CASE WHEN EXISTS (SELECT TOP 1 t.TenantId
  FROM TenantAspNetUsers t WITH(NOLOCK)
  WHERE t.AspNetUsersId = @AspNetUsersId AND t.TenantId = @TenantId) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END
                ", request, cancellationToken: cancellationToken);
        }
    }
}
