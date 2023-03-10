
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class SrkIListExtensionsTests
    {
        public class SingleOrCreateMethod
        {
            [Fact]
            public void NoMatchShouldCreateStruct()
            {
                string value = "hello";
                var list = new List<string>();
                var result = list.SingleOrCreate(x => x == value, () => value);
                Assert.Equal(value, result);
                Assert.Equal(1, list.Count);
            }

            [Fact]
            public void NoMatchShouldCreateClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var result = list.SingleOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.NotNull(result);
                Assert.Equal(value, result.Key);
                Assert.Equal(1, list.Count);
            }

            [Fact]
            public void OneMatchShouldCreateStruct()
            {
                string value = "hello";
                var list = new List<string>();
                list.Add(value);
                var result = list.SingleOrCreate(x => x == value, () => value);
                Assert.Equal(value, result);
                Assert.Equal(1, list.Count);
            }

            [Fact]
            public void OneMatchShouldCreateClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var item = new Yo { Key = value, };
                list.Add(item);
                var result = list.SingleOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.NotNull(result);
                Assert.Same(item, result);
                Assert.Equal(value, result.Key);
                Assert.Equal(1, list.Count);
            }

            [Fact]
            public void MultiMatchShouldCreateStruct()
            {
                string value = "hello";
                var list = new List<string>();
                list.Add(value);
                list.Add(value);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var result = list.SingleOrCreate(x => x == value, () => value);
                });
            }

            [Fact]
            public void MultiMatchShouldCreateClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var item = new Yo { Key = value, };
                list.Add(item);
                list.Add(item);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    var result = list.SingleOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                });
            }
        }

        public class FirstOrCreateMethod
        {
            [Fact]
            public void NoMatchShouldCreateStruct()
            {
                string value = "hello";
                var list = new List<string>();
                var result = list.FirstOrCreate(x => x == value, () => value);
                Assert.Equal(value, result);
                Assert.Equal(1, list.Count);
            }

            [Fact]
            public void NoMatchShouldCreateClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var result = list.FirstOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.NotNull(result);
                Assert.Equal(value, result.Key);
                Assert.Equal(1, list.Count);
            }

            [Fact]
            public void OneMatchShouldCreateStruct()
            {
                string value = "hello";
                var list = new List<string>();
                list.Add(value);
                var result = list.FirstOrCreate(x => x == value, () => value);
                Assert.Equal(value, result);
                Assert.Equal(1, list.Count);
            }

            [Fact]
            public void OneMatchShouldCreateClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var item = new Yo { Key = value, };
                list.Add(item);
                var result = list.FirstOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.NotNull(result);
                Assert.Same(item, result);
                Assert.Equal(value, result.Key);
                Assert.Equal(1, list.Count);
            }

            [Fact]
            public void MultiMatchShouldReturnFirstStruct()
            {
                string value = "hello";
                var list = new List<string>();
                list.Add(value);
                list.Add(value);
                var result = list.FirstOrCreate(x => x == value, () => value);
                Assert.Equal(value, result);
                Assert.Equal(2, list.Count);
            }

            [Fact]
            public void MultiMatchShouldReturnFirstClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var item1 = new Yo { Key = value, };
                list.Add(item1);
                var item2 = new Yo { Key = value, };
                list.Add(item2);
                var result = list.FirstOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.NotNull(result);
                Assert.Same(item1, result);
                Assert.Equal(value, result.Key);
                Assert.Equal(2, list.Count);
            }
        }

        public class LastOrCreateMethod
        {
            [Fact]
            public void NoMatchShouldCreateStruct()
            {
                string value = "hello";
                var list = new List<string>();
                var result = list.LastOrCreate(x => x == value, () => value);
                Assert.Equal(value, result);
                Assert.Equal(1, list.Count);
            }

            [Fact]
            public void NoMatchShouldCreateClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var result = list.LastOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.NotNull(result);
                Assert.Equal(value, result.Key);
                Assert.Equal(1, list.Count);
            }

            [Fact]
            public void OneMatchShouldCreateStruct()
            {
                string value = "hello";
                var list = new List<string>();
                list.Add(value);
                var result = list.LastOrCreate(x => x == value, () => value);
                Assert.Equal(value, result);
                Assert.Equal(1, list.Count);
            }

            [Fact]
            public void OneMatchShouldCreateClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var item = new Yo { Key = value, };
                list.Add(item);
                var result = list.LastOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.NotNull(result);
                Assert.Same(item, result);
                Assert.Equal(value, result.Key);
                Assert.Equal(1, list.Count);
            }

            [Fact]
            public void MultiMatchShouldReturnLastStruct()
            {
                string value = "hello";
                var list = new List<string>();
                list.Add(value);
                list.Add(value);
                var result = list.LastOrCreate(x => x == value, () => value);
                Assert.Equal(value, result);
                Assert.Equal(2, list.Count);
            }

            [Fact]
            public void MultiMatchShouldReturnLastClass()
            {
                string value = "hello";
                var list = new List<Yo>();
                var item1 = new Yo { Key = value, };
                list.Add(item1);
                var item2 = new Yo { Key = value, };
                list.Add(item2);
                var result = list.LastOrCreate(x => x.Key == value, () => new Yo { Key = value, });
                Assert.NotNull(result);
                Assert.Same(item2, result);
                Assert.Equal(value, result.Key);
                Assert.Equal(2, list.Count);
            }
        }

        public class Yo
        {
            public string Key { get; set; }
        }
    }
}
