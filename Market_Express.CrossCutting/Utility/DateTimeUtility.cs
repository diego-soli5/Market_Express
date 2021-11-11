using System;

namespace Market_Express.CrossCutting.Utility
{
    public static class DateTimeUtility
    {
        public static DateTime NowCostaRica
        {
            get { return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time")); }
        }

        public static DateTime Truncate(DateTime date) => new DateTime(date.Year, date.Month, date.Day);
    }
}
