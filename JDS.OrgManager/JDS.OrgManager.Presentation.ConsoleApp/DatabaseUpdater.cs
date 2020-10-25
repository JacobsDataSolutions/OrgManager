// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.System;
using JDS.OrgManager.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.ConsoleApp
{
    public class DatabaseUpdater
    {
        private readonly DummyDataInserter dummyDataInserter;

        private readonly IServiceProvider services;

        public DatabaseUpdater(IServiceProvider services, DummyDataInserter dummyDataInserter)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.dummyDataInserter = dummyDataInserter ?? throw new ArgumentNullException(nameof(dummyDataInserter));
        }

        public async Task UpdateDatabaseAsync(bool insertDummyData = false)
        {
            var dataInitializer = services.GetRequiredService<DataInitializerService>();
            await dataInitializer.InitializeDataForSystemAsync();
            if (insertDummyData)
            {
                await dummyDataInserter.InsertDummyDataAsync();
            }
        }
    }
}