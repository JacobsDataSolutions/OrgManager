// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Common.Text;
using JDS.OrgManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JDS.OrgManager.Domain.Common.Addresses
{
    public class ZipCode : ValueObject
    {
        private static readonly Regex regex = new Regex(@"^\d{5}(\-|\s*)?(\d{4})?$", RegexOptions.Compiled | RegexOptions.Singleline);

        public string Value { get; }

        public ZipCode(string value)
        {
            // perform regex matching to verify XXXXX or XXXXX-XXXX format
            value = value?.Trim();
            if (!IsValid(value))
            {
                throw new FormatException($"Zip code '{value}' is in an incorrect format.");
            }
            Value = value.StripNonDigits();
        }

        public static explicit operator ZipCode(string value) => new ZipCode(value);

        public static implicit operator string(ZipCode zipCode) => zipCode.Value;

        public override string ToString() => Value.Length == 5 ? Value : Value.Insert(5, "-");

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        private bool IsValid(string value) => regex.IsMatch(value);
    }
}