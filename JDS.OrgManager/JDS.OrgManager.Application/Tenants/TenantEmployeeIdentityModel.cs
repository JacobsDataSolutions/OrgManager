using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Application.Tenants
{
    public class TenantEmployeeIdentityModel
    {
        public int? EmployeeId { get; set; }

        public int TenantId { get; set; }

        public override string ToString() => $"{TenantId}-{EmployeeId}";
    }
}
