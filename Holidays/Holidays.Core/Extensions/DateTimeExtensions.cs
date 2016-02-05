using System;

namespace Holidays.Core.Extensions
{
    internal static class DateTimeExtensions
    {
        public static DateTime Previous(this DateTime dt, DayOfWeek day)
        {
            var date = dt;

            if (date.DayOfWeek == day) date = date.AddDays(-1);

            while (date.DayOfWeek != day)
            {
                date = date.AddDays(-1);
            }

            return date;
        }

        public static DateTime Next(this DateTime dt, DayOfWeek day)
        {
            var date = dt;

            date = date.AddDays(1);

            while (date.DayOfWeek != day)
            {
                date = date.AddDays(1);
            }

            return date;
        }
    }
}