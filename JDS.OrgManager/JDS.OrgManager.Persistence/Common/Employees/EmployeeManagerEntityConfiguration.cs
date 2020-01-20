// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Common.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JDS.OrgManager.Persistence.Common.Employees
{
    public class EmployeeManagerEntityConfiguration : IEntityTypeConfiguration<EmployeeManagerEntity>
    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<EmployeeManagerEntity> builder)
        {
            builder.HasKey(e => new { e.TenantId, e.ManagerId, e.EmployeeId });
            builder.HasOne(e => e.Manager).WithMany(e => e.Subordinates).OnDelete(DeleteBehavior.NoAction).HasForeignKey(e => new { e.TenantId, e.ManagerId });
            builder.HasOne(e => e.Employee).WithMany(e => e.Managers).OnDelete(DeleteBehavior.NoAction).HasForeignKey(e => new { e.TenantId, e.EmployeeId });
        }

        #endregion
    }
}