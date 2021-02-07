using System;

namespace JDS.OrgManager.Common.DateTimes
{
    public static class DateTimeHelper
    {
        public static DateTime GetLastDayOfTheMonth(DayOfWeek dayOfWeek, Month month, int year)
        {
            var date = new DateTime(year, (int)month, DateTime.DaysInMonth(year, (int)month));
            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(-1.0);
            }
            return date;
        }

        public static DateTime? GetNthDayOfTheMonth(int dayNum, DayOfWeek dayOfWeek, Month month, int year)
        {
            if (dayNum < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(dayNum));
            }

            var date = new DateTime(year, (int)month, 1);
            var currentDayNum = 0;
            do
            {
                if (date.DayOfWeek == dayOfWeek)
                {
                    currentDayNum++;
                    if (currentDayNum == dayNum)
                    {
                        return date;
                    }
                }
                date = date.AddDays(1.0);
            } while (date.Month == (int)month);
            return null;
        }
    }
}