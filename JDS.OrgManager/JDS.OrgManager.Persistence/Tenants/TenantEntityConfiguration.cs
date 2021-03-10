// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Tenants;
using JDS.OrgManager.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JDS.OrgManager.Persistence.Tenants
{
    public class TenantEntityConfiguration : ConfigurationBase<TenantEntity>
    {
        public override void Configure(EntityTypeBuilder<TenantEntity> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn().ValueGeneratedOnAdd();
            builder.Property(e => e.Name).HasMaxLength(Lengths.Name).IsRequired();
            builder.Property(e => e.Slug).HasMaxLength(Lengths.Slug).IsRequired();
            builder.HasOne(e => e.Customer).WithMany(e => e.Tenants).HasForeignKey(e => e.CustomerId);
        }
    }
}