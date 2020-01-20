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
        #region Public Properties + Indexers

        public Address AddressOnFile { get; private set; }

        public DateTime CheckDate { get; private set; }

        public int CheckNumber { get; private set; }

        public Money ContributionTo401k { get; private set; }

        public Money FederalMedicare { get; private set; }

        public Money FederalOasdi { get; private set; }

        public Money FederalWithholding { get; private set; }

        public Money GrossPay { get; private set; }

        public Money NetPay { get; private set; }

        public string RecipientName { get; private set; }

        public Money StateWithholding { get; private set; }

        #endregion

        #region Public Constructors

        public Paycheck(
            Address addressOnFile,
            DateTime checkDate,
            int checkNumber,
            Money contributionTo401k,
            Money federalMedicare,
            Money federalOasdi,
            Money federalWithholding,
            Money grossPay,
            Money netPay,
            string recipientName,
            Money stateWithholding)
        {
            AddressOnFile = addressOnFile ?? throw new ArgumentNullException(nameof(addressOnFile));
            CheckDate = checkDate;
            CheckNumber = checkNumber;
            ContributionTo401k = contributionTo401k ?? throw new ArgumentNullException(nameof(contributionTo401k));
            FederalMedicare = federalMedicare ?? throw new ArgumentNullException(nameof(federalMedicare));
            FederalOasdi = federalOasdi ?? throw new ArgumentNullException(nameof(federalOasdi));
            FederalWithholding = federalWithholding ?? throw new ArgumentNullException(nameof(federalWithholding));
            GrossPay = grossPay ?? throw new ArgumentNullException(nameof(grossPay));
            NetPay = netPay ?? throw new ArgumentNullException(nameof(netPay));
            RecipientName = recipientName ?? throw new ArgumentNullException(nameof(recipientName));
            StateWithholding = stateWithholding ?? throw new ArgumentNullException(nameof(stateWithholding));
        }

        #endregion
    }
}