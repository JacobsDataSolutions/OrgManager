// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Common.TimeOff;
using JDS.OrgManager.Common.Abstractions.DateTimes;
using JDS.OrgManager.Domain.Common.People;
using JDS.OrgManager.Domain.HumanResources.Employees;
using JDS.OrgManager.Domain.HumanResources.PaidTimeOffPolicies;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmployeeException = JDS.OrgManager.Application.Common.Employees.EmployeeException;

namespace JDS.OrgManager.Application.HumanResources.Employees.Commands.RegisterOrUpdateEmployee
{
    public class RegisterOrUpdateEmployeeCommand : IRequest<EmployeeViewModel>
    {
        public int AspNetUsersId { get; init; }

        public EmployeeViewModel Employee { get; set; } = default!;

        public class RegisterOrUpdateEmployeeCommandHandler : IRequestHandler<RegisterOrUpdateEmployeeCommand, EmployeeViewModel>
        {
            private readonly IApplicationWriteDbContext context;

            private readonly IApplicationWriteDbFacade facade;

            private readonly IDomainEntityToDbEntityMapper<Employee, EmployeeEntity> employeeDomainToDbEntityMapper;

            private readonly IDbEntityToDomainEntityMapper<EmployeeEntity, Employee> employeeDbEntityToDomainEntityMapper;

            private readonly IViewModelToDomainEntityMapper<EmployeeViewModel, Employee> employeeVmToDomainEntityMapper;

            private readonly IDbEntityToDomainEntityMapper<PaidTimeOffPolicyEntity, PaidTimeOffPolicy> ptoPolicyDbEntityToDomainEntityMapper;

            private readonly IDomainEntityToViewModelMapper<Employee, EmployeeViewModel> employeeDomainEntityToViewModelMapper;

            public RegisterOrUpdateEmployeeCommandHandler(
                IApplicationWriteDbContext context,
                IApplicationWriteDbFacade facade,
                IViewModelToDomainEntityMapper<EmployeeViewModel, Employee> employeeVmToDomainEntityMapper,
                IDomainEntityToViewModelMapper<Employee, EmployeeViewModel> employeeDomainEntityToViewModelMapper,
                IDomainEntityToDbEntityMapper<Employee, EmployeeEntity> employeeDomainToDbEntityMapper,
                IDbEntityToDomainEntityMapper<EmployeeEntity, Employee> employeeDbEntityToDomainEntityMapper,
                IDbEntityToDomainEntityMapper<PaidTimeOffPolicyEntity, PaidTimeOffPolicy> ptoPolicyDbEntityToDomainEntityMapper
                )
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
                this.employeeVmToDomainEntityMapper = employeeVmToDomainEntityMapper ?? throw new ArgumentNullException(nameof(employeeVmToDomainEntityMapper));
                this.employeeDomainEntityToViewModelMapper = employeeDomainEntityToViewModelMapper ?? throw new ArgumentNullException(nameof(employeeDomainEntityToViewModelMapper));
                this.employeeDomainToDbEntityMapper = employeeDomainToDbEntityMapper ?? throw new ArgumentNullException(nameof(employeeDomainToDbEntityMapper));
                this.employeeDbEntityToDomainEntityMapper = employeeDbEntityToDomainEntityMapper ?? throw new ArgumentNullException(nameof(employeeDbEntityToDomainEntityMapper));
                this.ptoPolicyDbEntityToDomainEntityMapper = ptoPolicyDbEntityToDomainEntityMapper ?? throw new ArgumentNullException(nameof(ptoPolicyDbEntityToDomainEntityMapper));
            }

