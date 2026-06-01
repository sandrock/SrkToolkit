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
    using SrkToolkit.Web.Open;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class OpenGraphNameTests
    {
        [Fact]
        public void HasDefinedNameAndNamespace()
        {
            var name = "toto";
            var nsName = "super";
            var ns = new OpenGraphNamespace(nsName, "a");
            var obj = new OpenGraphName(ns, name);
            Assert.Equal(nsName, obj.NamespaceName);
            Assert.Equal(name, obj.Name);
        }

        ////[Fact]
        ////public void StringImplicitOperatorGivesOgNamespaceName()
        ////{
        ////    var name = "toto";
        ////    OpenGraphName obj = name;
        ////    Assert.Equal("og", obj.NamespaceName);
        ////    Assert.Equal(name, obj.Name);
        ////}

        [Fact]
        public void NameCtorGivesOgNamespaceName()
        {
            var name = "toto";
            OpenGraphName obj = new OpenGraphName(name);
            Assert.Equal("og", obj.NamespaceName);
            Assert.Equal(name, obj.Name);
        }
    }
}
