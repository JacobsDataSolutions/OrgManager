// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.System.Commands.SeedInitialData
{
    public class SeedInitialDataCommand : IRequest
    {
        public class SeedSampleDataCommandHandler : IRequestHandler<SeedInitialDataCommand>
        {
            private readonly IOrgManagerDbContext context;

            private readonly ILogger logger;

            public SeedSampleDataCommandHandler(IOrgManagerDbContext context, ILogger<SeedSampleDataCommandHandler> logger)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Unit> Handle(SeedInitialDataCommand request, CancellationToken cancellationToken)
            {
                var seeder = new InitialDataSeeder(context, logger);

                await seeder.SeedAllAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}