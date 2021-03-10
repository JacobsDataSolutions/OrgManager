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

namespace JDS.OrgManager.Application.Customers.Queries.GetCustomer
{
    public class GetCustomerQuery : IRequest<CustomerViewModel?>
    {
        public int AspNetUsersId { get; set; }

        public class GetEmployeeListQueryHandler : IRequestHandler<GetCustomerQuery, CustomerViewModel?>
        {
            private readonly IApplicationReadDbFacade facade;

            public GetEmployeeListQueryHandler(IApplicationReadDbFacade facade) => this.facade = facade ?? throw new ArgumentNullException(nameof(facade));

            public async Task<CustomerViewModel?> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
            {
                var customer = await facade.QueryFirstOrDefaultAsync<CustomerViewModel?>(@"SELECT
                      c.[Id]
                      ,[Address1]
                      ,[Address2]
                      ,[City]
                      ,[CompanyName]
                      ,[CurrencyCode]
                      ,[FirstName]
                      ,[LastName]
                      ,[MiddleName]
                      ,[State]
                      ,[Title]
                      ,[ZipCode]
                      ,[AspNetUsersId]
                  FROM [dbo].[Customers] c WITH(NOLOCK)
				  JOIN [dbo].[AspNetUsers] u WITH(NOLOCK) ON u.Id = c.AspNetUsersId
                  WHERE AspNetUsersId = @AspNetUsersId AND u.IsCustomer = 1", request, cancellationToken: cancellationToken);
                return customer;
            }
        }
    }
}