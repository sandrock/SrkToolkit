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
    public class OpenGraphNameTests
    {
        [TestMethod]
        public void HasDefinedNameAndNamespace()
        {
            var name = "toto";
            var nsName = "super";
            var ns = new OpenGraphNamespace(nsName, "a");
            var obj = new OpenGraphName(ns, name);
            Assert.AreEqual(nsName, obj.NamespaceName);
            Assert.AreEqual(name, obj.Name);
        }

        ////[TestMethod]
        ////public void StringImplicitOperatorGivesOgNamespaceName()
        ////{
        ////    var name = "toto";
        ////    OpenGraphName obj = name;
        ////    Assert.AreEqual("og", obj.NamespaceName);
        ////    Assert.AreEqual(name, obj.Name);
        ////}

        [TestMethod]
        public void NameCtorGivesOgNamespaceName()
        {
            var name = "toto";
            OpenGraphName obj = new OpenGraphName(name);
            Assert.AreEqual("og", obj.NamespaceName);
            Assert.AreEqual(name, obj.Name);
        }
    }
}
