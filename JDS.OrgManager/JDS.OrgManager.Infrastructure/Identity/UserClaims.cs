// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application;
using JDS.OrgManager.Application.Tenants;
using System.Linq;

namespace JDS.OrgManager.Infrastructure.Identity
{
    public class UserClaims
    {
        public int AspNetUsersId { get; set; }

        public int[] AuthorizedTenantIds => (from t in TenantEmployees select t.TenantId).ToArray();

        public bool IsCustomer { get; set; }

        public TenantEmployeeIdentityModel[] TenantEmployees { get; set; } = new TenantEmployeeIdentityModel[0];

        public string UserName { get; set; }

        public int? GetEmployeeId(int tenantId)
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