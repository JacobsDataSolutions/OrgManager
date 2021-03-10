// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using Dapper;
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.DbFacades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Persistence.DbFacades
{
    public class ApplicationWriteDbFacade : IApplicationWriteDbFacade
    {
        // This db facade implementation is meant to receive a reference to the EF db context so that the dapper queries operate against the same SQL connection
        // and can participate in transactions alongside calls to EF. For example, you might want to execute commands to turn off identity increment in the same
        // transaction in which you are seeding the DB using EF.
        private readonly IApplicationWriteDbContext context;

        public ApplicationWriteDbFacade(IApplicationWriteDbContext context) => this.context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
            => await context.Connection.ExecuteAsync(sql, param, transaction);

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
            => (await context.Connection.QueryAsync<T>(sql, param, transaction)).AsList();

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
            => await context.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);

        public async Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
            => await context.Connection.QuerySingleAsync<T>(sql, param, transaction);
    }
}