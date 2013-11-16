
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
    }
}
