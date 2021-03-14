// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using AutoFixture;
using JDS.OrgManager.Application.Common.Mapping;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using Xunit;

namespace JDS.OrgManager.Application.UnitTests.Common.PaidTimeOffPolicies
{
    public class PaidTimeOffPolicyDomainEntityToDbEntityMapperTests
    {
        private readonly Fixture fixture;

        private readonly PaidTimeOffPolicyDomainEntityToDbEntityMapper mapper;

        public PaidTimeOffPolicyDomainEntityToDbEntityMapperTests()
        {
            mapper = new PaidTimeOffPolicyDomainEntityToDbEntityMapper();

            fixture = new Fixture();
            // client has a circular reference from AutoFixture point of view
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void Map_WorksAsExpected()
        {
            var p = new PaidTimeOffPolicy
            { 
                AllowsUnlimitedPto = true,
                EmployeeLevel = 6,
                IsDefaultForEmployeeLevel = true,
                MaxPtoHours = 55.0m,
                Name = "HELLO",
                PtoAccrualRate = 10.0M,
                Id = 67
            };
            var d = mapper.Map(p);
            Assert.Equal(p.Id, d.Id);
            Assert.Equal(p.AllowsUnlimitedPto, d.AllowsUnlimitedPto);
            Assert.Equal(p.EmployeeLevel, d.EmployeeLevel);
            Assert.Equal(p.IsDefaultForEmployeeLevel, d.IsDefaultForEmployeeLevel);
            Assert.Equal(p.MaxPtoHours, d.MaxPtoHours);
            Assert.Equal(p.Name, d.Name);
            Assert.Equal(p.PtoAccrualRate, d.PtoAccrualRate);
        }
    }
}