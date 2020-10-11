using JDS.OrgManager.Application;
using JDS.OrgManager.Application.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JDS.OrgManager.Infrastructure.Identity
{
    public class UserClaims
    {
        public int AspNetUsersId { get; set; }

        public int[] AuthorizedTenantIds { get; set; } = new int[0];

        public bool IsCustomer { get; set; }

        public TenantEmployeeIdentityModel[] TenantEmployees { get; set; } = new TenantEmployeeIdentityModel[0];

        public string UserName { get; set; }

        public int GetEmployeeId(int tenantId)
        {
            var tenantEmployee = TenantEmployees.FirstOrDefault(t => t.TenantId == tenantId);
            if (tenantEmployee == null)
            {
                throw new AuthorizationException();
            }
            return tenantEmployee.EmployeeId;
        }
    }
}
