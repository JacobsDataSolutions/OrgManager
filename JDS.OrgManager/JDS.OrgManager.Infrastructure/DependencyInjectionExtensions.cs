// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Identity;
using JDS.OrgManager.Application.Abstractions.Serialization;
using JDS.OrgManager.Common.Abstractions.DateTimes;
using JDS.OrgManager.Infrastructure.ErrorHandling;
using JDS.OrgManager.Infrastructure.Dates;
using JDS.OrgManager.Infrastructure.Http;
using JDS.OrgManager.Infrastructure.Identity;
using JDS.OrgManager.Infrastructure.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JDS.OrgManager.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services) =>
            services
            .AddSingleton<IDateTimeService, MachineDateTimeService>()
            .AddSingleton<ICurrentUserService, CurrentUserService>()
            .AddSingleton<IByteSerializer, ByteSerializer>();

        public static IApplicationBuilder UseCustomHttpContextAccessor(this IApplicationBuilder app)
        {
            MyHttpContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            return app;
        }

        public static IApplicationBuilder UseCustomErrorHandlingMiddleware(this IApplicationBuilder app) =>
            app.UseMiddleware<CustomErrorHandlingMiddleware>();

        public static void UseCustomErrors(this IApplicationBuilder app, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.Use(CustomErrorHandlerHelper.WriteDevelopmentResponse);
            }
            else
            {
                app.Use(CustomErrorHandlerHelper.WriteProductionResponse);
            }
        }
    }
}