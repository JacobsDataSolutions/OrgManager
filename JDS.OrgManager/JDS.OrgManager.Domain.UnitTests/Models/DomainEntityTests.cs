using JDS.OrgManager.Domain.HumanResources.TimeOff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
