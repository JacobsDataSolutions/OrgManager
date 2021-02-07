// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Common.Addresses;
using JDS.OrgManager.Domain.Common.Finance;
using JDS.OrgManager.Domain.Models;
using System;

namespace JDS.OrgManager.Domain.Accounting.Pay
{
    public class Paycheck : DomainEntity<Paycheck>
    {
        public Address AddressOnFile { get; init; }

        public DateTime CheckDate { get; init; }

        public int CheckNumber { get; init; }

        public Money ContributionTo401k { get; init; }

        public Money FederalMedicare { get; init; }

        public Money FederalOasdi { get; init; }

        public Money FederalWithholding { get; init; }

        public Money GrossPay { get; init; }

        public Money NetPay { get; init; }

        public string RecipientName { get; init; }

        public Money StateWithholding { get; init; }
    }
}