//// Copyright ©2020 Jacobs Data Solutions

//// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
//// License at

//// http://www.apache.org/licenses/LICENSE-2.0

//// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
//// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
//using JDS.OrgManager.Application.Abstractions.Mapping;
//using JDS.OrgManager.Domain.Common.Addresses;
//using JDS.OrgManager.Domain.Common.Finance;
//using JDS.OrgManager.Domain.HumanResources.Employees;
//using Mapster;
//using System.Linq;

//namespace JDS.OrgManager.Application.HumanResources.Employees.Commands.RegisterOrUpdateEmployee
//{
//    public class RegisterOrUpdateEmployeeDomainEntityMapper : IViewModelToDomainEntityMapper<RegisterOrUpdateEmployeeCommand, Employee>
//    {
//        private readonly TypeAdapterConfig config = new TypeAdapterConfig();

//        public RegisterOrUpdateEmployeeDomainEntityMapper() => ApplyMappingConfiguration();

//        public void ApplyMappingConfiguration()
//        {
//            // VM -> domain entity
//            config.NewConfig<RegisterOrUpdateEmployeeCommand, Employee>()
//                .Map(dest => dest.Salary, src => new Money(
//                    src.Salary,
//                    Currency.GetByCode(src.CurrencyCode)))
//                .Map(dest => dest.HomeAddress, src => new Address(
//                    src.Address1,
//                    src.City,
//                    new State(src.State, null),
//                    new ZipCode(src.Zip),
//                    src.Address2
//                ));

//            // domain entity -> VM
//            config.NewConfig<Employee, RegisterOrUpdateEmployeeCommand>()
//                .Ignore(dest => dest.SubordinateIds)
//                .Map(dest => dest.Address1, src => src.HomeAddress.Street1)
//                .Map(dest => dest.Address2, src => src.HomeAddress.Street2)
//                .Map(dest => dest.City, src => src.HomeAddress.City)
//                .Map(dest => dest.State, src => src.HomeAddress.State)
//                .Map(dest => dest.Zip, src => src.HomeAddress.Zip)
//                .Map(dest => dest.CurrencyCode, src => src.Salary.Currency.Code)
//                .Map(dest => dest.PaidTimeOffPolicyId, src => (int)src.PaidTimeOffPolicy.Id)
//                .Map(dest => dest.SocialSecurityNumber, src => src.SocialSecurityNumber.ToString())
//                ;
//        }

//        public Employee Map(RegisterOrUpdateEmployeeCommand viewModel)
//        {
//            return viewModel.Adapt<Employee>(config);
//        }

//        public RegisterOrUpdateEmployeeCommand MapToViewModel(Employee domainEntity)
//        {
//            var vm = domainEntity.Adapt<RegisterOrUpdateEmployeeCommand>(config);
//            vm.ManagerId = domainEntity.Manager?.Id;
//            vm.SubordinateIds = (from e in domainEntity.Subordinates select (int)e.Id).ToList();
//            return vm;
//        }
//    }
//}