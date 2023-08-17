
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class SrkStringExtensionsTests
    {
        public class NullIfEmptyMethod
        {
            [Fact]
            public void Null()
            {
                Assert.Null(SrkStringExtensions.NullIfEmpty(null));
            }

            [Fact]
            public void Empty()
            {
                Assert.Null(SrkStringExtensions.NullIfEmpty(""));
            }

            [Fact]
            public void Whitespace()
            {
                Assert.NotNull(SrkStringExtensions.NullIfEmpty("   \t\r\n"));
            }

            [Fact]
            public void Text()
            {
                string value = " hello ";
                Assert.Equal(value, SrkStringExtensions.NullIfEmpty(value));
            }

            [Fact]
            public void TextTrim()
            {
                string value = " hello ";
                Assert.Equal("hello", SrkStringExtensions.NullIfEmpty(value, true));
            }
        }

        public class NullIfEmptyOrWhitespaceMethod
        {
            [Fact]
            public void Null()
            {
                Assert.Null(SrkStringExtensions.NullIfEmptyOrWhitespace(null));
            }

            [Fact]
            public void Empty()
            {
                Assert.Null(SrkStringExtensions.NullIfEmptyOrWhitespace(""));
            }

            [Fact]
            public void Whitespace()
            {
                Assert.Null(SrkStringExtensions.NullIfEmptyOrWhitespace("   \t\r\n"));
            }

            [Fact]
            public void Text()
            {
                string value = " hello ";
                Assert.Equal(value, SrkStringExtensions.NullIfEmptyOrWhitespace(value));
            }

            [Fact]
            public void TextTrim()
            {
                string value = " hello ";
                Assert.Equal("hello", SrkStringExtensions.NullIfEmptyOrWhitespace(value, true));
            }
        }

        public class TrimToLengthMethod
        {
            [Fact]
            public void SubstringThrowsIfNotLongEnough()
            {
                string value = "hello";
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    value.Substring(10);
                });
            }

            [Fact]
            public void TrimToLengthCutsToDesiredLength4()
            {
                string value = "hello";
                var result = value.TrimToLength(4);
                Assert.Equal("hell", result);
            }

            [Fact]
            public void TrimToLengthCutsToDesiredLength5()
            {
                string value = "hello";
                var result = value.TrimToLength(5);
                Assert.Equal("hello", result);
            }

            [Fact]
            public void TrimToLengthCutsToDesiredLength6()
            {
                string value = "hello";
                var result = value.TrimToLength(6);
                Assert.Equal("hello", result);
            }

            [Fact]
            public void TrimToLengthCutsToDesiredLength7()
            {
                string value = "hello";
                var result = value.TrimToLength(10);
                Assert.Equal("hello", result);
            }
        }

        public class ContainsAnyStrings
        {
            [Fact]
            public void ThrowsIfArg0IsNull()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    SrkStringExtensions.ContainsAny(null, StringComparison.CurrentCulture, new string[0]);
                });
            }

            [Fact]
            public void FalseIsSourceIsEmpty()
            {
                string source = string.Empty;
                bool result = SrkStringExtensions.ContainsAny(source, StringComparison.CurrentCulture, new string[] { string.Empty, });
                Assert.False(result);
            }

            [Fact]
            public void Finds1()
            {
                string source = "hello world";
                var values = new string[] { "hello", "guys", };
                bool result = SrkStringExtensions.ContainsAny(source, StringComparison.InvariantCulture, values);
                Assert.True(result);
            }

            [Fact]
            public void Finds2()
            {
                string source = "hello world";
                var values = new string[] { "bye", "world", };
                bool result = SrkStringExtensions.ContainsAny(source, StringComparison.InvariantCulture, values);
                Assert.True(result);
            }

            [Fact]
            public void Finds1CaseMatch()
            {
                string source = "Hello World";
                var values = new string[] { "hello", "guys", };
                bool result = SrkStringExtensions.ContainsAny(source, StringComparison.InvariantCulture, values);
                Assert.False(result);
            }

            [Fact]
            public void Finds1CaseIgnore()
            {
                string source = "Hello World";
                var values = new string[] { "hello", "guys", };
                bool result = SrkStringExtensions.ContainsAny(source, StringComparison.InvariantCultureIgnoreCase, values);
                Assert.True(result);
            }
        }

        public class ContainsAnyChars
        {
            [Fact]
            public void ThrowsIfArg0IsNull()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    SrkStringExtensions.ContainsAny(null, StringComparison.CurrentCulture, new char[0]);
                });
            }

            [Fact]
            public void Finds1()
            {
                string source = "hello world";
                var values = new char[] { 'h', 'x', };
                bool result = SrkStringExtensions.ContainsAny(source, StringComparison.InvariantCulture, values);
                Assert.True(result);
            }

            [Fact]
            public void Finds1_Params()
            {
                string source = "hello world";
                bool result = SrkStringExtensions.ContainsAny(source, StringComparison.InvariantCulture, 'h', 'x');
                Assert.True(result);
            }

            [Fact]
            public void Finds2()
            {
                string source = "hello world";
                var values = new char[] { 'x', 'w', };
                bool result = SrkStringExtensions.ContainsAny(source, StringComparison.InvariantCulture, values);
                Assert.True(result);
            }

            [Fact]
            public void Finds1CaseMatch()
            {
                string source = "Hello World";
                var values = new char[] { 'h', 'w', };
                bool result = SrkStringExtensions.ContainsAny(source, StringComparison.InvariantCulture, values);
                Assert.False(result);
            }

            [Fact]
            public void Finds1CaseIgnore()
            {
                string source = "Hello World";
                var values = new char[] { 'h', 'x', };
                bool result = SrkStringExtensions.ContainsAny(source, StringComparison.InvariantCultureIgnoreCase, values);
                Assert.True(result);
            }
        }
    }
}
