using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Common.TimeOff;
using JDS.OrgManager.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Persistence.Common.TimeOff
{
    public class PaidTimeOffRequestEntityConfiguration : ConfigurationBase<PaidTimeOffRequestEntity>
    {
        public override void Configure(EntityTypeBuilder<PaidTimeOffRequestEntity> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.TenantId).ValueGeneratedNever();
            builder.Property(e => e.Id).UseIdentityColumn().ValueGeneratedOnAdd();
            builder.Property(e => e.EndDate).HasColumnType(PersistenceLayerConstants.SqlDateType);
            builder.Property(e => e.StartDate).HasColumnType(PersistenceLayerConstants.SqlDateType);
            builder.Property(e => e.Notes).HasMaxLength(Lengths.Notes);

            builder.HasOne(e => e.ForEmployee).WithMany(e => e.ForPaidTimeOffRequests).OnDelete(DeleteBehavior.NoAction).HasForeignKey(e => new { e.TenantId, e.ForEmployeeId });
            builder.HasOne(e => e.SubmittedBy).WithMany(e => e.SubmittedPaidTimeOffRequests).OnDelete(DeleteBehavior.NoAction).HasForeignKey(e => new { e.TenantId, e.SubmittedById });
            builder.HasOne(e => e.Tenant).WithMany(e => e.PaidTimeOffRequests).HasForeignKey(e => e.TenantId);
        }
    }
}
