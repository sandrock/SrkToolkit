// -----------------------------------------------------------------------
// <copyright file="SrkIDictionaryExtensionsTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections;

    public class SrkIDictionaryExtensionsTests
    {
        [TestClass]
        public class GetValueMethod
        {
            [TestMethod]
            public void ReturnsStoredValue()
            {
                var value = new object();
                var key = "key";
                var dictionary = new Dictionary<string, object>();
                dictionary.Add(key, value);
                var result = SrkIDictionaryExtensions.GetValue<object>(dictionary, key, () => null, true);
                Assert.IsTrue(object.ReferenceEquals(result, value));
            }

            [TestMethod]
            public void GeneratesValue()
            {
                var value = new object();
                var key = "key";
                var dictionary = new Dictionary<string, object>();
                var result = SrkIDictionaryExtensions.GetValue<object>(dictionary, key, () => value, true);
                Assert.IsTrue(object.ReferenceEquals(result, value));
            }

            [TestMethod]
            public void GeneratedValueIsStored()
            {
                var value = new object();
                var key = "key";
                var dictionary = new Dictionary<string, object>();
                var result1 = SrkIDictionaryExtensions.GetValue<object>(dictionary, key, () => value, true);
                var result2 = SrkIDictionaryExtensions.GetValue<object>(dictionary, key, () => null, true);
                Assert.IsTrue(object.ReferenceEquals(result1, value));
                Assert.IsTrue(object.ReferenceEquals(result2, value));
            }
        }
    }
}
