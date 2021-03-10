// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Common.TimeOff;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using MediatR;
using System;
using System.Linq;
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

            private readonly IDbEntityToDomainEntityMapper<PaidTimeOffRequestEntity, PaidTimeOffRequest> dbEntityMapper;

            private readonly IDomainEntityToViewModelMapper<PaidTimeOffRequest, PaidTimeOffRequestViewModel> domainEntityMapper;

            private readonly PaidTimeOffRequestService paidTimeOffRequestService;

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