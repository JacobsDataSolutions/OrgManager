// Copyright �2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Abstractions.Models;
using System;
using System.Text.RegularExpressions;

namespace JDS.OrgManager.Domain.Common.People
{
    public record SocialSecurityNumber : IValueObject
    {
        private static readonly Regex nonDigitsRegex = new Regex(@"\D", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex regex = new Regex(@"^\d{3}(\-|\s*)?\d{2}(\-|\s*)?(\d{4})?$", RegexOptions.Compiled | RegexOptions.Singleline);

        public string Value { get; init; }

        public SocialSecurityNumber(string Value)
        {
            var value = string.IsNullOrWhiteSpace(Value) ? throw new ArgumentNullException(nameof(Value)) : Value.Trim();
            if (!IsValid(value))
            {
                throw new FormatException($"SSN is in an incorrect format.");
            }
            this.Value = nonDigitsRegex.Replace(value, "");
        }

        public void Deconstruct(out string Value) => Value = this.Value;

        public static explicit operator SocialSecurityNumber(string value) => new SocialSecurityNumber(value);

        public static implicit operator string(SocialSecurityNumber socialSecurityNumber) => socialSecurityNumber.ToString();

        public override string ToString() => Value.Insert(3, "-").Insert(6, "-");

        private static bool IsValid(string value) => regex.IsMatch(value);
    }
}