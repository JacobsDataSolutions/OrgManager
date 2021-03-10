// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Common.TimeOff;
using JDS.OrgManager.Domain.HumanResources.Advanced;
using JDS.OrgManager.Domain.HumanResources.Employees;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.Employees.Queries.VerifyOrganization
{
    public class VerifyOrganizationQuery : IViewModel, IRequest<VerifyOrganizationViewModel>
    {
        public class VerifyOrganizationQueryHandler : IRequestHandler<VerifyOrganizationQuery, VerifyOrganizationViewModel>
        {
            private readonly IApplicationWriteDbContext context;

            private readonly IDbEntityToDomainEntityMapper<EmployeeEntity, Employee> employeeDbEntityToDomainEntityMapper;

            private readonly IOrganizationVerifier organizationVerifier;

            private readonly IDbEntityToDomainEntityMapper<PaidTimeOffPolicyEntity, PaidTimeOffPolicy> ptoPolicyDbEntityToDomainEntityMapper;

            public VerifyOrganizationQueryHandler(
                IApplicationWriteDbContext context,
                IDbEntityToDomainEntityMapper<EmployeeEntity, Employee> employeeDbEntityToDomainEntityMapper,
                IDbEntityToDomainEntityMapper<PaidTimeOffPolicyEntity, PaidTimeOffPolicy> ptoPolicyDbEntityToDomainEntityMapper,
                IOrganizationVerifier organizationVerifier)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.employeeDbEntityToDomainEntityMapper = employeeDbEntityToDomainEntityMapper ?? throw new ArgumentNullException(nameof(employeeDbEntityToDomainEntityMapper));
                this.organizationVerifier = organizationVerifier ?? throw new ArgumentNullException(nameof(organizationVerifier));
                this.ptoPolicyDbEntityToDomainEntityMapper = ptoPolicyDbEntityToDomainEntityMapper ?? throw new ArgumentNullException(nameof(ptoPolicyDbEntityToDomainEntityMapper));
            }

            public async Task<VerifyOrganizationViewModel> Handle(VerifyOrganizationQuery request, CancellationToken cancellationToken)
            {
                var employeeEntities =
                    await context.Employees
                    .Include(e => e.PaidTimeOffPolicy)
                    .Include(e => e.Subordinates)
                    .ThenInclude(em => em.Employee)
                    .OrderByDescending(e => e.EmployeeLevel)
                    .ToListAsync();

                Employee buildTree(EmployeeEntity employeeEntity) =>
                    employeeDbEntityToDomainEntityMapper
                    .Map(employeeEntity)
                    .WithSubordinates(from s in employeeEntity.Subordinates select buildTree(s.Employee))
                    .WithPaidTimeOffPolicy(ptoPolicyDbEntityToDomainEntityMapper.Map(employeeEntity.PaidTimeOffPolicy));

                var trees = (from emp in employeeEntities where !emp.Managers.Any() select buildTree(emp)).ToList();

                var stats = organizationVerifier.VerifyOrg(trees);
                return new VerifyOrganizationViewModel { Stats = (from s in stats select new OrgStats { NumEmployees = s.Item1, OrgComplexity = s.Item2 }).ToList() };
            }
        }
    }
}