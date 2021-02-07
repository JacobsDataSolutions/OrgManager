// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.DbFacades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable IDE0060 // Remove unused parameter

namespace JDS.OrgManager.Application.System
{
    public static class ApplicationWriteDbFacadeExtensions
    {
        private static readonly string[] tablesOrderedByRelation = new[]
        {
            nameof(IApplicationWriteDbContext.EmployeeManagers),
            nameof(IApplicationWriteDbContext.Employees),
            nameof(IApplicationWriteDbContext.PaidTimeOffPolicies),
            nameof(IApplicationWriteDbContext.TenantAspNetUsers),
            nameof(IApplicationWriteDbContext.Tenants),
            nameof(IApplicationWriteDbContext.Customers),
            nameof(IApplicationWriteDbContext.Currencies)
        };

        public static Task ClearAllTablesAsync(this IApplicationWriteDbFacade facade, IDbTransaction? transaction = null)
            => facade.ExecuteAsync(string.Join(Environment.NewLine, from t in tablesOrderedByRelation select GetDeleteStatement(t)), transaction: transaction);

        public static string[] GetTenantTables(this IApplicationWriteDbFacade facade)
        {
            var tenantTables =
            (from prop in typeof(IApplicationWriteDbContext).GetProperties()
             where prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)
             && prop.PropertyType.GetGenericArguments()[0].GetProperties().Any(p => p.Name == "TenantId")
             select prop.Name).ToArray();
            return (from t in tablesOrderedByRelation where tenantTables.Contains(t) select t).ToArray();
        }

        public static Task SetIdentitySeedAsync(this IApplicationWriteDbFacade facade, string tableName, int seedValue, IDbTransaction? transaction = null)
                     => facade.ExecuteAsync(GetReseedStatement(tableName, seedValue), transaction: transaction);

        public static Task SetIdentitySeedForAllTablesAsync(this IApplicationWriteDbFacade facade, int seedValue, IDbTransaction? transaction = null)
                    => facade.ExecuteAsync(string.Join(Environment.NewLine, from t in tablesOrderedByRelation select GetReseedStatement(t, seedValue)), transaction: transaction);

        public static Task TurnOffIdentityIncrementAsync(this IApplicationWriteDbFacade facade, string tableName, IDbTransaction? transaction = null)
            => facade.ExecuteAsync(GetIdentityIncrementStatement(tableName, true), transaction: transaction);

        public static Task TurnOnIdentityIncrementAsync(this IApplicationWriteDbFacade facade, string tableName, IDbTransaction? transaction = null)
            => facade.ExecuteAsync(GetIdentityIncrementStatement(tableName, false), transaction: transaction);

        // Note: you cannot turn off identity increment for all tables. SQL server only lets you do this for one table at a time.

        private static string GetDeleteStatement(string tableName) => $"DELETE FROM {tableName};";

        private static string GetIdentityIncrementStatement(string tableName, bool on) => $"IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_SCHEMA + '.' + '{tableName}'), 'Id', 'IsIdentity') = 1) SET IDENTITY_INSERT {tableName} {(on ? "ON" : "OFF")}";

        private static string GetReseedStatement(string tableName, int seedValue) => $"IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = '{tableName}' AND last_value IS NOT NULL) DBCC CHECKIDENT ({tableName}, RESEED, {seedValue});";
    }
}

#pragma warning restore IDE0060 // Remove unused parameter