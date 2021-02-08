// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JDS.OrgManager.Persistence.Common.Employees
{
    public class EmployeeEntityConfiguration : ConfigurationBase<EmployeeEntity>
    {
        public override void Configure(EntityTypeBuilder<EmployeeEntity> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => new { e.TenantId, e.Id });
            builder.Property(e => e.TenantId).ValueGeneratedNever();
            builder.Property(e => e.Id).UseIdentityColumn().ValueGeneratedOnAdd();
            builder.Property(e => e.Address1).HasMaxLength(Lengths.Address1).IsRequired();
            builder.Property(e => e.Address2).HasMaxLength(Lengths.Address2);
            builder.Property(e => e.City).HasMaxLength(Lengths.City).IsRequired();
            builder.Property(e => e.CurrencyCode).HasMaxLength(Lengths.CurrencyCode).IsRequired();
            builder.Property(e => e.FirstName).HasMaxLength(Lengths.FirstName).IsRequired();
            builder.Property(e => e.LastName).HasMaxLength(Lengths.LastName).IsRequired();
            builder.Property(e => e.MiddleName).HasMaxLength(Lengths.MiddleName);
            builder.Property(e => e.PtoHoursRemaining).HasColumnType(PersistenceLayerConstants.SqlDecimalType);
            builder.Property(e => e.Salary).HasColumnType(PersistenceLayerConstants.SqlDecimalType);
            builder.Property(e => e.SocialSecurityNumber).HasMaxLength(Lengths.SocialSecurityNumber);
            builder.Property(e => e.State).HasMaxLength(Lengths.State).IsRequired();
            builder.Property(e => e.ZipCode).HasMaxLength(Lengths.ZipCode).IsRequired();

            builder.Property(e => e.DateOfBirth).HasColumnType(PersistenceLayerConstants.SqlDateType);
            builder.Property(e => e.DateHired).HasColumnType(PersistenceLayerConstants.SqlDateType);
            builder.Property(e => e.DateTerminated).HasColumnType(PersistenceLayerConstants.SqlDateType);

            builder.HasOne(e => e.Currency).WithMany().OnDelete(DeleteBehavior.NoAction).HasForeignKey(e => e.CurrencyCode);
            builder.HasOne(e => e.Tenant).WithMany(e => e.Employees).HasForeignKey(e => e.TenantId);
            builder.HasOne(e => e.PaidTimeOffPolicy).WithMany().OnDelete(DeleteBehavior.NoAction).HasForeignKey(e => new { e.TenantId, e.PaidTimeOffPolicyId });
        }
    }
}