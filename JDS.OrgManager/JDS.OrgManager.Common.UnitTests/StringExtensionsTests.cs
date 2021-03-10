// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Common.Text;
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