// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JDS.OrgManager.Persistence.Common.Employees
{
    public class EmployeeEntityConfiguration : ConfigurationBase<EmployeeEntity>
    {
        #region Public Methods

        public override void Configure(EntityTypeBuilder<EmployeeEntity> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.TenantId, e.Id });
            builder.Property(e => e.Id).UseIdentityColumn();
            builder.Property(e => e.Address1).HasMaxLength(50);
            builder.Property(e => e.Address2).HasMaxLength(15).IsRequired(false);
            builder.Property(e => e.City).HasMaxLength(30);
            builder.Property(e => e.CurrencyCode).HasMaxLength(3);
            builder.Property(e => e.FirstName).HasMaxLength(25);
            builder.Property(e => e.LastName).HasMaxLength(25);
            builder.Property(e => e.MiddleName).HasMaxLength(25).IsRequired(false);
            builder.Property(e => e.PtoHoursRemaining).HasColumnType("decimal(18,4)");
            builder.Property(e => e.Salary).HasColumnType("decimal(18,4)");
            builder.Property(e => e.SocialSecurityNumber).HasMaxLength(11);
            builder.Property(e => e.State).HasMaxLength(2);
            builder.Property(e => e.Zip).HasMaxLength(10);

            builder.HasOne(e => e.Currency).WithMany();
            builder.HasOne(e => e.PaidTimeOffPolicy).WithMany().HasForeignKey(e => new { e.TenantId, e.PaidTimeOffPolicyId });
        }

        #endregion
    }
}