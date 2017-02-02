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
