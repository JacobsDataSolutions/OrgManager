// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbFacades;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Tenants.Queries.GetTenant
{
    public class GetTenantQuery : IRequest<TenantViewModel>
    {
        public int TenantId { get; set; }

        public class GetTenantQueryHandler : IRequestHandler<GetTenantQuery, TenantViewModel>
        {
            private readonly IApplicationReadDbFacade queryFacade;

            public GetTenantQueryHandler(IApplicationReadDbFacade queryFacade)
            {
                this.queryFacade = queryFacade ?? throw new ArgumentNullException(nameof(queryFacade));
            }

            public Task<TenantViewModel> Handle(GetTenantQuery request, CancellationToken cancellationToken) =>
                queryFacade.QuerySingleAsync<TenantViewModel>(@"SELECT TOP (1) [Id]
                      ,[AssignmentKey]
                      ,[Name]
                      ,[Slug]
                  FROM [Tenants] WITH(NOLOCK) WHERE Id = @TenantId", request, cancellationToken: cancellationToken);
        }
    }
}