using JDS.OrgManager.Application.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.TimeOff.Queries.ValidateRequestedPaidTimeOffHours
{
    public class ValidateRequestedPaidTimeOffHoursViewModel : IViewModel
    {
        public int ForEmployeeId { get; set; }

        public int HoursRequested { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public int TenantId { get; set; }
    }
}
