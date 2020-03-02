// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbQueryFacades;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.Employees.Queries.GetEmployeeDetail
{
    public class GetEmployeeDetailQuery : IRequest<GetEmployeeDetailViewModel>
    {
        public int Id { get; set; }

        public class GetEmployeeDetailQueryHandler : IRequestHandler<GetEmployeeDetailQuery, GetEmployeeDetailViewModel>
        {
            private readonly IOrgManagerDbQueryFacade queryFacade;

            public GetEmployeeDetailQueryHandler(IOrgManagerDbQueryFacade queryFacade)
            {
                this.queryFacade = queryFacade ?? throw new ArgumentNullException(nameof(queryFacade));
            }

            public Task<GetEmployeeDetailViewModel> Handle(GetEmployeeDetailQuery request, CancellationToken cancellationToken) =>
                queryFacade.QuerySingleAsync<GetEmployeeDetailViewModel>(@"SELECT TOP 1 Address1, Address2, City, CurrencyCode, DateExited, DateHired, DateOfBirth, EmployeeLevel, FirstName, Gender, Id, LastName, MiddleName, PaidTimeOffPolicyId, PtoHoursRemaining, Salary, State, Zip FROM Employees WHERE Id = @Id", request);
        }
    }
}