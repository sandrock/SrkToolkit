
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Specialized;

    public class SrkNameValueCollectionExtensionsTests
    {
        [TestClass]
        public class ToDictionaryMethod
        {
            [TestMethod]
            public void Works()
            {
                // prepare
                var source = new NameValueCollection();
                source.Add("k1", "v1");
                source.Add("k2", "v2");

                // execute
                var result = SrkNameValueCollectionExtensions.ToDictionary(source);

                // verify
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual("k1", result.Keys.First());
                Assert.AreEqual("k2", result.Keys.Skip(1).First());
                Assert.AreEqual("v1", result["k1"]);
                Assert.AreEqual("v2", result["k2"]);
            }
        }

        [TestClass]
        public class AsEnumerableMethod
        {
            [TestMethod]
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
                    Assert.AreEqual("k" + i, item.Key);
                    Assert.AreEqual("v" + i, item.Value);
                    i++;
                }

                Assert.AreEqual(3, i);
            }
        }
    }
}
