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
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.Employees.Queries.GetEmployeeList
{
    public class GetEmployeeListQuery : IRequest<IReadOnlyList<GetEmployeeListViewModel>>
    {
        #region Public Classes

        public class GetEmployeeListQueryHandler : IRequestHandler<GetEmployeeListQuery, IReadOnlyList<GetEmployeeListViewModel>>
        {
            #region Private Fields

            private readonly IOrgManagerDbQueryFacade queryFacade;

            #endregion

            #region Public Constructors

            public GetEmployeeListQueryHandler(IOrgManagerDbQueryFacade queryFacade)
            {
                this.queryFacade = queryFacade ?? throw new ArgumentNullException(nameof(queryFacade));
            }

            #endregion

            #region Public Methods

            public Task<IReadOnlyList<GetEmployeeListViewModel>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken) =>
                queryFacade.QueryAsync<GetEmployeeListViewModel>(
                    @"SELECT
                        e.Id,
                        e.LastName,
                        e.FirstName,
                        e.MiddleName,
                        e.Gender,
                        e.EmployeeLevel,
                        e.DateHired,
                        e.DateExited,
                        (SELECT COUNT(DISTINCT EmployeeId) FROM EmployeeManagers s WITH(NOLOCK) WHERE s.ManagerId = e.Id) AS NumSubordinates,
                        m.Id AS ManagerId,
                        m.LastName AS ManagerLastName,
                        m.FirstName AS ManagerFirstName,
                        m.MiddleName AS ManagerMiddleName
                        FROM Employees e WITH(NOLOCK)
                        LEFT JOIN EmployeeManagers em WITH(NOLOCK) ON em.EmployeeId = e.Id
                        LEFT JOIN Employees m WITH(NOLOCK) ON em.ManagerId = m.Id");

            #endregion
        }

        #endregion
    }
}