
namespace SrkToolkit.Web.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.Web.Fakes;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class SrkHttpContextExtensionsTests
    {
        [TestClass]
        public class SetTimezoneMethod
        {
            [TestMethod]
            public void WorksWithTzObject()
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                var http = new BasicHttpContext();
                SrkHttpContextExtensions.SetTimezone(http, tz);

                Assert.IsNotNull(http.Items["Timezone"]);
                Assert.AreEqual(tz, http.Items["Timezone"]);
            }

            [TestMethod]
            public void WorksWithTzName()
            {
                var tzName = "Romance Standard Time";
                var tz = TimeZoneInfo.FindSystemTimeZoneById(tzName);
                var http = new BasicHttpContext();
                SrkHttpContextExtensions.SetTimezone(http, tzName);

                Assert.IsNotNull(http.Items["Timezone"]);
                Assert.AreEqual(tz, http.Items["Timezone"]);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void NullTzName()
            {
                string tzName = null;
                var http = new BasicHttpContext();
                SrkHttpContextExtensions.SetTimezone(http, tzName);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void EmptyTzName()
            {
                string tzName = string.Empty;
                var http = new BasicHttpContext();
                SrkHttpContextExtensions.SetTimezone(http, tzName);
            }

            [TestMethod, ExpectedException(typeof(TimeZoneNotFoundException))]
            public void InvalidTzName()
            {
                string tzName = "Lunar Standard Time";
                var http = new BasicHttpContext();
                SrkHttpContextExtensions.SetTimezone(http, tzName);
            }

            [TestMethod]
            public void GetterWorks()
            {
                var tzName = "Romance Standard Time";
                var tz = TimeZoneInfo.FindSystemTimeZoneById(tzName);
                var http = new BasicHttpContext();
                SrkHttpContextExtensions.SetTimezone(http, tzName);
                var result = SrkHttpContextExtensions.GetTimezone(http);

                Assert.IsNotNull(result);
                Assert.AreEqual(tz, result);
            }
        }
    }
}
