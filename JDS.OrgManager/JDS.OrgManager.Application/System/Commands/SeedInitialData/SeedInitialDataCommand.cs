// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Tenants;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.System.Commands.SeedInitialData
{
    public class SeedInitialDataCommand : IRequest
    {
        public IEnumerable<TenantViewModel> Tenants { get; set; } = Enumerable.Empty<TenantViewModel>();

        public class SeedInitialDataCommandHandler : IRequestHandler<SeedInitialDataCommand>
        {
            private readonly IApplicationWriteDbContext context;

            private readonly ILogger logger;

            private readonly IApplicationWriteDbFacade queryFacade;

            private readonly IViewModelToDbEntityMapper<TenantViewModel, TenantEntity> tenantViewModelToDbEntityMapper;

            public SeedInitialDataCommandHandler(
                IApplicationWriteDbContext context,
                IApplicationWriteDbFacade queryFacade,
                ILogger<SeedInitialDataCommandHandler> logger,
                IViewModelToDbEntityMapper<TenantViewModel, TenantEntity> tenantViewModelToDbEntityMapper)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.queryFacade = queryFacade ?? throw new ArgumentNullException(nameof(queryFacade));
                this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
                this.tenantViewModelToDbEntityMapper = tenantViewModelToDbEntityMapper ?? throw new ArgumentNullException(nameof(tenantViewModelToDbEntityMapper));
            }

            public async Task<Unit> Handle(SeedInitialDataCommand request, CancellationToken cancellationToken)
            {
                var seeder = new InitialDataSeeder(context, queryFacade, logger, tenantViewModelToDbEntityMapper);

                await seeder.SeedAllAsync(request.Tenants);

                return Unit.Value;
            }
        }
    }
}