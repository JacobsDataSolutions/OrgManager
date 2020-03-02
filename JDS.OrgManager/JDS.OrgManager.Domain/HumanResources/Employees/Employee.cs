// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Common.Abstractions.Dates;
using JDS.OrgManager.Domain.Common.Addresses;
using JDS.OrgManager.Domain.Common.Finance;
using JDS.OrgManager.Domain.Common.People;
using JDS.OrgManager.Domain.HumanResources.PaidTimeOffPolicies;
using JDS.OrgManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JDS.OrgManager.Domain.HumanResources.Employees
{
    public class Employee : DomainEntity<Employee>
    {
        public DateTime? DateExited { get; private set; }

        public DateTime DateHired { get; private set; }

        public DateTime DateOfBirth { get; private set; }

        public int EmployeeLevel { get; private set; }

        public string FirstName { get; private set; }

        public Gender Gender { get; private set; }

        public Address HomeAddress { get; private set; }

        public string LastName { get; private set; }

        public Employee Manager { get; private set; }

        public string MiddleName { get; private set; }

        public PaidTimeOffPolicy PaidTimeOffPolicy { get; private set; }

        public decimal? PtoHoursRemaining { get; private set; }

        public Money Salary { get; private set; }

        public SocialSecurityNumber SocialSecurityNumber { get; private set; }

        public IReadOnlyList<Employee> Subordinates { get; private set; } = new List<Employee>();

        public Employee()
        {
        }

        public Employee(
            DateTime dateHired,
            DateTime dateOfBirth,
            int employeeLevel,
            string firstName,
            Gender gender,
            Address homeAddress,
            string lastName,
            PaidTimeOffPolicy paidTimeOffPolicy,
            Money salary,
            SocialSecurityNumber socialSecurityNumber,
            DateTime? dateExited = default,
            string middleName = default,
            decimal? ptoHoursRemaining = default,
            IReadOnlyList<Employee> subordinates = default
            )
        {
            if (employeeLevel <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(employeeLevel));
            }
            DateExited = dateExited;
            DateHired = dateHired;
            DateOfBirth = dateOfBirth;
            EmployeeLevel = employeeLevel;
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            Gender = gender;
            HomeAddress = homeAddress ?? throw new ArgumentNullException(nameof(homeAddress));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            MiddleName = middleName ?? "";
            PaidTimeOffPolicy = paidTimeOffPolicy;
            PtoHoursRemaining = ptoHoursRemaining;
            Salary = salary ?? throw new ArgumentNullException(nameof(salary));
            if (salary.Amount <= 0.0m)
            {
                throw new ArgumentOutOfRangeException(nameof(salary));
            }
            SocialSecurityNumber = socialSecurityNumber ?? throw new ArgumentNullException(nameof(socialSecurityNumber));
            Subordinates = (subordinates ?? Enumerable.Empty<Employee>()).ToList();
            CrossLinkSubordinates();
        }

        public override void AssertAggregates()
        {
            if (
                HomeAddress == null ||
                (Manager != null && Manager.Id == null) ||
                PaidTimeOffPolicy == null ||
                PaidTimeOffPolicy.Id == null ||
                Salary == null ||
                SocialSecurityNumber == null ||
                Subordinates == null ||
                Subordinates.Any(e => e.Id == null))
            {
                throw new EmployeeException("One or more child aggregates were invalid for this employee.");
            }
        }

        public void CreateEmployeeRegisteredEvent(IDateTimeService dateTimeService) => AddDomainEvent(new EmployeeRegisteredEvent(dateTimeService, this));

        public void VerifyEmployeeManagerAndSubordinates()
        {
            if (Manager?.EmployeeLevel <= EmployeeLevel)
            {
                throw new EmployeeException("Invalid manager level specified: must be greater than this employee's level.");
            }
            if (Subordinates.Any(e => e.EmployeeLevel >= EmployeeLevel))
            {
                throw new EmployeeException("Invalid subordinate level specified: must be less than this employee's level.");
            }
        }

        public void VerifyPtoHoursAreValid()
        {
            ThrowIfPaidTimeOffPolicyIsNull();
            if (!PaidTimeOffPolicy.AllowsUnlimitedPto)
            {
                if (PtoHoursRemaining > PaidTimeOffPolicy.MaxPtoHours)
                {
                    throw new PaidTimeOffException($"Invalid PTO hours specified: {PtoHoursRemaining} exceeds the policy maximum of {PaidTimeOffPolicy.MaxPtoHours}.");
                }
                if (PtoHoursRemaining == null)
                {
                    throw new PaidTimeOffException($"Invalid initial PTO hours specified: hour balance cannot be null. You must specify an initial value.");
                }
            }
        }

        public void VerifyStartingPtoHoursAreValid()
        {
            VerifyPtoHoursAreValid();
            if (!PaidTimeOffPolicy.AllowsUnlimitedPto)
            {
                if (PtoHoursRemaining < 0.0m)
                {
                    throw new PaidTimeOffException($"Invalid PTO hours specified: negative hour balances are not allowed when registering an employee.");
                }
            }
        }

        public Employee WithManager(Employee manager)
        {
            if (EmployeeLevel >= manager.EmployeeLevel)
            {
                throw new EmployeeException("You cannot assign a manager whose employee level is less than or equal to that of the current employee.");
            }

            var e = CreateShallowCopy();
            e.Manager = manager;

            return e;
        }

        public Employee WithPaidTimeOffPolicy(PaidTimeOffPolicy paidTimeOffPolicy)
        {
            if (paidTimeOffPolicy.EmployeeLevel != EmployeeLevel)
            {
                throw new EmployeeException("Invalid paid time off PaidTimeOffPolicy assigned to employee. The PaidTimeOffPolicy EmployeeLevel must be the same as the employee's EmployeeLevel.");
            }
            var e = CreateShallowCopy();
            e.PaidTimeOffPolicy = paidTimeOffPolicy;
            return e;
        }

        public Employee WithSubordinates(IEnumerable<Employee> subordinates)
        {
            var subordinatesList = (subordinates ?? Enumerable.Empty<Employee>()).ToList();
            if (subordinatesList.Any(s => s.EmployeeLevel >= EmployeeLevel))
            {
                throw new EmployeeException("You cannot assign subordinates whose employee levels are greater than or equal to that of the current employee.");
            }
            var e = CreateShallowCopy();
            e.Subordinates = subordinatesList;
            CrossLinkSubordinates();
            return e;
        }

        private void CrossLinkSubordinates()
        {
            foreach (var subordinate in Subordinates)
            {
                subordinate.Manager = this;
            }
        }

        private void ThrowIfPaidTimeOffPolicyIsNull()
        {
            if (PaidTimeOffPolicy == null)
            {
                throw new EmployeeException("Paid time off policy has not been set for this employee.");
            }
        }
    }
}