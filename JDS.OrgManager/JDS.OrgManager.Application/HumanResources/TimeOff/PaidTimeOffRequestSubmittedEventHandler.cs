using JDS.OrgManager.Domain.Abstractions.Events;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
