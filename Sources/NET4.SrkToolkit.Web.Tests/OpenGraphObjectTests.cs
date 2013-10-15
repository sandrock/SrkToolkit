// -----------------------------------------------------------------------
// <copyright file="OpenGraphObjectTests.cs" company="">
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
    using System.Web;

    [TestClass]
    public class OpenGraphObjectTests
    {
        [TestMethod]
        public void EmptyObjectRendersNothing()
        {
            var obj = new OpenGraphObject();
            Assert.AreEqual(string.Empty, obj.ToString());
        }

        [TestMethod]
        public void BasicObjectRendersTags()
        {
            var obj = new OpenGraphObject("my super title", new Uri("http://test.org/my-super-title"));
            var expected = Meta("og:title", "my super title") + Meta("og:url", "http://test.org/my-super-title");
            Assert.AreEqual(expected, obj.ToString());
        }

        [TestMethod]
        public void BasicObjectRendersNamespaces()
        {
            var obj = new OpenGraphObject("my super title", new Uri("http://test.org/my-super-title"));
            var expected = " xmlns:og=\"http://ogp.me/ns#\" ";
            Assert.AreEqual(expected, obj.ToHtmlAttributeNamespaces());
        }

        [TestMethod]
        public void RendersMultipleNamespaces()
        {
            var obj = new OpenGraphObject("my super title", new Uri("http://test.org/my-super-title"));
            obj.Add(new OpenGraphTag(new OpenGraphName("key", "xxx", "http://xxx/aaa/#"), "value"));
            var expected = " xmlns:og=\"http://ogp.me/ns#\"  xmlns:xxx=\"http://xxx/aaa/#\" ";
            Assert.AreEqual(expected, obj.ToHtmlAttributeNamespaces());
        }

        [TestMethod]
        public void TypicalHomepageWithLogo()
        {
            var obj = new OpenGraphObject("welcome to mysite", new Uri("http://mysite.com/"));
            obj.IsImage(imageUri: new Uri("http://mysite.com/Content/loog.png"));
            obj.SetDescription("With mysite, build your blog in 2 minutes");
            var expected = Meta("og:title", "welcome to mysite")
                + Meta("og:url", "http://mysite.com/")
                + Meta("og:image", "http://mysite.com/Content/loog.png")
                + Meta("og:description", "With mysite, build your blog in 2 minutes");
            Assert.AreEqual(expected, obj.ToString());
        }

        private string Meta(string key, string value)
        {
            return "<meta property=\"" + HttpUtility.HtmlAttributeEncode(key) + "\" content=\"" + HttpUtility.HtmlAttributeEncode(value) + "\" />";
        }
    }
}
