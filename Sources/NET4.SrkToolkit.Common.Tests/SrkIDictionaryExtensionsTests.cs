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

        [TestClass]
        public class AddRangeMethod
        {
            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfNullArg0()
            {
                List<TestModel> source = new List<TestModel>();
                Dictionary<int, object> target = null;
                target.AddRange(source, s => s.Id, s => s.Name);
            }

            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfNullArg1()
            {
                List<TestModel> source = null;
                var target = new Dictionary<int, object>();
                target.AddRange(source, s => s.Id, s => s.Name);
            }

            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfNullArg2()
            {
                List<TestModel> source = new List<TestModel>();
                var target = new Dictionary<int, object>();
                target.AddRange(source, null, s => s.Name);
            }

            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfNullArg3()
            {
                List<TestModel> source = new List<TestModel>();
                var target = new Dictionary<int, object>();
                target.AddRange(source, s => s.Id, null);
            }

            [TestMethod]
            public void Works()
            {
                var source = new List<TestModel>()
                {
                    new TestModel() { Id = 1, },
                    new TestModel() { Id = 2, },
                    new TestModel() { Id = 3, },
                };
                var target = new Dictionary<int, TestModel>();
                target.AddRange(source, s => s.Id, s => s);

                Assert.AreEqual(3, target.Count);
                Assert.IsTrue(target.ContainsKey(1));
                Assert.IsTrue(target.ContainsKey(2));
                Assert.IsTrue(target.ContainsKey(3));
                Assert.AreSame(source[0], target[1]);
                Assert.AreSame(source[1], target[2]);
                Assert.AreSame(source[2], target[3]);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void KeyAlreadyExists()
            {
                var source = new List<TestModel>()
                {
                    new TestModel() { Id = 1, },
                    new TestModel() { Id = 2, },
                    new TestModel() { Id = 3, },
                };
                var target = new Dictionary<int, object>();
                target.Add(1, new TestModel() { Id = 1, });
                target.AddRange(source, s => s.Id, s => s);
            }
        }

        [TestClass]
        public class MergeMethod
        {
            [TestMethod]
            public void NoConflict()
            {
                var target = new Dictionary<int, TestModel>();
                var first = new TestModel(1);
                target.Add(1, first);
                var source = new List<TestModel>()
                {
                    new TestModel(2),
                    new TestModel(3),
                };
                target.Merge(source, m => m.Id, m => m, null);

                Assert.AreEqual(3, target.Count);
                Assert.IsTrue(target.ContainsKey(1));
                Assert.IsTrue(target.ContainsKey(2));
                Assert.IsTrue(target.ContainsKey(3));
                Assert.AreSame(first, target[1]);
                Assert.AreSame(source[0], target[2]);
                Assert.AreSame(source[1], target[3]);
            }

            [TestMethod]
            public void EqualConflict()
            {
                var target = new Dictionary<int, TestModel>();
                var first = new TestModel(1);
                target.Add(1, first);
                var source = new List<TestModel>()
                {
                    first,
                    new TestModel(2),
                    new TestModel(3),
                };
                target.Merge(source, m => m.Id, m => m, null);

                Assert.AreEqual(3, target.Count);
                Assert.IsTrue(target.ContainsKey(1));
                Assert.IsTrue(target.ContainsKey(2));
                Assert.IsTrue(target.ContainsKey(3));
                Assert.AreSame(first, target[1]);
                Assert.AreSame(source[1], target[2]);
                Assert.AreSame(source[2], target[3]);
            }

            [TestMethod, ExpectedException(typeof(InvalidOperationException))]
            public void RealConflictWithNoResolverThrows()
            {
                var target = new Dictionary<int, TestModel>();
                var first = new TestModel(1);
                target.Add(1, first);
                var source = new List<TestModel>()
                {
                    new TestModel(1),
                    new TestModel(2),
                    new TestModel(3),
                };
                target.Merge(source, m => m.Id, m => m, null);
            }

            [TestMethod]
            public void WithConflictResolver()
            {
                var target = new Dictionary<int, TestModel>();
                var first = new TestModel(1);
                target.Add(1, first);
                var source = new List<TestModel>()
                {
                    new TestModel(1),
                    new TestModel(2),
                    new TestModel(3),
                };
                target.Merge(
                    source,
                    m => m.Id,
                    m => m,
                    (key, origValue, newValue) =>
                    {
                        return newValue;
                    });

                Assert.AreEqual(3, target.Count);
                Assert.IsTrue(target.ContainsKey(1));
                Assert.IsTrue(target.ContainsKey(2));
                Assert.IsTrue(target.ContainsKey(3));
                Assert.AreSame(source[0], target[1]);
                Assert.AreSame(source[1], target[2]);
                Assert.AreSame(source[2], target[3]);
            }
        }

        public class TestModel
        {
            private static readonly Random random = new Random();

            public TestModel()
            {
                this.Id = random.Next(0, int.MaxValue);
                this.Name = Guid.NewGuid().ToString();
            }

            public TestModel(int id)
            {
                this.Id = id;
                this.Name = Guid.NewGuid().ToString();
            }

            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
