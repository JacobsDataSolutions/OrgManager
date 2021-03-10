// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using FluentValidation;
using JDS.OrgManager.Application.Abstractions.Models;
using System;

namespace JDS.OrgManager.Application.HumanResources.Employees.Commands.AddOrUpdateEmployee
{
    public class AddOrUpdateEmployeeCommandValidator : AbstractValidator<AddOrUpdateEmployeeCommand>
    {
        public AddOrUpdateEmployeeCommandValidator()
        {
            RuleFor(e => e.AspNetUsersId).GreaterThan(0);

            RuleFor(e => e.Employee).NotNull();
            //RuleFor(e => e.Employee.AssignmentKey).NotEqual(Guid.Empty);
            RuleFor(e => e.Employee.Address1).MaximumLength(Lengths.Address1).NotEmpty();
            RuleFor(e => e.Employee.Address2).MaximumLength(Lengths.Address2);
            RuleFor(e => e.Employee.City).MaximumLength(Lengths.City).NotEmpty();
            RuleFor(e => e.Employee.DateOfBirth).GreaterThan(new DateTime(1950, 1, 1));
            RuleFor(e => e.Employee.ExternalEmployeeId).MaximumLength(Lengths.ExternalEmployeeId).NotEmpty();
            RuleFor(e => e.Employee.FirstName).MaximumLength(Lengths.FirstName).NotEmpty();
            RuleFor(e => e.Employee.LastName).MaximumLength(Lengths.LastName).NotEmpty();
            RuleFor(e => e.Employee.MiddleName).MaximumLength(Lengths.MiddleName);
            RuleFor(e => e.Employee.State).MaximumLength(Lengths.State).NotEmpty();
            RuleFor(e => e.Employee.ZipCode).MaximumLength(Lengths.ZipCode).NotEmpty();
        }
    }
}