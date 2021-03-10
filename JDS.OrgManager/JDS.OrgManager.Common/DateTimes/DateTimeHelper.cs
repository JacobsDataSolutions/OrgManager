// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
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