// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Common.TimeOff;
using JDS.OrgManager.Application.Customers;
using System;
using System.Collections.Generic;

namespace JDS.OrgManager.Application.Tenants
{
    public class TenantEntity : AuditableDbEntity
    {
        public ICollection<TenantAspNetUserEntity> AspNetUsers { get; set; }

        public Guid AssignmentKey { get; set; }

        public CustomerEntity Customer { get; set; } = default!;

        public int CustomerId { get; set; }

        public ICollection<EmployeeEntity> Employees { get; set; }

        public int Id { get; set; }

        public ICollection<PaidTimeOffRequestEntity> PaidTimeOffRequests { get; set; }

        public string Name { get; set; } = default!;

        public string Slug { get; set; } = default!;

        public TenantDefaultEntity TenantDefaults { get; set; } = default!;

        public TenantEntity()
        {
            AspNetUsers = new HashSet<TenantAspNetUserEntity>();
            Employees = new HashSet<EmployeeEntity>();
            PaidTimeOffRequests = new HashSet<PaidTimeOffRequestEntity>();
        }
    }
}