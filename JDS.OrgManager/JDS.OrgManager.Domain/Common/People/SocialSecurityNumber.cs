// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JDS.OrgManager.Domain.Common.People
{
    public class SocialSecurityNumber : ValueObject
    {
        #region Private Fields

        private static readonly Regex nonDigitsRegex = new Regex(@"\D", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex regex = new Regex(@"^\d{3}(\-|\s*)?\d{2}(\-|\s*)?(\d{4})?$", RegexOptions.Compiled | RegexOptions.Singleline);

        #endregion

        #region Public Properties + Indexers

        public string Value { get; }

        #endregion

        #region Public Constructors

        public SocialSecurityNumber(string value)
        {
            // perform regex matching to verify XXXXXXXXX or XXX-XX-XXXX format
            value = value?.Trim();
            if (!IsValid(value))
            {
                throw new FormatException($"SSN is in an incorrect format.");
            }
            Value = nonDigitsRegex.Replace(value, "");
        }

        #endregion

        #region Public Methods

        public static explicit operator SocialSecurityNumber(string value) => new SocialSecurityNumber(value);

        public static implicit operator string(SocialSecurityNumber socialSecurityNumber) => socialSecurityNumber.ToString();

        public override string ToString() => Value.Insert(3, "-").Insert(6, "-");

        #endregion

        #region Protected Methods

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        #endregion

        #region Private Methods

        private bool IsValid(string value) => regex.IsMatch(value);

        #endregion
    }
}