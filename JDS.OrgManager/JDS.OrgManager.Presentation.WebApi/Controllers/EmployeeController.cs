using JDS.OrgManager.Application.HumanResources.Employees;
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
    public class EmployeeController : CqrsControllerBase
    {
        public EmployeeController(IMediator mediator) : base(mediator)
        { }

        [HttpPost("[action]")]
        public async Task<ActionResult<EmployeeViewModel>> RegisterNewEmployee([FromBody]EmployeeViewModel employee) =>
            await WithAuthenticatedUserClaimsDo(async userClaims =>
            {
                return employee;
            });
    }
}
