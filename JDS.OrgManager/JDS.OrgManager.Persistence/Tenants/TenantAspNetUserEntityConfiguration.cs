using JDS.OrgManager.Application.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

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
