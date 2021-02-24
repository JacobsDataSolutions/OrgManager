using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Common.TimeOff;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.TimeOff.Commands.SubmitNewPaidTimeOffRequest
{
    public class SubmitNewPaidTimeOffRequestCommand : IRequest<SubmitNewPaidTimeOffRequestViewModel>
    {
        public int AspNetUsersId { get; set; }

        public int TenantId { get; set; }

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

                var submittedByEntity = await facade.QueryFirstOrDefaultAsync<EmployeeEntity>(@"SELECT TOP 1 * FROM Employees WITH(NOLOCK) WHERE AspNetUsersId = @AspNetUsersId AND TenantId = @TenantId", new { request.AspNetUsersId, request.TenantId });

                var forEmployeeEntity = 
                    timeOffRequestViewModel.ForEmployeeId == null ?
                    context.Employees.Include(e => e.PaidTimeOffPolicy).Include(e => e.ForPaidTimeOffRequests).FirstOrDefault(e => e.AspNetUsersId == request.AspNetUsersId && e.TenantId == request.TenantId) :
                    context.Employees.Include(e => e.PaidTimeOffPolicy).Include(e => e.ForPaidTimeOffRequests).FirstOrDefault(e => e.Id == timeOffRequestViewModel.ForEmployeeId && e.TenantId == request.TenantId)
                    ;

                // TODO: if submitting on behalf of another employee, use the domain layer to validate that they have the privileges to do so.

                if (
                    forEmployeeEntity == null ||
                    submittedByEntity == null)
                {
                    throw new ApplicationLayerException(@"Unable to query employees while trying to create time off request aggregate.");
                }

                // DOMAIN LAYER
                var paidTimeOffPolicy = mapper.MapDbEntityToDomainEntity<PaidTimeOffPolicyEntity, PaidTimeOffPolicy>(forEmployeeEntity.PaidTimeOffPolicy);
                var existingRequests = (from req in forEmployeeEntity.ForPaidTimeOffRequests select mapper.MapDbEntityToDomainEntity<PaidTimeOffRequestEntity, PaidTimeOffRequest>(req)).ToList();
                var tentativeRequest = mapper.MapViewModelToDomainEntity<SubmitNewPaidTimeOffRequestViewModel, PaidTimeOffRequest>(request.PaidTimeOffRequest);

                var validationResult = paidTimeOffRequestService.ValidatePaidTimeOffRequest(tentativeRequest, existingRequests, paidTimeOffPolicy, today);
                return null;
            }
        }
    }
}
