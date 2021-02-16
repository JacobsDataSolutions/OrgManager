using JDS.OrgManager.Application.Abstractions.DbFacades;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Customers.Queries.GetCustomerId
{
    public class GetCustomerIdQuery : IRequest<int?>
    {
        public int AspNetUsersId { get; set; }

        public class GetCustomerIdQueryHandler : IRequestHandler<GetCustomerIdQuery, int?>
        {
            private readonly IApplicationReadDbFacade facade;

            public GetCustomerIdQueryHandler(IApplicationReadDbFacade facade)
            {
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
            }

            public Task<int?> Handle(GetCustomerIdQuery request, CancellationToken cancellationToken)
            {
                return facade.QueryFirstOrDefaultAsync<int?>("SELECT TOP 1 Id FROM Customers WITH(NOLOCK) WHERE AspNetUsersId = @AspNetUsersId", request);
            }
        }
    }
}