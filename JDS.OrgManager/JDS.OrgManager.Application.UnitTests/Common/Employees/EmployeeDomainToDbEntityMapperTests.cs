// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using AutoFixture;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Utils;
using Xunit;

namespace JDS.OrgManager.Application.UnitTests.Common.Employees
{
    public class EmployeeDomainToDbEntityMapperTests
    {
        private Fixture fixture;

        private EmployeeDomainToDbEntityMapper mapper;

        public EmployeeDomainToDbEntityMapperTests()
        {
            mapper = new EmployeeDomainToDbEntityMapper();
            mapper.ApplyMappingConfiguration();

            fixture = new Fixture();
            // client has a circular reference from AutoFixture point of view
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void MapToDbEntity_WorksAsExpected()
        {
            var e = UnitTestEmployeeGenerator.GenerateEmployee(3);
            var d = mapper.MapToDbEntity(e);
            Assert.Equal(e.DateExited, d.DateExited);
            Assert.Equal(e.DateHired, d.DateHired);
            Assert.Equal(e.DateOfBirth, d.DateOfBirth);
            Assert.Equal(e.EmployeeLevel, d.EmployeeLevel);
            Assert.Equal(e.FirstName, d.FirstName);
            Assert.Equal(e.Gender, d.Gender);
            Assert.Equal(e.HomeAddress.City, d.City);
            Assert.Equal(e.HomeAddress.State.Abbreviation, d.State);
            Assert.Equal(e.HomeAddress.Street1, d.Address1);
            Assert.Equal(e.HomeAddress.Street2, d.Address2);
            Assert.Equal(e.HomeAddress.Zip.ToString(), d.Zip);
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

        [Fact]
        public void MapToDomainEntity_WorksAsExpected()
        {
            var d = fixture.Create<EmployeeEntity>();
            d.Zip = "99999-9999";
            d.CurrencyCode = "USD";
            d.SocialSecurityNumber = "999-99-9999";

            var e = mapper.MapToDomainEntity(d);
            Assert.Equal(d.DateExited, e.DateExited);
            Assert.Equal(d.DateHired, e.DateHired);
            Assert.Equal(d.DateOfBirth, e.DateOfBirth);
            Assert.Equal(d.EmployeeLevel, e.EmployeeLevel);
            Assert.Equal(d.FirstName, e.FirstName);
            Assert.Equal(d.Gender, e.Gender);
            Assert.Equal(d.City, e.HomeAddress.City);
            Assert.Equal(d.State, e.HomeAddress.State);
            Assert.Equal(d.Address1, e.HomeAddress.Street1);
            Assert.Equal(d.Address2, e.HomeAddress.Street2);
            Assert.Equal(d.Zip, e.HomeAddress.Zip.ToString());
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