using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Domain.HumanResources.TimeOff
{
    public enum PaidTimeOffRequestValidationResult
    {
        NotEnoughHours,
        OverlapsWithExisting,
        InThePast,
        TooFarInTheFuture,
        StartDateAfterEndDate,
        OK
    }
}
