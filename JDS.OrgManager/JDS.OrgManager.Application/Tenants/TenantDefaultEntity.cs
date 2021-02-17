using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Common.Currencies;
using JDS.OrgManager.Application.Common.TimeOff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Tenants
{
    public class TenantDefaultEntity : AuditableDbEntity
    {
        public CurrencyEntity Currency { get; set; } = default!;

        public string CurrencyCode { get; set; } = default!;

        public int EmployeeLevel { get; set; }

        public TenantEntity Tenant { get; set; } = default!;

        public int TenantId { get; set; }

        public PaidTimeOffPolicyEntity PaidTimeOffPolicy { get; set; } = default!;

        public int PaidTimeOffPolicyId { get; set; }

    }
}
