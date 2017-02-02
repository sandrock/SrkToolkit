// 
// Copyright 2014 SandRock
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

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

            [TestMethod]
            public void JsWithAttributesWorks()
            {
                string expected = "<script data-main=\"Scripts/main\" src=\"//js.js\" type=\"text/javascript\"></script>\r\n";
                var item = new WebDependency("test");
                var attributes = new Dictionary<string, object>()
                {
                    { "data-main", "Scripts/main" },
                };
                item.Add(new WebDependencyFile("//js.js", WebDependencyFileType.Javascript, attributes: attributes));
                var result = new WebDependencies().Render(item);
                SrkToolkit.Testing.Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void JsWith1AttributeAsStringParamsWorks()
            {
                string expected = "<script data-main=\"Scripts/main\" src=\"//js.js\" type=\"text/javascript\"></script>\r\n";
                var item = new WebDependency("test");
                var attributes = new Dictionary<string, object>()
                {
                    { "data-main", "Scripts/main" },
                };
                var dep = new WebDependencyFile("//js.js", WebDependencyFileType.Javascript, "data-main", "Scripts/main");
                item.Add(dep);
                var result = new WebDependencies().Render(item);
                SrkToolkit.Testing.Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void JsWith2AttributeAsStringParamsWorks()
            {
                string expected = "<script data-hello=\"world\" data-main=\"Scripts/main\" src=\"//js.js\" type=\"text/javascript\"></script>\r\n";
                var item = new WebDependency("test");
                var attributes = new Dictionary<string, object>()
                {
                    { "data-main", "Scripts/main" },
                };
                var dep = new WebDependencyFile("//js.js", WebDependencyFileType.Javascript, "data-main", "Scripts/main", "data-hello", "world");
                item.Add(dep);
                var result = new WebDependencies().Render(item);
                SrkToolkit.Testing.Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void AppliesOrder_AllZero_Order1()
            {
                string expected = "<!-- WebDependencies/Default - start -->\r\n"
                    + "<link href=\"//css1.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n"
                    + "<link href=\"//css2.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n"
                    + "<!-- WebDependencies/Default - end -->\r\n";
                var item1 = new WebDependency("test1");
                item1.Order = 0;
                item1.Add(new WebDependencyFile("//css1.css", WebDependencyFileType.Css));
                var item2 = new WebDependency("test2");
                item2.Order = 0;
                item2.Add(new WebDependencyFile("//css2.css", WebDependencyFileType.Css));

                var target = new WebDependencies();
                target.Include(item1);
                target.Include(item2);

                var result = target.RenderIncludes(WebDependencyPosition.Default);
                SrkToolkit.Testing.Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void AppliesOrder_AllZero_Order2()
            {
                string expected = "<!-- WebDependencies/Default - start -->\r\n"
                    + "<link href=\"//css2.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n"
                    + "<link href=\"//css1.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n"
                    + "<!-- WebDependencies/Default - end -->\r\n";
                var item1 = new WebDependency("test1");
                item1.Order = 0;
                item1.Add(new WebDependencyFile("//css1.css", WebDependencyFileType.Css));
                var item2 = new WebDependency("test2");
                item2.Order = 0;
                item2.Add(new WebDependencyFile("//css2.css", WebDependencyFileType.Css));

                var target = new WebDependencies();
                target.Include(item2);
                target.Include(item1);

                var result = target.RenderIncludes(WebDependencyPosition.Default);
                SrkToolkit.Testing.Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void AppliesOrder_Ordered_Order1()
            {
                string expected = "<!-- WebDependencies/Default - start -->\r\n"
                    + "<link href=\"//css1.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n"
                    + "<link href=\"//css2.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n"
                    + "<!-- WebDependencies/Default - end -->\r\n";
                var item1 = new WebDependency("test1");
                item1.Order = 1;
                item1.Add(new WebDependencyFile("//css1.css", WebDependencyFileType.Css));
                var item2 = new WebDependency("test2");
                item2.Order = 2;
                item2.Add(new WebDependencyFile("//css2.css", WebDependencyFileType.Css));

                var target = new WebDependencies();
                target.Include(item1);
                target.Include(item2);

                var result = target.RenderIncludes(WebDependencyPosition.Default);
                SrkToolkit.Testing.Assert.AreEqual(expected, result.ToString());
            }

            [TestMethod]
            public void AppliesOrder_Ordered_Order2()
            {
                string expected = "<!-- WebDependencies/Default - start -->\r\n"
                    + "<link href=\"//css1.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n"
                    + "<link href=\"//css2.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n"
                    + "<!-- WebDependencies/Default - end -->\r\n";
                var item1 = new WebDependency("test1");
                item1.Order = 1;
                item1.Add(new WebDependencyFile("//css1.css", WebDependencyFileType.Css));
                var item2 = new WebDependency("test2");
                item2.Order = 2;
                item2.Add(new WebDependencyFile("//css2.css", WebDependencyFileType.Css));

                var target = new WebDependencies();
                target.Include(item2);
                target.Include(item1);

                var result = target.RenderIncludes(WebDependencyPosition.Default);
                SrkToolkit.Testing.Assert.AreEqual(expected, result.ToString());
            }
        }
    }
}