            public async Task<EmployeeViewModel> Handle(RegisterOrUpdateEmployeeCommand request, CancellationToken cancellationToken)
            {
                // APPLICATION LAYER
                var employeeViewModel = request.Employee;
                var tenantId = employeeViewModel.TenantId;

                // DOMAIN LAYER
                var employee = employeeVmToDomainEntityMapper.Map(employeeViewModel);

                var isNewEmployee = employee.Id == 0;

                if (!isNewEmployee && tenantId == 0)
                {
                    throw new ApplicationLayerException("Invalid tenant ID.");
                }

                if (isNewEmployee)
                {
                    // Even though this is a read operation, we are using the Write dapper facade because commands should only talk to the Write database (never
                    // assume the two databases/sides of the stack will be in sync).
                    var t = await facade.QueryFirstOrDefaultAsync<int?>("SELECT TOP 1 Id FROM Tenants WITH(NOLOCK) WHERE AssignmentKey = @AssignmentKey", employeeViewModel, default!, cancellationToken);
                    if (t == null)
                    {
                        throw new ApplicationLayerException($"Invalid assignment key specified: {employeeViewModel.AssignmentKey}. There is no tenant that corresponds to the specified key.");
                    }
                    tenantId = (int)t;
                }

                // Look up manager from DB.
                EmployeeEntity? managerEntity = null;
                if (employeeViewModel.ManagerId != null)
                {
                    managerEntity = await context.Employees.FindAsync(employeeViewModel.ManagerId);
                    if (managerEntity == null)
                    {
                        throw new EmployeeException($"Manager with ID {employeeViewModel.ManagerId} not found.");
                    }
                    // Convert to domain entity as well.
                    employee = employee.WithManager(employeeDbEntityToDomainEntityMapper.Map(managerEntity));
                }

                // Look up subordinates and map those to domain entities.
                var subordinateIds = employeeViewModel.SubordinateIds.ToArray();
                var subordinateEntities = await (from e in context.Employees.AsNoTracking() where subordinateIds.Contains(e.Id) select e).ToArrayAsync();
                employee = employee.WithSubordinates(from e in subordinateEntities select employeeDbEntityToDomainEntityMapper.Map(e));

                // Get PTO policy for this employee and map to domain entity.
                var policyEntity = await context.PaidTimeOffPolicies.AsNoTracking().FirstOrDefaultAsync(p => p.Id == employeeViewModel.PaidTimeOffPolicyId);
                if (policyEntity == null)
                {
                    throw new EmployeeException($"PTO policy not found.");
                }
                var policy = ptoPolicyDbEntityToDomainEntityMapper.Map(policyEntity);
                employee = employee.WithPaidTimeOffPolicy(policy);

                // Check the validity of the employee's PTO hours and other business logic. All business rule validation is still happening within the Domain layer...
                employee.ValidateAggregate();
                employee.VerifyEmployeeManagerAndSubordinates();
                if (isNewEmployee)
                {
                    employee.VerifyStartingPtoHoursAreValid();
                }
                else
                {
                    employee.VerifyPtoHoursAreValid();
                }
                // End business logic.

                // PERSISTENCE LAYER
                // Convert back to persistence entity.
                var employeeEntity = employeeDomainToDbEntityMapper.Map(employee);
                employeeEntity.TenantId = employeeViewModel.TenantId;
                var entry = await context.Employees.AddAsync(employeeEntity);

                if (!isNewEmployee)
                {
                    entry.State = EntityState.Modified;
                }

                // Delete previous entity relationships.
                context.EmployeeManagers.RemoveRange(context.EmployeeManagers.Where(em => em.EmployeeId == employee.Id));
                context.EmployeeManagers.RemoveRange(context.EmployeeManagers.Where(em => em.ManagerId == employee.Id));

                // Commit transaction to DB.
                await context.SaveChangesAsync(cancellationToken);

                // Create new relationships.
                if (employee.Manager != null)
                {
                    await context.EmployeeManagers.AddAsync(new EmployeeManagerEntity { TenantId = tenantId, EmployeeId = employeeEntity.Id, ManagerId = employee.Manager.Id });
                }
                await context.EmployeeManagers.AddRangeAsync(from s in subordinateEntities select new EmployeeManagerEntity { TenantId = tenantId, EmployeeId = s.Id, ManagerId = employeeEntity.Id });

                // Commit transaction to DB.
                if (context.HasChanges)
                {
                    await context.SaveChangesAsync(cancellationToken);
                }

                // Set ID of domain entity.
                employee.Id = employeeEntity.Id;

                // Kick off domain events.
                if (isNewEmployee)
                { 
                    employee.CreateEmployeeRegisteredEvent();
                }
                else
                {
                    employee.CreateEmployeeUpdatedEvent();
                }
                await employee.DispatchDomainEventsAsync();

                // Map from domain entity back to VM (command) and return that.
                return employeeDomainEntityToViewModelMapper.Map(employee);
            }
        }
    }
}