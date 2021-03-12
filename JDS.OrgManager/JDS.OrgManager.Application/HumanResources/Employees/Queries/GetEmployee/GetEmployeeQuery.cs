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

namespace JDS.OrgManager.Application.HumanResources.Employees.Queries.GetEmployee
{
    public class GetEmployeeQuery : IRequest<EmployeeViewModel>
    {
        public int AspNetUsersId { get; set; }

        public class GetEmployeeDetailQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeViewModel>
        {
            private readonly IApplicationReadDbFacade queryFacade;

            public GetEmployeeDetailQueryHandler(IApplicationReadDbFacade queryFacade)
            {
                this.queryFacade = queryFacade ?? throw new ArgumentNullException(nameof(queryFacade));
            }

            public Task<EmployeeViewModel> Handle(GetEmployeeQuery request, CancellationToken cancellationToken) =>
                queryFacade.QuerySingleAsync<EmployeeViewModel>(@"
                    SELECT TOP 1
                        t.AssignmentKey,
                        Address1,
                        Address2,
                        City,
                        CurrencyCode,
                        DateTerminated,
                        DateHired,
                        DateOfBirth,
                        EmployeeLevel,
                        FirstName,
                        Gender,
                        e.Id,
                        LastName,
                        MiddleName,
                        PaidTimeOffPolicyId,
                        PtoHoursRemaining,
                        Salary,
                        State,
                        ZipCode,
                        SocialSecurityNumber
                        FROM Employees e
                        JOIN [dbo].[AspNetUsers] u WITH(NOLOCK) ON u.Id = e.AspNetUsersId
                        JOIN [dbo].[Tenants] t WITH(NOLOCK) ON e.TenantId = t.Id
                        WHERE AspNetUsersId = @AspNetUsersId", request);
        }
    }
}