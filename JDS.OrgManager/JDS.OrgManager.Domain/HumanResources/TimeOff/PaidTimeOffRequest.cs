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

        private PaidTimeOffPolicy paidTimeOffPolicy = default!;
        public PaidTimeOffPolicy PaidTimeOffPolicy { get => paidTimeOffPolicy; init => paidTimeOffPolicy = value; }

        public DateTime StartDate { get; init; }

        public PaidTimeOffRequestStatus Status { get; init; }

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
        }

        public void Deconstruct(out DateTime startDate, out PaidTimeOffRequestApprovalStatus approvalStatus)
        {
            startDate = StartDate;
            approvalStatus = ApprovalStatus;
        }
    }
}
