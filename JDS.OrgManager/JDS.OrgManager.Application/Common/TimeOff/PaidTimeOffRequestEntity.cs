using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Tenants;
using JDS.OrgManager.Domain.HumanResources.TimeOff;

namespace JDS.OrgManager.Application.Common.TimeOff
{
    public class PaidTimeOffRequestEntity : AuditableDbEntity
    {
        public PaidTimeOffRequestApprovalStatus ApprovalStatus { get; set; }
        
        public DateTime EndDate { get; set; }

        public EmployeeEntity ForEmployee { get; set; } = default!;

        public int ForEmployeeId { get; set; }

        public int HoursRequested { get; set; }

        public int Id { get; set; }

        public string? Notes { get; set; }

        public DateTime StartDate { get; set; }

        public bool Paid { get; set; }

        public EmployeeEntity SubmittedBy { get; set; } = default!;

        public int SubmittedById { get; set; }

        public int TenantId { get; set; }

        public TenantEntity Tenant { get; set; } = default!;
    }
}
