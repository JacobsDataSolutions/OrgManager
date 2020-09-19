using JDS.OrgManager.Application.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Application.Tenants
{
    public class TenantAspNetUserEntity : IDbEntity
    {
        public int AspNetUsersId { get; set; }

        public TenantEntity Tenant { get; set; }

        public int TenantId { get; set; }
    }
}
