// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
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