// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Common.TimeOff;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.TimeOff.Queries.GetPaidTimeOffRequestsForEmployee
{
    public class GetPaidTimeOffRequestsForEmployeeQuery : IRequest<PaidTimeOffRequestViewModel[]>
    {
        public int AspNetUsersId { get; set; }

        public int? EmployeeId { get; set; }

        public int TenantId { get; set; }

        public class GetPaidTimeOffRequestsForEmployeeQueryHandler : IRequestHandler<GetPaidTimeOffRequestsForEmployeeQuery, PaidTimeOffRequestViewModel[]>
        {
            private readonly IApplicationWriteDbContext context;

            private readonly IDbEntityToViewModelMapper<PaidTimeOffRequestEntity, PaidTimeOffRequestViewModel> mapper;

            public GetPaidTimeOffRequestsForEmployeeQueryHandler(IApplicationWriteDbContext context, IDbEntityToViewModelMapper<PaidTimeOffRequestEntity, PaidTimeOffRequestViewModel> mapper)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<PaidTimeOffRequestViewModel[]> Handle(GetPaidTimeOffRequestsForEmployeeQuery request, CancellationToken cancellationToken)
            {
                // PERSISTENCE LAYER
                var employee = request.EmployeeId != null ?
                    await context.Employees.Include(e => e.ForPaidTimeOffRequests).FirstOrDefaultAsync(e => e.Id == (int)request.EmployeeId && e.TenantId == request.TenantId) :
                    context.Employees.Include(e => e.ForPaidTimeOffRequests).FirstOrDefault(e => e.AspNetUsersId == request.AspNetUsersId) ??
                    throw new NotFoundException("Invalid employee ID or ASP.NET user ID specified.");

                // PRESENTATION LAYER
                var vms = (from req in employee.ForPaidTimeOffRequests.OrderBy(r => r.StartDate) select mapper.Map(req)).ToArray();
                return vms;
            }
        }
    }
}