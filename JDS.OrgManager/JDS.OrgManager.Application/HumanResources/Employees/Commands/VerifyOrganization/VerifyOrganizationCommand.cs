// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Common.PaidTimeOffPolicies;
using JDS.OrgManager.Domain.HumanResources.Advanced;
using JDS.OrgManager.Domain.HumanResources.Employees;
using JDS.OrgManager.Domain.HumanResources.PaidTimeOffPolicies;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.Employees.Commands.VerifyOrganization
{
    public class VerifyOrganizationCommand : IViewModel, IRequest<VerifyOrganizationViewModel>
    {
        public class VerifyOrganizationCommandHandler : IRequestHandler<VerifyOrganizationCommand, VerifyOrganizationViewModel>
        {
            private readonly IApplicationWriteDbContext context;

            private readonly IDomainEntityToDbEntityMapper<Employee, EmployeeEntity> employeeDomainToDbEntityMapper;

            private readonly IOrganizationVerifier organizationVerifier;

            private readonly IDomainEntityToDbEntityMapper<PaidTimeOffPolicy, PaidTimeOffPolicyEntity> ptoPolicyDomainToDbEntityMapper;

            public VerifyOrganizationCommandHandler(
                IApplicationWriteDbContext context,
                IDomainEntityToDbEntityMapper<Employee, EmployeeEntity> employeeDomainToDbEntityMapper,
                IDomainEntityToDbEntityMapper<PaidTimeOffPolicy, PaidTimeOffPolicyEntity> ptoPolicyDomainToDbEntityMapper,
                IOrganizationVerifier organizationVerifier)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.employeeDomainToDbEntityMapper = employeeDomainToDbEntityMapper ?? throw new ArgumentNullException(nameof(employeeDomainToDbEntityMapper));
                this.organizationVerifier = organizationVerifier ?? throw new ArgumentNullException(nameof(organizationVerifier));
                this.ptoPolicyDomainToDbEntityMapper = ptoPolicyDomainToDbEntityMapper ?? throw new ArgumentNullException(nameof(ptoPolicyDomainToDbEntityMapper));
            }

            public async Task<VerifyOrganizationViewModel> Handle(VerifyOrganizationCommand request, CancellationToken cancellationToken)
            {
                var employeeEntities =
                    await context.Employees
                    .Include(e => e.PaidTimeOffPolicy)
                    .Include(e => e.Subordinates)
                    .ThenInclude(em => em.Employee)
                    .OrderByDescending(e => e.EmployeeLevel)
                    .ToListAsync();

                Employee buildTree(EmployeeEntity employeeEntity) =>
                    employeeDomainToDbEntityMapper
                    .MapToDomainEntity(employeeEntity)
                    .WithSubordinates(from s in employeeEntity.Subordinates select buildTree(s.Employee))
                    .WithPaidTimeOffPolicy(ptoPolicyDomainToDbEntityMapper.MapToDomainEntity(employeeEntity.PaidTimeOffPolicy));

                var trees = (from emp in employeeEntities where !emp.Managers.Any() select buildTree(emp)).ToList();

                var stats = organizationVerifier.VerifyOrg(trees);
                return new VerifyOrganizationViewModel { Stats = (from s in stats select new OrgStats { NumEmployees = s.Item1, OrgComplexity = s.Item2 }).ToList() };
            }
        }
    }
}