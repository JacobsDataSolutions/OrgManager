// Copyright �2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Abstractions.Models;

namespace JDS.OrgManager.Domain.Common.Addresses
{
    public record State(string Abbreviation, string? Name = default) : IValueObject
    {
        public static explicit operator State(string value) => new State(value);

        public static implicit operator string(State state) => state.Abbreviation;

        public override string ToString() => Abbreviation;
    }
}