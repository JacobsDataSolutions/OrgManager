using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Domain.HumanResources.Employees;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
