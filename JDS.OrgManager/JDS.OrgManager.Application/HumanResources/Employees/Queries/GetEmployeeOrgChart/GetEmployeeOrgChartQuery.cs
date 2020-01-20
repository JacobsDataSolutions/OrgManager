// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbQueryFacades;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.Employees.Queries.GetEmployeeOrgChart
{
    public class GetEmployeeOrgChartQuery : IRequest<IReadOnlyList<GetEmployeeOrgChartViewModel>>
    {
        #region Public Classes

        public class GetEmployeeOrgChartQueryHandler : IRequestHandler<GetEmployeeOrgChartQuery, IReadOnlyList<GetEmployeeOrgChartViewModel>>
        {
            #region Private Fields

            private readonly IOrgManagerDbQueryFacade queryFacade;

            #endregion

            #region Public Constructors

            public GetEmployeeOrgChartQueryHandler(IOrgManagerDbQueryFacade queryFacade) => this.queryFacade = queryFacade ?? throw new ArgumentNullException(nameof(queryFacade));

            #endregion

            #region Public Methods

            public async Task<IReadOnlyList<GetEmployeeOrgChartViewModel>> Handle(GetEmployeeOrgChartQuery request, CancellationToken cancellationToken)
            {
                var employees = (await queryFacade.QueryAsync<GetEmployeeOrgChartViewModel>(
                    @"SELECT
                        e.Id,
                        e.LastName,
                        e.FirstName,
                        e.MiddleName,
                        em.ManagerId
                        FROM Employees e WITH(NOLOCK)
                        LEFT JOIN EmployeeManagers em WITH(NOLOCK) ON em.EmployeeId = e.Id
                        ORDER BY e.EmployeeLevel DESC
                    ")).ToArray();
                var list = new List<GetEmployeeOrgChartViewModel>();
                for (var i = 0; i < employees.Length; i++)
                {
                    var current = employees[i];
                    if (current.ManagerId == null)
                    {
                        list.Add(current);
                    }
                    for (var j = i + 1; j < employees.Length; j++)
                    {
                        if (employees[j].ManagerId == current.Id)
                        {
                            current.Subordinates.Add(employees[j]);
                        }
                    }
                }
                return list;
            }

            #endregion
        }

        #endregion
    }
}