// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using AutoFixture;
using JDS.OrgManager.Application.Common.Mapping;
using JDS.OrgManager.Utils;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace JDS.OrgManager.Application.UnitTests.HumanResources.Employees
{
    public class EmployeeDomainEntityToViewModelMapperTests
    {
        private readonly Fixture fixture;

        private readonly EmployeeDomainEntityToViewModelMapper mapper;

        public EmployeeDomainEntityToViewModelMapperTests()
        {
            mapper = new EmployeeDomainEntityToViewModelMapper();

            fixture = new Fixture();
            // client has a circular reference from AutoFixture point of view
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void Map_WorksAsExpected()
        {
            foreach (var isTopLevelManager in new[] { false, true })
            {
                var hierarchy = UnitTestEmployeeGenerator.GenerateEmployee(3);
                var e = isTopLevelManager ? hierarchy : hierarchy.Subordinates.First();
                var v = mapper.Map(e);
                Debug.WriteLine(mapper.ToScript(e));
                Assert.Equal(e.DateTerminated, v.DateTerminated);
                Assert.Equal(e.DateHired, v.DateHired);
                Assert.Equal(e.DateOfBirth, v.DateOfBirth);
                Assert.Equal(e.EmployeeLevel, v.EmployeeLevel);
                Assert.Equal(e.FirstName, v.FirstName);
                Assert.Equal(e.Gender, v.Gender);
                Assert.Equal(e.HomeAddress.City, v.City);
                Assert.Equal(e.HomeAddress.State.Abbreviation, v.State);
                Assert.Equal(e.HomeAddress.Street1, v.Address1);
                Assert.Equal(e.HomeAddress.Street2, v.Address2);
                Assert.Equal(e.HomeAddress.ZipCode.ToString(), v.ZipCode);
                Assert.Equal(e.LastName, v.LastName);
                Assert.Equal(e.MiddleName, v.MiddleName);
                Assert.Equal(e.PtoHoursRemaining, v.PtoHoursRemaining);
                Assert.Equal(e.Salary.Amount, v.Salary);
                Assert.Equal(e.Salary.Currency.Code, v.CurrencyCode);
                Assert.Equal(e.Id, v.Id);
                Assert.Equal(e.PaidTimeOffPolicy.Id, v.PaidTimeOffPolicyId);
                Assert.Equal(e.SocialSecurityNumber, v.SocialSecurityNumber);
                Assert.Equal(e.Manager?.Id, v.ManagerId);
                var subIds = v.SubordinateIds;
                Assert.True(e.Subordinates.All(s => subIds.Contains(s.Id)));
            }
        }
    }
}