using JDS.OrgManager.Application.Common.Addresses;
using JDS.OrgManager.Domain.Common.People;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Application.Customers
{
    public class CustomerViewModel : IAddressViewModel
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public int? CompanyId { get; set; }

        public string CurrencyCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string State { get; set; }

        public Title? Title { get; set; }

        public string Zip { get; set; }
    }
}
