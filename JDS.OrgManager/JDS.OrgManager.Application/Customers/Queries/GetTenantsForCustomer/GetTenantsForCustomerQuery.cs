// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Tenants;
using MediatR;
using System;
using System.Linq;
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

            public GetTenantsForCustomerQueryHandler(IApplicationReadDbFacade facade) => this.facade = facade ?? throw new ArgumentNullException(nameof(facade));

            public async Task<TenantViewModel[]> Handle(GetTenantsForCustomerQuery request, CancellationToken cancellationToken) =>
                (await facade.QueryAsync<TenantViewModel>(@"SELECT t.Id, t.AssignmentKey, t.Name, t.Slug FROM Tenants t WITH(NOLOCK)
                    JOIN Customers c WITH(NOLOCK) ON t.CustomerId = c.Id
                    WHERE c.AspNetUsersId = @AspNetUsersId
                    ", request, transaction: null, cancellationToken: cancellationToken)).ToArray();
        }
    }
}