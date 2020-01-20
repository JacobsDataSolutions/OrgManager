// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Identity;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Behaviors
{
    public class RequestLogger<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        #region Private Fields

        private readonly ICurrentUserService currentUserService;

        private readonly ILogger logger;

        #endregion

        #region Public Constructors

        public RequestLogger(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            this.logger = logger;
            this.currentUserService = currentUserService;
        }

        #endregion

        #region Public Methods

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var name = typeof(TRequest).Name;

            logger.LogInformation("Request: {Name} {@UserId} {@Request}",
                name, currentUserService.UserId, request);

            return next();
        }

        #endregion
    }
}