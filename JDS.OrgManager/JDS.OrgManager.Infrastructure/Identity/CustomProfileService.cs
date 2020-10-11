using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using JDS.OrgManager.Application.Tenants.GetAuthorizedTenantsForUser;
using JDS.OrgManager.Application.Tenants.GetTenantEmployeesForUser;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace JDS.OrgManager.Infrastructure.Identity
{
    /// <summary>
    /// Copied and modified from IdentityServer4.AspNetIdentity.ProfileService'ApplicationUser from the IdentityServer4 source code at:
    /// https://github.com/IdentityServer/IdentityServer4 IProfileService to integrate with ASP.NET Identity.
    /// </summary>
    /// <seealso cref="IdentityServer4.Services.IProfileService"/>
    public class CustomProfileService : IProfileService
    {
        /// <summary>
        /// The claims factory.
        /// </summary>
        protected readonly IUserClaimsPrincipalFactory<ApplicationUser> ClaimsFactory;

        /// <summary>
        /// The logger
        /// </summary>
        protected readonly ILogger<CustomProfileService> Logger;

        /// <summary>
        /// The user manager.
        /// </summary>
        protected readonly UserManager<ApplicationUser> UserManager;

        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService{ApplicationUser}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="claimsFactory">The claims factory.</param>
        public CustomProfileService(UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IMediator mediator)
        {
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            ClaimsFactory = claimsFactory ?? throw new ArgumentNullException(nameof(claimsFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService{ApplicationUser}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="claimsFactory">The claims factory.</param>
        /// <param name="logger">The logger.</param>
        public CustomProfileService(UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            ILogger<CustomProfileService> logger,
            IMediator mediator)
        {
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            ClaimsFactory = claimsFactory ?? throw new ArgumentNullException(nameof(claimsFactory));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            if (sub == null) throw new Exception("No sub claim present");

            await GetProfileDataAsync(context, sub);
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. if the user's account has been deactivated
        /// since they logged in). (e.g. during token issuance or validation).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            if (sub == null) throw new Exception("No subject Id claim present");

            await IsActiveAsync(context, sub);
        }

        /// <summary>
        /// Returns if the user is active.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Task<bool> IsUserActiveAsync(ApplicationUser user)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Loads the user by the subject id.
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        protected virtual async Task<ApplicationUser> FindUserAsync(string subjectId)
        {
            var user = await UserManager.FindByIdAsync(subjectId);
            if (user == null)
            {
                Logger?.LogWarning("No user found matching subject Id: {subjectId}", subjectId);
            }

            return user;
        }

        /// <summary>
        /// Called to get the claims for the subject based on the profile request.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        protected virtual async Task GetProfileDataAsync(ProfileDataRequestContext context, string subjectId)
        {
            var user = await FindUserAsync(subjectId);
            if (user != null)
            {
                await GetProfileDataAsync(context, user);
            }
        }

        /// <summary>
        /// Called to get the claims for the user based on the profile request.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual async Task GetProfileDataAsync(ProfileDataRequestContext context, ApplicationUser user)
        {
            var principal = await GetUserClaimsAsync(user);
            context.AddRequestedClaims(principal.Claims);
            await AddCustomClaimsAsync(user, context);
        }

        /// <summary>
        /// Gets the claims for a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual async Task<ClaimsPrincipal> GetUserClaimsAsync(ApplicationUser user)
        {
            var principal = await ClaimsFactory.CreateAsync(user);
            if (principal == null) throw new Exception("ClaimsFactory failed to create a principal");

            return principal;
        }

        /// <summary>
        /// Determines if the subject is active.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        protected virtual async Task IsActiveAsync(IsActiveContext context, string subjectId)
        {
            var user = await FindUserAsync(subjectId);
            if (user != null)
            {
                await IsActiveAsync(context, user);
            }
            else
            {
                context.IsActive = false;
            }
        }

        /// <summary>
        /// Determines if the user is active.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual async Task IsActiveAsync(IsActiveContext context, ApplicationUser user)
        {
            context.IsActive = await IsUserActiveAsync(user);
        }

        private async Task AddCustomClaimsAsync(ApplicationUser user, ProfileDataRequestContext context)
        {
            var tenantIds = await mediator.Send(new GetAuthorizedTenantsForUserQuery { AspNetUsersId = user.Id });
            var tenantEmployees = await mediator.Send(new GetTenantEmployeesForUserQuery { AspNetUsersId = user.Id });
            context.IssuedClaims.AddRange(
                new[]
                {
                    new Claim(nameof(UserClaims.IsCustomer), user.IsCustomer.ToString()),
                    new Claim(nameof(UserClaims.AuthorizedTenantIds), string.Join(",", tenantIds)),
                    new Claim(nameof(UserClaims.TenantEmployees), string.Join(",", from t in tenantEmployees select t.ToString())),
                    new Claim(nameof(UserClaims.UserName), user.UserName)
                });
        }
    }
}
