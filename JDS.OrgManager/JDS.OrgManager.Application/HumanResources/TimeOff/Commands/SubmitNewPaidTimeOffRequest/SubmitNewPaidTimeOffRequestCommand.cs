// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Common.TimeOff;
using JDS.OrgManager.Domain.HumanResources.Employees;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.TimeOff.Commands.SubmitNewPaidTimeOffRequest
{
    public class SubmitNewPaidTimeOffRequestCommand : IRequest<SubmitNewPaidTimeOffRequestViewModel>
    {
        public int AspNetUsersId { get; set; }

        public SubmitNewPaidTimeOffRequestViewModel PaidTimeOffRequest { get; set; } = default!;

        public class SubmitNewPaidTimeOffRequestCommandHandler : IRequestHandler<SubmitNewPaidTimeOffRequestCommand, SubmitNewPaidTimeOffRequestViewModel>
        {
            private readonly IApplicationWriteDbContext context;

            private readonly IApplicationWriteDbFacade facade;

            private readonly IModelMapper mapper;

            private readonly PaidTimeOffRequestService paidTimeOffRequestService;

            public SubmitNewPaidTimeOffRequestCommandHandler(
                IApplicationWriteDbContext context,
                IApplicationWriteDbFacade facade,
                PaidTimeOffRequestService paidTimeOffRequestService,
                IModelMapper mapper)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
                this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                this.paidTimeOffRequestService = paidTimeOffRequestService ?? throw new ArgumentNullException(nameof(paidTimeOffRequestService));
            }

            public async Task<SubmitNewPaidTimeOffRequestViewModel> Handle(SubmitNewPaidTimeOffRequestCommand request, CancellationToken cancellationToken)
            {
                var today = DateTime.Today;

                // PRESENTATION/APPLICATION LAYER
                var timeOffRequestViewModel = request.PaidTimeOffRequest;
                timeOffRequestViewModel.StartDate = timeOffRequestViewModel.StartDate.Date;
                timeOffRequestViewModel.EndDate = timeOffRequestViewModel.EndDate.Date;

                // PERSISTENCE LAYER

                var submittedByEntity = await facade.QueryFirstOrDefaultAsync<EmployeeEntity>(@"SELECT TOP 1 * FROM Employees WITH(NOLOCK) WHERE AspNetUsersId = @AspNetUsersId AND TenantId = @TenantId", new { request.AspNetUsersId, request.PaidTimeOffRequest.TenantId });

                var forEmployeeEntity =
                    timeOffRequestViewModel.ForEmployeeId == null ?
                    context.Employees.Include(e => e.PaidTimeOffPolicy).Include(e => e.ForPaidTimeOffRequests).FirstOrDefault(e => e.AspNetUsersId == request.AspNetUsersId && e.TenantId == request.PaidTimeOffRequest.TenantId) :
                    context.Employees.Include(e => e.PaidTimeOffPolicy).Include(e => e.ForPaidTimeOffRequests).FirstOrDefault(e => e.Id == timeOffRequestViewModel.ForEmployeeId && e.TenantId == request.PaidTimeOffRequest.TenantId)
                    ;

                // TODO: if submitting on behalf of another employee, use the domain layer to validate that they have the privileges to do so.

                if (
                    forEmployeeEntity == null ||
                    submittedByEntity == null)
                {
                    throw new ApplicationLayerException(@"Unable to query employees while trying to create time off request aggregate.");
                }

                // DOMAIN LAYER
                var forEmployee = mapper.MapDbEntityToDomainEntity<EmployeeEntity, Employee>(forEmployeeEntity);
                var submittedByEmployee = mapper.MapDbEntityToDomainEntity<EmployeeEntity, Employee>(submittedByEntity);
                var paidTimeOffPolicy = mapper.MapDbEntityToDomainEntity<PaidTimeOffPolicyEntity, PaidTimeOffPolicy>(forEmployeeEntity.PaidTimeOffPolicy);
                var existingRequests = (from req in forEmployeeEntity.ForPaidTimeOffRequests select mapper.MapDbEntityToDomainEntity<PaidTimeOffRequestEntity, PaidTimeOffRequest>(req)).ToList();

                // Build up the Domain aggregate entity so that complex business logic can be executed against it. In an enterprise (non-demo) solution this
                // would likely involve special rules involving accrued hours, whether the company allows going negative in PTO hours, managerial overrides,
                // etc. The point is that the entities, and the logic which operates against them, are separate from view models and database persistence models.
                var submittedRequest =
                    mapper.MapViewModelToDomainEntity<SubmitNewPaidTimeOffRequestViewModel, PaidTimeOffRequest>(request.PaidTimeOffRequest)
                    .WithForEmployee(forEmployee)
                    .WithSubmittedBy(submittedByEmployee)
                    .WithPaidTimeOffPolicy(paidTimeOffPolicy);

                // Ensure the aggregate is in a valid state with which to perform business logic.
                submittedRequest.ValidateAggregate();

                // Perform basic validation against the request to make sure that the employee is OK to submit this paid time off request. Once again, the logic
                // inside the service is naive and overly-simplistic. A real-life solution would be much more involved.
                var validationResult = paidTimeOffRequestService.ValidatePaidTimeOffRequest(submittedRequest, existingRequests, paidTimeOffPolicy, today);
                timeOffRequestViewModel.Result = validationResult;
                if (validationResult != PaidTimeOffRequestValidationResult.OK)
                {
                    return timeOffRequestViewModel;
                }

                // Submit the time off request (changes Domain state and creates event).
                submittedRequest = submittedRequest.Submit();

                // PERSISTENCE LAYER
                var paidTimeOffRequestEntity = mapper.MapDomainEntityToDbEntity<PaidTimeOffRequest, PaidTimeOffRequestEntity>(submittedRequest);
                paidTimeOffRequestEntity.TenantId = timeOffRequestViewModel.TenantId;
                await context.PaidTimeOffRequests.AddAsync(paidTimeOffRequestEntity, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                // PRESENTATION LAYER
                request.PaidTimeOffRequest.CreatedPaidTimeOffRequest = mapper.MapDomainEntityToViewModel<PaidTimeOffRequest, PaidTimeOffRequestViewModel>(submittedRequest);

                // DISPATCH DOMAIN EVENTS
                await submittedRequest.DispatchDomainEventsAsync();

                return request.PaidTimeOffRequest;
            }
        }
    }
}