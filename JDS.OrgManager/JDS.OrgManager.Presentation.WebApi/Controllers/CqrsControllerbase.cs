using JDS.OrgManager.Application.Tenants;
using JDS.OrgManager.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.WebApi.Controllers
{
    public abstract class CqrsControllerBase : ControllerBase
    {
        protected IMediator Mediator { get; }

        protected CqrsControllerBase(IMediator mediator)
        {
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected int? GetAspNetUsersId() =>
            GetClaimsValue(ClaimTypes.NameIdentifier, value => int.TryParse(value, out var id) ? id : (int?)null);

        protected int[] GetAuthorizedTenantIds() =>
            GetClaimsValue(nameof(UserClaims.AuthorizedTenantIds), value =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return new int[0];
                }
                return (from t in value.Split(",") select int.Parse(t)).ToArray();
            });

        protected bool GetIsCustomer() =>
            GetClaimsValue(nameof(UserClaims.IsCustomer), value => bool.TryParse(value, out var isCustomer) ? isCustomer : false);

        protected TenantEmployeeIdentityModel[] GetTenantEmployees() =>
            GetClaimsValue(nameof(UserClaims.TenantEmployees), value =>
                string.IsNullOrWhiteSpace(value) ? new TenantEmployeeIdentityModel[0] :
                (
                    from tenantEmployee in value.Split(',')
                    let n = tenantEmployee.Split('-')
                    select new TenantEmployeeIdentityModel { TenantId = int.Parse(n[0]), EmployeeId = int.Parse(n[1]) }).ToArray());

        protected UserClaims GetUserClaims()
        {
            var userId = GetAspNetUsersId();
            if (userId == null)
            {
                return null;
            }
            return new UserClaims
            {
                AspNetUsersId = (int)userId,
                AuthorizedTenantIds = GetAuthorizedTenantIds(),
                IsCustomer = GetIsCustomer(),
                TenantEmployees = GetTenantEmployees()
            };
        }

        protected async Task<ActionResult<T>> WithAuthenticatedEmployeeUserClaimsDo<T>(Func<UserClaims, Task<T>> func, int tenantId)
        {
            var userClaims = GetUserClaims();
            if (userClaims == null)
            {
                return Unauthorized();
            }
            var employeeId = userClaims.GetEmployeeId(tenantId);
            return await WithAuthenticatedUserClaimsDo(func, tenantId, employeeId);
        }

        protected async Task<ActionResult<T>> WithAuthenticatedUserClaimsDo<T>(Func<UserClaims, Task<T>> func, int? tenantId = null, int? employeeId = null)
        {
            var userClaims = GetUserClaims();
            if (
                // Ensure user is authenticated.
                userClaims == null ||
                // Ensure user is authenticated and associated with the specified tenant.
                (tenantId != null && !userClaims.AuthorizedTenantIds.Contains((int)tenantId)) ||
                // Ensure user is authenticated and associated with the specified tenant and employee.
                (tenantId != null && employeeId != null && !userClaims.TenantEmployees.Any(t => t.TenantId == tenantId && t.EmployeeId == employeeId)))
            {
                return Unauthorized();
            }
            return Ok(await func(userClaims));
        }

        private T GetClaimsValue<T>(string claimName, Func<string, T> parseClaimValue)
        {
            var claim = User.FindFirst(claimName);
            if (claim != null)
            {
                return parseClaimValue(claim.Value);
            }
            return default;
        }
    }
}
