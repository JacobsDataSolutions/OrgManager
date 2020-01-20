// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.System.Commands.ClearAndReinitializeAllData;
using JDS.OrgManager.Application.System.Commands.SeedInitialData;
using JDS.OrgManager.Persistence.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.ConsoleApp
{
    public class DatabaseUpdater
    {
        #region Private Fields

        private readonly IServiceProvider services;

        #endregion

        #region Public Constructors

        public DatabaseUpdater(IServiceProvider services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion

        #region Public Methods

        public async Task UpdateDatabaseAsync()
        {
            var context = services.GetRequiredService<OrgManagerDbContext>();
            context.Database.Migrate();

            var mediator = services.GetRequiredService<IMediator>();
            await mediator.Send(new ClearAndReinitializeAllDataCommand(), CancellationToken.None);
            await mediator.Send(new SeedInitialDataCommand(), CancellationToken.None);
        }

        #endregion
    }
}