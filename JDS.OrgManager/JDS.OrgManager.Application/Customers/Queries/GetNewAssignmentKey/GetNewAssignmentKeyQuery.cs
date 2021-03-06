﻿// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Customers.Queries.GetNewAssignmentKey
{
    public class GetNewAssignmentKeyQuery : IRequest<Guid>
    {
        public int AspNetUsersId { get; set; }

        public class GetNewAssignmentKeyQueryHandler : IRequestHandler<GetNewAssignmentKeyQuery, Guid>
        {
            public Task<Guid> Handle(GetNewAssignmentKeyQuery request, CancellationToken cancellationToken) => Task.FromResult(Guid.NewGuid());
        }
    }
}