using JDS.OrgManager.Application.Common.TimeOff;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Common.Mapping
{
    public partial class PaidTimeOffRequestDomainEntityToDbEntityMapper
    {
        protected override TypeAdapterSetter<PaidTimeOffRequest, PaidTimeOffRequestEntity> Configure(TypeAdapterSetter<PaidTimeOffRequest, PaidTimeOffRequestEntity> typeAdapterSetter)
            => base.Configure(typeAdapterSetter)
            .Map(dest => dest.ForEmployeeId, src => src.ForEmployee.Id)
            .Map(dest => dest.SubmittedById, src => src.SubmittedBy.Id);
    }
}
