
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class SrkArrayExtensionsTests
    {
        public class CombineWithMethod_CombineArray
        {
            [Fact]
            public void ThrowsArg0Null()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    SrkArrayExtensions.CombineWith(default(string[]), new string[0]);
                });
            }

            [Fact]
            public void ThrowsArg1Null()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    SrkArrayExtensions.CombineWith(new string[0], default(string[]));
                });
            }

            [Fact]
            public void Works1()
            {
                var source = new int[] { 1, 2, };
                var items = new int[] { 3, 4, };
                var result = SrkArrayExtensions.CombineWith(source, items);
                Assert.Equal(4, result.Length);
                Assert.Equal(1, result[0]);
                Assert.Equal(2, result[1]);
                Assert.Equal(3, result[2]);
                Assert.Equal(4, result[3]);
            }

            [Fact]
            public void Works2()
            {
                var source = new int[] { 1, 2, };
                var items1 = new int[] { 3, 4, };
                var items2 = new int[] { 5, 6, };
                var result = SrkArrayExtensions.CombineWith(source, items1, items2);
                Assert.Equal(6, result.Length);
                Assert.Equal(1, result[0]);
                Assert.Equal(2, result[1]);
                Assert.Equal(3, result[2]);
                Assert.Equal(4, result[3]);
                Assert.Equal(5, result[4]);
                Assert.Equal(6, result[5]);
            }

            [Fact]
            public void EmptyArg0()
            {
                var source = new int[] { };
                var items = new int[] { 3, 4, };
                var result = SrkArrayExtensions.CombineWith(source, items);
                Assert.Equal(2, result.Length);
                Assert.Equal(3, result[0]);
                Assert.Equal(4, result[1]);
            }

            [Fact]
            public void EmptyArg1()
            {
                var source = new int[] { 1, 2, };
                var items = new int[] { };
                var result = SrkArrayExtensions.CombineWith(source, items);
                Assert.Equal(2, result.Length);
                Assert.Equal(1, result[0]);
                Assert.Equal(2, result[1]);
            }

            [Fact]
            public void EmptyArgs()
            {
                var source = new int[] { };
                var items = new int[] { };
                var result = SrkArrayExtensions.CombineWith(source, items);
                Assert.Equal(0, result.Length);
            }
        }

        public class CombineWithMethod_CombineItem
        {
            [Fact]
            public void ThrowsArg0Null()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    SrkArrayExtensions.CombineWith(default(string[]), string.Empty);
                });
            }

            [Fact]
            public void Works1()
            {
                var source = new int[] { 1, 2, };
                var item = 3;
                var result = SrkArrayExtensions.CombineWith(source, item);
                Assert.Equal(3, result.Length);
                Assert.Equal(1, result[0]);
                Assert.Equal(2, result[1]);
                Assert.Equal(3, result[2]);
            }

            [Fact]
            public void Works2()
            {
                var source = new int[] { 1, 2, };
                var item1 = 3;
                var item2 = 4;
                var result = SrkArrayExtensions.CombineWith(source, item1, item2);
                Assert.Equal(4, result.Length);
                Assert.Equal(1, result[0]);
                Assert.Equal(2, result[1]);
                Assert.Equal(3, result[2]);
                Assert.Equal(4, result[3]);
            }

            [Fact]
            public void EmptyArg0()
            {
                var source = new int[] { };
                var item = 3;
                var result = SrkArrayExtensions.CombineWith(source, item);
                Assert.Equal(1, result.Length);
                Assert.Equal(3, result[0]);
            }

            [Fact]
            public void EmptyArg1()
            {
                var source = new string[] { "1", "2", };
                string item = null;
                var result = SrkArrayExtensions.CombineWith(source, item);
                Assert.Equal(3, result.Length);
                Assert.Equal("1", result[0]);
                Assert.Equal("2", result[1]);
                Assert.Equal(item, result[2]);
            }

            [Fact]
            public void EmptyArgs()
            {
                var source = new string[] { };
                var item = default(string);
                var result = SrkArrayExtensions.CombineWith(source, item);
                Assert.Equal(1, result.Length);
                Assert.Equal(item, result[0]);
            }
        }
    }
}
