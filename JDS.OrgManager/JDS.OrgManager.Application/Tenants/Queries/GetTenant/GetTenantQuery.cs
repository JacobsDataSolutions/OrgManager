using JDS.OrgManager.Application.Abstractions.DbFacades;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Tenants.Queries.GetTenant
{
    public class GetTenantQuery : IRequest<TenantViewModel>
    {
        public int TenantId { get; set; }

        public class GetTenantQueryHandler : IRequestHandler<GetTenantQuery, TenantViewModel>
        {
            private readonly IApplicationReadDbFacade queryFacade;

            public GetTenantQueryHandler(IApplicationReadDbFacade queryFacade)
            {
                this.queryFacade = queryFacade ?? throw new ArgumentNullException(nameof(queryFacade));
            }

            public Task<TenantViewModel> Handle(GetTenantQuery request, CancellationToken cancellationToken) =>
                queryFacade.QuerySingleAsync<TenantViewModel>(@"SELECT TOP (1) [Id]
                      ,[AssignmentKey]
                      ,[Name]
                      ,[Slug]
                  FROM [Tenants] WITH(NOLOCK) WHERE Id = @TenantId", request, cancellationToken: cancellationToken);
        }
    }
}
