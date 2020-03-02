// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Models;
using System;

namespace JDS.OrgManager.Domain.HumanResources.PaidTimeOffPolicies
{
    public class PaidTimeOffPolicy : DomainEntity<PaidTimeOffPolicy>
    {
        public bool AllowsUnlimitedPto { get; private set; }

        public int EmployeeLevel { get; private set; }

        public bool IsDefaultForEmployeeLevel { get; private set; }

        public decimal? MaxPtoHours { get; private set; }

        public string Name { get; private set; }

        public decimal? PtoAccrualRate { get; private set; }

        public PaidTimeOffPolicy()
        { }

        public PaidTimeOffPolicy(bool allowsUnlimitedPto, int employeeLevel, bool isDefaultForEmployeeLevel, decimal? maxPtoHours, string name, decimal? ptoAccruralRate)
        {
            if (employeeLevel < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(employeeLevel));
            }
            if (!allowsUnlimitedPto && (maxPtoHours == null || ptoAccruralRate == null))
            {
                throw new PaidTimeOffException($"Invalid PTO policy. If policy does not specify unlimited hours, then you must specify both max PTO hours and PTO accrual rate.");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            AllowsUnlimitedPto = allowsUnlimitedPto;
            EmployeeLevel = employeeLevel;
            MaxPtoHours = allowsUnlimitedPto ? null : maxPtoHours;
            PtoAccrualRate = allowsUnlimitedPto ? null : ptoAccruralRate;
            Name = name;
            IsDefaultForEmployeeLevel = isDefaultForEmployeeLevel;
        }
    }
}