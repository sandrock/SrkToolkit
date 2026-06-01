// -----------------------------------------------------------------------
// <copyright file="SrkIDictionaryExtensionsTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CSrkIDictionaryExtensions = System.Collections.SrkIDictionaryExtensions;
    using GSrkIDictionaryExtensions = System.Collections.Generic.SrkIDictionaryExtensions;
    using Xunit;

    public class SrkIDictionaryExtensionsTests
    {
        public class GetValueMethod
        {
            [Fact]
            public void NonGeneric_ReturnsStoredValue()
            {
                var value = new object();
                var key = "key";
                IDictionary dictionary = new Dictionary<string, object>();
                dictionary.Add(key, value);
                var result = CSrkIDictionaryExtensions.GetValue<object>(dictionary, key, () => null, true);
                Assert.True(object.ReferenceEquals(result, value));
            }

            [Fact]
            public void NonGeneric_GeneratesValue()
            {
                var value = new object();
                var key = "key";
                IDictionary dictionary = new Dictionary<string, object>();
                var result = CSrkIDictionaryExtensions.GetValue<object>(dictionary, key, () => value, true);
                Assert.True(object.ReferenceEquals(result, value));
            }

            [Fact]
            public void NonGeneric_GeneratedValueIsStored()
            {
                var value = new object();
                var key = "key";
                IDictionary dictionary = new Dictionary<string, object>();
                var result1 = CSrkIDictionaryExtensions.GetValue<object>(dictionary, key, () => value, true);
                var result2 = CSrkIDictionaryExtensions.GetValue<object>(dictionary, key, () => null, true);
                Assert.True(object.ReferenceEquals(result1, value));
                Assert.True(object.ReferenceEquals(result2, value));
            }

            [Fact]
            public void Generic_ReturnsStoredValue()
            {
                var value = new object();
                var key = "key";
                IDictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add(key, value);
                var result = GSrkIDictionaryExtensions.GetValue(dictionary, key);
                Assert.Same(value, result);
            }

            [Fact]
            public void Generic_KeyNotFound_ReturnsNull()
            {
                var key = "key";
                IDictionary<string, object> dictionary = new Dictionary<string, object>();
                var result = GSrkIDictionaryExtensions.GetValue(dictionary, key);
                Assert.Null(result);
            }

            [Fact]
            public void Generic_NullValue()
            {
                object value = null;
                var key = "key";
                IDictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add(key, value);
                var result = GSrkIDictionaryExtensions.GetValue(dictionary, key);
                Assert.Null(result);
            }
        }

        public class AddRangeMethod
        {
            [Fact]
            public void ThrowsIfNullArg0()
            {
                List<TestModel> source = new List<TestModel>();
                Dictionary<int, object> target = null;
                Assert.Throws<ArgumentNullException>(() =>
                {
                    GSrkIDictionaryExtensions.AddRange(target, source, s => s.Id, s => s.Name);
                });
            }

            [Fact]
            public void ThrowsIfNullArg1()
            {
                List<TestModel> source = null;
                var target = new Dictionary<int, object>();
                Assert.Throws<ArgumentNullException>(() =>
                {
                    GSrkIDictionaryExtensions.AddRange(target, source, s => s.Id, s => s.Name);
                });
            }

            [Fact]
            public void ThrowsIfNullArg2()
            {
                List<TestModel> source = new List<TestModel>();
                var target = new Dictionary<int, object>();
                Assert.Throws<ArgumentNullException>(() =>
                {
                    GSrkIDictionaryExtensions.AddRange(target, source, null, s => s.Name);
                });
            }

            [Fact]
            public void ThrowsIfNullArg3()
            {
                List<TestModel> source = new List<TestModel>();
                var target = new Dictionary<int, object>();
                Assert.Throws<ArgumentNullException>(() =>
                {
                    GSrkIDictionaryExtensions.AddRange(target, source, s => s.Id, null);
                });
            }

            [Fact]
            public void Works()
            {
                var source = new List<TestModel>()
                {
                    new TestModel() { Id = 1, },
                    new TestModel() { Id = 2, },
                    new TestModel() { Id = 3, },
                };
                var target = new Dictionary<int, TestModel>();
                GSrkIDictionaryExtensions.AddRange(target, source, s => s.Id, s => s);

                Assert.Equal(3, target.Count);
                Assert.True(target.ContainsKey(1));
                Assert.True(target.ContainsKey(2));
                Assert.True(target.ContainsKey(3));
                Assert.Same(source[0], target[1]);
                Assert.Same(source[1], target[2]);
                Assert.Same(source[2], target[3]);
            }

            [Fact]
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
                Assert.Throws<ArgumentException>(() =>
                {
                    GSrkIDictionaryExtensions.AddRange(target, source, s => s.Id, s => s);
                });
            }
        }

        public class MergeMethod
        {
            [Fact]
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

                Assert.Equal(3, target.Count);
                Assert.True(target.ContainsKey(1));
                Assert.True(target.ContainsKey(2));
                Assert.True(target.ContainsKey(3));
                Assert.Same(first, target[1]);
                Assert.Same(source[0], target[2]);
                Assert.Same(source[1], target[3]);
            }

            [Fact]
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

                Assert.Equal(3, target.Count);
                Assert.True(target.ContainsKey(1));
                Assert.True(target.ContainsKey(2));
                Assert.True(target.ContainsKey(3));
                Assert.Same(first, target[1]);
                Assert.Same(source[1], target[2]);
                Assert.Same(source[2], target[3]);
            }

            [Fact]
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
                Assert.Throws<InvalidOperationException>(() =>
                {
                    target.Merge(source, m => m.Id, m => m, null);
                });
            }

            [Fact]
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

                Assert.Equal(3, target.Count);
                Assert.True(target.ContainsKey(1));
                Assert.True(target.ContainsKey(2));
                Assert.True(target.ContainsKey(3));
                Assert.Same(source[0], target[1]);
                Assert.Same(source[1], target[2]);
                Assert.Same(source[2], target[3]);
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
