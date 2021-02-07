// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Domain.Common.Addresses;
using JDS.OrgManager.Domain.Common.Finance;
using JDS.OrgManager.Domain.HumanResources.Employees;
using Mapster;

namespace JDS.OrgManager.Application.Employees
{
    //public class EmployeeDomainToDbEntityMapper : IDomainEntityToDbEntityMapper<Employee, EmployeeEntity>
    //{
    //    private readonly TypeAdapterConfig config = new TypeAdapterConfig();

    //    public EmployeeDomainToDbEntityMapper() => ApplyMappingConfiguration();

    //    public void ApplyMappingConfiguration()
    //    {
    //        // domain -> DB entity
    //        config.NewConfig<Employee, EmployeeEntity>()
    //            .Ignore(dest => dest.Currency)
    //            .Ignore(dest => dest.PaidTimeOffPolicy)
    //            .Ignore(dest => dest.Subordinates)
    //            .Map(dest => dest.Address1, src => src.HomeAddress.Street1)
    //            .Map(dest => dest.Address2, src => src.HomeAddress.Street2)
    //            .Map(dest => dest.City, src => src.HomeAddress.City)
    //            .Map(dest => dest.State, src => src.HomeAddress.State)
    //            .Map(dest => dest.Zip, src => src.HomeAddress.Zip)
    //            .Map(dest => dest.CurrencyCode, src => src.Salary.Currency.Code)
    //            ;

    //        // DB entity -> domain
    //        config.NewConfig<EmployeeEntity, Employee>()
    //            .Ignore(dest => dest.PaidTimeOffPolicy)
    //            .Ignore(dest => dest.Manager)
    //            .Ignore(dest => dest.Subordinates)
    //            .Map(dest => dest.HomeAddress, src => new Address(
    //                src.Address1,
    //                src.City,
    //                new State(src.State, null),
    //                new ZipCode(src.Zip),
    //                src.Address2))
    //            .Map(dest => dest.Salary, src => new Money(
    //                src.Salary,
    //                Currency.GetByCode(src.CurrencyCode)));
    //    }

    //    public EmployeeEntity MapToDbEntity(Employee domainEntity) => domainEntity.Adapt<EmployeeEntity>(config);

    //    public Employee Map(EmployeeEntity dbEntity) => dbEntity.Adapt<Employee>(config);
    //}
}