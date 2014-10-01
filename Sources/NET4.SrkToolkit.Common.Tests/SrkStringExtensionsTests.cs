
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

        [TestClass]
        public class ContainsAny
        {
            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfArg0IsNull()
            {
                SrkStringExtensions.ContainsAny(null, new string[0], StringComparison.CurrentCulture);
            }

            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfArg1IsNull()
            {
                SrkStringExtensions.ContainsAny(string.Empty, null, StringComparison.CurrentCulture);
            }

            [TestMethod]
            public void FalseIsSourceIsEmpty()
            {
                string source = string.Empty;
                bool result = SrkStringExtensions.ContainsAny(source, new string[] { string.Empty, }, StringComparison.CurrentCulture);
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void Finds1()
            {
                string source = "hello world";
                var values = new string[] { "hello", "guys", };
                bool result = SrkStringExtensions.ContainsAny(source, values, StringComparison.InvariantCulture);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void Finds2()
            {
                string source = "hello world";
                var values = new string[] { "bye", "world", };
                bool result = SrkStringExtensions.ContainsAny(source, values, StringComparison.InvariantCulture);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void Finds1CaseMatch()
            {
                string source = "Hello World";
                var values = new string[] { "hello", "guys", };
                bool result = SrkStringExtensions.ContainsAny(source, values, StringComparison.InvariantCulture);
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void Finds1CaseIgnore()
            {
                string source = "Hello World";
                var values = new string[] { "hello", "guys", };
                bool result = SrkStringExtensions.ContainsAny(source, values, StringComparison.InvariantCultureIgnoreCase);
                Assert.IsTrue(result);
            }
        }
    }
}
