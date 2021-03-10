// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.TimeOff.Queries.ValidateRequestedPaidTimeOffHours
{
    public class ValidateRequestedPaidTimeOffHoursQuery : IRequest<PaidTimeOffRequestValidationResult>
    {
        public ValidateRequestedPaidTimeOffHoursViewModel ValidationRequest { get; set; } = default!;

        public class ValidateRequestedPaidTimeOffHoursQueryHandler : IRequestHandler<ValidateRequestedPaidTimeOffHoursQuery, PaidTimeOffRequestValidationResult>
        {
            private readonly IApplicationReadDbFacade facade;

            private readonly IViewModelToDomainEntityMapper<ValidateRequestedPaidTimeOffHoursViewModel, PaidTimeOffRequest> mapper;

            private readonly PaidTimeOffRequestService paidTimeOffRequestService;

            public ValidateRequestedPaidTimeOffHoursQueryHandler(
                IApplicationReadDbFacade facade,
                PaidTimeOffRequestService paidTimeOffRequestService,
                IViewModelToDomainEntityMapper<ValidateRequestedPaidTimeOffHoursViewModel, PaidTimeOffRequest> mapper)
            {
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
                this.paidTimeOffRequestService = paidTimeOffRequestService ?? throw new ArgumentNullException(nameof(paidTimeOffRequestService));
                this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<PaidTimeOffRequestValidationResult> Handle(ValidateRequestedPaidTimeOffHoursQuery request, CancellationToken cancellationToken)
            {
                var tentativeRequest = mapper.Map(request.ValidationRequest);
                var today = DateTime.Today;

                var paidTimeOffPolicy = await facade.QueryFirstOrDefaultAsync<PaidTimeOffPolicy>(@"
                    SELECT TOP 1 p.[Id]
                          ,p.[AllowsUnlimitedPto]
                          ,p.[EmployeeLevel]
                          ,p.[IsDefaultForEmployeeLevel]
                          ,p.[MaxPtoHours]
                          ,p.[Name]
                          ,p.[PtoAccrualRate]
                      FROM PaidTimeOffPolicies p WITH(NOLOCK)
                      JOIN Employees e WITH(NOLOCK) ON e.PaidTimeOffPolicyId = p.Id AND e.TenantId = p.TenantId
                      WHERE e.Id = @ForEmployeeId AND e.TenantId = @TenantId
                ", request.ValidationRequest, cancellationToken: cancellationToken);
                if (paidTimeOffPolicy == null)
                {
                    throw new NotFoundException("PTO policy not found or invalid.");
                }
                var existingRequests = await facade.QueryAsync<PaidTimeOffRequest>(@"
                    SELECT [Id]
                          ,[ApprovalStatus]
                          ,[EndDate]
                          ,[ForEmployeeId]
                          ,[HoursRequested]
                          ,[StartDate]
                          ,[Paid]
                          ,[SubmittedById]
                      FROM [PaidTimeOffRequests] WITH(NOLOCK)
                      WHERE @ForEmployeeId = @ForEmployeeId AND TenantId = @TenantId
                ", request.ValidationRequest, cancellationToken: cancellationToken);

                return paidTimeOffRequestService.ValidatePaidTimeOffRequest(tentativeRequest, existingRequests, paidTimeOffPolicy, today);
            }
        }
    }
}