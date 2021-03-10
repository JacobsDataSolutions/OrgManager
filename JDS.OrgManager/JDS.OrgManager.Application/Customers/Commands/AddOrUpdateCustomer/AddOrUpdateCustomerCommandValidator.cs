// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using FluentValidation;
using JDS.OrgManager.Application.Abstractions.Models;

namespace JDS.OrgManager.Application.Customers.Commands.AddOrUpdateCustomer
{
    public class AddOrUpdateCustomerCommandValidator : AbstractValidator<AddOrUpdateCustomerCommand>
    {
        public AddOrUpdateCustomerCommandValidator()
        {
            RuleFor(e => e.AspNetUsersId).GreaterThan(0);

            RuleFor(e => e.Customer).NotNull();
            RuleFor(e => e.Customer.Address1).MaximumLength(Lengths.Address1).NotEmpty();
            RuleFor(e => e.Customer.Address2).MaximumLength(Lengths.Address2);
            RuleFor(e => e.Customer.City).MaximumLength(Lengths.City).NotEmpty();
            RuleFor(e => e.Customer.CompanyName).MaximumLength(Lengths.Name).NotEmpty();
            RuleFor(e => e.Customer.CurrencyCode).MaximumLength(Lengths.CurrencyCode).NotEmpty();
            RuleFor(e => e.Customer.FirstName).MaximumLength(Lengths.CurrencyCode).NotEmpty();
            RuleFor(e => e.Customer.LastName).MaximumLength(Lengths.LastName).NotEmpty();
            RuleFor(e => e.Customer.MiddleName).MaximumLength(Lengths.MiddleName);
            RuleFor(e => e.Customer.State).MaximumLength(Lengths.State).NotEmpty();
            RuleFor(e => e.Customer.ZipCode).MaximumLength(Lengths.ZipCode).NotEmpty();
        }
    }
}