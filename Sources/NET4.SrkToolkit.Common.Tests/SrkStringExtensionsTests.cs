
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class SrkStringExtensionsTests
    {
        [TestClass]
        public class NullIfEmptyMethod
        {
            [TestMethod]
            public void Null()
            {
                Assert.IsNull(SrkStringExtensions.NullIfEmpty(null));
            }

            [TestMethod]
            public void Empty()
            {
                Assert.IsNull(SrkStringExtensions.NullIfEmpty(""));
            }

            [TestMethod]
            public void Whitespace()
            {
                Assert.IsNotNull(SrkStringExtensions.NullIfEmpty("   \t\r\n"));
            }

            [TestMethod]
            public void Text()
            {
                string value = " hello ";
                Assert.AreEqual(value, SrkStringExtensions.NullIfEmpty(value));
            }

            [TestMethod]
            public void TextTrim()
            {
                string value = " hello ";
                Assert.AreEqual("hello", SrkStringExtensions.NullIfEmpty(value, true));
            }
        }

        [TestClass]
        public class NullIfEmptyOrWhitespaceMethod
        {
            [TestMethod]
            public void Null()
            {
                Assert.IsNull(SrkStringExtensions.NullIfEmptyOrWhitespace(null));
            }

            [TestMethod]
            public void Empty()
            {
                Assert.IsNull(SrkStringExtensions.NullIfEmptyOrWhitespace(""));
            }

            [TestMethod]
            public void Whitespace()
            {
                Assert.IsNull(SrkStringExtensions.NullIfEmptyOrWhitespace("   \t\r\n"));
            }

            [TestMethod]
            public void Text()
            {
                string value = " hello ";
                Assert.AreEqual(value, SrkStringExtensions.NullIfEmptyOrWhitespace(value));
            }

            [TestMethod]
            public void TextTrim()
            {
                string value = " hello ";
                Assert.AreEqual("hello", SrkStringExtensions.NullIfEmptyOrWhitespace(value, true));
            }
        }

        [TestClass]
        public class TrimToLengthMethod
        {
            [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void SubstringThrowsIfNotLongEnough()
            {
                string value = "hello";
                value.Substring(10);
            }

            [TestMethod]
            public void TrimToLengthCutsToDesiredLength4()
            {
                string value = "hello";
                var result = value.TrimToLength(4);
                Assert.AreEqual("hell", result);
            }

            [TestMethod]
            public void TrimToLengthCutsToDesiredLength5()
            {
                string value = "hello";
                var result = value.TrimToLength(5);
                Assert.AreEqual("hello", result);
            }

            [TestMethod]
            public void TrimToLengthCutsToDesiredLength6()
            {
                string value = "hello";
                var result = value.TrimToLength(6);
                Assert.AreEqual("hello", result);
            }

            [TestMethod]
            public void TrimToLengthCutsToDesiredLength7()
            {
                string value = "hello";
                var result = value.TrimToLength(10);
                Assert.AreEqual("hello", result);
            }
        }
    }
}
