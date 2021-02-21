using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Common.TimeOff;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    await context.Employees.FindAsync(new { request.TenantId, Id = (int)request.EmployeeId }) :
                    context.Employees.FirstOrDefault(e => e.AspNetUsersId == request.AspNetUsersId) ??
                    throw new NotFoundException("Invalid employee ID or ASP.NET user ID specified.");

                // PRESENTATION LAYER
                var vms = (from req in employee.ForPaidTimeOffRequests.OrderBy(r => r.StartDate) select mapper.Map(req)).ToArray();
                return vms;
            }
        }
    }
}
