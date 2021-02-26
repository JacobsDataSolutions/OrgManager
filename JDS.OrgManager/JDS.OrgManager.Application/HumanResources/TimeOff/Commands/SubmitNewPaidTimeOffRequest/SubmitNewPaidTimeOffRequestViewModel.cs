using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.TimeOff.Commands.SubmitNewPaidTimeOffRequest
{
    public class SubmitNewPaidTimeOffRequestViewModel : IViewModel
    {
        public DateTime EndDate { get; set; }

        public int? ForEmployeeId { get; set; }

        public int HoursRequested { get; set; }

        public PaidTimeOffRequestValidationResult Result { get; set; }

        public DateTime StartDate { get; set; }

        public PaidTimeOffRequestViewModel? CreatedPaidTimeOffRequest { get; set; }

        public int TenantId { get; set; }
    }
}
