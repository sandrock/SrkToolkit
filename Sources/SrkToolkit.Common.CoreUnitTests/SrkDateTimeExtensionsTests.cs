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

    public class SrkDateTimeExtensionsTests
    {
        public class GetDateMethod
        {
            [Fact]
            public void WorksForNow()
            {
                var date = DateTime.Now;

                var result = date.GetDate();

                Assert.Equal(date.Year, result.Year);
                Assert.Equal(date.Month, result.Month);
                Assert.Equal(date.Day, result.Day);
                Assert.Equal(0, result.Hour);
                Assert.Equal(0, result.Minute);
                Assert.Equal(0, result.Second);
                Assert.Equal(0, result.Millisecond);
                Assert.Equal(date.Kind, result.Kind);
            }

            [Fact]
            public void WorksForUtcNow()
            {
                var date = DateTime.UtcNow;

                var result = date.GetDate();

                Assert.Equal(date.Year, result.Year);
                Assert.Equal(date.Month, result.Month);
                Assert.Equal(date.Day, result.Day);
                Assert.Equal(0, result.Hour);
                Assert.Equal(0, result.Minute);
                Assert.Equal(0, result.Second);
                Assert.Equal(0, result.Millisecond);
                Assert.Equal(date.Kind, result.Kind);
            }

            [Fact]
            public void Works1()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 111, DateTimeKind.Unspecified);

                var result = date.GetDate();

                Assert.Equal(date.Year, result.Year);
                Assert.Equal(date.Month, result.Month);
                Assert.Equal(date.Day, result.Day);
                Assert.Equal(0, result.Hour);
                Assert.Equal(0, result.Minute);
                Assert.Equal(0, result.Second);
                Assert.Equal(0, result.Millisecond);
                Assert.Equal(date.Kind, result.Kind);
            }
        }

        public class ToPrecisionMethod
        {
            [Fact]
            public void ToMillisecond()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date.ToPrecision(DateTimePrecision.Millisecond);

                Assert.Equal(date.Year, result.Year);
                Assert.Equal(date.Month, result.Month);
                Assert.Equal(date.Day, result.Day);
                Assert.Equal(date.Hour, result.Hour);
                Assert.Equal(date.Minute, result.Minute);
                Assert.Equal(date.Second, result.Second);
                Assert.Equal(date.Millisecond, result.Millisecond);
                Assert.Equal(date.Kind, result.Kind);
            }

            [Fact]
            public void ToSecond()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date.ToPrecision(DateTimePrecision.Second);

                Assert.Equal(date.Year, result.Year);
                Assert.Equal(date.Month, result.Month);
                Assert.Equal(date.Day, result.Day);
                Assert.Equal(date.Hour, result.Hour);
                Assert.Equal(date.Minute, result.Minute);
                Assert.Equal(date.Second, result.Second);
                Assert.Equal(0, result.Millisecond);
                Assert.Equal(date.Kind, result.Kind);
            }

            [Fact]
            public void ToMinute()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date.ToPrecision(DateTimePrecision.Minute);

                Assert.Equal(date.Year, result.Year);
                Assert.Equal(date.Month, result.Month);
                Assert.Equal(date.Day, result.Day);
                Assert.Equal(date.Hour, result.Hour);
                Assert.Equal(date.Minute, result.Minute);
                Assert.Equal(0, result.Second);
                Assert.Equal(0, result.Millisecond);
                Assert.Equal(date.Kind, result.Kind);
            }

            [Fact]
            public void ToHour()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date.ToPrecision(DateTimePrecision.Hour);

                Assert.Equal(date.Year, result.Year);
                Assert.Equal(date.Month, result.Month);
                Assert.Equal(date.Day, result.Day);
                Assert.Equal(date.Hour, result.Hour);
                Assert.Equal(0, result.Minute);
                Assert.Equal(0, result.Second);
                Assert.Equal(0, result.Millisecond);
                Assert.Equal(date.Kind, result.Kind);
            }

            [Fact]
            public void ToDay()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date.ToPrecision(DateTimePrecision.Day);

                Assert.Equal(date.Year, result.Year);
                Assert.Equal(date.Month, result.Month);
                Assert.Equal(date.Day, result.Day);
                Assert.Equal(0, result.Hour);
                Assert.Equal(0, result.Minute);
                Assert.Equal(0, result.Second);
                Assert.Equal(0, result.Millisecond);
                Assert.Equal(date.Kind, result.Kind);
            }

            [Fact]
            public void ToMonth()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date.ToPrecision(DateTimePrecision.Month);

                Assert.Equal(date.Year, result.Year);
                Assert.Equal(date.Month, result.Month);
                Assert.Equal(1, result.Day);
                Assert.Equal(0, result.Hour);
                Assert.Equal(0, result.Minute);
                Assert.Equal(0, result.Second);
                Assert.Equal(0, result.Millisecond);
                Assert.Equal(date.Kind, result.Kind);
            }

            [Fact]
            public void ToYear()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date.ToPrecision(DateTimePrecision.Year);

                Assert.Equal(date.Year, result.Year);
                Assert.Equal(1, result.Month);
                Assert.Equal(1, result.Day);
                Assert.Equal(0, result.Hour);
                Assert.Equal(0, result.Minute);
                Assert.Equal(0, result.Second);
                Assert.Equal(0, result.Millisecond);
                Assert.Equal(date.Kind, result.Kind);
            }
        }

        public class IsEqualToMethod
        {
            [Fact]
            public void CompareMillisecondDifference()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 2, 2, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Millisecond);
                Assert.False(result);
            }

            [Fact]
            public void CompareMillisecondEquality()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Millisecond);
                Assert.True(result);
            }

            [Fact]
            public void CompareSecondDifference()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 3, 1, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Second);
                Assert.False(result);
            }

            [Fact]
            public void CompareSecondDifference_MsDifferent()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 3, 2, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Second);
                Assert.False(result);
            }

            [Fact]
            public void CompareSecondEquality()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Second);
                Assert.True(result);
            }

            [Fact]
            public void CompareSecondEquality_MsDifferent()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 2, 2, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Second);
                Assert.True(result);
            }

            [Fact]
            public void CompareMinuteDifference()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 4, 2, 1, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Minute);
                Assert.False(result);
            }

            [Fact]
            public void CompareMinuteDifference_SecondDifferent()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 4, 3, 1, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Minute);
                Assert.False(result);
            }

            [Fact]
            public void CompareMinuteEquality()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Minute);
                Assert.True(result);
            }

            [Fact]
            public void CompareMinuteEquality_SecondDifferent()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 3, 2, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Minute);
                Assert.True(result);
            }
        }

        public class AsLocalMethod
        {
            [Fact]
            public void SameValues()
            {
                // prepare
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Unspecified);
                var result = SrkDateTimeExtensions.AsLocal(date);

                // verify
                Assert.Equal(date.Year, result.Year);
                Assert.Equal(date.Month, result.Month);
                Assert.Equal(date.Day, result.Day);
                Assert.Equal(date.Hour, result.Hour);
                Assert.Equal(date.Minute, result.Minute);
                Assert.Equal(date.Second, result.Second);
                Assert.Equal(date.Millisecond, result.Millisecond);
                Assert.Equal(date.Ticks, result.Ticks);
                Assert.Equal(DateTimeKind.Local, result.Kind);
            }
        }

        public class AsUtcMethod
        {
            [Fact]
            public void SameValues()
            {
                // prepare
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Unspecified);
                var result = SrkDateTimeExtensions.AsUtc(date);

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
        }

        public class AsUnspecifiedMethod
        {
            [Fact]
            public void SameValues()
            {
                // prepare
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Local);
                var result = SrkDateTimeExtensions.AsUnspecified(date);

                // verify
                Assert.Equal(date.Year, result.Year);
                Assert.Equal(date.Month, result.Month);
                Assert.Equal(date.Day, result.Day);
                Assert.Equal(date.Hour, result.Hour);
                Assert.Equal(date.Minute, result.Minute);
                Assert.Equal(date.Second, result.Second);
                Assert.Equal(date.Millisecond, result.Millisecond);
                Assert.Equal(date.Ticks, result.Ticks);
                Assert.Equal(DateTimeKind.Unspecified, result.Kind);
            }
        }

        public class ToUnixTimeMethod
        {
            [Fact]
            public void UtcTime()
            {
                DateTime time = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                long result = time.ToUnixTime();
                Assert.Equal(1359466101, result);
            }
        }
    }
}
