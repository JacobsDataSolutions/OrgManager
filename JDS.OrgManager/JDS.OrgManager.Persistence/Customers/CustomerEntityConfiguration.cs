using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Customers;
using JDS.OrgManager.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

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
            builder.Property(e => e.Zip).HasMaxLength(Lengths.Zip).IsRequired();

            builder.HasOne(e => e.Currency).WithMany().OnDelete(DeleteBehavior.NoAction).HasForeignKey(e => e.CurrencyCode);
        }
    }
}
