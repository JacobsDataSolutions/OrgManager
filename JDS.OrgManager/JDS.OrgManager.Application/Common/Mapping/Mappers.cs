using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Common.Addresses;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Common.TimeOff;
using JDS.OrgManager.Application.Customers;
using JDS.OrgManager.Application.HumanResources.Employees;
using JDS.OrgManager.Application.HumanResources.TimeOff;
using JDS.OrgManager.Application.HumanResources.TimeOff.Queries.ValidateRequestedPaidTimeOffHours;
using JDS.OrgManager.Application.Tenants;
using JDS.OrgManager.Domain.Common.Addresses;
using JDS.OrgManager.Domain.HumanResources.Employees;
using JDS.OrgManager.Domain.HumanResources.TimeOff;

namespace JDS.OrgManager.Application.Common.Mapping
{
    public partial class AddressDbEntityToValueObjectMapper : MapperBase<IAddressEntity, Address>, IDbEntityToValueObjectMapper<IAddressEntity, Address>
    { }

    public partial class AddressViewModelToValueObjectMapper : MapperBase<IAddressViewModel, Address>, IViewModelToValueObjectMapper<IAddressViewModel, Address>
    { }

    public partial class CustomerViewModelToDbEntityMapper : MapperBase<CustomerViewModel, CustomerEntity>, IViewModelToDbEntityMapper<CustomerViewModel, CustomerEntity>
    { }

    public partial class EmployeeDbEntityToDomainEntityMapper : MapperBase<EmployeeEntity, Employee>, IDbEntityToDomainEntityMapper<EmployeeEntity, Employee>
    { }

    public partial class EmployeeDomainEntityToDbEntityMapper : MapperBase<Employee, EmployeeEntity>, IDomainEntityToDbEntityMapper<Employee, EmployeeEntity>
    { }

    public partial class EmployeeDomainEntityToViewModelMapper : MapperBase<Employee, EmployeeViewModel>, IDomainEntityToViewModelMapper<Employee, EmployeeViewModel>
    { }

    public partial class EmployeeViewModelToDomainEntityMapper : MapperBase<EmployeeViewModel, Employee>, IViewModelToDomainEntityMapper<EmployeeViewModel, Employee>
    { }

    public partial class PaidTimeOffPolicyDbEntityToDomainEntityMapper : MapperBase<PaidTimeOffPolicyEntity, PaidTimeOffPolicy>, IDbEntityToDomainEntityMapper<PaidTimeOffPolicyEntity, PaidTimeOffPolicy>
    { }

    public partial class PaidTimeOffPolicyDomainEntityToDbEntityMapper : MapperBase<PaidTimeOffPolicy, PaidTimeOffPolicyEntity>, IDomainEntityToDbEntityMapper<PaidTimeOffPolicy, PaidTimeOffPolicyEntity>
    { }

    public partial class PaidTimeOffRequestDbEntityToDomainEntityMapper : MapperBase<PaidTimeOffRequestEntity, PaidTimeOffRequest>, IDbEntityToDomainEntityMapper<PaidTimeOffRequestEntity, PaidTimeOffRequest>
    { }

    public partial class PaidTimeOffRequestDbEntityToViewModelMapper : MapperBase<PaidTimeOffRequestEntity, PaidTimeOffRequestViewModel>, IDbEntityToViewModelMapper<PaidTimeOffRequestEntity, PaidTimeOffRequestViewModel>
    { }

    public partial class PaidTimeOffRequestDomainEntityToDbEntityMapper : MapperBase<PaidTimeOffRequest, PaidTimeOffRequestEntity>, IDomainEntityToDbEntityMapper<PaidTimeOffRequest, PaidTimeOffRequestEntity>
    { }

    public partial class PaidTimeOffRequestDomainEntityToViewModelMapper : MapperBase<PaidTimeOffRequest, PaidTimeOffRequestViewModel>, IDomainEntityToViewModelMapper<PaidTimeOffRequest, PaidTimeOffRequestViewModel>
    { }

    public partial class TenantViewModelToDbEntityMapper : MapperBase<TenantViewModel, TenantEntity>, IViewModelToDbEntityMapper<TenantViewModel, TenantEntity>
    { }

    public partial class ValidateRequestedPaidTimeOffHoursViewModelToDomainEntityMapper : MapperBase<ValidateRequestedPaidTimeOffHoursViewModel, PaidTimeOffRequest>, IViewModelToDomainEntityMapper<ValidateRequestedPaidTimeOffHoursViewModel, PaidTimeOffRequest>
    { }
}