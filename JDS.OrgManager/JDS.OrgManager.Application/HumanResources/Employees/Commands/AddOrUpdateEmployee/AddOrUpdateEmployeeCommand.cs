// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Common.TimeOff;
using JDS.OrgManager.Domain.HumanResources.Employees;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmployeeException = JDS.OrgManager.Application.Common.Employees.EmployeeException;

namespace JDS.OrgManager.Application.HumanResources.Employees.Commands.AddOrUpdateEmployee
{
    public class AddOrUpdateEmployeeCommand : IRequest<AddOrUpdateEmployeeViewModel>
    {
        public int AspNetUsersId { get; init; }

        public AddOrUpdateEmployeeViewModel Employee { get; set; } = default!;

        public class AddOrUpdateEmployeeCommandHandler : IRequestHandler<AddOrUpdateEmployeeCommand, AddOrUpdateEmployeeViewModel>
        {
            private readonly IApplicationWriteDbContext context;

            private readonly IViewModelToDbEntityMapper<AddOrUpdateEmployeeViewModel, EmployeeEntity> employeeVmToDbEntityMapper;

            private readonly IDbEntityToDomainEntityMapper<EmployeeEntity, Employee> employeeDbEntityToDomainEntityMapper;

            private readonly IDomainEntityToViewModelMapper<Employee, AddOrUpdateEmployeeViewModel> employeeDomainEntityToViewModelMapper;

            private readonly IDomainEntityToDbEntityMapper<Employee, EmployeeEntity> employeeDomainToDbEntityMapper;

            private readonly IViewModelToDomainEntityMapper<AddOrUpdateEmployeeViewModel, Employee> employeeVmToDomainEntityMapper;

            private readonly IApplicationWriteDbFacade facade;

            private readonly IDbEntityToDomainEntityMapper<PaidTimeOffPolicyEntity, PaidTimeOffPolicy> ptoPolicyDbEntityToDomainEntityMapper;

            public AddOrUpdateEmployeeCommandHandler(
                IApplicationWriteDbContext context,
                IApplicationWriteDbFacade facade,
                IViewModelToDomainEntityMapper<AddOrUpdateEmployeeViewModel, Employee> employeeVmToDomainEntityMapper,
                IViewModelToDbEntityMapper<AddOrUpdateEmployeeViewModel, EmployeeEntity> employeeVmToDbEntityMapper,
                IDomainEntityToViewModelMapper<Employee, AddOrUpdateEmployeeViewModel> employeeDomainEntityToViewModelMapper,
                IDomainEntityToDbEntityMapper<Employee, EmployeeEntity> employeeDomainToDbEntityMapper,
                IDbEntityToDomainEntityMapper<EmployeeEntity, Employee> employeeDbEntityToDomainEntityMapper,
                IDbEntityToDomainEntityMapper<PaidTimeOffPolicyEntity, PaidTimeOffPolicy> ptoPolicyDbEntityToDomainEntityMapper
                )
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
                this.employeeVmToDbEntityMapper = employeeVmToDbEntityMapper ?? throw new ArgumentNullException(nameof(employeeVmToDbEntityMapper));
                this.employeeVmToDomainEntityMapper = employeeVmToDomainEntityMapper ?? throw new ArgumentNullException(nameof(employeeVmToDomainEntityMapper));
                this.employeeDomainEntityToViewModelMapper = employeeDomainEntityToViewModelMapper ?? throw new ArgumentNullException(nameof(employeeDomainEntityToViewModelMapper));
                this.employeeDomainToDbEntityMapper = employeeDomainToDbEntityMapper ?? throw new ArgumentNullException(nameof(employeeDomainToDbEntityMapper));
                this.employeeDbEntityToDomainEntityMapper = employeeDbEntityToDomainEntityMapper ?? throw new ArgumentNullException(nameof(employeeDbEntityToDomainEntityMapper));
                this.ptoPolicyDbEntityToDomainEntityMapper = ptoPolicyDbEntityToDomainEntityMapper ?? throw new ArgumentNullException(nameof(ptoPolicyDbEntityToDomainEntityMapper));
            }

