// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Behaviors;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Common.PaidTimeOffPolicies;
using JDS.OrgManager.Application.HumanResources.Employees.Commands.RegisterOrUpdateEmployee;
using JDS.OrgManager.Domain.HumanResources.Employees;
using JDS.OrgManager.Domain.HumanResources.PaidTimeOffPolicies;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JDS.OrgManager.Application
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, bool addValidation = false, bool addRequestLogging = false, bool useReadThroughCachingForQueries = false)
        {
            services.AddSingleton<IDomainEntityToDbEntityMapper<Employee, EmployeeEntity>, EmployeeDomainToDbEntityMapper>()
            .AddSingleton<IDomainEntityToDbEntityMapper<PaidTimeOffPolicy, PaidTimeOffPolicyEntity>, PaidTimeOffPolicyDomainToDbEntityMapper>()
            .AddSingleton<IViewModelToDomainEntityMapper<RegisterOrUpdateEmployeeCommand, Employee>, RegisterOrUpdateEmployeeDomainEntityMapper>();

            if (addValidation)
            {
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            }

            if (addRequestLogging)
            {
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            }

            if (useReadThroughCachingForQueries)
            {
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestCachingBehavior<,>));
            }
            return services;
        }
    }
}