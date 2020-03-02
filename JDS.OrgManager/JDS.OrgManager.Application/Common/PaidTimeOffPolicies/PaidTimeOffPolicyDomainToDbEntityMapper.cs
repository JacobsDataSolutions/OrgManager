// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Domain.HumanResources.PaidTimeOffPolicies;
using Mapster;

namespace JDS.OrgManager.Application.Common.PaidTimeOffPolicies
{
    public class PaidTimeOffPolicyDomainToDbEntityMapper : IDomainEntityToDbEntityMapper<PaidTimeOffPolicy, PaidTimeOffPolicyEntity>
    {
        private readonly TypeAdapterConfig config = new TypeAdapterConfig();

        public PaidTimeOffPolicyDomainToDbEntityMapper() => ApplyMappingConfiguration();

        public void ApplyMappingConfiguration()
        {
        }

        public PaidTimeOffPolicyEntity MapToDbEntity(PaidTimeOffPolicy domainEntity) => domainEntity.Adapt<PaidTimeOffPolicyEntity>(config);

        public PaidTimeOffPolicy MapToDomainEntity(PaidTimeOffPolicyEntity dbEntity) => dbEntity.Adapt<PaidTimeOffPolicy>(config);
    }
}