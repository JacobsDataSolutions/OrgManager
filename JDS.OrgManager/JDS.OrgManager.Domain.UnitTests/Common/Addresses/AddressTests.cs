using JDS.OrgManager.Domain.Common.Addresses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

#pragma warning disable IDE0059 // Unnecessary assignment of a value

namespace JDS.OrgManager.Domain.UnitTests.Common.Addresses
{
    public class AddressTests
    {
        [Fact]
        public void Ctor_CanCreateAddress()
        {
            var address = new Address("1111 N. Lakeshore Dr.", "Chicago", new State("IL"), new ZipCode("60606"));
        }

        [Fact]
        public void CannotMutateAddress()
        {
            var address = new Address("1111 N. Lakeshore Dr.", "Chicago", new State("IL"), new ZipCode("60606"));
            // City is declared as an init property. Compiler doesn't even allow this.
            //address.City = "Milwaukee";
        }
    }
}

#pragma warning restore IDE0059 // Unnecessary assignment of a value