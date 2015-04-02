
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SrkIListExtensionsTests
    {
        [TestClass]
        public class SingleOrCreateMethod
        {
            [TestMethod]
            public void NoMatchShouldCreateStruct()
            {
                string value = "hello";
                var list = new List<string>();
                var result = list.SingleOrCreate(x => x == value, () => value);
                Assert.AreEqual(value, result);
                Assert.AreEqual(1, list.Count);
            }

            [TestMethod]
            public void NoMatchShouldCreateClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var result = list.SingleOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.IsNotNull(result);
                Assert.AreEqual(value, result.Key);
                Assert.AreEqual(1, list.Count);
            }

            [TestMethod]
            public void OneMatchShouldCreateStruct()
            {
                string value = "hello";
                var list = new List<string>();
                list.Add(value);
                var result = list.SingleOrCreate(x => x == value, () => value);
                Assert.AreEqual(value, result);
                Assert.AreEqual(1, list.Count);
            }

            [TestMethod]
            public void OneMatchShouldCreateClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var item = new Yo { Key = value, };
                list.Add(item);
                var result = list.SingleOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.IsNotNull(result);
                Assert.AreSame(item, result);
                Assert.AreEqual(value, result.Key);
                Assert.AreEqual(1, list.Count);
            }

            [TestMethod, ExpectedException(typeof(InvalidOperationException))]
            public void MultiMatchShouldCreateStruct()
            {
                string value = "hello";
                var list = new List<string>();
                list.Add(value);
                list.Add(value);
                var result = list.SingleOrCreate(x => x == value, () => value);
            }

            [TestMethod, ExpectedException(typeof(InvalidOperationException))]
            public void MultiMatchShouldCreateClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var item = new Yo { Key = value, };
                list.Add(item);
                list.Add(item);
                var result = list.SingleOrCreate(x => x.Key == value, () => new Yo { Key = value, });
            }
        }

        [TestClass]
        public class FirstOrCreateMethod
        {
            [TestMethod]
            public void NoMatchShouldCreateStruct()
            {
                string value = "hello";
                var list = new List<string>();
                var result = list.FirstOrCreate(x => x == value, () => value);
                Assert.AreEqual(value, result);
                Assert.AreEqual(1, list.Count);
            }

            [TestMethod]
            public void NoMatchShouldCreateClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var result = list.FirstOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.IsNotNull(result);
                Assert.AreEqual(value, result.Key);
                Assert.AreEqual(1, list.Count);
            }

            [TestMethod]
            public void OneMatchShouldCreateStruct()
            {
                string value = "hello";
                var list = new List<string>();
                list.Add(value);
                var result = list.FirstOrCreate(x => x == value, () => value);
                Assert.AreEqual(value, result);
                Assert.AreEqual(1, list.Count);
            }

            [TestMethod]
            public void OneMatchShouldCreateClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var item = new Yo { Key = value, };
                list.Add(item);
                var result = list.FirstOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.IsNotNull(result);
                Assert.AreSame(item, result);
                Assert.AreEqual(value, result.Key);
                Assert.AreEqual(1, list.Count);
            }

            [TestMethod]
            public void MultiMatchShouldReturnFirstStruct()
            {
                string value = "hello";
                var list = new List<string>();
                list.Add(value);
                list.Add(value);
                var result = list.FirstOrCreate(x => x == value, () => value);
                Assert.AreEqual(value, result);
                Assert.AreEqual(2, list.Count);
            }

            [TestMethod]
            public void MultiMatchShouldReturnFirstClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var item1 = new Yo { Key = value, };
                list.Add(item1);
                var item2 = new Yo { Key = value, };
                list.Add(item2);
                var result = list.FirstOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.IsNotNull(result);
                Assert.AreSame(item1, result);
                Assert.AreEqual(value, result.Key);
                Assert.AreEqual(2, list.Count);
            }
        }

        [TestClass]
        public class LastOrCreateMethod
        {
            [TestMethod]
            public void NoMatchShouldCreateStruct()
            {
                string value = "hello";
                var list = new List<string>();
                var result = list.LastOrCreate(x => x == value, () => value);
                Assert.AreEqual(value, result);
                Assert.AreEqual(1, list.Count);
            }

            [TestMethod]
            public void NoMatchShouldCreateClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var result = list.LastOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.IsNotNull(result);
                Assert.AreEqual(value, result.Key);
                Assert.AreEqual(1, list.Count);
            }

            [TestMethod]
            public void OneMatchShouldCreateStruct()
            {
                string value = "hello";
                var list = new List<string>();
                list.Add(value);
                var result = list.LastOrCreate(x => x == value, () => value);
                Assert.AreEqual(value, result);
                Assert.AreEqual(1, list.Count);
            }

            [TestMethod]
            public void OneMatchShouldCreateClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var item = new Yo { Key = value, };
                list.Add(item);
                var result = list.LastOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.IsNotNull(result);
                Assert.AreSame(item, result);
                Assert.AreEqual(value, result.Key);
                Assert.AreEqual(1, list.Count);
            }

            [TestMethod]
            public void MultiMatchShouldReturnLastStruct()
            {
                string value = "hello";
                var list = new List<string>();
                list.Add(value);
                list.Add(value);
                var result = list.LastOrCreate(x => x == value, () => value);
                Assert.AreEqual(value, result);
                Assert.AreEqual(2, list.Count);
            }

            [TestMethod]
            public void MultiMatchShouldReturnLastClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var item1 = new Yo { Key = value, };
                list.Add(item1);
                var item2 = new Yo { Key = value, };
                list.Add(item2);
                var result = list.LastOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.IsNotNull(result);
                Assert.AreSame(item2, result);
                Assert.AreEqual(value, result.Key);
                Assert.AreEqual(2, list.Count);
            }
        }

        public class Yo
        {
            public string Key { get; set; }
        }
    }
}
