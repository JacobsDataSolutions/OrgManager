// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using FastExpressionCompiler;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Behaviors;
using JDS.OrgManager.Application.Common.Mapping;
using JDS.OrgManager.Application.System;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JDS.OrgManager.Application
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, bool addValidation = false, bool addRequestLogging = false, bool useReadThroughCachingForQueries = false)
        {
            services.Scan(scan =>
                scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IDbEntityToViewModelMapper<,>))).AsImplementedInterfaces().WithSingletonLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IDbEntityToDomainEntityMapper<,>))).AsImplementedInterfaces().WithSingletonLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IDbEntityToValueObjectMapper<,>))).AsImplementedInterfaces().WithSingletonLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEntityToDbEntityMapper<,>))).AsImplementedInterfaces().WithSingletonLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEntityToViewModelMapper<,>))).AsImplementedInterfaces().WithSingletonLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IViewModelToDbEntityMapper<,>))).AsImplementedInterfaces().WithSingletonLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IViewModelToDomainEntityMapper<,>))).AsImplementedInterfaces().WithSingletonLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IViewModelToValueObjectMapper<,>))).AsImplementedInterfaces().WithSingletonLifetime()
            );

            if (addValidation)
            {
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            }

            if (addRequestLogging)
            {
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            }

            if (useReadThroughCachingForQueries)
            {
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestCachingBehavior<,>));
            }

            services.AddScoped<DataInitializerService>();
            services.AddSingleton<IModelMapper, ModelMapper>();

            // Mapster
            TypeAdapterConfig.GlobalSettings.Compiler = exp => exp.CompileFast();

            return services;
        }
    }
}