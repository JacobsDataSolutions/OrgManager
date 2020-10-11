using FluentValidation;
using JDS.OrgManager.Application.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Application.Customers.AddOrUpdateTenant
{
    public class AddOrUpdateTenantCommandValidator : AbstractValidator<AddOrUpdateTenantCommand>
    {
        public AddOrUpdateTenantCommandValidator()
        {
            RuleFor(e => e.AspNetUsersId).GreaterThan(0);

            RuleFor(e => e.Tenant).NotNull();
            RuleFor(e => e.Tenant.Name).MaximumLength(Lengths.Name).NotEmpty();
            RuleFor(e => e.Tenant.Slug).MinimumLength(4).MaximumLength(Lengths.Slug).NotEmpty();
            RuleFor(e => e.Tenant.AssignmentKey).NotEqual(Guid.Empty);
        }
    }
}
