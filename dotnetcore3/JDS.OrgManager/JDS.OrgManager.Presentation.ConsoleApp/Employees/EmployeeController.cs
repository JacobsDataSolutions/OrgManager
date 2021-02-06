// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.HumanResources.Employees.Commands.RegisterOrUpdateEmployee;
using JDS.OrgManager.Application.HumanResources.Employees.Commands.VerifyOrganization;
using JDS.OrgManager.Application.HumanResources.Employees.Queries.GetEmployeeDetail;
using JDS.OrgManager.Application.HumanResources.Employees.Queries.GetEmployeeList;
using JDS.OrgManager.Application.HumanResources.Employees.Queries.GetEmployeeOrgChart;
using JDS.OrgManager.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.ConsoleApp
{
    public class EmployeeController : BaseController
    {
        public EmployeeController(IMediator mediator) : base(mediator)
        { }

        public async Task<Result<GetEmployeeDetailViewModel>> GetEmployeeDetailsAsync(int id)
        {
            try
            {
                var result = await Mediator.Send(new GetEmployeeDetailQuery { Id = id });
                return Result<GetEmployeeDetailViewModel>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<GetEmployeeDetailViewModel>.Failure(new[] { ex.Message });
            }
        }

        public async Task<Result<IReadOnlyList<GetEmployeeListViewModel>>> GetEmployeeListAsync()
        {
            try
            {
                var result = await Mediator.Send(new GetEmployeeListQuery());
                return Result<IReadOnlyList<GetEmployeeListViewModel>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<IReadOnlyList<GetEmployeeListViewModel>>.Failure(new[] { ex.Message });
            }
        }

        public async Task<Result<IReadOnlyList<GetEmployeeOrgChartViewModel>>> GetEmployeeOrgChartAsync()
        {
            try
            {
                var result = await Mediator.Send(new GetEmployeeOrgChartQuery());
                return Result<IReadOnlyList<GetEmployeeOrgChartViewModel>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<IReadOnlyList<GetEmployeeOrgChartViewModel>>.Failure(new[] { ex.Message });
            }
        }

        public async Task<Result<RegisterOrUpdateEmployeeCommand>> RegisterOrUpdateEmployeeAsync(RegisterOrUpdateEmployeeCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Result<RegisterOrUpdateEmployeeCommand>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<RegisterOrUpdateEmployeeCommand>.Failure(new[] { ex.Message });
            }
        }

        public async Task<Result<VerifyOrganizationViewModel>> VerifyOrganizationAsync()
        {
            try
            {
                var result = await Mediator.Send(new VerifyOrganizationCommand());
                return Result<VerifyOrganizationViewModel>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<VerifyOrganizationViewModel>.Failure(new[] { ex.Message });
            }
        }
    }
}