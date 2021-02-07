// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using AutoFixture;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Common.Mapping;
using JDS.OrgManager.Utils;
using Xunit;

namespace JDS.OrgManager.Application.UnitTests.HumanResources.Employees
{
    public class EmployeeDomainEntityToDbEntityMapperTests
    {
        private Fixture fixture;

        private EmployeeDomainEntityToDbEntityMapper mapper;

        public EmployeeDomainEntityToDbEntityMapperTests()
        {
            mapper = new EmployeeDomainEntityToDbEntityMapper();

            fixture = new Fixture();
            // client has a circular reference from AutoFixture point of view
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void Map_WorksAsExpected()
        {
            var e = UnitTestEmployeeGenerator.GenerateEmployee(3);
            var d = mapper.Map(e);
            Assert.Equal(e.DateTerminated, d.DateTerminated);
            Assert.Equal(e.DateHired, d.DateHired);
            Assert.Equal(e.DateOfBirth, d.DateOfBirth);
            Assert.Equal(e.EmployeeLevel, d.EmployeeLevel);
            Assert.Equal(e.FirstName, d.FirstName);
            Assert.Equal(e.Gender, d.Gender);
            Assert.Equal(e.HomeAddress.City, d.City);
            Assert.Equal(e.HomeAddress.State.Abbreviation, d.State);
            Assert.Equal(e.HomeAddress.Street1, d.Address1);
            Assert.Equal(e.HomeAddress.Street2, d.Address2);
            Assert.Equal(e.HomeAddress.ZipCode.ToString(), d.ZipCode);
            Assert.Equal(e.LastName, d.LastName);
            Assert.Equal(e.MiddleName, d.MiddleName);
            Assert.Equal(e.PtoHoursRemaining, d.PtoHoursRemaining);
            Assert.Equal(e.Salary.Amount, d.Salary);
            Assert.Equal(e.Salary.Currency.Code, d.CurrencyCode);
            Assert.Equal(e.Id, d.Id);
            Assert.Equal(e.SocialSecurityNumber.ToString(), d.SocialSecurityNumber);
            Assert.Equal(e.PaidTimeOffPolicy.Id, d.PaidTimeOffPolicyId);
            Assert.Null(d.PaidTimeOffPolicy);
            Assert.Null(d.Currency);
            Assert.NotNull(d.Subordinates);
        }
    }
}