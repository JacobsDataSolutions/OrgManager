// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using Dapper;
using JDS.OrgManager.Application.Abstractions.DbQueryFacades;
using JDS.OrgManager.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Persistence.DbQueryFacades
{
    public class DapperDbQueryFacade : IOrgManagerDbQueryFacade
    {
        private readonly OrgManagerDbContext context;

        public DapperDbQueryFacade(OrgManagerDbContext context) => this.context = context ?? throw new ArgumentNullException(nameof(context));

        public Task<int> ExecuteAsync(string sql, object param = null, CancellationToken cancellationToken = default) => context.Database.GetDbConnection().ExecuteAsync(sql, param);

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, CancellationToken cancellationToken = default) => (await context.Database.GetDbConnection().QueryAsync<T>(sql, param)).AsList();

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CancellationToken cancellationToken = default) => context.Database.GetDbConnection().QueryFirstOrDefaultAsync<T>(sql, param);

        public Task<T> QuerySingleAsync<T>(string sql, object param = null, CancellationToken cancellationToken = default) => context.Database.GetDbConnection().QuerySingleAsync<T>(sql, param);
    }
}