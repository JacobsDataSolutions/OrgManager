using JDS.OrgManager.Domain.HumanResources.Employees;
using JDS.OrgManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Domain.HumanResources.TimeOff
{
    public class PaidTimeOffRequest : DomainEntity<PaidTimeOffRequest>
    {
        public PaidTimeOffRequestApprovalStatus ApprovalStatus { get; init; }

        public DateTime EndDate { get; init; }

        public int HoursRequested { get; init; }

        public string? Notes { get; init; }

        public PaidTimeOffPolicy PaidTimeOffPolicy { get; init; } = default!;

        public DateTime StartDate { get; init; }

        public PaidTimeOffRequestStatus Status { get; init; }

        public Employee ForEmployee { get; init; } = default!;

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

        public PaidTimeOffRequest WithSubmittedBy(Employee employee) => ReflectionCloneWith(r => r.SubmittedBy, employee);

        public PaidTimeOffRequest WithPaidTimeOffPolicy(PaidTimeOffPolicy paidTimeOffPolicy) => ReflectionCloneWith(r => r.PaidTimeOffPolicy, paidTimeOffPolicy);

        private void CreatePaidTimeOffRequestSubmittedEvent(PaidTimeOffRequest paidTimeOffRequest) => AddDomainEvent(new PaidTimeOffRequestSubmittedEvent(paidTimeOffRequest));
    }
}
