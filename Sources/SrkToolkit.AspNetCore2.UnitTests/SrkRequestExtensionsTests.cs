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
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using SrkToolkit.Web.Fakes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class SrkHttpRequestExtensionsTests
    {
        public class IsUrlLocalToHostMethod
        {
            [Fact]
            public void LocalNoSlash_IsNotLocal()
            {
                HttpRequest request = null;
                string url = "controller/action/id";
                Assert.False(request.IsUrlLocalToHost(url));
            }

            [Fact]
            public void LocalSlash_IsLocal()
            {
                HttpRequest request = null;
                string url = "/controller/action/id";
                Assert.True(request.IsUrlLocalToHost(url));
            }
        
            [Fact]
            public void WithProtocal_IsNotLocal()
            {
                HttpRequest request = null;
                string url = "http://test.com/controller/action/id";
                Assert.False(request.IsUrlLocalToHost(url));
            }
        }

        public class PrefersJsonMethod
        {
            private readonly AspNetCoreTestContext context;

            public PrefersJsonMethod()
            {
                this.context = new AspNetCoreTestContext();
            }

            [Fact]
            public void ArgNullThrows()
            {
                HttpRequest request = null;
                Assert.Throws<ArgumentNullException>(() =>
                {
                    request.PrefersJson();
                });
            }

            [Fact]
            public void NullAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = null;
                var request = new DefaultHttpRequest(this.context.Context);
                ////request.Headers["Accept"] = null;
                ////request.AcceptTypesCollection = acceptTypes;
                Assert.False(request.PrefersJson());
            }

            [Fact]
            public void EmptyAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[0];
                var request = new DefaultHttpRequest(this.context.Context);
                request.Headers["Accept"] = acceptTypes;
                Assert.False(request.PrefersJson());
            }

            [Fact]
            public void HtmlAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[] { "text/html", "application/xhtml+xml", "application/xml", "*/*", };
                
                var request = new DefaultHttpRequest(this.context.Context);
                request.Headers["Accept"] = acceptTypes;
                Assert.False(request.PrefersJson());
            }

            [Fact]
            public void JsonXmlAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[] { "application/json", "application/xml", };
                var request = new DefaultHttpRequest(this.context.Context);
                request.Headers["Accept"] = acceptTypes;
                Assert.True(request.PrefersJson());
            }

            [Fact]
            public void XmlJsonAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[] { "application/xml", "application/json", };
                var request = new DefaultHttpRequest(this.context.Context);
                request.Headers["Accept"] = acceptTypes;
                Assert.False(request.PrefersJson());
            }

            [Fact]
            public void TextJsonAcceptTypesReturnsFalse()
            {
                string[] acceptTypes = new string[] { "text/json", "application/xml", };
                var request = new DefaultHttpRequest(this.context.Context);
                request.Headers["Accept"] = acceptTypes;
                Assert.True(request.PrefersJson());
            }
        }
    }
}
