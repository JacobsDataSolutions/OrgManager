using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.TimeOff
{
    public class PaidTimeOffRequestViewModel : IViewModel
    {
        public PaidTimeOffRequestApprovalStatus ApprovalStatus { get; set; }

        public PaidTimeOffRequestStatus Status { get; set; }

        public DateTime EndDate { get; set; }

        public int ForEmployeeId { get; set; }

        public string ForEmployeeName { get; set; } = default!;

        public int HoursRequested { get; set; }

        public int Id { get; set; }

        public string? Notes { get; set; }

        public DateTime StartDate { get; set; }

        public bool Paid { get; set; }

        public int SubmittedById { get; set; }

        public string SubmittedByName { get; set; } = default!;
    }
}
