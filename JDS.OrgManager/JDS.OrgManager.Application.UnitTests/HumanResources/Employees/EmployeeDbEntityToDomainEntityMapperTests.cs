using AutoFixture;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Common.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
