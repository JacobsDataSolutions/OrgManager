// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Domain.HumanResources.TimeOff;
using System;
using Xunit;

namespace JDS.OrgManager.Domain.UnitTests.Models
{
    public class DomainEntityTests
    {
        [Fact]
        public void ReflectionCloneWith_WorksWithValueType()
        {
            var ptoRequest = new PaidTimeOffRequest { StartDate = DateTime.Today, EndDate = DateTime.Today, HoursRequested = 8, Notes = "HELLO", Status = PaidTimeOffRequestStatus.Pending, Id = 999, ApprovalStatus = PaidTimeOffRequestApprovalStatus.Submitted };
            var newRequest = ptoRequest.ReflectionCloneWith(req => req.Status, PaidTimeOffRequestStatus.Taken);
            Assert.Equal(ptoRequest.StartDate, newRequest.StartDate);
            Assert.Equal(ptoRequest.EndDate, newRequest.EndDate);
            Assert.Equal(ptoRequest.HoursRequested, newRequest.HoursRequested);
            Assert.Equal(ptoRequest.Notes, newRequest.Notes);
            Assert.Equal(ptoRequest.Id, newRequest.Id);
            Assert.Equal(ptoRequest.ApprovalStatus, newRequest.ApprovalStatus);
            Assert.Equal(PaidTimeOffRequestStatus.Taken, newRequest.Status);
            Assert.Equal(PaidTimeOffRequestStatus.Pending, ptoRequest.Status);
            Assert.NotSame(newRequest, ptoRequest);
        }
    }
}