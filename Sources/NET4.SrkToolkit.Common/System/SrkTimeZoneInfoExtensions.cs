// 
// Copyright 2014 SandRock
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

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
                return TimeZoneInfo.ConvertTime(dateTime.AsUnspecified(), tz, TimeZoneInfo.Utc);
            else
                return TimeZoneInfo.ConvertTime(dateTime, tz, TimeZoneInfo.Utc);
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
                return TimeZoneInfo.ConvertTime(dateTime.ToUniversalTime(), TimeZoneInfo.Utc, tz);
            else
                return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Utc, tz);
        }
    }
}
