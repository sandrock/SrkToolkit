
namespace SrkToolkit.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Web;

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
    }
}
