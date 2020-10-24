using JDS.OrgManager.Application.Tenants;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace JDS.OrgManager.Infrastructure.Identity
{
    public static class ClaimsExtensions
    {
        public static int? GetAspNetUsersId(this ClaimsPrincipal user) =>
            TryGetClaimsValue(user, ClaimTypes.NameIdentifier, value => int.TryParse(value, out var id) ? id : (int?)null);
        
        public static bool GetIsCustomer(this ClaimsPrincipal user) =>
            TryGetClaimsValue(user, nameof(UserClaims.IsCustomer), value => bool.TryParse(value, out var isCustomer) ? isCustomer : false);

        public static TenantEmployeeIdentityModel[] GetTenantEmployees(this ClaimsPrincipal user) =>
            TryGetClaimsValue(user, nameof(UserClaims.TenantEmployees), value =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return new TenantEmployeeIdentityModel[0];
                }
                var tenantEmployees = new List<TenantEmployeeIdentityModel>();
                foreach (var tenantEmployeeString in value.Split(','))
                {
                    var ids = tenantEmployeeString.Split('-');
                    if (!int.TryParse(ids[0], out var tenantId))
                    {
                        throw new InvalidOperationException("Invalid tenant Id in Tenant/Employee tuple.");
                    }
                    tenantEmployees.Add(new TenantEmployeeIdentityModel() { TenantId = tenantId, EmployeeId = int.TryParse(ids[1], out var employeeId) ? employeeId : (int?)null });
                }
                return tenantEmployees.ToArray();
            });

        public static UserClaims GetUserClaims(this ClaimsPrincipal user)
        {
            var userId = GetAspNetUsersId(user);
            if (userId == null)
            {
                return null;
            }
            return new UserClaims
            {
                AspNetUsersId = (int)userId,
                IsCustomer = GetIsCustomer(user),
                TenantEmployees = GetTenantEmployees(user)
            };
        }

        public static T TryGetClaimsValue<T>(this ClaimsPrincipal user, string claimName, Func<string, T> parseClaimValue)
        {
            var claim = user.FindFirst(claimName);
            if (claim != null)
            {
                return parseClaimValue(claim.Value);
            }
            return default;
        }
    }
}
