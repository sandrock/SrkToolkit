// -----------------------------------------------------------------------
// <copyright file="OpenGraphTagTEsts.cs" company="">
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
    public class OpenGraphTagTests
    {
        [TestMethod]
        public void ToStringFormatsHtmlTag()
        {
            var tag = new OpenGraphTag(new OpenGraphName(new OpenGraphNamespace("ns", "http://ns/"), "key"), "value");
            var expected = "<meta property=\"ns:key\" content=\"value\" />";
            Assert.AreEqual(expected, tag.ToString());
        }

        [TestMethod]
        public void ToStringFormatsEscapedHtmlTag()
        {
            var tag = new OpenGraphTag(new OpenGraphName(new OpenGraphNamespace("<ns>", "http://ns/"), "<key>"), "<val\"ue>");
            var expected = "<meta property=\"&lt;ns&gt;:&lt;key&gt;\" content=\"&lt;val&quot;ue&gt;\" />";
            Assert.AreEqual(expected, tag.ToString());
        }
    }
}
