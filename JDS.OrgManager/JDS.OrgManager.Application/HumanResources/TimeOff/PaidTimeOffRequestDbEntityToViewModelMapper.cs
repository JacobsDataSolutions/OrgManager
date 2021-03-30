using JDS.OrgManager.Application.Common.TimeOff;
using JDS.OrgManager.Application.HumanResources.TimeOff;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Common.Mapping
{
    public partial class PaidTimeOffRequestDbEntityToViewModelMapper
    {
        protected override TypeAdapterSetter<PaidTimeOffRequestEntity, PaidTimeOffRequestViewModel> Configure(TypeAdapterSetter<PaidTimeOffRequestEntity, PaidTimeOffRequestViewModel> typeAdapterSetter)
            => base.Configure(typeAdapterSetter)
            .Map(dest => dest.SubmittedByName, src => src.SubmittedBy != null ? $"{src.SubmittedBy.FirstName} {src.SubmittedBy.LastName}" : "");
    }
}
