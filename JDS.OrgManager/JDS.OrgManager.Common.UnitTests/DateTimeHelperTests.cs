using JDS.OrgManager.Common.DateTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace JDS.OrgManager.Common.UnitTests
{
    public class DateTimeHelperTests
    {
        public static readonly object[][] Data =
        {
            new object[] { 2020, Month.February, 1, DayOfWeek.Sunday, new DateTime(2020, 2, 2) },
            new object[] { 2020, Month.February, 2, DayOfWeek.Wednesday, new DateTime(2020, 2, 12) },
            new object[] { 2020, Month.February, 5, DayOfWeek.Saturday, new DateTime(2020, 2, 29) },
            new object[] { 2020, Month.February, 5, DayOfWeek.Sunday, (DateTime?)null },
            new object[] { 2020, Month.March, 5, DayOfWeek.Sunday, new DateTime(2020, 3, 29) },
            new object[] { 2020, Month.March, 1, DayOfWeek.Sunday, new DateTime(2020, 3, 1) },
            new object[] { 2020, Month.March, 1, DayOfWeek.Monday, new DateTime(2020, 3, 2) },
            new object[] { 2020, Month.March, 1, DayOfWeek.Tuesday, new DateTime(2020, 3, 3) },
            new object[] { 2020, Month.March, 1, DayOfWeek.Wednesday, new DateTime(2020, 3, 4) },
            new object[] { 2020, Month.March, 1, DayOfWeek.Thursday, new DateTime(2020, 3, 5) },
            new object[] { 2020, Month.March, 1, DayOfWeek.Friday, new DateTime(2020, 3, 6) },
            new object[] { 2020, Month.March, 1, DayOfWeek.Saturday, new DateTime(2020, 3, 7) },
            new object[] { 2020, Month.March, 5, DayOfWeek.Tuesday, new DateTime(2020, 3, 31) },
            new object[] { 2020, Month.March, 6, DayOfWeek.Tuesday, (DateTime?)null },
        };

        public static readonly object[][] Data2 =
        {
            new object[] { 2020, DayOfWeek.Sunday, Month.April, new DateTime(2020, 04, 26) },
            new object[] { 2021, DayOfWeek.Wednesday, Month.June, new DateTime(2021, 06, 30) },
            new object[] { 2020, DayOfWeek.Saturday, Month.February, new DateTime(2020, 02, 29) },
            new object[] { 2020, DayOfWeek.Friday, Month.December, new DateTime(2020, 12, 25) },
            new object[] { 2019, DayOfWeek.Wednesday, Month.October, new DateTime(2019, 10, 30) },
            new object[] { 2019, DayOfWeek.Monday, Month.November, new DateTime(2019, 11, 25) },
            new object[] { 2019, DayOfWeek.Tuesday, Month.December, new DateTime(2019, 12, 31) }
        };

        [Theory]
        [MemberData(nameof(Data2))]
        public void GetLastDayOfTheMonth_Works(int year, DayOfWeek dayOfWeek, Month month, DateTime expected) => Assert.Equal(expected, DateTimeHelper.GetLastDayOfTheMonth(dayOfWeek, month, year));

        [Fact]
        public void GetNthDayOfTheMonth_InvalidDayNum_ThrowsException() => Assert.Throws<ArgumentOutOfRangeException>(() => DateTimeHelper.GetNthDayOfTheMonth(0, DayOfWeek.Friday, Month.January, 2020));

        [Theory]
        [MemberData(nameof(Data))]
        public void GetNthDayOfTheMonth_Works(int year, Month month, int dayNum, DayOfWeek dayOfWeek, DateTime? expected) => Assert.Equal(expected, DateTimeHelper.GetNthDayOfTheMonth(dayNum, dayOfWeek, month, year));
    }
}

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.