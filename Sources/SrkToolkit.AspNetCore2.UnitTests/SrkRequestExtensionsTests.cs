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
    using System.Web;
    using SrkToolkit.Web.Fakes;

    public class SrkRequestExtensionsTests
    {
        [TestClass]
        public class IsUrlLocalToHostMethod
        {
            [TestMethod]
            public void LocalNoSlash_IsNotLocal()
            {
                HttpRequestBase request = null;
                string url = "controller/action/id";
                Assert.IsFalse(SrkRequestExtensions.IsUrlLocalToHost(request, url));
            }

            [TestMethod]
            public void LocalSlash_IsLocal()
            {
                HttpRequestBase request = null;
                string url = "/controller/action/id";
                Assert.IsTrue(SrkRequestExtensions.IsUrlLocalToHost(request, url));
            }
        
            [TestMethod]
            public void WithProtocal_IsNotLocal()
            {
                HttpRequestBase request = null;
                string url = "http://test.com/controller/action/id";
                Assert.IsFalse(SrkRequestExtensions.IsUrlLocalToHost(request, url));
            }
        }

        [TestClass]
        public class PrefersJsonMethod
        {
            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ArgNullThrows()
            {
                BasicHttpRequest request = null;
                SrkRequestExtensions.PrefersJson(request);
            }

            [TestMethod]
            public void NullAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = null;
                var request = new BasicHttpRequest();
                request.AcceptTypesCollection = acceptTypes;
                Assert.IsFalse(SrkRequestExtensions.PrefersJson(request));
            }

            [TestMethod]
            public void EmptyAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[0];
                var request = new BasicHttpRequest();
                request.AcceptTypesCollection = acceptTypes;
                Assert.IsFalse(SrkRequestExtensions.PrefersJson(request));
            }

            [TestMethod]
            public void HtmlAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[] { "text/html", "application/xhtml+xml", "application/xml", "*/*", };
                var request = new BasicHttpRequest();
                request.AcceptTypesCollection = acceptTypes;
                Assert.IsFalse(SrkRequestExtensions.PrefersJson(request));
            }

            [TestMethod]
            public void JsonXmlAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[] { "application/json", "application/xml", };
                var request = new BasicHttpRequest();
                request.AcceptTypesCollection = acceptTypes;
                Assert.IsTrue(SrkRequestExtensions.PrefersJson(request));
            }

            [TestMethod]
            public void XmlJsonAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[] { "application/xml", "application/json", };
                var request = new BasicHttpRequest();
                request.AcceptTypesCollection = acceptTypes;
                Assert.IsFalse(SrkRequestExtensions.PrefersJson(request));
            }

            [TestMethod]
            public void TextJsonAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[] { "text/json", "application/xml", };
                var request = new BasicHttpRequest();
                request.AcceptTypesCollection = acceptTypes;
                Assert.IsTrue(SrkRequestExtensions.PrefersJson(request));
            }
        }
    }
}
