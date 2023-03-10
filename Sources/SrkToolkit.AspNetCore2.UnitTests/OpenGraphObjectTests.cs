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

    public class OpenGraphObjectTests
    {
        [Fact]
        public void EmptyObjectRendersNothing()
        {
            var obj = new OpenGraphObject();
            Assert.Equal(string.Empty, obj.ToString());
        }

        [Fact]
        public void BasicObjectRendersTags()
        {
            var obj = new OpenGraphObject("my super title", new Uri("http://test.org/my-super-title"));
            var expected = Meta("og:title", "my super title") + Meta("og:url", "http://test.org/my-super-title");
            Assert.Equal(expected, obj.ToString());
        }

        [Fact]
        public void BasicObjectRendersNamespaces()
        {
            var obj = new OpenGraphObject("my super title", new Uri("http://test.org/my-super-title"));
            var expected = " xmlns:og=\"http://ogp.me/ns#\" ";
            Assert.Equal(expected, obj.ToHtmlAttributeNamespaces());
        }

        [Fact]
        public void RendersMultipleNamespaces()
        {
            var obj = new OpenGraphObject("my super title", new Uri("http://test.org/my-super-title"));
            obj.Add(new OpenGraphTag(new OpenGraphName("key", "xxx", "http://xxx/aaa/#"), "value"));
            var expected = " xmlns:og=\"http://ogp.me/ns#\"  xmlns:xxx=\"http://xxx/aaa/#\" ";
            Assert.Equal(expected, obj.ToHtmlAttributeNamespaces());
        }

        [Fact]
        public void TypicalHomepageWithLogo()
        {
            var obj = new OpenGraphObject("welcome to mysite", new Uri("http://mysite.com/"));
            obj.IsImage(imageUri: new Uri("http://mysite.com/Content/loog.png"));
            obj.SetDescription("With mysite, build your blog in 2 minutes");
            var expected = Meta("og:title", "welcome to mysite")
                + Meta("og:url", "http://mysite.com/")
                + Meta("og:image", "http://mysite.com/Content/loog.png")
                + Meta("og:description", "With mysite, build your blog in 2 minutes");
            Assert.Equal(expected, obj.ToString());
        }

        private string Meta(string key, string value)
        {
            return "<meta property=\"" + key.ProperHtmlAttributeEscape() + "\" content=\"" + value.ProperHtmlAttributeEscape() + "\" />";
        }
    }
}
