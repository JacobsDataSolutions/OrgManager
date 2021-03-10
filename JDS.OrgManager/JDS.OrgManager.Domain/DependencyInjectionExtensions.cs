// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Common.Abstractions.DateTimes;
using JDS.OrgManager.Domain.Abstractions.Events;
using JDS.OrgManager.Domain.Events;
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using JDS.OrgManager.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JDS.OrgManager.Domain
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDomainLayer(this IServiceCollection services)
        {
            services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddSingleton<PaidTimeOffRequestService>();
            return services;
        }

        public static IServiceProvider WireUpDateTimeService(this IServiceProvider serviceProvider)
        {
            DomainEvent.WireUpDateTimeService(serviceProvider.GetRequiredService<IDateTimeService>());
            return serviceProvider;
        }

        public static IServiceProvider WireUpDomainEventHandlers(this IServiceProvider serviceProvider)
        {
            DomainEntity.WireUpDispatcher(serviceProvider.GetRequiredService<IDomainEventDispatcher>());
            return serviceProvider;
        }
    }
}