﻿// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Common.Addresses;
using JDS.OrgManager.Domain.Common.People;
using System;
using System.Collections.Generic;

namespace JDS.OrgManager.Application.HumanResources.Employees
{
    public class EmployeeViewModel : IAddressViewModel
    {
        public string Address1 { get; set; } = default!;

        public string? Address2 { get; set; }

        public Guid AssignmentKey { get; set; }

        public string City { get; set; } = default!;

        public string CurrencyCode { get; set; } = default!;

        public DateTime DateHired { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime? DateTerminated { get; set; }

        public int EmployeeLevel { get; set; }

        public string? ExternalEmployeeId { get; set; }

        public string FirstName { get; set; } = default!;

        public Gender Gender { get; set; }

        public int Id { get; set; }

        public string LastName { get; set; } = default!;

        public int? ManagerId { get; set; }

        public string? MiddleName { get; set; }

        public int PaidTimeOffPolicyId { get; set; }

        public decimal? PtoHoursRemaining { get; set; }

        public decimal Salary { get; set; }

        public string SocialSecurityNumber { get; set; } = default!;

        public string State { get; set; } = default!;

        public List<int> SubordinateIds { get; set; } = new List<int>();

        public int TenantId { get; set; }

        public string ZipCode { get; set; } = default!;
    }
}