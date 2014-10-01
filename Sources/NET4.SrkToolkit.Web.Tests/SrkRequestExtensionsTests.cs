
namespace SrkToolkit.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Web;
    using SrkToolkit.Web.Fakes;

    public class SrkRequestExtensionsTests
    {
        [TestClass]
        public class IsUrlLocalToHostMethod
        {
            [TestMethod]
            public void LocalNoSlash_IsNotLocal()
            {
                HttpRequestBase request = null;
                string url = "controller/action/id";
                Assert.IsFalse(SrkRequestExtensions.IsUrlLocalToHost(request, url));
            }

            [TestMethod]
            public void LocalSlash_IsLocal()
            {
                HttpRequestBase request = null;
                string url = "/controller/action/id";
                Assert.IsTrue(SrkRequestExtensions.IsUrlLocalToHost(request, url));
            }
        
            [TestMethod]
            public void WithProtocal_IsNotLocal()
            {
                HttpRequestBase request = null;
                string url = "http://test.com/controller/action/id";
                Assert.IsFalse(SrkRequestExtensions.IsUrlLocalToHost(request, url));
            }
        }

        [TestClass]
        public class PrefersJsonMethod
        {
            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ArgNullThrows()
            {
                BasicHttpRequest request = null;
                SrkRequestExtensions.PrefersJson(request);
            }

            [TestMethod]
            public void NullAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = null;
                var request = new BasicHttpRequest();
                request.AcceptTypesCollection = acceptTypes;
                Assert.IsFalse(SrkRequestExtensions.PrefersJson(request));
            }

            [TestMethod]
            public void EmptyAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[0];
                var request = new BasicHttpRequest();
                request.AcceptTypesCollection = acceptTypes;
                Assert.IsFalse(SrkRequestExtensions.PrefersJson(request));
            }

            [TestMethod]
            public void HtmlAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[] { "text/html", "application/xhtml+xml", "application/xml", "*/*", };
                var request = new BasicHttpRequest();
                request.AcceptTypesCollection = acceptTypes;
                Assert.IsFalse(SrkRequestExtensions.PrefersJson(request));
            }

            [TestMethod]
            public void JsonXmlAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[] { "application/json", "application/xml", };
                var request = new BasicHttpRequest();
                request.AcceptTypesCollection = acceptTypes;
                Assert.IsTrue(SrkRequestExtensions.PrefersJson(request));
            }

            [TestMethod]
            public void XmlJsonAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[] { "application/xml", "application/json", };
                var request = new BasicHttpRequest();
                request.AcceptTypesCollection = acceptTypes;
                Assert.IsFalse(SrkRequestExtensions.PrefersJson(request));
            }

            [TestMethod]
            public void TextJsonAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[] { "text/json", "application/xml", };
                var request = new BasicHttpRequest();
                request.AcceptTypesCollection = acceptTypes;
                Assert.IsTrue(SrkRequestExtensions.PrefersJson(request));
            }
        }
    }
}
