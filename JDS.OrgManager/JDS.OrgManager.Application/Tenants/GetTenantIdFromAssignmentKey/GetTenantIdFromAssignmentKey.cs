using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Tenants.GetTenantIdFromAssignmentKey
{
    public class GetTenantIdFromAssignmentKeyQuery : IRequest<int>, ICacheableQuery
    {
        public Guid AssignmentKey { get; set; }

        public bool BypassCache { get; set; }

        public string CacheKey => $"{nameof(GetTenantIdFromAssignmentKeyQuery)}-{AssignmentKey}";

        public bool RefreshCachedEntry { get; set; }

        public bool ReplaceCachedEntry { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public class GetTenantIdFromAssignmentKeyQueryHandler : IRequestHandler<GetTenantIdFromAssignmentKeyQuery, int>
        {
            private readonly IApplicationReadDbFacade facade;

            public GetTenantIdFromAssignmentKeyQueryHandler(IApplicationReadDbFacade facade)
            {
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
            }

            public async Task<int> Handle(GetTenantIdFromAssignmentKeyQuery request, CancellationToken cancellationToken)
            {
                var id = await facade.QueryFirstOrDefaultAsync<int?>(@"SELECT TOP 1 Id FROM Tenants WITH(NOLOCK) WHERE AssignmentKey = @AssignmentKey", request, null, cancellationToken);
                if (id == null)
                {
                    throw new NotFoundException($"Tenant not found for assignment key '{request.AssignmentKey}'.");
                }
                return (int)id;
            }
        }
    }
}
