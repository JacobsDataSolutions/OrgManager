// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Utils;
using System.Linq;
using Xunit;

namespace JDS.OrgManager.Domain.HumanResources.Advanced.UnitTests
{
    public class OrganizationVerifierTests
    {
        private IOrganizationVerifier organizationVerifier;

        public OrganizationVerifierTests()
        {
            organizationVerifier = new OrganizationVerifier();
        }

        [Fact]
        public void VerifyOrg_MultipleTrees_ReturnsExpectedStats()
        {
            var trees = (from n in Enumerable.Range(1, 4) select UnitTestEmployeeGenerator.GenerateEmployee(n)).ToList();
            var results = organizationVerifier.VerifyOrg(trees);
            Assert.Equal(1, results[0].Item1);
            Assert.Equal(1, results[0].Item2);
            Assert.Equal(3, results[1].Item1);
            Assert.Equal(4, results[1].Item2);
            Assert.Equal(10, results[2].Item1);
            Assert.Equal(15, results[2].Item2);
            Assert.Equal(41, results[3].Item1);
            Assert.Equal(64, results[3].Item2);
        }
    }
}