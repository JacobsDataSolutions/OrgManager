using JDS.OrgManager.Common.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JDS.OrgManager.Common.UnitTests
{
    public class StringExtensionsTests
    {
        public static readonly object[][] SluggifyData =
        {
            new object[] { " test tenant ", "test-tenant"},
            new object[] { "- test--tenant-- ", "test-tenant" },
            new object[] { "Test- -Tenant ", "test-tenant" },
            new object[] { "*&^%$Test- - Tenant 2", "test-tenant-2" },
            new object[] { "$ - test%%%tenant **", "test-tenant" },
            new object[] { "$$TestTenant**", "test-tenant" },
            new object[] { "---test Tenant---", "test-tenant" },
            new object[] { "MyTestTenant", "my-test-tenant" }
        };

        [Theory]
        [MemberData(nameof(SluggifyData))]
        public void Sluggify_Works(string input, string expected) => Assert.Equal(expected, StringExtensions.Sluggify(input));
    }
}
