using JDS.OrgManager.Application.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Application.Users.GetUserStatus
{
    public class UserStatusViewModel : IViewModel
    {
        public int[] AuthorizedTenants { get; set; }

        public bool HasProvidedCustomerInformation { get; set; }

        public bool IsApprovedEmployee { get; set; }

        public bool IsCustomer { get; set; }
    }
}
