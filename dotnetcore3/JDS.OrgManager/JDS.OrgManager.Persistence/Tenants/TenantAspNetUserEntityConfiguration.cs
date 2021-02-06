// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JDS.OrgManager.Persistence.Tenants
{
    public class TenantAspNetUserEntityConfiguration : IEntityTypeConfiguration<TenantAspNetUserEntity>
    {
        public void Configure(EntityTypeBuilder<TenantAspNetUserEntity> builder)
        {
            builder.HasKey(e => new { e.TenantId, e.AspNetUsersId });
            builder.Property(e => e.TenantId).ValueGeneratedNever();
            builder.Property(e => e.AspNetUsersId);
            builder.HasOne(e => e.Tenant).WithMany(e => e.AspNetUsers).OnDelete(DeleteBehavior.Cascade).HasForeignKey(e => e.TenantId);
        }
    }
}