// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Common.Abstractions.DateTimes;
using JDS.OrgManager.Domain.Common.Addresses;
using JDS.OrgManager.Domain.Common.Employees;
using JDS.OrgManager.Domain.Common.Finance;
using JDS.OrgManager.Domain.Common.People;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using JDS.OrgManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JDS.OrgManager.Domain.HumanResources.Employees
{
    public class Employee : DomainEntity<Employee>
    {
        public DateTime DateHired { get; init; }

        public DateTime DateOfBirth { get; init; }

        public DateTime? DateTerminated { get; init; }

        public int EmployeeLevel { get; init; }

        public string FirstName { get; init; } = default!;

        public Gender Gender { get; init; }

        public Address HomeAddress { get; init; } = default!;

        public string LastName { get; init; } = default!;

        private Employee? manager;
        public Employee? Manager { get => manager; init => manager = value; }

        public string? MiddleName { get; init; }

        private PaidTimeOffPolicy paidTimeOffPolicy = default!;
        public PaidTimeOffPolicy PaidTimeOffPolicy { get => paidTimeOffPolicy; init => paidTimeOffPolicy = value; }

        private List<Employee> subordinates = new List<Employee>();
        public List<Employee> Subordinates { get => subordinates; init => subordinates = value; }

        private List<PaidTimeOffRequest> paidTimeOffRequests = new List<PaidTimeOffRequest>();
        public List<PaidTimeOffRequest> PaidTimeOffRequests { get => paidTimeOffRequests; init => paidTimeOffRequests = value; }

        public decimal? PtoHoursRemaining { get; init; }

        public Money Salary { get; init; } = default!;

        public SocialSecurityNumber SocialSecurityNumber { get; init; } = default!;

        public override void ValidateAggregate()
        {
            base.ValidateAggregate();
            if (DateHired < EmployeeConstants.MinimumValidDateOfHire)
            {
                throw new EmployeeException($"Invalid date of hire: {DateHired:d}.");
            }
            if (DateOfBirth < EmployeeConstants.MinimumValidDateOfBirth)
            {
                throw new EmployeeException($"Invalid date of birth: {DateOfBirth:d}.");
            }
            if (Manager?.Subordinates?.Any() == true)
            {
                throw new EmployeeException("To avoid cyclical references, a manager instance may not have any subordinates for this employee aggregate.");
            }
            if (Salary.Amount <= 0.0m)
            {
                throw new EmployeeException($"Invalid salary: {Salary.Amount}");
            }

            Manager?.ValidateAggregate();
            foreach (var subordinate in Subordinates)
            {
                subordinate.ValidateAggregate();
            }
        }

        public void CreateEmployeeRegisteredEvent() => AddDomainEvent(new EmployeeRegisteredEvent(this));
        public void CreateEmployeeUpdatedEvent() => AddDomainEvent(new EmployeeUpdatedEvent(this));

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
            return CloneWith(e => e.manager = manager);
        }

        public Employee WithPaidTimeOffPolicy(PaidTimeOffPolicy paidTimeOffPolicy)
        {
            if (paidTimeOffPolicy.EmployeeLevel != EmployeeLevel)
            {
                throw new EmployeeException("Invalid paid time off PaidTimeOffPolicy assigned to employee. The PaidTimeOffPolicy EmployeeLevel must be the same as the employee's EmployeeLevel.");
            }
            return CloneWith(e => e.paidTimeOffPolicy = paidTimeOffPolicy);
        }

        public Employee WithSubordinates(IEnumerable<Employee> subordinates)
        {
            var subordinatesList = (subordinates ?? Enumerable.Empty<Employee>()).ToList();
            if (subordinatesList.Any(s => s.EmployeeLevel >= EmployeeLevel))
            {
                throw new EmployeeException("You cannot assign subordinates whose employee levels are greater than or equal to that of the current employee.");
            }
            // Prevent circular references. Subordinates reference a copy of this employee.
            var employeeAsManager = CloneWith(e => e.subordinates = new List<Employee>());
            var employee = CloneWith(e => e.subordinates = subordinatesList);
            employee.CrossLinkSubordinates(employeeAsManager);
            return employee;
        }

        private void CrossLinkSubordinates(Employee employeeAsManager)
        {
            foreach (var subordinate in Subordinates)
            {
                subordinate.manager = employeeAsManager;
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