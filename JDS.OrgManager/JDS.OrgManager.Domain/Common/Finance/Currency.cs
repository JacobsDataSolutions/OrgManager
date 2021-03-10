// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JDS.OrgManager.Domain.Common.Finance
{
    public record Currency(string Code, string Name, string PluralName, char Symbol) : IValueObject
    {
        public static readonly Currency BTC = new Currency("BTC", "Bitcoin", "Bitcoins", '₿');

        public static readonly Currency EUR = new Currency("EUR", "Euro", "Euros", '€');

        public static readonly Currency GBP = new Currency("GBP", "Pound sterling", "Pounds sterling", '£');

        public static readonly Currency USD = new Currency("USD", "Dollar", "Dollars", '$');

        private static readonly IReadOnlyDictionary<string, Currency> lookup = new ReadOnlyDictionary<string, Currency>((from c in new[] { BTC, EUR, GBP, USD } select (c.Code, c)).ToDictionary(tup => tup.Code, tup => tup.c));

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
            catch (ArgumentNullException)
            {
                throw new CurrencyException($"Currency code cannot be null.");
            }
        }
    }
}