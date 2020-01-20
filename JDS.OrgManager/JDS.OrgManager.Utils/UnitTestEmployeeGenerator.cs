// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Common.Addresses;
using JDS.OrgManager.Domain.Common.Finance;
using JDS.OrgManager.Domain.Common.People;
using JDS.OrgManager.Domain.HumanResources.Employees;
using JDS.OrgManager.Domain.HumanResources.PaidTimeOffPolicies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JDS.OrgManager.Utils
{
    public static class UnitTestEmployeeGenerator
    {
        #region Private Fields

        private static readonly Random random = new Random();

        #endregion

        #region Public Methods

        public static Employee GenerateEmployee(int employeeLevel, bool? isMale = null)
        {
            var (addr1, addr2) = DummyData.GetRandomChitownStreet();
            var zip = DummyData.GetRandomChitownZip();
            var dateHired = DummyData.GetRandomHireDate();
            var male = isMale ?? DummyData.CoinToss();

            var employee = new Employee(
                dateHired: DummyData.GetRandomHireDate(),
                dateOfBirth: DummyData.GetRandomBirthDate(),
                employeeLevel: employeeLevel,
                firstName: DummyData.GenerateFakeFirstOrMiddleName(male),
                gender: male ? Gender.Male : Gender.Female,
                homeAddress: new Address(addr1, "Chicago", new State("IL"), new ZipCode(zip), addr2),
                lastName: DummyData.GenerateFakeLastName(),
                paidTimeOffPolicy: new PaidTimeOffPolicy(false, employeeLevel, true, 320.0m, "DEMO POLICY", 10.0m) { Id = random.Next() },
                salary: new Money((random.Next(25) + 25) * 1000.0M, Currency.USD),
                socialSecurityNumber: new SocialSecurityNumber(DummyData.GenerateFakeSSN()),
                dateExited: DummyData.GetRandomTerminationDate(dateHired),
                middleName: DummyData.GenerateFakeFirstOrMiddleName(male),
                ptoHoursRemaining: 10.0m,
                subordinates: employeeLevel > 1 ? (from n in Enumerable.Range(1, employeeLevel) select GenerateEmployee(employeeLevel - 1)).ToList() : new List<Employee>()
                );
            employee.Id = random.Next();
            return employee;
        }

        #endregion
    }
}