            public async Task<AddOrUpdateEmployeeViewModel> Handle(AddOrUpdateEmployeeCommand request, CancellationToken cancellationToken)
            {
                // APPLICATION LAYER
                var employeeViewModel = request.Employee;
                var tenantId = employeeViewModel.TenantId;

                var isNewEmployee = employeeViewModel.Id == 0;

                if (tenantId == 0)
                {
                    throw new ApplicationLayerException("Invalid tenant ID.");
                }

                // PERISTENCE LAYER: Hydrate existing employee or get defaults for tenant.
                EmployeeEntity? employeeEntity = null;
                if (isNewEmployee)
                {
                    // Even though this is a read operation, we are using the Write dapper facade because commands should only talk to the Write database (never
                    // assume the two databases/sides of the stack will be in sync).
                    var t = await facade.QueryFirstOrDefaultAsync<int?>("SELECT TOP 1 Id FROM Tenants WITH(NOLOCK) WHERE AssignmentKey = @AssignmentKey AND Id = @TenantId", employeeViewModel, null, cancellationToken);
                    if (t == null)
                    {
                        throw new AccessDeniedException($"Invalid assignment key specified: {employeeViewModel.AssignmentKey}.");
                    }
                    tenantId = (int)t;
                    employeeEntity = await facade.QueryFirstOrDefaultAsync<EmployeeEntity>(@"
                        SELECT TOP (1) [TenantId]
                              ,[CurrencyCode]
                              ,[EmployeeLevel]
                              ,[PaidTimeOffPolicyId]
                          FROM [OrgManager].[dbo].[TenantDefaults] td WITH(NOLOCK)
                          WHERE TenantId = @TenantId", employeeViewModel, null, cancellationToken);
                    employeeEntity.PtoHoursRemaining = 0.0m;
                    employeeEntity.AspNetUsersId = request.AspNetUsersId;
                    employeeEntity.IsPending = true;
                }
                else
                {
                    employeeEntity = 
                        await context.Employees
                        .Include(e => e.Managers)
                        .Include(e => e.Subordinates)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Id == employeeViewModel.Id && e.TenantId == tenantId, cancellationToken);
                }

                // Perform updates against persistence entity.
                employeeVmToDbEntityMapper.Map(employeeViewModel, employeeEntity);

                // Look up subordinates.
                var subordinateIds = (from e in employeeEntity.Subordinates select e.EmployeeId).ToList();
                var subordinateEntities = await (from e in context.Employees.AsNoTracking() where subordinateIds.Contains(e.Id) && e.TenantId == tenantId select e).ToListAsync();

                // Get PTO policy for this employee.
                var policyEntity = await context.PaidTimeOffPolicies.AsNoTracking().FirstOrDefaultAsync(p => p.Id == employeeEntity.PaidTimeOffPolicyId && p.TenantId == tenantId);
                if (policyEntity == null)
                {
                    throw new EmployeeException($"PTO policy not found.");
                }

                // Look up manager from DB.
                EmployeeEntity? managerEntity = null;
                if (employeeEntity.Managers.Any())
                {
                    var managerId = employeeEntity.Managers.First().ManagerId;
                    managerEntity = await context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == managerId && e.TenantId == tenantId);
                    if (managerEntity == null)
                    {
                        throw new EmployeeException($"Manager with ID {managerId} not found.");
                    }
                }

                // DOMAIN LAYER
                var employee = employeeDbEntityToDomainEntityMapper.Map(employeeEntity);

                // Map manager to domain entity.
                if (managerEntity != null)
                {
                    employee = employee.WithManager(employeeDbEntityToDomainEntityMapper.Map(managerEntity));
                }

                // Map subordinates to domain entities.
                employee = employee.WithSubordinates(from e in subordinateEntities select employeeDbEntityToDomainEntityMapper.Map(e));

                // Map PTO policy to domain entity.
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

                // PERSISTENCE LAYER Convert back to persistence entity.
                employeeDomainToDbEntityMapper.Map(employee, employeeEntity);
                var entry = await context.Employees.AddAsync(employeeEntity);

                if (!isNewEmployee)
                {
                    entry.State = EntityState.Modified;
                }

                // We are not updating employee/manager relationships at this time.
                employeeEntity.Subordinates.Clear();
                employeeEntity.Managers.Clear();

                // Commit transaction to DB.
                await context.SaveChangesAsync(cancellationToken);

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
                employeeDomainEntityToViewModelMapper.Map(employee, employeeViewModel);
                return employeeViewModel;
            }
        }
    }
}