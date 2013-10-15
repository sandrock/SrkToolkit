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

    public class SrkDateTimeExtensionsTests
    {
        [TestClass]
        public class GetDateMethod
        {
            [TestMethod]
            public void WorksForNow()
            {
                var date = DateTime.Now;

                var result = date.GetDate();

                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(date.Day, result.Day);
                Assert.AreEqual(0, result.Hour);
                Assert.AreEqual(0, result.Minute);
                Assert.AreEqual(0, result.Second);
                Assert.AreEqual(0, result.Millisecond);
                Assert.AreEqual(date.Kind, result.Kind);
            }

            [TestMethod]
            public void WorksForUtcNow()
            {
                var date = DateTime.UtcNow;

                var result = date.GetDate();

                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(date.Day, result.Day);
                Assert.AreEqual(0, result.Hour);
                Assert.AreEqual(0, result.Minute);
                Assert.AreEqual(0, result.Second);
                Assert.AreEqual(0, result.Millisecond);
                Assert.AreEqual(date.Kind, result.Kind);
            }

            [TestMethod]
            public void Works1()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 111, DateTimeKind.Unspecified);

                var result = date.GetDate();

                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(date.Day, result.Day);
                Assert.AreEqual(0, result.Hour);
                Assert.AreEqual(0, result.Minute);
                Assert.AreEqual(0, result.Second);
                Assert.AreEqual(0, result.Millisecond);
                Assert.AreEqual(date.Kind, result.Kind);
            }
        }

        [TestClass]
        public class ToPrecisionMethod
        {
            [TestMethod]
            public void ToMillisecond()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date.ToPrecision(DateTimePrecision.Millisecond);

                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(date.Day, result.Day);
                Assert.AreEqual(date.Hour, result.Hour);
                Assert.AreEqual(date.Minute, result.Minute);
                Assert.AreEqual(date.Second, result.Second);
                Assert.AreEqual(date.Millisecond, result.Millisecond);
                Assert.AreEqual(date.Kind, result.Kind);
            }

            [TestMethod]
            public void ToSecond()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date.ToPrecision(DateTimePrecision.Second);

                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(date.Day, result.Day);
                Assert.AreEqual(date.Hour, result.Hour);
                Assert.AreEqual(date.Minute, result.Minute);
                Assert.AreEqual(date.Second, result.Second);
                Assert.AreEqual(0, result.Millisecond);
                Assert.AreEqual(date.Kind, result.Kind);
            }

            [TestMethod]
            public void ToMinute()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date.ToPrecision(DateTimePrecision.Minute);

                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(date.Day, result.Day);
                Assert.AreEqual(date.Hour, result.Hour);
                Assert.AreEqual(date.Minute, result.Minute);
                Assert.AreEqual(0, result.Second);
                Assert.AreEqual(0, result.Millisecond);
                Assert.AreEqual(date.Kind, result.Kind);
            }

            [TestMethod]
            public void ToHour()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date.ToPrecision(DateTimePrecision.Hour);

                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(date.Day, result.Day);
                Assert.AreEqual(date.Hour, result.Hour);
                Assert.AreEqual(0, result.Minute);
                Assert.AreEqual(0, result.Second);
                Assert.AreEqual(0, result.Millisecond);
                Assert.AreEqual(date.Kind, result.Kind);
            }

            [TestMethod]
            public void ToDay()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date.ToPrecision(DateTimePrecision.Day);

                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(date.Day, result.Day);
                Assert.AreEqual(0, result.Hour);
                Assert.AreEqual(0, result.Minute);
                Assert.AreEqual(0, result.Second);
                Assert.AreEqual(0, result.Millisecond);
                Assert.AreEqual(date.Kind, result.Kind);
            }

            [TestMethod]
            public void ToMonth()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date.ToPrecision(DateTimePrecision.Month);

                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(date.Month, result.Month);
                Assert.AreEqual(1, result.Day);
                Assert.AreEqual(0, result.Hour);
                Assert.AreEqual(0, result.Minute);
                Assert.AreEqual(0, result.Second);
                Assert.AreEqual(0, result.Millisecond);
                Assert.AreEqual(date.Kind, result.Kind);
            }

            [TestMethod]
            public void ToYear()
            {
                var date = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date.ToPrecision(DateTimePrecision.Year);

                Assert.AreEqual(date.Year, result.Year);
                Assert.AreEqual(1, result.Month);
                Assert.AreEqual(1, result.Day);
                Assert.AreEqual(0, result.Hour);
                Assert.AreEqual(0, result.Minute);
                Assert.AreEqual(0, result.Second);
                Assert.AreEqual(0, result.Millisecond);
                Assert.AreEqual(date.Kind, result.Kind);
            }
        }

        [TestClass]
        public class IsEqualToMethod
        {
            [TestMethod]
            public void CompareMillisecondDifference()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 2, 2, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Millisecond);
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void CompareMillisecondEquality()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Millisecond);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void CompareSecondDifference()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 3, 1, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Second);
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void CompareSecondDifference_MsDifferent()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 3, 2, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Second);
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void CompareSecondEquality()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Second);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void CompareSecondEquality_MsDifferent()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 2, 2, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Second);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void CompareMinuteDifference()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 4, 2, 1, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Minute);
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void CompareMinuteDifference_SecondDifferent()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 4, 3, 1, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Minute);
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void CompareMinuteEquality()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Minute);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void CompareMinuteEquality_SecondDifferent()
            {
                var date1 = new DateTime(1987, 6, 5, 4, 3, 2, 1, DateTimeKind.Utc);
                var date2 = new DateTime(1987, 6, 5, 4, 3, 3, 2, DateTimeKind.Utc);
                var result = date1.IsEqualTo(date2, DateTimePrecision.Minute);
                Assert.IsTrue(result);
            }
        }
    }
}
