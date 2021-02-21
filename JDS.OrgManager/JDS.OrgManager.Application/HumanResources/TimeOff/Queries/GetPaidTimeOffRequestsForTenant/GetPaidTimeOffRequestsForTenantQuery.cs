using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Common.TimeOff;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.TimeOff.Queries.GetPaidTimeOffRequestsForTenant
{
    public class GetPaidTimeOffRequestsForTenantQuery : IRequest<PaidTimeOffRequestViewModel[]>
    {
        public int TenantId { get; set; }

        public class GetPaidTimeOffRequestsForTenantQueryHandler : IRequestHandler<GetPaidTimeOffRequestsForTenantQuery, PaidTimeOffRequestViewModel[]>
        {
            private readonly IApplicationWriteDbContext context;
            private readonly PaidTimeOffRequestService paidTimeOffRequestService;
            private readonly IDbEntityToDomainEntityMapper<PaidTimeOffRequestEntity, PaidTimeOffRequest> dbEntityMapper;
            private readonly IDomainEntityToViewModelMapper<PaidTimeOffRequest, PaidTimeOffRequestViewModel> domainEntityMapper;

            public GetPaidTimeOffRequestsForTenantQueryHandler(
                IApplicationWriteDbContext context,
                PaidTimeOffRequestService paidTimeOffRequestService,
                IDbEntityToDomainEntityMapper<PaidTimeOffRequestEntity, PaidTimeOffRequest> dbEntityMapper,
                IDomainEntityToViewModelMapper<PaidTimeOffRequest, PaidTimeOffRequestViewModel> domainEntityMapper
                )
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.paidTimeOffRequestService = paidTimeOffRequestService ?? throw new ArgumentNullException(nameof(paidTimeOffRequestService));
                this.dbEntityMapper = dbEntityMapper ?? throw new ArgumentNullException(nameof(dbEntityMapper));
                this.domainEntityMapper = domainEntityMapper ?? throw new ArgumentNullException(nameof(domainEntityMapper));
            }

            public Task<PaidTimeOffRequestViewModel[]> Handle(GetPaidTimeOffRequestsForTenantQuery request, CancellationToken cancellationToken)
            {
                // PERSISTENCE
                var tenantRequestEntities = (from r in context.PaidTimeOffRequests where r.TenantId == request.TenantId select r).ToList();

                // DOMAIN
                var paidTimeOffRequests = (from r in tenantRequestEntities select dbEntityMapper.Map(r)).ToList();
                paidTimeOffRequests = paidTimeOffRequestService.GetPaidTimeOffRequestsWithStatusUpdates(paidTimeOffRequests, DateTime.Today);

                return Task.FromResult((from r in paidTimeOffRequests select domainEntityMapper.Map(r)).ToArray());
            }
        }
    }
}
