// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Identity;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Behaviors
{
    public class RequestPerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ICurrentUserService currentUserService;

        private readonly ILogger<TRequest> logger;

        private readonly Stopwatch timer;

        public RequestPerformanceBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            timer = new Stopwatch();

            this.logger = logger;
            this.currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            timer.Start();

            var response = await next();

            timer.Stop();

            var name = typeof(TRequest).Name;

            if (timer.ElapsedMilliseconds > 500)
            {
                logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}",
                    name, timer.ElapsedMilliseconds, currentUserService.UserId, request);
            }
            else
            {
                logger.LogInformation("Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}",
                    name, timer.ElapsedMilliseconds, currentUserService.UserId, request);
            }

            return response;
        }
    }
}