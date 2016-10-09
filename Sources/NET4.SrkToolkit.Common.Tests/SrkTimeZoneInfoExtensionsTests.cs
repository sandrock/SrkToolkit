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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class SrkTimeZoneInfoExtensionsTests
    {
        [TestClass]
        public class ConvertToUtcMethod
        {
            [TestMethod]
            public void ConvertsTimeFromUnspecified()
            {
                // prepare
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Unspecified);
                var tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertToUtc(tz, date);

                // verify
                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(date.Day, result.Day);
                Assert.AreEqual(5, result.Hour);
                Assert.AreEqual(date.Minute, result.Minute);
                Assert.AreEqual(date.Second, result.Second);
                Assert.AreEqual(date.Millisecond, result.Millisecond);
                Assert.AreEqual(DateTimeKind.Utc, result.Kind);
            }

            [TestMethod]
            public void ConvertsTimeFromUtc()
            {
                // prepare
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Utc);
                var tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertToUtc(tz, date);

                // verify
                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(date.Day, result.Day);
                Assert.AreEqual(date.Hour, result.Hour);
                Assert.AreEqual(date.Minute, result.Minute);
                Assert.AreEqual(date.Second, result.Second);
                Assert.AreEqual(date.Millisecond, result.Millisecond);
                Assert.AreEqual(date.Ticks, result.Ticks);
                Assert.AreEqual(DateTimeKind.Utc, result.Kind);
            }

            [TestMethod]
            public void ConvertsTimeFromLocal()
            {
                // prepare
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Local);
                var tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertToUtc(tz, date);

                // verify
                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(date.Day, result.Day);
                Assert.AreEqual(5, result.Hour);
                Assert.AreEqual(date.Minute, result.Minute);
                Assert.AreEqual(date.Second, result.Second);
                Assert.AreEqual(date.Millisecond, result.Millisecond);
                Assert.AreEqual(DateTimeKind.Utc, result.Kind);
            }
        }

        [TestClass]
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

            [TestMethod]
            public void ConvertsTimeFromUnspecified()
            {
                // prepare
                ////var tzFrom = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"); // assume Unknown is UTC
                var dateFrom = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Unspecified);
                var tzTo = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tzTo, dateFrom);

                // verify
                Assert.AreEqual(dateFrom.Year, result.Year);
                Assert.AreEqual(dateFrom.Month, result.Month);
                Assert.AreEqual(dateFrom.Day, result.Day);
                Assert.AreEqual(dateFrom.Hour + 2, result.Hour);
                Assert.AreEqual(dateFrom.Minute, result.Minute);
                Assert.AreEqual(dateFrom.Second, result.Second);
                Assert.AreEqual(dateFrom.Millisecond, result.Millisecond);
                Assert.AreEqual(DateTimeKind.Unspecified, result.Kind);
            }

            [TestMethod]
            public void ConvertsTimeFromUtc()
            {
                // prepare
                var dateFrom = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Utc);
                var tzTo = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tzTo, dateFrom);

                // verify
                Assert.AreEqual(dateFrom.Year, result.Year);
                Assert.AreEqual(dateFrom.Month, result.Month);
                Assert.AreEqual(dateFrom.Day, result.Day);
                Assert.AreEqual(dateFrom.Hour + 2, result.Hour);
                Assert.AreEqual(dateFrom.Minute, result.Minute);
                Assert.AreEqual(dateFrom.Second, result.Second);
                Assert.AreEqual(dateFrom.Millisecond, result.Millisecond);
                Assert.AreEqual(DateTimeKind.Unspecified, result.Kind);
            }

            [TestMethod]
            public void ConvertsTimeFromLocal()
            {
                // prepare
                var tzFrom = TimeZoneInfo.Local;
                var dateFrom = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Local);
                var tzTo = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tzTo, dateFrom);

                // verify
                Assert.AreEqual(dateFrom.Year, result.Year);
                Assert.AreEqual(dateFrom.Month, result.Month);
                Assert.AreEqual(dateFrom.Day, result.Day);
                var expectedHour = dateFrom.Hour - tzFrom.GetUtcOffset(dateFrom).TotalHours + tzTo.GetUtcOffset(dateFrom).TotalHours;
                Assert.AreEqual(expectedHour, result.Hour);
                Assert.AreEqual(dateFrom.Minute, result.Minute);
                Assert.AreEqual(dateFrom.Second, result.Second);
                Assert.AreEqual(dateFrom.Millisecond, result.Millisecond);
                Assert.AreEqual(DateTimeKind.Unspecified, result.Kind);
            }

            [TestMethod]
            public void ConvertsTimeFromUnspecified_ToUtc()
            {
                // prepare
                var dateFrom = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Unspecified); // assume Unknown is UTC
                var tzTo = TimeZoneInfo.FindSystemTimeZoneById("UTC");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tzTo, dateFrom);

                // verify
                Assert.AreEqual(dateFrom.Year, result.Year);
                Assert.AreEqual(dateFrom.Month, result.Month);
                Assert.AreEqual(dateFrom.Day, result.Day);
                Assert.AreEqual(dateFrom.Hour, result.Hour);
                Assert.AreEqual(dateFrom.Minute, result.Minute);
                Assert.AreEqual(dateFrom.Second, result.Second);
                Assert.AreEqual(dateFrom.Millisecond, result.Millisecond);
                Assert.AreEqual(DateTimeKind.Utc, result.Kind);
            }

            [TestMethod]
            public void ConvertsTimeFromUtc_ToUtc()
            {
                // prepare
                var dateFrom = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Utc);
                var tzTo = TimeZoneInfo.FindSystemTimeZoneById("UTC");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tzTo, dateFrom);

                // verify
                Assert.AreEqual(dateFrom.Year, result.Year);
                Assert.AreEqual(dateFrom.Month, result.Month);
                Assert.AreEqual(dateFrom.Day, result.Day);
                Assert.AreEqual(dateFrom.Hour, result.Hour);
                Assert.AreEqual(dateFrom.Minute, result.Minute);
                Assert.AreEqual(dateFrom.Second, result.Second);
                Assert.AreEqual(dateFrom.Millisecond, result.Millisecond);
                Assert.AreEqual(DateTimeKind.Utc, result.Kind);
            }

            [TestMethod]
            public void ConvertsTimeFromLocal_ToUtc()
            {
                // prepare
                var tzFrom = TimeZoneInfo.Local;
                var dateFrom = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Local);
                var tzTo = TimeZoneInfo.FindSystemTimeZoneById("UTC");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tzTo, dateFrom);

                // verify
                Assert.AreEqual(dateFrom.Year, result.Year);
                Assert.AreEqual(dateFrom.Month, result.Month);
                Assert.AreEqual(dateFrom.Day, result.Day);
                var expectedHour = dateFrom.Hour - tzFrom.GetUtcOffset(dateFrom).TotalHours + tzTo.GetUtcOffset(dateFrom).TotalHours;
                Assert.AreEqual(expectedHour, result.Hour);
                Assert.AreEqual(dateFrom.Minute, result.Minute);
                Assert.AreEqual(dateFrom.Second, result.Second);
                Assert.AreEqual(dateFrom.Millisecond, result.Millisecond);
                Assert.AreEqual(DateTimeKind.Utc, result.Kind);
            }
        }
    }
}
