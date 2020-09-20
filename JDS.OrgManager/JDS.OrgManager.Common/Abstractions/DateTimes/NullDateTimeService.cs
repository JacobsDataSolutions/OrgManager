using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Common.Abstractions.DateTimes
{
    public class NullDateTimeService : IDateTimeService
    {
        public DateTime UtcNow => new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
