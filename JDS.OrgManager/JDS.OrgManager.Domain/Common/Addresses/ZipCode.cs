// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Common.Text;
using JDS.OrgManager.Domain.Abstractions.Models;
using JDS.OrgManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JDS.OrgManager.Domain.Common.Addresses
{
    public record ZipCode : IValueObject
    {
        private static readonly Regex regex = new Regex(@"^\d{5}(\-|\s*)?(\d{4})?$", RegexOptions.Compiled | RegexOptions.Singleline);

        public ZipCode(string Value)
        {
            var value = string.IsNullOrWhiteSpace(Value) ? throw new ArgumentNullException(nameof(Value)) : Value.Trim();
            if (!IsValid(value))
            {
                throw new FormatException($"Zip code '{Value}' is in an incorrect format.");
            }

            this.Value = value.StripNonDigits();
        }

        public void Deconstruct(out string Value) => Value = this.Value;

        public string Value { get; init; }

        public static explicit operator ZipCode(string value) => new ZipCode(value);

        public static implicit operator string(ZipCode zipCode) => zipCode.Value;

        public override string ToString() => Value.Length == 5 ? Value : Value.Insert(5, "-");

        private static bool IsValid(string value) => regex.IsMatch(value);
    }
}