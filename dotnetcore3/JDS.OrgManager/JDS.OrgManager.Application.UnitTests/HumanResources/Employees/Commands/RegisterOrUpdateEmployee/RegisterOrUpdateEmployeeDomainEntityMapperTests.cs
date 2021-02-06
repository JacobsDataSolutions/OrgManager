// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using AutoFixture;
using JDS.OrgManager.Application.HumanResources.Employees.Commands.RegisterOrUpdateEmployee;
using JDS.OrgManager.Utils;
using System.Linq;
using Xunit;

namespace JDS.OrgManager.Application.UnitTests.HumanResources.Employees.Commands.RegisterOrUpdateEmployee
{
    public class RegisterOrUpdateEmployeeDomainEntityMapperTests
    {
        private Fixture fixture;

        private RegisterOrUpdateEmployeeDomainEntityMapper mapper;

        public RegisterOrUpdateEmployeeDomainEntityMapperTests()
        {
            mapper = new RegisterOrUpdateEmployeeDomainEntityMapper();
            mapper.ApplyMappingConfiguration();

            fixture = new Fixture();
            // client has a circular reference from AutoFixture point of view
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void MapToDomainEntity_WorksAsExpected()
        {
            var v = fixture.Create<RegisterOrUpdateEmployeeCommand>();
            v.Zip = "99999-9999";
            v.CurrencyCode = "USD";
            v.SocialSecurityNumber = "999-99-9999";

            var e = mapper.MapToDomainEntity(v);
            Assert.Equal(v.DateExited, e.DateTerminated);
            Assert.Equal(v.DateHired, e.DateHired);
            Assert.Equal(v.DateOfBirth, e.DateOfBirth);
            Assert.Equal(v.EmployeeLevel, e.EmployeeLevel);
            Assert.Equal(v.FirstName, e.FirstName);
            Assert.Equal(v.Gender, e.Gender);
            Assert.Equal(v.City, e.HomeAddress.City);
            Assert.Equal(v.State, e.HomeAddress.State);
            Assert.Equal(v.Address1, e.HomeAddress.Street1);
            Assert.Equal(v.Address2, e.HomeAddress.Street2);
            Assert.Equal(v.Zip, e.HomeAddress.Zip.ToString());
            Assert.Equal(v.LastName, e.LastName);
            Assert.Equal(v.MiddleName, e.MiddleName);
            Assert.Equal(v.PtoHoursRemaining, e.PtoHoursRemaining);
            Assert.Equal(v.Salary, e.Salary.Amount);
            Assert.Equal(v.CurrencyCode, e.Salary.Currency.Code);
            Assert.Equal(v.Id, e.Id);
            Assert.Null(e.PaidTimeOffPolicy);
            Assert.NotNull(e.Subordinates);
        }

        [Fact]
        public void MapToViewModel_WorksAsExpected()
        {
            var e = UnitTestEmployeeGenerator.GenerateEmployee(3).Subordinates.First();
            var v = mapper.MapToViewModel(e);
            Assert.Equal(e.DateTerminated, v.DateExited);
            Assert.Equal(e.DateHired, v.DateHired);
            Assert.Equal(e.DateOfBirth, v.DateOfBirth);
            Assert.Equal(e.EmployeeLevel, v.EmployeeLevel);
            Assert.Equal(e.FirstName, v.FirstName);
            Assert.Equal(e.Gender, v.Gender);
            Assert.Equal(e.HomeAddress.City, v.City);
            Assert.Equal(e.HomeAddress.State.Abbreviation, v.State);
            Assert.Equal(e.HomeAddress.Street1, v.Address1);
            Assert.Equal(e.HomeAddress.Street2, v.Address2);
            Assert.Equal(e.HomeAddress.Zip.ToString(), v.Zip);
            Assert.Equal(e.LastName, v.LastName);
            Assert.Equal(e.MiddleName, v.MiddleName);
            Assert.Equal(e.PtoHoursRemaining, v.PtoHoursRemaining);
            Assert.Equal(e.Salary.Amount, v.Salary);
            Assert.Equal(e.Salary.Currency.Code, v.CurrencyCode);
            Assert.Equal(e.Id, v.Id);
            Assert.Equal(e.PaidTimeOffPolicy.Id, v.PaidTimeOffPolicyId);
            Assert.Equal(e.SocialSecurityNumber, v.SocialSecurityNumber);
            Assert.Equal(e.Manager.Id, v.ManagerId);
            var subIds = v.SubordinateIds;
            Assert.True(e.Subordinates.All(s => subIds.Contains((int)s.Id)));
        }
    }
}