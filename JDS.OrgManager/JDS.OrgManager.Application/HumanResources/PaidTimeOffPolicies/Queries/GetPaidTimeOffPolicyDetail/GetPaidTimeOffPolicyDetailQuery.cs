// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbQueryFacades;
using JDS.OrgManager.Application.Abstractions.Queries;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.PaidTimeOffPolicies.Queries.GetPaidTimeOffPolicyDetail
{
    public class GetPaidTimeOffPolicyDetailQuery : IRequest<GetPaidTimeOffPolicyDetailViewModel>, ICacheableQuery
    {
        public bool BypassCache { get; set; }

        public string CacheKey => nameof(GetPaidTimeOffPolicyDetailQuery);

        public int Id { get; set; }

        public bool RefreshCachedEntry { get; set; }

        public bool ReplaceCachedEntry { get; set; }

        public class GetPaidTimeOffPolicyDetailQueryHandler : IRequestHandler<GetPaidTimeOffPolicyDetailQuery, GetPaidTimeOffPolicyDetailViewModel>
        {
            private readonly IOrgManagerDbQueryFacade queryFacade;

            public GetPaidTimeOffPolicyDetailQueryHandler(IOrgManagerDbQueryFacade queryFacade)
            {
                this.queryFacade = queryFacade ?? throw new ArgumentNullException(nameof(queryFacade));
            }

            public Task<GetPaidTimeOffPolicyDetailViewModel> Handle(GetPaidTimeOffPolicyDetailQuery request, CancellationToken cancellationToken) =>
                queryFacade.QuerySingleAsync<GetPaidTimeOffPolicyDetailViewModel>("SELECT TOP 1 AllowsUnlimitedPto, EmployeeLevel, Id, IsDefaultForEmployeeLevel, MaxPtoHours, Name, PtoAccrualRate FROM PaidTimeOffPolicies WITH(NOLOCK) WHERE Id = @Id", request);
        }
    }
}