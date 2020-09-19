using JDS.OrgManager.Application.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Application.Common.Addresses
{
    public interface IAddressEntity : IDbEntity
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }
    }
}
