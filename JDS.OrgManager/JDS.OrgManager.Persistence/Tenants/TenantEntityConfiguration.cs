using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Tenants;
using JDS.OrgManager.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

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
