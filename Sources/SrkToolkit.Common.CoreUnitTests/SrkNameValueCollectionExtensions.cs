
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class SrkNameValueCollectionExtensionsTests
    {
        public class ToDictionaryMethod
        {
            [Fact]
            public void Works()
            {
                // prepare
                var source = new NameValueCollection();
                source.Add("k1", "v1");
                source.Add("k2", "v2");

                // execute
                var result = SrkNameValueCollectionExtensions.ToDictionary(source);

                // verify
                Assert.Equal(2, result.Count);
                Assert.Equal("k1", result.Keys.First());
                Assert.Equal("k2", result.Keys.Skip(1).First());
                Assert.Equal("v1", result["k1"]);
                Assert.Equal("v2", result["k2"]);
            }
        }

        public class AsEnumerableMethod
        {
            [Fact]
            public void Works()
            {
                // prepare
                var source = new NameValueCollection();
                source.Add("k1", "v1");
                source.Add("k2", "v2");

                // execute
                int i = 1;
                foreach (var item in SrkNameValueCollectionExtensions.AsEnumerable(source))
                {
                    Assert.Equal("k" + i, item.Key);
                    Assert.Equal("v" + i, item.Value);
                    i++;
                }

                Assert.Equal(3, i);
            }
        }
    }
}
