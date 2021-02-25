using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Domain.HumanResources.TimeOff
{
    // Naive implementation of a Domain service encapsulating complex business rules for PTO requests.
    // An actual production version of this would need to handle relatively complex scenarios, such as requests that cut across fiscal years, etc.
    // and probably use a more sophisticated library like NodaTime.
    // This is only for demonstration purposes.
    public class PaidTimeOffRequestService
    {
        public PaidTimeOffRequestValidationResult ValidatePaidTimeOffRequest(PaidTimeOffRequest tentativeRequest, IEnumerable<PaidTimeOffRequest> existingRequests, PaidTimeOffPolicy paidTimeOffPolicy, DateTime today)
        {
            _ = existingRequests ?? throw new ArgumentNullException(nameof(existingRequests));
            var year = today.Year;
            var requestsForCurrentYear = (
                from req in existingRequests
                where req.StartDate.Year == year &&
                (req.ApprovalStatus == PaidTimeOffRequestApprovalStatus.Approved || req.ApprovalStatus == PaidTimeOffRequestApprovalStatus.Submitted)
                select req).ToList();

            var availableHours = (tentativeRequest.StartDate.Month - today.Month) * paidTimeOffPolicy.PtoAccrualRate - requestsForCurrentYear.Sum(r => r.HoursRequested);

            var validationResult =
                tentativeRequest switch
                {
                    PaidTimeOffRequest req when req.StartDate > req.EndDate => PaidTimeOffRequestValidationResult.StartDateAfterEndDate,
                    PaidTimeOffRequest req when req.StartDate < today => PaidTimeOffRequestValidationResult.InThePast,
                    PaidTimeOffRequest req when requestsForCurrentYear.Any(r =>
                        (r.StartDate >= req.StartDate && r.StartDate <= req.EndDate) ||
                        (r.EndDate >= req.StartDate && r.EndDate <= req.EndDate) ||
                        (req.StartDate >= r.StartDate && req.EndDate <= r.EndDate))
                        => PaidTimeOffRequestValidationResult.OverlapsWithExisting,
                    PaidTimeOffRequest req when req.StartDate.Year > today.Year => PaidTimeOffRequestValidationResult.TooFarInTheFuture,
                    PaidTimeOffRequest req when req.HoursRequested > availableHours => PaidTimeOffRequestValidationResult.NotEnoughHours,
                    _ => PaidTimeOffRequestValidationResult.OK
                };
            return validationResult;
        }

        public List<PaidTimeOffRequest> GetPaidTimeOffRequestsWithStatusUpdates(IEnumerable<PaidTimeOffRequest> paidTimeOffRequests, DateTime today)
        {
            _ = paidTimeOffRequests ?? throw new ArgumentNullException(nameof(paidTimeOffRequests));
            return (from request in paidTimeOffRequests
                    select request switch
                    {
                        PaidTimeOffRequest req when req.ApprovalStatus != PaidTimeOffRequestApprovalStatus.Approved &&
                        req.ApprovalStatus != PaidTimeOffRequestApprovalStatus.Submitted => req.ReflectionCloneWith(r => r.Status, PaidTimeOffRequestStatus.Canceled),
                        PaidTimeOffRequest req when req.StartDate <= today => req.ReflectionCloneWith(r => r.Status, PaidTimeOffRequestStatus.Taken),
                        _ => request.ReflectionCloneWith(r => r.Status, PaidTimeOffRequestStatus.Pending)
                    }).ToList();
        }
    }
}
