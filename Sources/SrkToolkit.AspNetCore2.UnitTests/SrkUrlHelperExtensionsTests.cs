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
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Routing;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Xunit;

    public class SrkUrlHelperExtensionsTests
    {
        public class SetQueryStringMethod
        {
            [Fact]
            public void WithNoQuery()
            {
                // prepare
                string path = "/path";
                string input = path;
                string expected = "/path?k1=v1";
                var helper = GetHelper(path, "");

                // execute
                string actual = helper.SetQueryString(input, "k1", "v1");

                // verify
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void WithNoPath()
            {
                // prepare
                string path = "";
                string input = path;
                string expected = "?k1=v1";
                var helper = GetHelper(path, "");

                // execute
                string actual = helper.SetQueryString(input, "k1", "v1");

                // verify
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void WithQueryDelimiter()
            {
                // prepare
                string path = "/path";
                string query = "";
                string input = path + "?" + query;
                string expected = "/path?k1=v1";
                var helper = GetHelper(path, query);

                // execute
                string actual = helper.SetQueryString(input, "k1", "v1");

                // verify
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void WithOtherQueryKey()
            {
                // prepare
                string path = "/path";
                string query = "k2=v2";
                string input = path + "?" + query;
                string expected = "/path?k2=v2&k1=v1";
                var helper = GetHelper(path, query);

                // execute
                string actual = helper.SetQueryString(input, "k1", "v1");

                // verify
                Assert.Equal(expected, actual);
            }

            private static UrlHelper GetHelper(string path, string query)
            {
                var httpRequest = new HttpRequest("", "http://localhost/" + path, query);
                var writer = new StringWriter();
                var response = new HttpResponse(writer);
                var context = new HttpContext(httpRequest, response);
                var wrapped = new HttpContextWrapper(context);
                var helper = new UrlHelper(new RequestContext(wrapped, new RouteData()));
                return helper;
            }

            [Fact]
            public void WithSameQueryKey()
            {
                // prepare
                string path = "/path";
                string query = "k1=v0";
                string input = path + "?" + query;
                string expected = "/path?k1=v0&k1=v1";
                var helper = GetHelper(path, query);

                // execute
                string actual = helper.SetQueryString(input, "k1", "v1");

                // verify
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void MultipleValues()
            {
                // prepare
                string path = "/path";
                string input = path;
                string expected = "/path?k1=v111&k2=v2";
                var helper = GetHelper(path, "");

                // execute
                string actual = helper.SetQueryString(input, "k1", "v111", "k2", "v2");

                // verify
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void MultipleValuesOneSet()
            {
                // prepare
                string path = "/path?k1=v1";
                string input = path;
                string expected = "/path?k1=v1&k1=v111&k2=v2";
                var helper = GetHelper(path, "");

                // execute
                string actual = helper.SetQueryString(input, "k1", "v111", "k2", "v2");

                // verify
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void SpecialCharInOriginalQuery()
            {
                string path = @"/Area/Ctrl/Action";
                string query = @"search=hello%20world";
                string url = path + "?" + query;
                string expected = url + "&page=2";
                var helper = GetHelper(path, query);

                string actual = helper.SetQueryString(url, "page", "2");

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void SpecialCharInNewQueryValue()
            {
                string path = @"/Area/Ctrl/Action";
                string query = @"mode=search";
                string url = path + "?" + query;
                string expected = url + "&search=hello%20world";
                var helper = GetHelper(path, query);

                string actual = helper.SetQueryString(url, "search", "hello world");

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NullValueProducesNoKVP()
            {
                string path = @"/Area/Ctrl/Action";
                string query = @"";
                string url = path + (!string.IsNullOrEmpty(query) ? ("?" + query) : string.Empty);
                string expected = path;
                var helper = GetHelper(path, query);

                string actual = helper.SetQueryString(url, "nullstuff", null);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NullValueRemovesExistingValue()
            {
                string path = @"/Area/Ctrl/Action";
                string query = @"nullstuff=notnull";
                string url = path + (!string.IsNullOrEmpty(query) ? ("?" + query) : string.Empty);
                string expected = path;
                var helper = GetHelper(path, query);

                string actual = helper.SetQueryString(url, "nullstuff", null);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void BaseMultipleValues_SetMultipleValues()
            {
                // prepare
                string path = "/path?k1=v1&k1=v2"; // key has 2 values
                string input = path;
                string expected = "/path?k1=v1&k1=v2&k1=v1&k1=v3";
                var helper = GetHelper(path, "");

                // execute
                string actual = helper.SetQueryString(input, "k1", "v1", "k1", "v3"); // key has 2 values

                // verify
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void BaseMultipleValues_ResetAndSetMultipleValues()
            {
                // prepare
                string path = "/path?k1=v1&k1=v2"; // key has 2 values
                string input = path;
                string expected = "/path?k1=v1&k1=v3";
                var helper = GetHelper(path, "");

                // execute
                string actual = helper.SetQueryString(input, "k1", null, "k1", "v1", "k1", "v3"); // key has 2 values

                // verify
                Assert.Equal(expected, actual);
            }
        }
        
        public class ResetQueryStringMethod
        {
            [Fact]
            public void WithNoQuery()
            {
                // prepare
                string path = "/path";
                string input = path;
                string expected = "/path?k1=v1";
                var helper = GetHelper(path, "");

                // execute
                string actual = helper.ResetQueryString(input, "k1", "v1");

                // verify
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void WithNoPath()
            {
                // prepare
                string path = "";
                string input = path;
                string expected = "?k1=v1";
                var helper = GetHelper(path, "");

                // execute
                string actual = helper.ResetQueryString(input, "k1", "v1");

                // verify
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void WithQueryDelimiter()
            {
                // prepare
                string path = "/path";
                string query = "";
                string input = path + "?" + query;
                string expected = "/path?k1=v1";
                var helper = GetHelper(path, query);

                // execute
                string actual = helper.ResetQueryString(input, "k1", "v1");

                // verify
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void WithOtherQueryKey()
            {
                // prepare
                string path = "/path";
                string query = "k2=v2";
                string input = path + "?" + query;
                string expected = "/path?k2=v2&k1=v1";
                var helper = GetHelper(path, query);

                // execute
                string actual = helper.ResetQueryString(input, "k1", "v1");

                // verify
                Assert.Equal(expected, actual);
            }

            private static UrlHelper GetHelper(string path, string query)
            {
                var httpRequest = new HttpRequest("", "http://localhost/" + path, query);
                var writer = new StringWriter();
                var response = new HttpResponse(writer);
                var context = new HttpContext(httpRequest, response);
                var wrapped = new HttpContextWrapper(context);
                var helper = new UrlHelper(new RequestContext(wrapped, new RouteData()));
                return helper;
            }

            [Fact]
            public void WithSameQueryKey()
            {
                // prepare
                string path = "/path";
                string query = "k1=v0";
                string input = path + "?" + query;
                string expected = "/path?k1=v1";
                var helper = GetHelper(path, query);

                // execute
                string actual = helper.ResetQueryString(input, "k1", "v1");

                // verify
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void MultipleValues()
            {
                // prepare
                string path = "/path";
                string input = path;
                string expected = "/path?k1=v111&k2=v2";
                var helper = GetHelper(path, "");

                // execute
                string actual = helper.ResetQueryString(input, "k1", "v111", "k2", "v2");

                // verify
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void MultipleValuesOneSet()
            {
                // prepare
                string path = "/path?k1=v1";
                string input = path;
                string expected = "/path?k1=v111&k2=v2";
                var helper = GetHelper(path, "");

                // execute
                string actual = helper.ResetQueryString(input, "k1", "v111", "k2", "v2");

                // verify
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void SpecialCharInOriginalQuery()
            {
                string path = @"/Area/Ctrl/Action";
                string query = @"search=hello%20world";
                string url = path + "?" + query;
                string expected = url + "&page=2";
                var helper = GetHelper(path, query);

                string actual = helper.ResetQueryString(url, "page", "2");

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void SpecialCharInNewQueryValue()
            {
                string path = @"/Area/Ctrl/Action";
                string query = @"mode=search";
                string url = path + "?" + query;
                string expected = url + "&search=hello%20world";
                var helper = GetHelper(path, query);

                string actual = helper.ResetQueryString(url, "search", "hello world");

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NullValueProducesNoKVP()
            {
                string path = @"/Area/Ctrl/Action";
                string query = @"";
                string url = path + (!string.IsNullOrEmpty(query) ? ("?" + query) : string.Empty);
                string expected = path;
                var helper = GetHelper(path, query);

                string actual = helper.ResetQueryString(url, "nullstuff", null);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NullValueRemovesExistingValue()
            {
                string path = @"/Area/Ctrl/Action";
                string query = @"nullstuff=notnull";
                string url = path + (!string.IsNullOrEmpty(query) ? ("?" + query) : string.Empty);
                string expected = path;
                var helper = GetHelper(path, query);

                string actual = helper.ResetQueryString(url, "nullstuff", null);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void BaseMultipleValues_SetMultipleValues()
            {
                // prepare
                string path = "/path?k1=v1&k1=v2"; // key has 2 values
                string input = path;
                string expected = "/path?k1=v1&k1=v3";
                var helper = GetHelper(path, "");

                // execute
                string actual = helper.ResetQueryString(input, "k1", "v1", "k1", "v3"); // key has 2 values

                // verify
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void BaseMultipleValues_ResetAndSetMultipleValues()
            {
                // prepare
                string path = "/path?k1=v1&k1=v2"; // key has 2 values
                string input = path;
                string expected = "/path?k1=v1&k1=v3";
                var helper = GetHelper(path, "");

                // execute
                string actual = helper.ResetQueryString(input, "k1", null, "k1", "v1", "k1", "v3"); // key has 2 values

                // verify
                Assert.Equal(expected, actual);
            }
        }
    }
}
