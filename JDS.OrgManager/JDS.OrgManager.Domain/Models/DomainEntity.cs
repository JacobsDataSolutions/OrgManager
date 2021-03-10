// Copyright �2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.Abstractions.Events;
using JDS.OrgManager.Domain.Abstractions.Models;
using JDS.OrgManager.Domain.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JDS.OrgManager.Domain.Models
{
    public abstract class DomainEntity : IDomainEntity
    {
        private static IDomainEventDispatcher dispatcher = new NullDomainEventDispatcher();

        private readonly List<IDomainEvent> domainEvents = new List<IDomainEvent>();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => domainEvents.AsReadOnly();

        // Slightly breaking the rules here by having a public setter.
        public int Id { get; set; }

        public void AddDomainEvent(IDomainEvent eventItem) => domainEvents.Add(eventItem);

        public void ClearDomainEvents() => domainEvents?.Clear();

        public async Task DispatchDomainEventsAsync()
        {
            foreach (var domainEvent in domainEvents)
            {
                await dispatcher.PublishAsync(domainEvent);
            }
            ClearDomainEvents();
        }

        public bool Equals(DomainEntity other) => other != null && Id.Equals(other.Id);

        public override bool Equals(object? obj) => obj is DomainEntity entity && entity != null ? Equals(entity) : base.Equals(obj);

        public override int GetHashCode() => Id;

        public void RemoveDomainEvent(IDomainEvent eventItem) => domainEvents?.Remove(eventItem);

        internal static void WireUpDispatcher(IDomainEventDispatcher dispatcher) => DomainEntity.dispatcher = dispatcher;
    }
}