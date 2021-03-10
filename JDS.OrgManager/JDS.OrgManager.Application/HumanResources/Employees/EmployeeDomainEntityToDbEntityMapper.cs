// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Domain.HumanResources.Employees;
using Mapster;

namespace JDS.OrgManager.Application.Common.Mapping
{
    public partial class EmployeeDomainEntityToDbEntityMapper
    {
        protected override TypeAdapterSetter<Employee, EmployeeEntity> Configure(TypeAdapterSetter<Employee, EmployeeEntity> typeAdapterSetter)
            => base.Configure(typeAdapterSetter)
                .Ignore(dest => dest.Tenant)
                .Ignore(dest => dest.TenantId)
                .Map(dest => dest.Address1, src => src.HomeAddress.Street1)
                .Map(dest => dest.Address2, src => src.HomeAddress.Street2)
                .Map(dest => dest.City, src => src.HomeAddress.City)
                .Map(dest => dest.State, src => src.HomeAddress.State)
                .Map(dest => dest.ZipCode, src => src.HomeAddress.ZipCode)
                .Map(dest => dest.CurrencyCode, src => src.Salary.Currency.Code)
                ;
    }
}