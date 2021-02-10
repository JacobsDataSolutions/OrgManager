// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace JDS.OrgManager.Infrastructure.ErrorHandling
{
    public class CustomErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public CustomErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IWebHostEnvironment env)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, env);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, IWebHostEnvironment env)
        {
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

            var includeDetails = env.IsEnvironment("Development");
            var title = includeDetails ? "An error occured: " + ex.Message : "An error occured";
            var details = includeDetails ? ex.ToString() : null;

            var problem = new ProblemDetails
            {
                Status = (int?)code,
                Title = title,
                Detail = details
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)code;

            var result = JsonConvert.SerializeObject(problem);
            return context.Response.WriteAsync(result);
        }
    }
}