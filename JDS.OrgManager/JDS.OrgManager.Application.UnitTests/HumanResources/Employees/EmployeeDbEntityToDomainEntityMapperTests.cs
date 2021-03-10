// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using AutoFixture;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Common.Mapping;
using Xunit;

namespace JDS.OrgManager.Application.UnitTests.HumanResources.Employees
{
    public class EmployeeDbEntityToDomainEntityMapperTests
    {
        private readonly Fixture fixture;

        private readonly EmployeeDbEntityToDomainEntityMapper mapper;

        public EmployeeDbEntityToDomainEntityMapperTests()
        {
            mapper = new EmployeeDbEntityToDomainEntityMapper(new AddressDbEntityToValueObjectMapper());

            fixture = new Fixture();
            // client has a circular reference from AutoFixture point of view
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void Map_WorksAsExpected()
        {
            var d = fixture.Create<EmployeeEntity>();
            d.ZipCode = "99999-9999";
            d.CurrencyCode = "USD";
            d.SocialSecurityNumber = "999-99-9999";

            var e = mapper.Map(d);
            Assert.Equal(d.DateTerminated, e.DateTerminated);
            Assert.Equal(d.DateHired, e.DateHired);
            Assert.Equal(d.DateOfBirth, e.DateOfBirth);
            Assert.Equal(d.EmployeeLevel, e.EmployeeLevel);
            Assert.Equal(d.FirstName, e.FirstName);
            Assert.Equal(d.Gender, e.Gender);
            Assert.Equal(d.City, e.HomeAddress.City);
            Assert.Equal(d.State, e.HomeAddress.State);
            Assert.Equal(d.Address1, e.HomeAddress.Street1);
            Assert.Equal(d.Address2, e.HomeAddress.Street2);
            Assert.Equal(d.ZipCode, e.HomeAddress.ZipCode.ToString());
            Assert.Equal(d.LastName, e.LastName);
            Assert.Equal(d.MiddleName, e.MiddleName);
            Assert.Equal(d.PtoHoursRemaining, e.PtoHoursRemaining);
            Assert.Equal(d.Salary, e.Salary.Amount);
            Assert.Equal(d.CurrencyCode, e.Salary.Currency.Code);
            Assert.Equal(d.Id, e.Id);
            Assert.Null(e.PaidTimeOffPolicy);
            Assert.NotNull(e.Subordinates);
        }
    }
}