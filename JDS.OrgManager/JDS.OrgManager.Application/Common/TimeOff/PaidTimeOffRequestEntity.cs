// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Tenants;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using System;

namespace JDS.OrgManager.Application.Common.TimeOff
{
    public class PaidTimeOffRequestEntity : AuditableDbEntity
    {
        public PaidTimeOffRequestApprovalStatus ApprovalStatus { get; set; }

        public DateTime EndDate { get; set; }

        public EmployeeEntity ForEmployee { get; set; } = default!;

        public int ForEmployeeId { get; set; }

        public int HoursRequested { get; set; }

        public int Id { get; set; }

        public string? Notes { get; set; }

        public bool Paid { get; set; }

        public DateTime StartDate { get; set; }

        public EmployeeEntity SubmittedBy { get; set; } = default!;

        public int SubmittedById { get; set; }

        public TenantEntity Tenant { get; set; } = default!;

        public int TenantId { get; set; }
    }
}