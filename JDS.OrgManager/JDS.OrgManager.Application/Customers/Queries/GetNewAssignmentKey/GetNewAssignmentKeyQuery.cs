using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Customers.Queries.GetNewAssignmentKey
{
    public class GetNewAssignmentKeyQuery : IRequest<Guid>
    {
        public int AspNetUsersId { get; set; }

        public class GetNewAssignmentKeyQueryHandler : IRequestHandler<GetNewAssignmentKeyQuery, Guid>
        {
            public Task<Guid> Handle(GetNewAssignmentKeyQuery request, CancellationToken cancellationToken) => Task.FromResult(Guid.NewGuid());
        }
    }
}
