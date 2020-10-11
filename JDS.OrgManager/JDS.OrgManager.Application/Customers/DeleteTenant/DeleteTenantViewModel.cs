using JDS.OrgManager.Application.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Application.Customers.DeleteTenant
{
    public class DeleteTenantViewModel : IViewModel
    {
        public string ConfirmationCode { get; set; }

        public int TenantId { get; set; }
    }
}
