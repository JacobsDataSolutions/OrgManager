using JDS.OrgManager.Application;
using JDS.OrgManager.Application.Customers;
using JDS.OrgManager.Application.Customers.Commands.AddOrUpdateTenant;
using JDS.OrgManager.Application.Customers.Commands.DeleteTenant;
using JDS.OrgManager.Application.Customers.Commands.ProvisionTenant;
using JDS.OrgManager.Application.Customers.Queries.GetNewAssignmentKey;
using JDS.OrgManager.Application.Customers.Queries.GetTenantsForCustomer;
using JDS.OrgManager.Application.Tenants;
using JDS.OrgManager.Application.Tenants.Queries.GetAuthorizedTenantsForUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : CqrsControllerBase
    {
        public CustomerController(IMediator mediator) : base(mediator)
        { }

        [HttpPost("[action]")]
        public async Task<ActionResult<TenantViewModel>> AddOrUpdateTenant([FromBody]TenantViewModel tenant) =>
            await WithAuthenticatedUserClaimsDo(async userClaims =>
            {
                var isNewTenant = tenant.Id == null;
                if (!userClaims.IsCustomer)
                {
                    throw new AuthorizationException("Only customers may perform this operation.");
                }
                // Get accessible tenants directly, not from the JWT token, because the claims will only be refreshed when user logs in.
                var authorizedTenantIds = await Mediator.Send(new GetAuthorizedTenantsForUserQuery { AspNetUsersId = userClaims.AspNetUsersId });
                var tenantId = tenant.Id;
                if (!isNewTenant && !authorizedTenantIds.Contains((int)tenantId))
                {
                    throw new AuthorizationException("You do not have access to the specified tenant.");
                }

                tenant = await Mediator.Send(new AddOrUpdateTenantCommand { AspNetUsersId = userClaims.AspNetUsersId, Tenant = tenant });
                if (tenant.Id == null)
                {
                    throw new InvalidOperationException();
                }

                if (isNewTenant)
                {
                    await Mediator.Send(new ProvisionTenantCommand { TenantId = (int)tenant.Id });
                }

                return tenant;
            });

        [HttpPost("[action]")]
        public async Task<ActionResult<DeleteTenantViewModel>> DeleteTenant([FromBody]DeleteTenantViewModel deleteTenant) =>
            await WithAuthenticatedUserClaimsDo(async userClaims =>
            {
                if (!userClaims.IsCustomer)
                {
                    throw new AuthorizationException("Only customers may perform this operation.");
                }
                var authorizedTenantIds = await Mediator.Send(new GetAuthorizedTenantsForUserQuery { AspNetUsersId = userClaims.AspNetUsersId });
                var tenantId = deleteTenant.TenantId;
                if (!authorizedTenantIds.Contains(tenantId))
                {
                    throw new AuthorizationException("You do not have access to the specified tenant.");
                }

                return await Mediator.Send(new DeleteTenantCommand { DeleteTenant = deleteTenant });
            });

        [HttpGet("[action]")]
        public async Task<ActionResult<Guid>> GetNewAssignmentKey() =>
            await WithAuthenticatedUserClaimsDo(async userClaims =>
            {
                return await Mediator.Send(new GetNewAssignmentKeyQuery() { AspNetUsersId = userClaims.AspNetUsersId });
            });

        [HttpGet("[action]")]
        public async Task<ActionResult<TenantViewModel[]>> GetTenantsForCustomer() =>
            await WithAuthenticatedUserClaimsDo(async userClaims =>
            {
                return await Mediator.Send(new GetTenantsForCustomerQuery() { AspNetUsersId = userClaims.AspNetUsersId });
            });
    }
}
