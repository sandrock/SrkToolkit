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
            var expected = " xmlns:test=\"http://myns.me/test\" ";
            Assert.AreEqual(expected, ns.ToHtmlAttributeString());
        }

        [TestMethod]
        public void DefaultRendersHtmlAttribute()
        {
            var name = new OpenGraphName("test");
            var expected = " xmlns:og=\"http://ogp.me/ns#\" ";
            Assert.AreEqual(expected, name.Namespace.ToHtmlAttributeString());
        }
    }
}
