// Copyright �2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Models;

namespace JDS.OrgManager.Application.Common.TimeOff
{
    public class PaidTimeOffPolicyEntity : AuditableDbEntity
    {
        public bool AllowsUnlimitedPto { get; set; }

        public int EmployeeLevel { get; set; }

        public int Id { get; set; }

        public bool IsDefaultForEmployeeLevel { get; set; }

        public decimal? MaxPtoHours { get; set; }

        public string Name { get; set; } = default!;

        public decimal? PtoAccrualRate { get; set; }

        public int TenantId { get; set; }
    }
}