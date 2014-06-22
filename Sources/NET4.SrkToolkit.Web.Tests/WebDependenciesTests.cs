
namespace SrkToolkit.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WebDependenciesTests
    {
        [TestClass]
        public class RenderMethod
        {
            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ArgNull()
            {
                new WebDependencies().Render(null);
            }

            [TestMethod]
            public void JsWorks()
            {
                string expected = "<script src=\"//js.js\" type=\"text/javascript\"></script>\r\n";
                var item = new WebDependency("test");
                item.Add(new WebDependencyFile("//js.js", WebDependencyFileType.Javascript));
                var result = new WebDependencies().Render(item);
                SrkToolkit.Testing.Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void CssWorks()
            {
                string expected = "<link href=\"//css.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n";
                var item = new WebDependency("test");
                item.Add(new WebDependencyFile("//css.css", WebDependencyFileType.Css));
                var result = new WebDependencies().Render(item);
                SrkToolkit.Testing.Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void CssWithMediaWorks()
            {
                string expected = "<link href=\"//css.css\" media=\"all\" rel=\"stylesheet\" type=\"text/css\" />\r\n";
                var item = new WebDependency("test");
                item.Add(new WebDependencyFile("//css.css", WebDependencyFileType.Css)
                {
                    Media = WebDependencyMedia.All,
                });
                var result = new WebDependencies().Render(item);
                SrkToolkit.Testing.Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void CssWithMediasWorks()
            {
                string expected = "<link href=\"//css.css\" media=\"all, braille\" rel=\"stylesheet\" type=\"text/css\" />\r\n";
                var item = new WebDependency("test");
                item.Add(new WebDependencyFile("//css.css", WebDependencyFileType.Css)
                {
                    Media = WebDependencyMedia.All | WebDependencyMedia.Braille,
                });
                var result = new WebDependencies().Render(item);
                SrkToolkit.Testing.Assert.AreEqual(expected, result.ToString());
            }
        }
    }
}
