using JDS.OrgManager.Application;
using JDS.OrgManager.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
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
        public CqrsControllerBase()
        {
        }

        protected int GetAspNetUsersId() =>
            GetClaimsValue(ClaimTypes.NameIdentifier, value => int.TryParse(value, out var id) ? id : throw new AuthorizationException("Can't retrieve user ID for anonymous user."));

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
