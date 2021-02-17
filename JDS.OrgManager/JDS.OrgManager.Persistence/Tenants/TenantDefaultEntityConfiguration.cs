using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Tenants;
using JDS.OrgManager.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Persistence.Tenants
{
    public class TenantDefaultEntityConfiguration : ConfigurationBase<TenantDefaultEntity>
    {
        public override void Configure(EntityTypeBuilder<TenantDefaultEntity> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.TenantId);
            builder.Property(e => e.TenantId).ValueGeneratedNever();
            builder.Property(e => e.CurrencyCode).HasMaxLength(Lengths.CurrencyCode).IsRequired();
            builder.HasOne(e => e.Currency).WithMany().OnDelete(DeleteBehavior.NoAction).HasForeignKey(e => e.CurrencyCode);
            builder.HasOne(e => e.Tenant).WithOne(e => e.TenantDefaults).OnDelete(DeleteBehavior.NoAction).HasForeignKey<TenantDefaultEntity>(e => e.TenantId);
            builder.HasOne(e => e.PaidTimeOffPolicy).WithMany().OnDelete(DeleteBehavior.NoAction).HasForeignKey(e => new { e.TenantId, e.PaidTimeOffPolicyId });
        }
    }
}
