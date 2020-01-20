// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Models;
using System;
using System.Collections.Generic;

namespace JDS.OrgManager.Domain.Common.Finance
{
    public class Money : ValueObject
    {
        #region Public Properties + Indexers

        public decimal Amount { get; }

        public Currency Currency { get; }

        #endregion

        #region Public Constructors

        public Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }

        #endregion

        #region Public Methods

        public static implicit operator decimal(Money money) => money.Amount;

        public static implicit operator string(Money money) => money.GetShortString();

        public string GetLongString()
        {
            if (Amount == decimal.One)
            {
                return $"1 {Currency.Name}";
            }
            if (decimal.Truncate(Amount) == Amount)
            {
                return $"{Amount:D} {Currency.PluralName}";
            }
            return $"{Amount:F2} {Currency.PluralName}";
        }

        // This is actually a much more complex problem to tackle, which involves I18N, etc. Below is a naive solution since this is a demo app.
        public string GetShortString() => $"{Currency.Symbol}{Amount:F2}";

        public override string ToString() => GetShortString();

        #endregion

        #region Protected Methods

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
            yield return Currency;
        }

        #endregion
    }
}