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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.PaidTimeOffPolicies.Queries.GetPaidTimeOffPolicyList
{
    public class GetPaidTimeOffPolicyListQuery : IRequest<IReadOnlyList<GetPaidTimeOffPolicyListViewModel>>, ICacheableQuery
    {
        public bool BypassCache { get; set; }

        public string CacheKey => nameof(GetPaidTimeOffPolicyListQuery);

        public bool RefreshCachedEntry { get; set; }

        public bool ReplaceCachedEntry { get; set; }

        public class GetPaidTimeOffPolicyListQueryHandler : IRequestHandler<GetPaidTimeOffPolicyListQuery, IReadOnlyList<GetPaidTimeOffPolicyListViewModel>>
        {
            private readonly IOrgManagerDbQueryFacade queryFacade;

            public GetPaidTimeOffPolicyListQueryHandler(IOrgManagerDbQueryFacade queryFacade)
            {
                this.queryFacade = queryFacade ?? throw new ArgumentNullException(nameof(queryFacade));
            }

            public Task<IReadOnlyList<GetPaidTimeOffPolicyListViewModel>> Handle(GetPaidTimeOffPolicyListQuery request, CancellationToken cancellationToken) =>
                queryFacade.QueryAsync<GetPaidTimeOffPolicyListViewModel>("SELECT Id, Name, AllowsUnlimitedPto, EmployeeLevel, IsDefaultForEmployeeLevel FROM PaidTimeOffPolicies WITH(NOLOCK)");
        }
    }
}