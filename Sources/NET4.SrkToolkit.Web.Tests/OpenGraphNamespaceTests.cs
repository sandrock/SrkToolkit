// -----------------------------------------------------------------------
// <copyright file="OpenGraphNamespaceTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.Web.Open;

    [TestClass]
    public class OpenGraphNamespaceTests
    {
        [TestMethod]
        public void HasDefinedName()
        {
            var nsName  = "test";
            var ns = new OpenGraphNamespace(nsName, "http://nsname/");
            Assert.AreEqual(nsName, ns.Name);
        }

        ////[TestMethod]
        ////public void ImplicitOperatorHasDefinedName()
        ////{
        ////    var nsName = "test";
        ////    OpenGraphNamespace ns = nsName;
        ////    Assert.AreEqual(nsName, ns.Name);
        ////}

        [TestMethod]
        public void CustomRendersHtmlAttribute()
        {
            var nsName  = "test";
            var ns = new OpenGraphNamespace(nsName, "http://myns.me/test");
            var expected = " xmlns:test=\"http:&#x2F;&#x2F;myns.me&#x2F;test\" ";
            Assert.AreEqual(expected, ns.ToHtmlAttributeString());
        }

        [TestMethod]
        public void DefaultRendersHtmlAttribute()
        {
            var name = new OpenGraphName("test");
            var expected = " xmlns:og=\"http:&#x2F;&#x2F;ogp.me&#x2F;ns#\" ";
            Assert.AreEqual(expected, name.Namespace.ToHtmlAttributeString());
        }
    }
}
