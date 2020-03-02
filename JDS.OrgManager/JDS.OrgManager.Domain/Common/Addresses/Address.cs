// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Models;
using System;
using System.Collections.Generic;

namespace JDS.OrgManager.Domain.Common.Addresses
{
    public class Address : ValueObject
    {
        public string City { get; }

        public State State { get; }

        public string Street1 { get; }

        public string Street2 { get; }

        public ZipCode Zip { get; }

        public Address(string street1, string city, State state, ZipCode zip, string street2 = null)
        {
            Street1 = street1 ?? throw new ArgumentNullException(nameof(street1));
            City = city ?? throw new ArgumentNullException(nameof(city));
            State = state ?? throw new ArgumentNullException(nameof(state));
            Zip = zip ?? throw new ArgumentNullException(nameof(zip));
            Street2 = string.IsNullOrWhiteSpace(street2) ? "" : street2;
        }

        public override string ToString()
        {
            var apt = !string.IsNullOrWhiteSpace(Street2) ? " " + Street2 : "";
            return $"{Street1}{apt}, {City}, {State} {Zip}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Street1;
            yield return Street2;
            yield return City;
            yield return State;
            yield return Zip;
        }
    }
}