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
        public Money Elected401kContribution { get; private set; }

        public string FirstName { get; private set; }

        public Gender Gender { get; private set; }

        public Address HomeAddress { get; private set; }

        public string LastName { get; private set; }

        public string MiddleName { get; private set; }

        public IReadOnlyList<Paycheck> Paychecks { get; private set; }

        public decimal PtoHoursRemaining { get; private set; }

        public Money Salary { get; private set; }

        public SocialSecurityNumber SocialSecurityNumber { get; private set; }

        public Employee(
            string firstName,
            string lastName,
            SocialSecurityNumber socialSecurityNumber,
            Money salary,
            Money elected401kContribution,
            decimal ptoHoursRemaining,
            int? id = null,
            string middleName = null,
            IEnumerable<Paycheck> paychecks = null)
        {
            Salary = salary ?? throw new ArgumentNullException(nameof(salary));
            if (salary.Amount <= 0.0m)
            {
                throw new ArgumentOutOfRangeException(nameof(salary));
            }
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            SocialSecurityNumber = socialSecurityNumber ?? throw new ArgumentNullException(nameof(socialSecurityNumber));
            Elected401kContribution = elected401kContribution ?? throw new ArgumentNullException(nameof(elected401kContribution));
            PtoHoursRemaining = ptoHoursRemaining;
            Id = id;
            MiddleName = middleName;
            Paychecks = (paychecks ?? Enumerable.Empty<Paycheck>()).ToList();
        }
    }
}