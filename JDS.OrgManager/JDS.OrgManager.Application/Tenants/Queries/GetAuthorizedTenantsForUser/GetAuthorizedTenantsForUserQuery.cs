﻿// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbFacades;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Tenants.Queries.GetAuthorizedTenantsForUser
{
    public class GetAuthorizedTenantsForUserQuery : IRequest<int[]>
    {
        public int AspNetUsersId { get; set; }

        public class GetAuthorizedTenantsForUserQueryHandler : IRequestHandler<GetAuthorizedTenantsForUserQuery, int[]>
        {
            private readonly IApplicationReadDbFacade facade;

            public GetAuthorizedTenantsForUserQueryHandler(IApplicationReadDbFacade facade) => this.facade = facade ?? throw new ArgumentNullException(nameof(facade));

            public async Task<int[]> Handle(GetAuthorizedTenantsForUserQuery request, CancellationToken cancellationToken)
                => (await facade.QueryAsync<int>("SELECT TenantId FROM TenantAspNetUsers WITH(NOLOCK) WHERE AspNetUsersId = @AspNetUsersId", request, cancellationToken: cancellationToken)).ToArray();
        }
    }
}