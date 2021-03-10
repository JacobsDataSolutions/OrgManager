// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Common.Addresses;
using Mapster;
using System.ComponentModel;
using Xunit;

namespace JDS.OrgManager.Domain.UnitTests
{
    public class MapsterTests
    {
        [Fact]
        public void CanMapToCSharp9RecordTypeWithConstructorSyntax()
        {
            var person = new PersonTestClass { FirstName = "John", MiddleName = "Paul", LastName = "Doe" };
            var record = person.Adapt<CSharp9RecordTypeWithConstructorSyntax>();
            Assert.Equal("John", record.FirstName);
            Assert.Equal("Paul", record.MiddleName);
            Assert.Equal("Doe", record.LastName);
        }

        [Fact]
        public void CanMapToCSharp9RecordTypeWithGetPropertiesAndConstructor()
        {
            var person = new PersonTestClass { FirstName = "John", MiddleName = "Paul", LastName = "Doe" };
            var record = person.Adapt<CSharp9RecordTypeWithGetPropertiesAndConstructor>();
            Assert.Equal("John", record.FirstName);
            Assert.Equal("Paul", record.MiddleName);
            Assert.Equal("Doe", record.LastName);
        }

        [Fact]
        public void CanMapToCSharp9RecordTypeWithInitProperties()
        {
            var person = new PersonTestClass { FirstName = "John", MiddleName = "Paul", LastName = "Doe" };
            var record = person.Adapt<CSharp9RecordTypeWithInitProperties>();
            Assert.Equal("John", record.FirstName);
            Assert.Equal("Paul", record.MiddleName);
            Assert.Equal("Doe", record.LastName);
        }

        [Fact]
        [Description("This demonstrates that Mapster is smart enough to map to complex property types _if and only if_ the DTO properties have the same name as the constructor parameters.")]
        public void CanMapValueObjectsAsRecordTypesWithConstructorInitialization()
        {
            var dto = new
            {
                Street2 = "APT 999",
                City = "CHICAGO",
                State = "IL",
                Street1 = "1111 N WELLS",
                ZipCode = "60606"
            };
            var address = dto.Adapt<Address>();
            Assert.Equal(dto.Street1, address.Street1);
            Assert.Equal(dto.Street2, address.Street2);
            Assert.Equal(dto.City, address.City);
            Assert.Equal(dto.State, address.State.Abbreviation);
            Assert.Equal(dto.ZipCode, address.ZipCode.Value);
        }
    }

    public class PersonTestClass
    {
        public string FirstName { get; init; } = default!;

        public string LastName { get; init; } = default!;

        public string? MiddleName { get; init; }
    }

    public record CSharp9RecordTypeWithConstructorSyntax(string FirstName, string LastName, string? MiddleName = default) { }

    public record CSharp9RecordTypeWithInitProperties
    {
        public string FirstName { get; init; } = default!;
        public string? MiddleName { get; init; }
        public string LastName { get; init; } = default!;
    }

    public record CSharp9RecordTypeWithGetPropertiesAndConstructor
    {
        public string FirstName { get; }
        public string? MiddleName { get; }
        public string LastName { get; }

        public CSharp9RecordTypeWithGetPropertiesAndConstructor(string firstName, string lastName, string? middleName = default)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }
    }
}