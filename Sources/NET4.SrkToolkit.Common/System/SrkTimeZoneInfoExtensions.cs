
namespace System
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension methods for the <see cref="TimeZoneInfo"/> class.
    /// </summary>
    public static class SrkTimeZoneInfoExtensions
    {
        /// <summary>
        /// Converts the specified time from the current time zone to Coordinated Universal Time (UTC).
        /// </summary>
        /// <param name="tz">The tz.</param>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <returns></returns>
        public static DateTime ConvertToUtc(this TimeZoneInfo tz, DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
                return dateTime;
            else if (dateTime.Kind == DateTimeKind.Local)
                return TimeZoneInfo.ConvertTimeToUtc(dateTime.AsUnspecified(), tz);
            else
                return TimeZoneInfo.ConvertTimeToUtc(dateTime, tz);
        }

        /// <summary>
        /// Converts the specified time from Coordinated Universal Time (UTC) to the current time zone.
        /// </summary>
        /// <param name="tz">The tz.</param>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <returns></returns>
        public static DateTime ConvertFromUtc(this TimeZoneInfo tz, DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Local)
                return TimeZoneInfo.ConvertTimeFromUtc(dateTime.ToUniversalTime(), tz);
            else
                return TimeZoneInfo.ConvertTimeFromUtc(dateTime, tz);
        }
    }
}
