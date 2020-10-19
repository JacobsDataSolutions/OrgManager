using JDS.OrgManager.Application.Abstractions.DbFacades;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Tenants.Queries.GetAuthorizedTenantsForUser
{
    public class GetAuthorizedTenantsForUserQuery : IRequest<int[]>
    {
        public int AspNetUsersId { get; set; }

        public class GetAuthorizedTenantsForUserQueryHandler : IRequestHandler<GetAuthorizedTenantsForUserQuery, int[]>
        {
            private readonly IApplicationReadDbFacade facade;

            public GetAuthorizedTenantsForUserQueryHandler(IApplicationReadDbFacade facade)
            {
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
            }

            public async Task<int[]> Handle(GetAuthorizedTenantsForUserQuery request, CancellationToken cancellationToken)
                => (await facade.QueryAsync<int>("SELECT TenantId FROM TenantAspNetUsers WITH(NOLOCK) WHERE AspNetUsersId = @AspNetUsersId", request)).ToArray();
        }
    }
}
