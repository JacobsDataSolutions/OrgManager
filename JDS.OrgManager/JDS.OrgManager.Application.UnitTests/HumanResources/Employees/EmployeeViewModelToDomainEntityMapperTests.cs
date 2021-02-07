// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using AutoFixture;
using JDS.OrgManager.Application.Common.Mapping;
using JDS.OrgManager.Application.HumanResources.Employees;
using JDS.OrgManager.Application.HumanResources.Employees.Commands.RegisterOrUpdateEmployee;
using JDS.OrgManager.Utils;
using System.Linq;
using Xunit;

namespace JDS.OrgManager.Application.UnitTests.HumanResources.Employees
{
    public class EmployeeViewModelToDomainEntityMapperTests
    {
        private Fixture fixture;

        private EmployeeViewModelToDomainEntityMapper mapper;

        public EmployeeViewModelToDomainEntityMapperTests()
        {
            mapper = new EmployeeViewModelToDomainEntityMapper(new AddressViewModelToValueObjectMapper());

            fixture = new Fixture();
            // client has a circular reference from AutoFixture point of view
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void Map_WorksAsExpected()
        {
            var v = fixture.Create<EmployeeViewModel>();
            v.ZipCode = "99999-9999";
            v.CurrencyCode = "USD";
            v.SocialSecurityNumber = "999-99-9999";

            var e = mapper.Map(v);
            Assert.Equal(v.DateTerminated, e.DateTerminated);
            Assert.Equal(v.DateHired, e.DateHired);
            Assert.Equal(v.DateOfBirth, e.DateOfBirth);
            Assert.Equal(v.EmployeeLevel, e.EmployeeLevel);
            Assert.Equal(v.FirstName, e.FirstName);
            Assert.Equal(v.Gender, e.Gender);
            Assert.Equal(v.City, e.HomeAddress.City);
            Assert.Equal(v.State, e.HomeAddress.State);
            Assert.Equal(v.Address1, e.HomeAddress.Street1);
            Assert.Equal(v.Address2, e.HomeAddress.Street2);
            Assert.Equal(v.ZipCode, e.HomeAddress.ZipCode.ToString());
            Assert.Equal(v.LastName, e.LastName);
            Assert.Equal(v.MiddleName, e.MiddleName);
            Assert.Equal(v.PtoHoursRemaining, e.PtoHoursRemaining);
            Assert.Equal(v.Salary, e.Salary.Amount);
            Assert.Equal(v.CurrencyCode, e.Salary.Currency.Code);
            Assert.Equal(v.Id, e.Id);
            Assert.Null(e.PaidTimeOffPolicy);
            Assert.NotNull(e.Subordinates);
        }
    }
}