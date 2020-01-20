// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.HumanResources.PaidTimeOffPolicies.Queries.GetPaidTimeOffPolicyDetail;
using JDS.OrgManager.Application.HumanResources.PaidTimeOffPolicies.Queries.GetPaidTimeOffPolicyList;
using JDS.OrgManager.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.ConsoleApp.PaidTimeOffPolicies
{
    public class PaidTimeOffPolicyController : BaseController
    {
        #region Public Constructors

        public PaidTimeOffPolicyController(IMediator mediator) : base(mediator)
        {
        }

        #endregion

        #region Public Methods

        public async Task<Result<IReadOnlyList<GetPaidTimeOffPolicyListViewModel>>> GetAllPtoPoliciesAsync()
        {
            try
            {
                var result = await Mediator.Send(new GetPaidTimeOffPolicyListQuery());
                return Result<IReadOnlyList<GetPaidTimeOffPolicyListViewModel>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<IReadOnlyList<GetPaidTimeOffPolicyListViewModel>>.Failure(new[] { ex.Message }); ;
            }
        }

        public async Task<Result<GetPaidTimeOffPolicyDetailViewModel>> GetPtoPolicyDetailAsync(int id)
        {
            try
            {
                var result = await Mediator.Send(new GetPaidTimeOffPolicyDetailQuery() { Id = id });
                return Result<GetPaidTimeOffPolicyDetailViewModel>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<GetPaidTimeOffPolicyDetailViewModel>.Failure(new[] { ex.Message }); ;
            }
        }

        #endregion
    }
}