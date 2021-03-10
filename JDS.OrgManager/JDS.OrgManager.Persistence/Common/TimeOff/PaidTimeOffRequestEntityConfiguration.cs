// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Common.TimeOff;
using JDS.OrgManager.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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