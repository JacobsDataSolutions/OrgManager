using AutoFixture;
using JDS.OrgManager.Application.Common.Mapping;
using JDS.OrgManager.Application.Common.TimeOff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JDS.OrgManager.Application.UnitTests.HumanResources.PaidTimeOffPolicies
{
    public class PaidTimeOffPolicyDbEntityToDomainEntityMapperTests
    {
        private readonly Fixture fixture;

        private readonly PaidTimeOffPolicyDbEntityToDomainEntityMapper mapper;

        public PaidTimeOffPolicyDbEntityToDomainEntityMapperTests()
        {
            mapper = new PaidTimeOffPolicyDbEntityToDomainEntityMapper();

            fixture = new Fixture();
            // client has a circular reference from AutoFixture point of view
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void Map_WorksAsExpected()
        {
            var d = fixture.Create<PaidTimeOffPolicyEntity>();
            var e = mapper.Map(d);
            Assert.Equal(d.Id, e.Id);
            Assert.Equal(d.AllowsUnlimitedPto, e.AllowsUnlimitedPto);
            Assert.Equal(d.EmployeeLevel, e.EmployeeLevel);
            Assert.Equal(d.IsDefaultForEmployeeLevel, e.IsDefaultForEmployeeLevel);
            Assert.Equal(d.MaxPtoHours, e.MaxPtoHours);
            Assert.Equal(d.Name, e.Name);
            Assert.Equal(d.PtoAccrualRate, e.PtoAccrualRate);
        }
    }
}
