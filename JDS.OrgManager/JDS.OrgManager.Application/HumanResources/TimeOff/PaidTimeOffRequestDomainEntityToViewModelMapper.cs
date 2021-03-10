// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.HumanResources.TimeOff;
using JDS.OrgManager.Common.Text;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using Mapster;

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