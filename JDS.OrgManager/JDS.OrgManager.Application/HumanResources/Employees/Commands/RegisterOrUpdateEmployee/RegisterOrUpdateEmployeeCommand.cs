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
using JDS.OrgManager.Common.Abstractions.Dates;
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
    public class RegisterOrUpdateEmployeeCommand : IViewModel, IRequest<RegisterOrUpdateEmployeeCommand>
    {
        // Hard-coded for the time being.
        private const int TenantId = 1;

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string CurrencyCode { get; set; }

        public DateTime? DateExited { get; set; }

        public DateTime DateHired { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int EmployeeLevel { get; set; }

        public string FirstName { get; set; }

        public Gender Gender { get; set; }

        public int? Id { get; set; }

        public string LastName { get; set; }

        public int? ManagerId { get; set; }

        public string MiddleName { get; set; }

        public int PaidTimeOffPolicyId { get; set; }

        public decimal? PtoHoursRemaining { get; set; }

        public decimal Salary { get; set; }

        public string SocialSecurityNumber { get; set; }

        public string State { get; set; }

        public List<int> SubordinateIds { get; set; } = new List<int>();

        public string Zip { get; set; }

        public class RegisterOrUpdateEmployeeCommandHandler : IRequestHandler<RegisterOrUpdateEmployeeCommand, RegisterOrUpdateEmployeeCommand>
        {
            private readonly IApplicationWriteDbContext context;

            private readonly IDateTimeService dateTimeService;

            private readonly IDomainEntityToDbEntityMapper<Employee, EmployeeEntity> employeeDomainToDbEntityMapper;

            private readonly IViewModelToDomainEntityMapper<RegisterOrUpdateEmployeeCommand, Employee> employeeVmToDomainEntityMapper;

            private readonly IDomainEntityToDbEntityMapper<PaidTimeOffPolicy, PaidTimeOffPolicyEntity> ptoPolicyDomainToDbEntityMapper;

            public RegisterOrUpdateEmployeeCommandHandler(
                IApplicationWriteDbContext context,
                IViewModelToDomainEntityMapper<RegisterOrUpdateEmployeeCommand, Employee> employeeVmToDomainEntityMapper,
                IDomainEntityToDbEntityMapper<Employee, EmployeeEntity> employeeDomainToDbEntityMapper,
                IDomainEntityToDbEntityMapper<PaidTimeOffPolicy, PaidTimeOffPolicyEntity> ptoPolicyDomainToDbEntityMapper,
                IDateTimeService dateTimeService)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.employeeVmToDomainEntityMapper = employeeVmToDomainEntityMapper ?? throw new ArgumentNullException(nameof(employeeVmToDomainEntityMapper));
                this.employeeDomainToDbEntityMapper = employeeDomainToDbEntityMapper ?? throw new ArgumentNullException(nameof(employeeDomainToDbEntityMapper));
                this.ptoPolicyDomainToDbEntityMapper = ptoPolicyDomainToDbEntityMapper ?? throw new ArgumentNullException(nameof(ptoPolicyDomainToDbEntityMapper));
                this.dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
            }

            public async Task<RegisterOrUpdateEmployeeCommand> Handle(RegisterOrUpdateEmployeeCommand request, CancellationToken cancellationToken)
            {
                // Map the view model to a domain entity. We are now in the realm of business entities and logic.
                var employee = employeeVmToDomainEntityMapper.MapToDomainEntity(request);

                var isNewEmployee = employee.Id == null;

                // Look up manager from DB.
                EmployeeEntity managerEntity = null;
                if (request.ManagerId != null)
                {
                    managerEntity = await context.Employees.FindAsync(request.ManagerId);
                    if (managerEntity == null)
                    {
                        throw new EmployeeException($"Manager with ID {request.ManagerId} not found.");
                    }
                    // Convert to domain entity as well.
                    employee = employee.WithManager(employeeDomainToDbEntityMapper.MapToDomainEntity(managerEntity));
                }

                // Look up subordinates and map those to domain entities.
                var subordinateIds = request.SubordinateIds.ToArray();
                var subordinateEntities = await (from e in context.Employees.AsNoTracking() where subordinateIds.Contains(e.Id) select e).ToArrayAsync();
                employee = employee.WithSubordinates(from e in subordinateEntities select employeeDomainToDbEntityMapper.MapToDomainEntity(e));

                // Get PTO policy for this employee and map to domain entity.
                var policyEntity = await context.PaidTimeOffPolicies.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.PaidTimeOffPolicyId);
                if (policyEntity == null)
                {
                    throw new EmployeeException($"PTO policy not found.");
                }
                var policy = ptoPolicyDomainToDbEntityMapper.MapToDomainEntity(policyEntity);
                employee = employee.WithPaidTimeOffPolicy(policy);

                // Check the validity of the employee's PTO hours and other business logic. All business rule validation is still happening within the Domain layer...
                employee.AssertAggregates();
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

                // Convert back to persistence entity.
                var employeeEntity = employeeDomainToDbEntityMapper.MapToDbEntity(employee);
                employeeEntity.TenantId = TenantId;
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
                    await context.EmployeeManagers.AddAsync(new EmployeeManagerEntity { TenantId = TenantId, EmployeeId = employeeEntity.Id, ManagerId = managerEntity.Id });
                }
                await context.EmployeeManagers.AddRangeAsync(from s in subordinateEntities select new EmployeeManagerEntity { TenantId = TenantId, EmployeeId = s.Id, ManagerId = employeeEntity.Id });

                // Commit transaction to DB.
                if (context.HasChanges)
                {
                    await context.SaveChangesAsync(cancellationToken);
                }

                // Set ID of domain entity.
                employee.Id = employeeEntity.Id;

                // Kick off domain events.
                employee.CreateEmployeeRegisteredEvent(dateTimeService);
                await employee.DispatchDomainEventsAsync();

                // Map from domain entity back to VM (command) and return that.
                return employeeVmToDomainEntityMapper.MapToViewModel(employee);
            }
        }
    }
}