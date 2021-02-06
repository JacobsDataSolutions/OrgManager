// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using FluentValidation;

namespace JDS.OrgManager.Application.HumanResources.Employees.Commands.RegisterOrUpdateEmployee
{
    public class RegisterOrUpdateEmployeeCommandValidator : AbstractValidator<RegisterOrUpdateEmployeeCommand>
    {
        public RegisterOrUpdateEmployeeCommandValidator()
        {
            RuleFor(e => e.Address1).MaximumLength(50).NotEmpty();
            RuleFor(e => e.Address2).MaximumLength(15);
            RuleFor(e => e.City).MaximumLength(30).NotEmpty();
            RuleFor(e => e.CurrencyCode).MaximumLength(3).NotEmpty();
            RuleFor(e => e.FirstName).MaximumLength(25).NotEmpty();
            RuleFor(e => e.LastName).MaximumLength(25).NotEmpty();
            RuleFor(e => e.MiddleName).MaximumLength(25);
            RuleFor(e => e.SocialSecurityNumber).MaximumLength(11).NotEmpty();
            RuleFor(e => e.State).MaximumLength(2).NotEmpty();
            RuleFor(e => e.Zip).MaximumLength(10).NotEmpty();
        }
    }
}