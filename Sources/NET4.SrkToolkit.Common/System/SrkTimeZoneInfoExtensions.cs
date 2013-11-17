
namespace System
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class SrkTimeZoneInfoExtensions
    {
        public static DateTime ConvertToUtc(this TimeZoneInfo tz, DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
                return dateTime;
            else if (dateTime.Kind == DateTimeKind.Local)
                return TimeZoneInfo.ConvertTimeToUtc(dateTime.AsUnspecified(), tz);
            else
                return TimeZoneInfo.ConvertTimeToUtc(dateTime, tz);
        }

        public static DateTime ConvertFromUtc(this TimeZoneInfo tz, DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Local)
                return TimeZoneInfo.ConvertTimeFromUtc(dateTime.AsUnspecified(), tz).AsUnspecified();
            else
                return TimeZoneInfo.ConvertTimeFromUtc(dateTime, tz).AsUnspecified();
        }
    }
}
