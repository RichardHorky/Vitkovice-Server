using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V.Server.Helpers
{
    public class DateHelper
    {
        public long GetSeconds()
        {
            var now = DateTime.Now.AddHours(1);
            //get last march sunday
            var marchDays = DateTime.DaysInMonth(now.Year, 3);
            var lastDayOfMarch = new DateTime(now.Year, 3, marchDays);
            var marchSpan = -(int)lastDayOfMarch.DayOfWeek;
            var summerBegin = lastDayOfMarch.AddDays(marchSpan);

            //get last october sunday
            var octDays = DateTime.DaysInMonth(now.Year, 10);
            var lastDayOfOct = new DateTime(now.Year, 10, octDays);
            var octSpan = -(int)lastDayOfOct.DayOfWeek;
            var summerEnd = lastDayOfOct.AddDays(octSpan);

            if (now >= summerBegin && now < summerEnd)
                now = now.AddHours(1);

            var year = now.Year;
            var firstInYear = new DateTime(year, 1, 1);
            return (long)Math.Floor((now - firstInYear).TotalSeconds);
        }
    }
}
