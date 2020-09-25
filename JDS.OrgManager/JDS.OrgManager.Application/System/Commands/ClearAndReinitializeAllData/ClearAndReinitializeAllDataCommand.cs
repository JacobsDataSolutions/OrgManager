// Copyright ©2020 Jacobs Data Solutions

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

namespace JDS.OrgManager.Application.System.Commands.ClearAndReinitializeAllData
{
    public class ClearAndReinitializeAllDataCommand : IRequest
    {
        public class ClearAndReinitializeAllDataCommandHandler : IRequestHandler<ClearAndReinitializeAllDataCommand>
        {
            private readonly IApplicationWriteDbFacade queryFacade;

            public ClearAndReinitializeAllDataCommandHandler(IApplicationWriteDbFacade queryFacade)
            {
                this.queryFacade = queryFacade ?? throw new ArgumentNullException(nameof(queryFacade));
            }

            public async Task<Unit> Handle(ClearAndReinitializeAllDataCommand request, CancellationToken cancellationToken)
            {
                await queryFacade.ExecuteAsync(@"
                    DELETE FROM EmployeeManagers
                    DELETE FROM Employees
                    IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = 'Employees' AND last_value IS NOT NULL) DBCC CHECKIDENT (Employees, RESEED, 0);");
                return Unit.Value;
            }
        }
    }
}