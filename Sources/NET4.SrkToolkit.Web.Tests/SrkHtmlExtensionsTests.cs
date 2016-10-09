
namespace SrkToolkit.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Web.Mvc;
    using Moq;
    using System.Web.Routing;
    using System.Web;
    using System.IO;
    using System.Diagnostics;
    using System.Threading;
    using System.Globalization;
    using System.ComponentModel.DataAnnotations;
    using SrkToolkit.Web.Fakes;

    public class SrkHtmlExtensionsTests
    {
        public static readonly CultureInfo TestCulture1 = new CultureInfo("en-GB");

        public static HtmlHelper CreateHtmlHelper(ViewDataDictionary vd)
        {
            var httpContext = new BasicHttpContext();
            var mockViewContext = new ViewContext(
                new ControllerContext(
                    httpContext,
                    new RouteData(),
                    new Mock<ControllerBase>().Object),
                new Mock<IView>().Object,
                vd,
                new TempDataDictionary(),
                new Mock<TextWriter>().Object);
            var mockViewDataContainer = new Mock<IViewDataContainer>();
            mockViewDataContainer.Setup(v => v.ViewData)
                .Returns(vd);
            return new HtmlHelper(mockViewContext, mockViewDataContainer.Object);
        }

        [TestClass]
        public class SetTimezoneMethod
        {
            [TestMethod]
            public void WorksWithTzObject1()
            {
                var data = new ViewDataDictionary();
                var tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                var html = CreateHtmlHelper(data);
                SrkHtmlExtensions.SetTimezone(html, tz);

                Assert.IsNotNull(data["Timezone"]);
                Assert.AreEqual(tz, data["Timezone"]);
            }

            [TestMethod]
            public void WorksWithTzObject2()
            {
                var data = new ViewDataDictionary();
                var tz = TimeZoneInfo.FindSystemTimeZoneById("Russia Time Zone 3");
                var html = CreateHtmlHelper(data);
                SrkHtmlExtensions.SetTimezone(html, tz);

                Assert.IsNotNull(data["Timezone"]);
                Assert.AreEqual(tz, data["Timezone"]);
            }

            [TestMethod]
            public void WorksWithTzName1()
            {
                var data = new ViewDataDictionary();
                var tzName = "Romance Standard Time";
                var tz = TimeZoneInfo.FindSystemTimeZoneById(tzName);
                var html = CreateHtmlHelper(data);
                SrkHtmlExtensions.SetTimezone(html, tzName);

                Assert.IsNotNull(data["Timezone"]);
                Assert.AreEqual(tz, data["Timezone"]);
            }

            [TestMethod]
            public void WorksWithTzName2()
            {
                var data = new ViewDataDictionary();
                var tzName = "Russia Time Zone 3";
                var tz = TimeZoneInfo.FindSystemTimeZoneById(tzName);
                var html = CreateHtmlHelper(data);
                SrkHtmlExtensions.SetTimezone(html, tzName);

                Assert.IsNotNull(data["Timezone"]);
                Assert.AreEqual(tz, data["Timezone"]);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void NullTzName()
            {
                string tzName = null;
                var html = CreateHtmlHelper(new ViewDataDictionary());
                SrkHtmlExtensions.SetTimezone(html, tzName);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void EmptyTzName()
            {
                string tzName = string.Empty;
                var html = CreateHtmlHelper(new ViewDataDictionary());
                SrkHtmlExtensions.SetTimezone(html, tzName);
            }

            [TestMethod, ExpectedException(typeof(TimeZoneNotFoundException))]
            public void InvalidTzName()
            {
                string tzName = "Lunar Standard Time";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                SrkHtmlExtensions.SetTimezone(html, tzName);
            }

            [TestMethod]
            public void GetterWorks1()
            {
                var data = new ViewDataDictionary();
                var tzName = "Romance Standard Time";
                var tz = TimeZoneInfo.FindSystemTimeZoneById(tzName);
                var html = CreateHtmlHelper(data);
                SrkHtmlExtensions.SetTimezone(html, tzName);
                var result = SrkHtmlExtensions.GetTimezone(html);

                Assert.IsNotNull(result);
                Assert.AreEqual(tz, result);
            }

            [TestMethod]
            public void GetterWorks2()
            {
                var data = new ViewDataDictionary();
                var tzName = "Russia Time Zone 3";
                var tz = TimeZoneInfo.FindSystemTimeZoneById(tzName);
                var html = CreateHtmlHelper(data);
                SrkHtmlExtensions.SetTimezone(html, tzName);
                var result = SrkHtmlExtensions.GetTimezone(html);

                Assert.IsNotNull(result);
                Assert.AreEqual(tz, result);
            }

            [TestMethod]
            public void WorksWhenSetInHttpContext1()
            {
                var data = new ViewDataDictionary();
                var tzName = "Romance Standard Time";
                var tz = TimeZoneInfo.FindSystemTimeZoneById(tzName);
                var html = CreateHtmlHelper(data);
                html.ViewContext.HttpContext.SetTimezone(tz);
                var result = SrkHtmlExtensions.GetTimezone(html);

                Assert.IsNotNull(result);
                Assert.AreEqual(tz, result);
            }

            [TestMethod]
            public void WorksWhenSetInHttpContext2()
            {
                var data = new ViewDataDictionary();
                var tzName = "Russia Time Zone 3";
                var tz = TimeZoneInfo.FindSystemTimeZoneById(tzName);
                var html = CreateHtmlHelper(data);
                html.ViewContext.HttpContext.SetTimezone(tz);
                var result = SrkHtmlExtensions.GetTimezone(html);

                Assert.IsNotNull(result);
                Assert.AreEqual(tz, result);
            }
        }

        [TestClass]
        public class GetUserDateMethod
        {
            [TestMethod]
            public void UndefinedTzIsUtc_ArgIsUtc_ResultIsUtc()
            {
                DateTime source = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                TimeZoneInfo tz = null;
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                DateTime utcResult;
                var result = SrkHtmlExtensions.GetUserDate(html, source, out utcResult);

                Assert.AreEqual(source, result);
                Assert.AreEqual(source, utcResult);
            }

            [TestMethod]
            public void UndefinedTzIsUtc_ArgIsLocal_ResultIsUtc()
            {
                DateTime orig = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                DateTime source = orig.ToLocalTime();
                Assert.AreEqual(DateTimeKind.Local, source.Kind);
                TimeZoneInfo tz = null;
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                DateTime utcResult;
                var result = SrkHtmlExtensions.GetUserDate(html, source, out utcResult);

                Assert.AreEqual(orig, result);
                Assert.AreEqual(orig, utcResult);
            }

            [TestMethod]
            public void UndefinedTzIsUtc_ArgIsUnspecified_ResultIsUtc()
            {
                DateTime orig = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                DateTime source = TimeZoneInfo.Utc.ConvertFromUtc(orig);
                Assert.AreEqual(DateTimeKind.Utc, source.Kind);
                TimeZoneInfo tz = null;
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                DateTime utcResult;
                var result = SrkHtmlExtensions.GetUserDate(html, source, out utcResult);

                Assert.AreEqual(orig, result);
                Assert.AreEqual(orig, utcResult);
            }

            [TestMethod]
            public void RomanceTz_ArgIsUtc_ResultIsRomance()
            {
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime orig = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                DateTime source = tz.ConvertFromUtc(orig);
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                DateTime utcResult;
                var result = SrkHtmlExtensions.GetUserDate(html, source, out utcResult);

                Assert.AreEqual(source, result, "wrong user result");
                Assert.AreEqual(orig, utcResult, "wrong UTC result");
            }

            [TestMethod]
            public void RomanceTz_ArgIsLocal_ResultIsRomance()
            {
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime orig = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                DateTime source = orig.ToLocalTime();
                DateTime expected = tz.ConvertFromUtc(orig);
                Assert.AreEqual(DateTimeKind.Local, source.Kind);
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                DateTime utcResult;
                var result = SrkHtmlExtensions.GetUserDate(html, source, out utcResult);

                Assert.AreEqual(expected, result, "wrong user result");
                Assert.AreEqual(orig, utcResult, "wrong UTC result");
            }

            [TestMethod]
            public void RomanceTz_ArgIsUnspecified_ResultIsUtc()
            {
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime orig = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                DateTime source = tz.ConvertFromUtc(orig);
                Assert.AreEqual(DateTimeKind.Unspecified, source.Kind);
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                DateTime utcResult;
                var result = SrkHtmlExtensions.GetUserDate(html, source, out utcResult);

                Assert.AreEqual(source, result, "wrong user result");
                Assert.AreEqual(orig, utcResult, "wrong UTC result");
            }
        }

        [TestClass]
        public class DisplayDateMethod
        {
            [TestMethod]
            public void UserIsUtc_ArgIsUtc_ResultIsUtc()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                DateTime source = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                TimeZoneInfo tz = null;
                string expected = "<time datetime=\"2013-01-29T13:28:21.0010000Z\" title=\"29 January 2013\" class=\"past not-today display-date\">29 January 2013</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayDate(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsUtc_ArgIsLocal_ResultIsUtc()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                var localTz = TimeZoneInfo.Local;
                DateTime orig = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                DateTime source = orig.ToLocalTime();
                Debug.Assert(source.Kind == DateTimeKind.Local);
                TimeZoneInfo tz = null;
                string expected = "<time datetime=\"2013-01-29T" + orig.Hour + ":28:21.0010000Z\" title=\"29 January 2013\" class=\"past not-today display-date\">29 January 2013</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayDate(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsUtc_ArgIsUser_ResultIsUtc()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                DateTime source = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Unspecified);
                Debug.Assert(source.Kind == DateTimeKind.Unspecified);
                TimeZoneInfo tz = null;
                string expected = "<time datetime=\"2013-01-29T13:28:21.0010000Z\" title=\"29 January 2013\" class=\"past not-today display-date\">29 January 2013</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayDate(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsRomance_ArgIsUtc_ResultIsRomance()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime source = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                DateTime romance = tz.ConvertFromUtc(source);
                string expected = "<time datetime=\"2013-01-29T" + source.Hour + ":28:21.0010000Z\" title=\"29 January 2013\" class=\"past not-today display-date\">29 January 2013</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayDate(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsRomance_ArgIsLocal_ResultIsRomance()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                var localTz = TimeZoneInfo.Local;
                DateTime orig = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                DateTime source = orig.ToLocalTime();
                DateTime romance = tz.ConvertFromUtc(source);
                Debug.Assert(source.Kind == DateTimeKind.Local);
                string expected = "<time datetime=\"2013-01-29T" + orig.Hour + ":28:21.0010000Z\" title=\"29 January 2013\" class=\"past not-today display-date\">29 January 2013</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayDate(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsRomance_ArgIsUser_ResultIsRomance()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime source = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Unspecified);
                DateTime utc = tz.ConvertToUtc(source);
                Debug.Assert(source.Kind == DateTimeKind.Unspecified);
                string expected = "<time datetime=\"2013-01-29T" + utc.Hour + ":28:21.0010000Z\" title=\"29 January 2013\" class=\"past not-today display-date\">29 January 2013</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayDate(html, source);

                Assert.AreEqual(expected, result.ToString());
            }
        }

        [TestClass]
        public class DisplayDateTimeMethod
        {
            [TestMethod]
            public void UserIsUtc_ArgIsUtc_ResultIsUtc()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                DateTime source = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                TimeZoneInfo tz = null;
                string expected = "<time datetime=\"2013-01-29T13:28:21.0010000Z\" title=\"29/01/2013 13:28:21\" class=\"past not-today display-datetime\">29 January 2013 13:28:21</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayDateTime(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsUtc_ArgIsLocal_ResultIsUtc()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                var localTz = TimeZoneInfo.Local;
                DateTime orig = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                DateTime source = orig.ToLocalTime();
                Debug.Assert(source.Kind == DateTimeKind.Local);
                TimeZoneInfo tz = null;
                string expected = "<time datetime=\"2013-01-29T" + orig.Hour + ":28:21.0010000Z\" title=\"29/01/2013 13:28:21\" class=\"past not-today display-datetime\">29 January 2013 13:28:21</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayDateTime(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsUtc_ArgIsUser_ResultIsUtc()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                DateTime source = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Unspecified);
                Debug.Assert(source.Kind == DateTimeKind.Unspecified);
                TimeZoneInfo tz = null;
                string expected = "<time datetime=\"2013-01-29T13:28:21.0010000Z\" title=\"29/01/2013 13:28:21\" class=\"past not-today display-datetime\">29 January 2013 13:28:21</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayDateTime(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsRomance_ArgIsUtc_ResultIsRomance()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime source = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                DateTime romance = tz.ConvertFromUtc(source);
                string expected = "<time datetime=\"2013-01-29T13:28:21.0010000Z\" title=\"29/01/2013 14:28:21\" class=\"past not-today display-datetime\">29 January 2013 14:28:21</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayDateTime(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsRomance_ArgIsLocal_ResultIsRomance()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                var localTz = TimeZoneInfo.Local;
                DateTime orig = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                DateTime source = orig.ToLocalTime();
                DateTime romance = tz.ConvertFromUtc(source);
                Debug.Assert(source.Kind == DateTimeKind.Local);
                string expected = "<time datetime=\"2013-01-29T" + orig.Hour + ":28:21.0010000Z\" title=\"29/01/2013 14:28:21\" class=\"past not-today display-datetime\">29 January 2013 14:28:21</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayDateTime(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsRomance_ArgIsUser_ResultIsRomance()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime source = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Unspecified);
                DateTime utc = tz.ConvertToUtc(source);
                Debug.Assert(source.Kind == DateTimeKind.Unspecified);
                string expected = "<time datetime=\"2013-01-29T12:28:21.0010000Z\" title=\"29/01/2013 13:28:21\" class=\"past not-today display-datetime\">29 January 2013 13:28:21</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayDateTime(html, source);

                Assert.AreEqual(expected, result.ToString());
            }
        }

        [TestClass]
        public class DisplayTimeMethod
        {
            [TestMethod]
            public void UserIsUtc_ArgIsUtc_ResultIsUtc()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                DateTime source = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                TimeZoneInfo tz = null;
                string expected = "<time datetime=\"2013-01-29T13:28:21.0010000Z\" title=\"29/01/2013 13:28:21\" class=\"past not-today display-time\">13:28:21</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayTime(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsUtc_ArgIsLocal_ResultIsUtc()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                var localTz = TimeZoneInfo.Local;
                DateTime orig = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                DateTime source = orig.ToLocalTime();
                Debug.Assert(source.Kind == DateTimeKind.Local);
                TimeZoneInfo tz = null;
                string expected = "<time datetime=\"2013-01-29T" + orig.Hour + ":28:21.0010000Z\" title=\"29/01/2013 13:28:21\" class=\"past not-today display-time\">13:28:21</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayTime(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsUtc_ArgIsUser_ResultIsUtc()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                DateTime source = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Unspecified);
                Debug.Assert(source.Kind == DateTimeKind.Unspecified);
                TimeZoneInfo tz = null;
                string expected = "<time datetime=\"2013-01-29T13:28:21.0010000Z\" title=\"29/01/2013 13:28:21\" class=\"past not-today display-time\">13:28:21</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayTime(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsRomance_ArgIsUtc_ResultIsRomance()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime source = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                DateTime romance = tz.ConvertFromUtc(source);
                string expected = "<time datetime=\"2013-01-29T13:28:21.0010000Z\" title=\"29/01/2013 14:28:21\" class=\"past not-today display-time\">14:28:21</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayTime(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsRomance_ArgIsLocal_ResultIsRomance()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                var localTz = TimeZoneInfo.Local;
                DateTime orig = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Utc);
                DateTime source = orig.ToLocalTime();
                DateTime romance = tz.ConvertFromUtc(source);
                Debug.Assert(source.Kind == DateTimeKind.Local);
                string expected = "<time datetime=\"2013-01-29T" + orig.Hour + ":28:21.0010000Z\" title=\"29/01/2013 14:28:21\" class=\"past not-today display-time\">14:28:21</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayTime(html, source);

                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void UserIsRomance_ArgIsUser_ResultIsRomance()
            {
                Thread.CurrentThread.CurrentCulture = TestCulture1;
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime source = new DateTime(2013, 1, 29, 13, 28, 21, 1, DateTimeKind.Unspecified);
                DateTime utc = tz.ConvertToUtc(source);
                Debug.Assert(source.Kind == DateTimeKind.Unspecified);
                string expected = "<time datetime=\"2013-01-29T12:28:21.0010000Z\" title=\"29/01/2013 13:28:21\" class=\"past not-today display-time\">13:28:21</time>";
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);

                var result = SrkHtmlExtensions.DisplayTime(html, source);

                Assert.AreEqual(expected, result.ToString());
            }
        }

        [TestClass]
        public class JsDateMethod
        {
            private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            [TestMethod]
            public void BackAndForth_UtcIn()
            {
                long epoch = 13912786171000L; // GMT: Sat, 01 Feb 2014 18:16:57 GMT
                DateTime utc = UnixEpoch.AddMilliseconds(epoch);
                var result = SrkHtmlExtensions.JsDate(null, utc);
                var expected = "new Date(13912786171000)";
                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void BackAndForth_UserTzIn()
            {
                long epoch = 13912786171000L; // GMT: Sat, 01 Feb 2014 18:16:57 GMT
                DateTime utc = UnixEpoch.AddMilliseconds(epoch);
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime user = tz.ConvertFromUtc(utc);
                var html = CreateHtmlHelper(new ViewDataDictionary());
                html.SetTimezone(tz);
                var result = SrkHtmlExtensions.JsDate(html, user);
                var expected = "new Date(13912786171000)";
                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void BackAndForth_LocalTzIn()
            {
                long epoch = 13912786171000L; // GMT: Sat, 01 Feb 2014 18:16:57 GMT
                DateTime utc = UnixEpoch.AddMilliseconds(epoch);
                DateTime local = utc.ToLocalTime();
                var html = CreateHtmlHelper(new ViewDataDictionary());
                var result = SrkHtmlExtensions.JsDate(html, local);
                var expected = "new Date(13912786171000)";
                Assert.AreEqual(expected, result.ToString());
            }
        }

        [TestClass]
        public class DisplayTextMethod
        {
            [TestMethod]
            public void BugWithSingleQuoteAndTwotter()
            {
                var input = "J'ai ajouté une";
                var expected = "J&#x27;ai ajouté une";
                var result = SrkHtmlExtensions.DisplayText(null, input, twitterLinks: true, makeParagraphs: false);
                SrkToolkit.Testing.Assert.AreEqual(expected, result.ToString());
            }
        }

        [TestClass]
        public class HasOtherValidationErrorsMethod
        {
            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsWhenArg0IsNull()
            {
                SrkHtmlExtensions.HasOtherValidationErrors(null);
            }

            [TestMethod]
            public void ModelIsValid_ReturnsFalse()
            {
                var model = new TestModel { Name = "hello", };
                var html = GetHtmlHelper(model);

                var result = SrkHtmlExtensions.HasOtherValidationErrors(html);

                Assert.IsFalse(result);
            }

            [TestMethod]
            public void ModelIsPropertyInvalid_ReturnsFalse()
            {
                var model = new TestModel { Name = null, };
                var html = GetHtmlHelper(model);

                var result = SrkHtmlExtensions.HasOtherValidationErrors(html);

                Assert.IsFalse(result);
            }

            [TestMethod]
            public void ModelIsOtherInvalid_ReturnsFalse()
            {
                var model = new TestModel { Name = "hello", };
                var html = GetHtmlHelper(model);
                html.ViewData.ModelState.AddModelError(string.Empty, "Other error");

                var result = SrkHtmlExtensions.HasOtherValidationErrors(html);

                Assert.IsTrue(result);
            }
        }

        [TestClass]
        public class ValidationSummaryExMethod
        {
            [TestMethod]
            public void ModelIsOtherInvalid_Shows()
            {
                var model = new TestModel { Name = "hello", };
                var html = GetHtmlHelper(model);
                html.ViewData.ModelState.AddModelError(string.Empty, "Other error");
                var response = SrkHtmlExtensions.ValidationSummaryEx(html);
                var isDisplayed = response != null && response.ToString().Length > 0;
                Assert.IsTrue(isDisplayed);
            }

            [TestMethod]
            public void ModelIsPropertyInvalid_Hides()
            {
                var model = new TestModel { Name = "hello", };
                var html = GetHtmlHelper(model);
                var response = SrkHtmlExtensions.ValidationSummaryEx(html);
                var isDisplayed = response != null && response.ToString().Length > 0;
                Assert.IsFalse(isDisplayed);
            }

            [TestMethod]
            public void ModelIsValid_Hides()
            {
                var model = new TestModel { Name = "hello", };
                var html = GetHtmlHelper(model);
                var response = SrkHtmlExtensions.ValidationSummaryEx(html);
                var isDisplayed = response != null && response.ToString().Length > 0;
                Assert.IsFalse(isDisplayed);
            }
        }

        [TestClass]
        public class DescriptionForMethod
        {
            [TestMethod]
            public void NoDescriptionReturnsEmptyString()
            {
                var model = new TestModel();
                var html = this.GetHtmlHeper<TestModel>(model);
                var result = SrkHtmlExtensions.DescriptionFor(html, m => m.Name);
                Assert.IsNull(result.ToString().NullIfEmpty());
            }

            [TestMethod]
            public void EmptyDescriptionReturnsEmptyString()
            {
                var model = new TestModel();
                var html = this.GetHtmlHeper<TestModel>(model);
                var result = SrkHtmlExtensions.DescriptionFor(html, m => m.Description);
                Assert.IsNull(result.ToString().NullIfEmpty());
            }

            [TestMethod]
            public void ValidDescriptionReturnsHtmlAndValue()
            {
                var model = new TestModel();
                var html = this.GetHtmlHeper<TestModel>(model);
                var result = SrkHtmlExtensions.DescriptionFor(html, m => m.Description2);
                Assert.AreEqual("<span data-for=\"Description2\">Desc a2a</span>", result.ToString().NullIfEmpty());
            }

            [TestMethod]
            public void ForIsValidAtLevel1()
            {
                var model = new TestModel();
                var html = this.GetHtmlHeper<TestModel>(model);
                var result = SrkHtmlExtensions.DescriptionFor(html, m => m.Description2);
                Assert.AreEqual("<span data-for=\"Description2\">Desc a2a</span>", result.ToString().NullIfEmpty());
            }

            [TestMethod]
            public void ForIsValidAtLevel2()
            {
                var model = new TestModel
                {
                    SubModel = new TestModel(),
                };
                var html = this.GetHtmlHeper<TestModel>(model);
                var result = SrkHtmlExtensions.DescriptionFor(html, m => m.SubModel.Description2);
                Assert.AreEqual("<span data-for=\"SubModel_Description2\">Desc a2a</span>", result.ToString().NullIfEmpty());
            }

            [TestMethod]
            public void AcceptsHtmlAttributes()
            {
                var model = new TestModel();
                var html = this.GetHtmlHeper<TestModel>(model);
                var result = SrkHtmlExtensions.DescriptionFor(html, m => m.Description2, new { attr1 = "value", });
                Assert.AreEqual("<span attr1=\"value\" data-for=\"Description2\">Desc a2a</span>", result.ToString().NullIfEmpty());
            }

            private HtmlHelper<TModel> GetHtmlHeper<TModel>(TModel model)
            {
                var controllerContext = new ControllerContext();
                var view = new Mock<IView>();
                var viewData = new ViewDataDictionary();
                viewData.Model = model;
                var tempData = new TempDataDictionary();
                var writer = new StreamWriter(new MemoryStream());
                var viewContext = new ViewContext(controllerContext, view.Object, viewData, tempData, writer);
                var viewDataContainer = new ViewPage();
                var html = new HtmlHelper<TModel>(viewContext, viewDataContainer);
                return html;
            }
        }

        [TestClass]
        public class CallLinkMethod
        {
            [TestMethod]
            public void InternationalFrenchStandardFormat()
            {
                string phone = "+33 123456789";
                string expected = @"<a class=""tel"" href=""tel:" + phone + @""">" + phone + "</a>";
                var result = SrkHtmlExtensions.CallLink(null, phone);
                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void NationalFrenchStandardFormat()
            {
                string phone = "0123456789";
                string expected = @"<a class=""tel"" href=""tel:" + phone + @""">" + phone + "</a>";
                var result = SrkHtmlExtensions.CallLink(null, phone);
                Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void NationalUsaStandardFormat()
            {
                string phone = "123-456-7890";
                string expected = @"<a class=""tel"" href=""tel:" + phone + @""">" + phone + "</a>";
                var result = SrkHtmlExtensions.CallLink(null, phone);
                Assert.AreEqual(expected, result.ToString());
            }
        }

        private static HtmlHelper GetHtmlHelper(TestModel model)
        {
            var http = new BasicHttpContext();
            var viewData = new ViewDataDictionary(model);
            var controllerContext = new ControllerContext();
            controllerContext.HttpContext = http;
            var viewDataContainer = new ViewPage();
            var view = new Mock<IView>();
            var tempData = new TempDataDictionary();
            var writer = new StreamWriter(new MemoryStream());
            var viewContext = new ViewContext(controllerContext, view.Object, viewData, tempData, writer);
            viewContext.HttpContext = http;
            var html = new HtmlHelper(viewContext, viewDataContainer);
            html.ViewContext.HttpContext = http;
            var modelState = html.ViewData.ModelState;
            return html;
        }

        public class TestModel
        {
            [Required]
            public string Name { get; set; }

            [Display(Name = "Desc aa")]
            public string Description { get; set; }

            [Display(Description = "Desc a2a")]
            public string Description2 { get; set; }

            public TestModel SubModel { get; set; }
        }
    }
}
