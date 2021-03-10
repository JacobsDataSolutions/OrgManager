// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace JDS.OrgManager.Infrastructure.ErrorHandling
{
    public static class CustomErrorHandlerHelper
    {
        public static Task WriteDevelopmentResponse(HttpContext httpContext, Func<Task> next)
            => WriteResponse(httpContext, includeDetails: true);

        public static Task WriteProductionResponse(HttpContext httpContext, Func<Task> next)
            => WriteResponse(httpContext, includeDetails: false);

        private static async Task WriteResponse(HttpContext httpContext, bool includeDetails)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            // Try and retrieve the error from the ExceptionHandler middleware
            var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = exceptionDetails?.Error;

            // Should always exist, but best to be safe!
            if (ex != null)
            {
                // Get the details to display, depending on whether we want to expose the raw exception
                var title = includeDetails ? "An error occured: " + ex.Message : "An error occured";
                var details = includeDetails ? ex.ToString() : null;

                var code = HttpStatusCode.InternalServerError; // 500 if unexpected

                if (ex is AccessDeniedException)
                {
                    code = HttpStatusCode.Forbidden;
                }
                if (ex is AuthorizationException)
                {
                    code = HttpStatusCode.Unauthorized;
                }
                else if (ex is NotFoundException)
                {
                    code = HttpStatusCode.NotFound;
                }
                else if (ex is ApplicationLayerException)
                {
                    code = HttpStatusCode.BadRequest;
                }

                var problem = new ProblemDetails
                {
                    Status = (int?)code,
                    Title = title,
                    Detail = details
                };

                // This is often very handy information for tracing the specific request
                var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
                if (traceId != null)
                {
                    problem.Extensions["traceId"] = traceId;
                }

                if (httpContext.Response == null)
                {
                    throw new InfrastructureLayerException("Reponse was null.");
                }

                httpContext.Response.ContentType = "application/problem+json";
                httpContext.Response.StatusCode = (int)code;

                //Serialize the problem details object to the Response as JSON (using System.Text.Json)
                var stream = httpContext.Response.Body;
                await JsonSerializer.SerializeAsync(stream, problem);
            }
        }
    }
}