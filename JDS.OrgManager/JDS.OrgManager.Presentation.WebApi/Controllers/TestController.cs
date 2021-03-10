﻿// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : CqrsControllerBase
    {
        private readonly IMediator mediator;

        public TestController(IMediator mediator) => this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet("[action]")]
        public async Task<ActionResult> TestApplicationLayerException()
        {
            throw new ApplicationLayerException("ApplicationLayerException was thrown.");
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> TestAuthorizationException()
        {
            throw new AuthorizationException("AuthorizationException was thrown.");
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> TestInvalidOperationException()
        {
            throw new InvalidOperationException("InvalidOperationException was thrown.");
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> TestNotFoundException()
        {
            throw new NotFoundException("NotFoundException was thrown.");
        }
    }
}