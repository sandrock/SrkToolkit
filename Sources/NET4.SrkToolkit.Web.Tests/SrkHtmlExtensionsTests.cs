
namespace SrkToolkit.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Web.Mvc;

    public class SrkHtmlExtensionsTests
    {
        [TestClass]
        public class JsDate
        {
            [TestMethod]
            public void PrecisionDefault()
            {
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Local);
                var expected = "new Date(1234, 4, 6, 7, 8, 9)";
                var result = SrkHtmlExtensions.JsDate(null, date);
                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void PrecisionSecond()
            {
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Local);
                var expected = "new Date(1234, 4, 6, 7, 8, 9)";
                var result = SrkHtmlExtensions.JsDate(null, date);
                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void PrecisionMinute()
            {
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Local);
                var expected = "new Date(1234, 4, 6, 7, 8, 0)";
                var result = SrkHtmlExtensions.JsDate(null, date, DateTimePrecision.Minute);
                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void PrecisionHour()
            {
                var date = new DateTime(1234, 5, 6, 7, 8, 9, 123, DateTimeKind.Local);
                var expected = "new Date(1234, 4, 6, 7, 0, 0)";
                var result = SrkHtmlExtensions.JsDate(null, date, DateTimePrecision.Hour);
                Assert.AreEqual(expected, result.ToString());
            }
        }
    }
}
