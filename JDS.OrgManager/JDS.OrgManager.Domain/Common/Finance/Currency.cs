// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JDS.OrgManager.Domain.Common.Finance
{
    public class Currency : ValueObject
    {
        #region Public Fields

        public static readonly Currency BTC = new Currency("BTC", "Bitcoin", "Bitcoins", '₿');

        public static readonly Currency EUR = new Currency("EUR", "Euro", "Euros", '€');

        public static readonly Currency GBP = new Currency("GBP", "Pound sterling", "Pounds sterling", '£');

        public static readonly Currency USD = new Currency("USD", "Dollar", "Dollars", '$');

        #endregion

        #region Private Fields

        private static readonly IReadOnlyDictionary<string, Currency> lookup;

        #endregion

        #region Public Properties + Indexers

        public string Code { get; }

        public string Name { get; }

        public string PluralName { get; }

        public char Symbol { get; }

        #endregion

        #region Public Constructors

        static Currency() => lookup = new ReadOnlyDictionary<string, Currency>((from c in new[] { BTC, EUR, GBP, USD } select (c.Code, c)).ToDictionary(tup => tup.Code, tup => tup.c));

        public Currency(string code, string name, string pluralName, char symbol)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(pluralName))
            {
                throw new ArgumentNullException(nameof(pluralName));
            }
            Code = code.Trim().ToUpper();
            Name = name.Trim();
            PluralName = pluralName.Trim();
            Symbol = symbol;
        }

        #endregion

        #region Public Methods

        public static IReadOnlyList<Currency> GetAll() => lookup.Values.OrderBy(c => c.Code).ToList();

        public static Currency GetByCode(string code)
        {
            try
            {
                return lookup[code];
            }
            catch (KeyNotFoundException ex)
            {
                throw new CurrencyException($"Attempted to get a currency with invalid code: '{code}'.", ex);
            }
        }

        #endregion

        #region Protected Methods

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
        }

        #endregion
    }
}