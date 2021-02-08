// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Customers;
using JDS.OrgManager.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JDS.OrgManager.Persistence.Customers
{
    public class CustomerEntityConfiguration : ConfigurationBase<CustomerEntity>
    {
        public override void Configure(EntityTypeBuilder<CustomerEntity> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn().ValueGeneratedOnAdd();
            builder.Property(e => e.Address1).HasMaxLength(Lengths.Address1).IsRequired();
            builder.Property(e => e.Address2).HasMaxLength(Lengths.Address2);
            builder.Property(e => e.AspNetUsersId);
            builder.Property(e => e.City).HasMaxLength(Lengths.City).IsRequired();
            builder.Property(e => e.CurrencyCode).HasMaxLength(Lengths.CurrencyCode).IsRequired();
            builder.Property(e => e.FirstName).HasMaxLength(Lengths.FirstName).IsRequired();
            builder.Property(e => e.LastName).HasMaxLength(Lengths.LastName).IsRequired();
            builder.Property(e => e.MiddleName).HasMaxLength(Lengths.MiddleName);
            builder.Property(e => e.State).HasMaxLength(Lengths.State).IsRequired();
            builder.Property(e => e.ZipCode).HasMaxLength(Lengths.ZipCode).IsRequired();

            builder.HasOne(e => e.Currency).WithMany().OnDelete(DeleteBehavior.NoAction).HasForeignKey(e => e.CurrencyCode);
        }
    }
}