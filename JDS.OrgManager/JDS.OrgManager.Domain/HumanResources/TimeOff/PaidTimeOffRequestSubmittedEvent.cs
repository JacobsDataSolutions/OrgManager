using JDS.OrgManager.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Domain.HumanResources.TimeOff
{
    public class PaidTimeOffRequestSubmittedEvent : DomainEvent
    {
        public PaidTimeOffRequest PaidTimeOffRequest { get; private set; }

        public PaidTimeOffRequestSubmittedEvent(PaidTimeOffRequest paidTimeOffRequest)
            : base() => PaidTimeOffRequest = paidTimeOffRequest ?? throw new ArgumentNullException(nameof(paidTimeOffRequest));

        public override string ToString() => $"{DateTimeOccurredUtc} - {PaidTimeOffRequest.SubmittedBy.FirstName} {PaidTimeOffRequest.SubmittedBy.LastName} - {PaidTimeOffRequest.StartDate:d}-{PaidTimeOffRequest.EndDate:d}";
    }
}
