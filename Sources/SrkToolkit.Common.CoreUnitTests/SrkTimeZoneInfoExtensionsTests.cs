// -----------------------------------------------------------------------
// <copyright file="SrkDateTimeExtensionsTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class SrkTimeZoneInfoExtensionsTests
    {
        public class ConvertToUtcMethod
        {
            [Fact]
            public void ConvertsTimeFromUnspecified()
            {
                // prepare
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Unspecified);
                var tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertToUtc(tz, date);

                // verify
                Assert.Equal(date.Year, result.Year);
                Assert.Equal(date.Month, result.Month);
                Assert.Equal(date.Day, result.Day);
                Assert.Equal(5, result.Hour);
                Assert.Equal(date.Minute, result.Minute);
                Assert.Equal(date.Second, result.Second);
                Assert.Equal(date.Millisecond, result.Millisecond);
                Assert.Equal(DateTimeKind.Utc, result.Kind);
            }

            [Fact]
            public void ConvertsTimeFromUtc()
            {
                // prepare
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Utc);
                var tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertToUtc(tz, date);

                // verify
                Assert.Equal(date.Year, result.Year);
                Assert.Equal(date.Month, result.Month);
                Assert.Equal(date.Day, result.Day);
                Assert.Equal(date.Hour, result.Hour);
                Assert.Equal(date.Minute, result.Minute);
                Assert.Equal(date.Second, result.Second);
                Assert.Equal(date.Millisecond, result.Millisecond);
                Assert.Equal(date.Ticks, result.Ticks);
                Assert.Equal(DateTimeKind.Utc, result.Kind);
            }

            [Fact]
            public void ConvertsTimeFromLocal()
            {
                // prepare
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Local);
                var tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertToUtc(tz, date);

                // verify
                Assert.Equal(date.Year, result.Year);
                Assert.Equal(date.Month, result.Month);
                Assert.Equal(date.Day, result.Day);
                Assert.Equal(5, result.Hour);
                Assert.Equal(date.Minute, result.Minute);
                Assert.Equal(date.Second, result.Second);
                Assert.Equal(date.Millisecond, result.Millisecond);
                Assert.Equal(DateTimeKind.Utc, result.Kind);
            }
        }

        public class ConvertFromUtcMethod
        {
            /*
             * Use case: convert a UTC time to the user's time.
             * When given time is UTC     => directly convert
             * When given time is machine => convert to UTC then convert
             * When given time is unknown => assume UTC
             * 
             * When converting to the UTC TZ, ensure the DateTime.Kind is UTC.
             */

            [Fact]
            public void ConvertsTimeFromUnspecified()
            {
                // prepare
                ////var tzFrom = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"); // assume Unknown is UTC
                var dateFrom = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Unspecified);
                var tzTo = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tzTo, dateFrom);

                // verify
                Assert.Equal(dateFrom.Year, result.Year);
                Assert.Equal(dateFrom.Month, result.Month);
                Assert.Equal(dateFrom.Day, result.Day);
                Assert.Equal(dateFrom.Hour + 2, result.Hour);
                Assert.Equal(dateFrom.Minute, result.Minute);
                Assert.Equal(dateFrom.Second, result.Second);
                Assert.Equal(dateFrom.Millisecond, result.Millisecond);
                Assert.Equal(DateTimeKind.Unspecified, result.Kind);
            }

            [Fact]
            public void ConvertsTimeFromUtc()
            {
                // prepare
                var dateFrom = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Utc);
                var tzTo = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tzTo, dateFrom);

                // verify
                Assert.Equal(dateFrom.Year, result.Year);
                Assert.Equal(dateFrom.Month, result.Month);
                Assert.Equal(dateFrom.Day, result.Day);
                Assert.Equal(dateFrom.Hour + 2, result.Hour);
                Assert.Equal(dateFrom.Minute, result.Minute);
                Assert.Equal(dateFrom.Second, result.Second);
                Assert.Equal(dateFrom.Millisecond, result.Millisecond);
                Assert.Equal(DateTimeKind.Unspecified, result.Kind);
            }

            [Fact]
            public void ConvertsTimeFromLocal()
            {
                // prepare
                var tzFrom = TimeZoneInfo.Local;
                var dateFrom = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Local);
                var tzTo = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tzTo, dateFrom);

                // verify
                Assert.Equal(dateFrom.Year, result.Year);
                Assert.Equal(dateFrom.Month, result.Month);
                Assert.Equal(dateFrom.Day, result.Day);
                var expectedHour = dateFrom.Hour - tzFrom.GetUtcOffset(dateFrom).TotalHours + tzTo.GetUtcOffset(dateFrom).TotalHours;
                Assert.Equal(expectedHour, result.Hour);
                Assert.Equal(dateFrom.Minute, result.Minute);
                Assert.Equal(dateFrom.Second, result.Second);
                Assert.Equal(dateFrom.Millisecond, result.Millisecond);
                Assert.Equal(DateTimeKind.Unspecified, result.Kind);
            }

            [Fact]
            public void ConvertsTimeFromUnspecified_ToUtc()
            {
                // prepare
                var dateFrom = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Unspecified); // assume Unknown is UTC
                var tzTo = TimeZoneInfo.FindSystemTimeZoneById("UTC");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tzTo, dateFrom);

                // verify
                Assert.Equal(dateFrom.Year, result.Year);
                Assert.Equal(dateFrom.Month, result.Month);
                Assert.Equal(dateFrom.Day, result.Day);
                Assert.Equal(dateFrom.Hour, result.Hour);
                Assert.Equal(dateFrom.Minute, result.Minute);
                Assert.Equal(dateFrom.Second, result.Second);
                Assert.Equal(dateFrom.Millisecond, result.Millisecond);
                Assert.Equal(DateTimeKind.Utc, result.Kind);
            }

            [Fact]
            public void ConvertsTimeFromUtc_ToUtc()
            {
                // prepare
                var dateFrom = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Utc);
                var tzTo = TimeZoneInfo.FindSystemTimeZoneById("UTC");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tzTo, dateFrom);

                // verify
                Assert.Equal(dateFrom.Year, result.Year);
                Assert.Equal(dateFrom.Month, result.Month);
                Assert.Equal(dateFrom.Day, result.Day);
                Assert.Equal(dateFrom.Hour, result.Hour);
                Assert.Equal(dateFrom.Minute, result.Minute);
                Assert.Equal(dateFrom.Second, result.Second);
                Assert.Equal(dateFrom.Millisecond, result.Millisecond);
                Assert.Equal(DateTimeKind.Utc, result.Kind);
            }

            [Fact]
            public void ConvertsTimeFromLocal_ToUtc()
            {
                // prepare
                var tzFrom = TimeZoneInfo.Local;
                var dateFrom = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Local);
                var tzTo = TimeZoneInfo.FindSystemTimeZoneById("UTC");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tzTo, dateFrom);

                // verify
                Assert.Equal(dateFrom.Year, result.Year);
                Assert.Equal(dateFrom.Month, result.Month);
                Assert.Equal(dateFrom.Day, result.Day);
                var expectedHour = dateFrom.Hour - tzFrom.GetUtcOffset(dateFrom).TotalHours + tzTo.GetUtcOffset(dateFrom).TotalHours;
                Assert.Equal(expectedHour, result.Hour);
                Assert.Equal(dateFrom.Minute, result.Minute);
                Assert.Equal(dateFrom.Second, result.Second);
                Assert.Equal(dateFrom.Millisecond, result.Millisecond);
                Assert.Equal(DateTimeKind.Utc, result.Kind);
            }
        }
    }
}
