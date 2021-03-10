// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.HumanResources.Employees;
using JDS.OrgManager.Domain.Models;
using System;

namespace JDS.OrgManager.Domain.HumanResources.TimeOff
{
    public class PaidTimeOffRequest : DomainEntity<PaidTimeOffRequest>
    {
        public PaidTimeOffRequestApprovalStatus ApprovalStatus { get; init; }

        public DateTime EndDate { get; init; }

        public Employee ForEmployee { get; init; } = default!;

        public int HoursRequested { get; init; }

        public string? Notes { get; init; }

        public PaidTimeOffPolicy PaidTimeOffPolicy { get; init; } = default!;

        public DateTime StartDate { get; init; }

        public PaidTimeOffRequestStatus Status { get; init; }

        public Employee SubmittedBy { get; init; } = default!;

        public PaidTimeOffRequest Submit()
        {
            var submittedRequest = ReflectionCloneWith(r => r.ApprovalStatus, PaidTimeOffRequestApprovalStatus.Submitted);
            CreatePaidTimeOffRequestSubmittedEvent(submittedRequest);
            return submittedRequest;
        }

        public override void ValidateAggregate()
        {
            base.ValidateAggregate();
            if (HoursRequested < 1)
            {
                throw new PaidTimeOffException("PTO hours requested must be greater than zero.");
            }
            if (EndDate < StartDate)
            {
                throw new PaidTimeOffException("PTO end date must be later than start date.");
            }
            ValidateNotNull(PaidTimeOffPolicy, ForEmployee, SubmittedBy);
            ForEmployee.ValidateAggregate();
            SubmittedBy.ValidateAggregate();
            PaidTimeOffPolicy.ValidateAggregate();
        }

        public PaidTimeOffRequest WithForEmployee(Employee employee) => ReflectionCloneWith(r => r.ForEmployee, employee);

        public PaidTimeOffRequest WithPaidTimeOffPolicy(PaidTimeOffPolicy paidTimeOffPolicy) => ReflectionCloneWith(r => r.PaidTimeOffPolicy, paidTimeOffPolicy);

        public PaidTimeOffRequest WithSubmittedBy(Employee employee) => ReflectionCloneWith(r => r.SubmittedBy, employee);

        private void CreatePaidTimeOffRequestSubmittedEvent(PaidTimeOffRequest paidTimeOffRequest) => AddDomainEvent(new PaidTimeOffRequestSubmittedEvent(paidTimeOffRequest));
    }
}