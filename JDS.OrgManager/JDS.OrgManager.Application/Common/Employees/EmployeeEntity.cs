// Copyright �2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Common.Addresses;
using JDS.OrgManager.Application.Common.Currencies;
using JDS.OrgManager.Application.Common.TimeOff;
using JDS.OrgManager.Application.Tenants;
using JDS.OrgManager.Domain.Common.People;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JDS.OrgManager.Application.Common.Employees
{
    [DebuggerDisplay("({Id}) {LastName}, {FirstName}")]
    public class EmployeeEntity : AuditableDbEntity, IAddressEntity
    {
        public string Address1 { get; set; } = default!;

        public string? Address2 { get; set; }

        public int AspNetUsersId { get; set; }

        public string City { get; set; } = default!;

        public CurrencyEntity Currency { get; set; } = default!;

        public string CurrencyCode { get; set; } = default!;

        public DateTime? DateHired { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime? DateTerminated { get; set; }

        public int EmployeeLevel { get; set; }

        public string FirstName { get; set; } = default!;

        public ICollection<PaidTimeOffRequestEntity> ForPaidTimeOffRequests { get; set; }

        public Gender Gender { get; set; }

        public int Id { get; set; }

        public bool IsPending { get; set; }

        public string LastName { get; set; } = default!;

        public ICollection<EmployeeManagerEntity> Managers { get; set; }

        public string? MiddleName { get; set; }

        public PaidTimeOffPolicyEntity PaidTimeOffPolicy { get; set; } = default!;

        public int PaidTimeOffPolicyId { get; set; }

        public decimal? PtoHoursRemaining { get; set; }

        public decimal Salary { get; set; }

        // Note: in a real application, do NOT store this in the DB. It should be kept in some kind of a secure data store.
        public string SocialSecurityNumber { get; set; } = default!;

        public string State { get; set; } = default!;

        public ICollection<PaidTimeOffRequestEntity> SubmittedPaidTimeOffRequests { get; set; }

        public ICollection<EmployeeManagerEntity> Subordinates { get; set; }

        public TenantEntity Tenant { get; set; } = default!;

        public int TenantId { get; set; }

        public string ZipCode { get; set; } = default!;

        public EmployeeEntity()
        {
            Subordinates = new HashSet<EmployeeManagerEntity>();
            Managers = new HashSet<EmployeeManagerEntity>();
            ForPaidTimeOffRequests = new HashSet<PaidTimeOffRequestEntity>();
            SubmittedPaidTimeOffRequests = new HashSet<PaidTimeOffRequestEntity>();
        }
    }
}