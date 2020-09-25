// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using Dapper;
using JDS.OrgManager.Application.Abstractions.DbFacades;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Persistence.DbFacades
{
    public class ApplicationReadDbFacade : IApplicationReadDbFacade, IDisposable
    {
        private readonly IDbConnection connection;

        private bool disposedValue = false;

        public ApplicationReadDbFacade(IConfiguration configuration)
        {
            connection = new SqlConnection(configuration.GetConnectionString(PersistenceLayerConstants.ReadDatabaseConnectionStringName));
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above. GC.SuppressFinalize(this);
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
                    => (await connection.QueryAsync<T>(sql, param, transaction)).AsList();

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
            => await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);

        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
            => await connection.QuerySingleAsync<T>(sql, param, transaction);

        // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    connection.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources. ~ApplicationReadDbFacade() { // Do not change
        // this code. Put cleanup code in Dispose(bool disposing) above. Dispose(false); }
    }
}