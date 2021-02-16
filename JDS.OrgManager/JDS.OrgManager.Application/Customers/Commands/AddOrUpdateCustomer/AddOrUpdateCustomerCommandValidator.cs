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