using JDS.OrgManager.Application.System;
using JDS.OrgManager.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.WebApi
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
