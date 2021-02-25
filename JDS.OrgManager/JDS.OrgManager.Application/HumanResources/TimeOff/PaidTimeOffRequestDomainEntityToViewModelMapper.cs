using JDS.OrgManager.Application.HumanResources.TimeOff;
using JDS.OrgManager.Common.Text;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Common.Mapping
{
    public partial class PaidTimeOffRequestDomainEntityToViewModelMapper
    {
        protected override TypeAdapterSetter<PaidTimeOffRequest, PaidTimeOffRequestViewModel> Configure(TypeAdapterSetter<PaidTimeOffRequest, PaidTimeOffRequestViewModel> typeAdapterSetter)
            => base.Configure(typeAdapterSetter)
            .Map(dest => dest.ForEmployeeId, src => src.ForEmployee.Id)
            .Map(dest => dest.SubmittedById, src => src.SubmittedBy.Id)
            .Map(dest => dest.ForEmployeeName, src => StringHelper.GetFullName(src.ForEmployee.FirstName, src.ForEmployee.MiddleName, src.ForEmployee.LastName))
            .Map(dest => dest.SubmittedByName, src => StringHelper.GetFullName(src.SubmittedBy.FirstName, src.SubmittedBy.MiddleName, src.SubmittedBy.LastName));
    }
}
