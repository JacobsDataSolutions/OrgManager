// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Abstractions.Events;
using JDS.OrgManager.Domain.HumanResources.Employees;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.Employees.Commands.AddOrUpdateEmployee
{
    public class EmployeeRegisteredEventHandler : IDomainEventHandler<EmployeeRegisteredEvent>
    {
        private readonly ILogger logger;

        public EmployeeRegisteredEventHandler(ILogger<EmployeeRegisteredEventHandler> logger) => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task Handle(EmployeeRegisteredEvent domainEvent, CancellationToken cancellationToken) =>
            logger.LogInformation($"New employee [{domainEvent.Employee.LastName}, {domainEvent.Employee.FirstName}] registered.");
    }
}