
namespace SrkToolkit.Common.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [TestClass]
    public class SrkArrayExtensionsTests
    {
        [TestClass]
        public class CombineWithMethod_CombineArray
        {
            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsArg0Null()
            {
                SrkArrayExtensions.CombineWith(default(string[]), new string[0]);
            }

            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsArg1Null()
            {
                SrkArrayExtensions.CombineWith(new string[0], default(string[]));
            }

            [TestMethod]
            public void Works1()
            {
                var source = new int[] { 1, 2, };
                var items = new int[] { 3, 4, };
                var result = SrkArrayExtensions.CombineWith(source, items);
                Assert.AreEqual(4, result.Length);
                Assert.AreEqual(1, result[0]);
                Assert.AreEqual(2, result[1]);
                Assert.AreEqual(3, result[2]);
                Assert.AreEqual(4, result[3]);
            }

            [TestMethod]
            public void Works2()
            {
                var source = new int[] { 1, 2, };
                var items1 = new int[] { 3, 4, };
                var items2 = new int[] { 5, 6, };
                var result = SrkArrayExtensions.CombineWith(source, items1, items2);
                Assert.AreEqual(6, result.Length);
                Assert.AreEqual(1, result[0]);
                Assert.AreEqual(2, result[1]);
                Assert.AreEqual(3, result[2]);
                Assert.AreEqual(4, result[3]);
                Assert.AreEqual(5, result[4]);
                Assert.AreEqual(6, result[5]);
            }

            [TestMethod]
            public void EmptyArg0()
            {
                var source = new int[] { };
                var items = new int[] { 3, 4, };
                var result = SrkArrayExtensions.CombineWith(source, items);
                Assert.AreEqual(2, result.Length);
                Assert.AreEqual(3, result[0]);
                Assert.AreEqual(4, result[1]);
            }

            [TestMethod]
            public void EmptyArg1()
            {
                var source = new int[] { 1, 2, };
                var items = new int[] { };
                var result = SrkArrayExtensions.CombineWith(source, items);
                Assert.AreEqual(2, result.Length);
                Assert.AreEqual(1, result[0]);
                Assert.AreEqual(2, result[1]);
            }

            [TestMethod]
            public void EmptyArgs()
            {
                var source = new int[] { };
                var items = new int[] { };
                var result = SrkArrayExtensions.CombineWith(source, items);
                Assert.AreEqual(0, result.Length);
            }
        }

        [TestClass]
        public class CombineWithMethod_CombineItem
        {
            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsArg0Null()
            {
                SrkArrayExtensions.CombineWith(default(string[]), string.Empty);
            }

            [TestMethod]
            public void Works1()
            {
                var source = new int[] { 1, 2, };
                var item = 3;
                var result = SrkArrayExtensions.CombineWith(source, item);
                Assert.AreEqual(3, result.Length);
                Assert.AreEqual(1, result[0]);
                Assert.AreEqual(2, result[1]);
                Assert.AreEqual(3, result[2]);
            }

            [TestMethod]
            public void Works2()
            {
                var source = new int[] { 1, 2, };
                var item1 = 3;
                var item2 = 4;
                var result = SrkArrayExtensions.CombineWith(source, item1, item2);
                Assert.AreEqual(4, result.Length);
                Assert.AreEqual(1, result[0]);
                Assert.AreEqual(2, result[1]);
                Assert.AreEqual(3, result[2]);
                Assert.AreEqual(4, result[3]);
            }

            [TestMethod]
            public void EmptyArg0()
            {
                var source = new int[] { };
                var item = 3;
                var result = SrkArrayExtensions.CombineWith(source, item);
                Assert.AreEqual(1, result.Length);
                Assert.AreEqual(3, result[0]);
            }

            [TestMethod]
            public void EmptyArg1()
            {
                var source = new string[] { "1", "2", };
                string item = null;
                var result = SrkArrayExtensions.CombineWith(source, item);
                Assert.AreEqual(3, result.Length);
                Assert.AreEqual("1", result[0]);
                Assert.AreEqual("2", result[1]);
                Assert.AreEqual(item, result[2]);
            }

            [TestMethod]
            public void EmptyArgs()
            {
                var source = new string[] { };
                var item = default(string);
                var result = SrkArrayExtensions.CombineWith(source, item);
                Assert.AreEqual(1, result.Length);
                Assert.AreEqual(item, result[0]);
            }
        }
    }
}
