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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.Web.Open;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;

    [TestClass]
    public class PageInfoTests
    {
        [TestMethod]
        public void BasicUsage()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            var title = "super page title";
            var description = "page description";
            var keywords = "hello, keyworlds";
            var page = new PageInfo()
                .Set(PageInfo.Title, title)
                .Set(PageInfo.Description, description)
                .Set(PageInfo.Language)
                .Set(PageInfo.Keywords, keywords)
                .Set(new PageInfoItem("analytics1", "AE4F67862-B1").Add(new PageInfoObject().MetaWithValue("analytics1")))
            ;

            ////var sb = new StringBuilder();
            ////page.Write(sb);
            ////var result = sb.ToString();
            var result = page.ToString();

            SrkToolkit.Testing.Assert.Contains("<title>" + title + "</title>", result);
            SrkToolkit.Testing.Assert.Contains("<meta content=\"" + description + "\" name=\"description\" />", result);
            SrkToolkit.Testing.Assert.Contains("<meta content=\"" + Thread.CurrentThread.CurrentCulture.Name + "\" name=\"language\" />", result);
            SrkToolkit.Testing.Assert.Contains("<meta content=\"" + keywords + "\" name=\"keywords\" />", result);
            SrkToolkit.Testing.Assert.Contains("<meta content=\"AE4F67862-B1\" name=\"analytics1\" />", result);
        }

        [TestMethod]
        public void OpenGraphTitle()
        {
            var title = "super page title";
            var page = new PageInfo().Set(PageInfo.Title, title);
            var result = page.ToString(PageInfoObjectSection.Basic | PageInfoObjectSection.OpenGraph, false);
            SrkToolkit.Testing.Assert.AreEqual("<title>" + title + "</title><meta property=\"og:title\" content=\"" + title.ProperHtmlAttributeEscape() + "\" />", result);
        }

        [TestMethod]
        public void OpenGraphDescription()
        {
            var title = "super page title";
            var page = new PageInfo().Set(PageInfo.Description, title);
            var result = page.ToString(PageInfoObjectSection.Basic | PageInfoObjectSection.OpenGraph, false);
            SrkToolkit.Testing.Assert.AreEqual("<meta content=\"" + title + "\" name=\"description\" /><meta property=\"og:description\" content=\"" + title.ProperHtmlAttributeEscape() + "\" />", result);
        }

        [TestMethod]
        public void AlternateLanguage()
        {
            var name = "fr-CA";
            var href = "/fr-CA/Home";
            var page = new PageInfo().Set(PageInfo.AlternateLanguage(name, href, "text/html"));
            var result = page.ToString(PageInfoObjectSection.Basic | PageInfoObjectSection.OpenGraph, false);
            SrkToolkit.Testing.Assert.AreEqual("<link href=\"" + href + "\" hrefLang=\"" + name + "\" rel=\"alternate\" type=\"text/html\" /><meta property=\"og:locale:alternate\" content=\"" + name.Replace("-", "_").ProperHtmlAttributeEscape() + "\" />", result);
        }

        [TestMethod]
        public void ContainsNot()
        {
            var page = new PageInfo();
            Assert.IsFalse(page.Contains("title"));
        }

        [TestMethod]
        public void Contains()
        {
            var page = new PageInfo();
            page.Set(PageInfo.Title, "hello");
            Assert.IsTrue(page.Contains("title"));
        }
    }
}
