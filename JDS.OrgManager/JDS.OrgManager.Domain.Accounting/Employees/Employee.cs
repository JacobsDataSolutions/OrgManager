// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Accounting.Pay;
using JDS.OrgManager.Domain.Common.Addresses;
using JDS.OrgManager.Domain.Common.Finance;
using JDS.OrgManager.Domain.Common.People;
using JDS.OrgManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JDS.OrgManager.Domain.Accounting.Employees
{
    public class Employee : DomainEntity<Employee>
    {
        public Money Elected401kContribution { get; init; }

        public string FirstName { get; init; }

        public Gender Gender { get; init; }

        public Address HomeAddress { get; init; }

        public string LastName { get; init; }

        public string MiddleName { get; init; }

        public IReadOnlyList<Paycheck> Paychecks { get; init; }

        public decimal PtoHoursRemaining { get; init; }

        public Money Salary { get; init; }

        public SocialSecurityNumber SocialSecurityNumber { get; init; }
    }
}