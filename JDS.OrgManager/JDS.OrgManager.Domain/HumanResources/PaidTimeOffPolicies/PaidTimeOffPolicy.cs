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
        public bool AllowsUnlimitedPto { get; init; }

        public int EmployeeLevel { get; init; }

        public bool IsDefaultForEmployeeLevel { get; init; }

        public decimal? MaxPtoHours { get; init; }

        public string Name { get; init; } = default!;

        public decimal? PtoAccrualRate { get; init; }

        public override void ValidateAggregate()
        {
            base.ValidateAggregate();
            if (EmployeeLevel < 1)
            {
                throw new PaidTimeOffException("Invalid employee level for PTO policy.");
            }
            if (!AllowsUnlimitedPto && (MaxPtoHours == null || PtoAccrualRate == null))
            {
                throw new PaidTimeOffException($"Invalid PTO policy. If policy does not specify unlimited hours, then you must specify both max PTO hours and PTO accrual rate.");
            }
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new PaidTimeOffException("Invalid PTO policy name.");
            }
        }
    }
}