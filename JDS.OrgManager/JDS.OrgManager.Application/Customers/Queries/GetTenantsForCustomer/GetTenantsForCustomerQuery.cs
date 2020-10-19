using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Tenants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Customers.Queries.GetTenantsForCustomer
{
    public class GetTenantsForCustomerQuery : IRequest<TenantViewModel[]>
    {
        public int AspNetUsersId { get; set; }

        public class GetTenantsForCustomerQueryHandler : IRequestHandler<GetTenantsForCustomerQuery, TenantViewModel[]>
        {
            private readonly IApplicationReadDbFacade facade;

            public GetTenantsForCustomerQueryHandler(IApplicationReadDbFacade facade)
            {
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
            }

            public async Task<TenantViewModel[]> Handle(GetTenantsForCustomerQuery request, CancellationToken cancellationToken) =>
                (await facade.QueryAsync<TenantViewModel>(
                    @"SELECT t.Id, t.AssignmentKey, t.Name, t.Slug FROM Tenants t WITH(NOLOCK)
                    JOIN Customers c WITH(NOLOCK) ON t.CustomerId = c.Id
                    WHERE c.AspNetUsersId = @AspNetUsersId
                    ", request)).ToArray();
        }
    }
}
