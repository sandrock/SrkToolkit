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
            [TestMethod]
            public void ConvertsTimeFromUnspecified()
            {
                // prepare
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Unspecified);
                var tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tz, date);

                // verify
                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(date.Day, result.Day);
                Assert.AreEqual(date.Hour + 2, result.Hour);
                Assert.AreEqual(date.Minute, result.Minute);
                Assert.AreEqual(date.Second, result.Second);
                Assert.AreEqual(date.Millisecond, result.Millisecond);
                Assert.AreEqual(DateTimeKind.Unspecified, result.Kind);
            }

            [TestMethod]
            public void ConvertsTimeFromUtc()
            {
                // prepare
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Utc);
                var tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tz, date);

                // verify
                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(date.Day, result.Day);
                Assert.AreEqual(date.Hour + 2, result.Hour);
                Assert.AreEqual(date.Minute, result.Minute);
                Assert.AreEqual(date.Second, result.Second);
                Assert.AreEqual(date.Millisecond, result.Millisecond);
                Assert.AreEqual(DateTimeKind.Unspecified, result.Kind);
            }

            [TestMethod]
            public void ConvertsTimeFromLocal()
            {
                // prepare
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Local);
                var tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

                // execute
                var result = SrkTimeZoneInfoExtensions.ConvertFromUtc(tz, date);

                // verify
                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(date.Day, result.Day);
                Assert.AreEqual(date.Hour + 2, result.Hour);
                Assert.AreEqual(date.Minute, result.Minute);
                Assert.AreEqual(date.Second, result.Second);
                Assert.AreEqual(date.Millisecond, result.Millisecond);
                Assert.AreEqual(DateTimeKind.Unspecified, result.Kind);
            }
        }
    }
}
