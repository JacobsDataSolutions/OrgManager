using System;

namespace JDS.OrgManager.Common.Abstractions.DateTimes
{
    public interface IDateTimeService
    {
        #region Public Properties + Indexers

        DateTime UtcNow { get; }

        #endregion
    }
}