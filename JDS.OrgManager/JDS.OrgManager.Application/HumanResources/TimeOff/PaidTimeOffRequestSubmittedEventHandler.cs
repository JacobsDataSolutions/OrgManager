﻿// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Abstractions.Events;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.HumanResources.TimeOff
{
    public class PaidTimeOffRequestSubmittedEventHandler : IDomainEventHandler<PaidTimeOffRequestSubmittedEvent>
    {
        private readonly ILogger logger;

        public PaidTimeOffRequestSubmittedEventHandler(ILogger<PaidTimeOffRequestSubmittedEventHandler> logger) => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public Task Handle(PaidTimeOffRequestSubmittedEvent domainEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation($"New PTO request submitted [{domainEvent.PaidTimeOffRequest}].");
            return Task.CompletedTask;
        }
    }
}