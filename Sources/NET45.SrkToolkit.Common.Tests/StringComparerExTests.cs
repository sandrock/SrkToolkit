
namespace SrkToolkit.Common.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    [TestClass]
    public class StringComparerExTests
    {
        [TestMethod]
        public void StringComparerEx_AccentedStringAreEqual1()
        {
            string a = "écoûtèr", b = "ëcöutêr";
            var stringComparer = new StringComparerEx(CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
            bool result = stringComparer.Equals(a, b);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void StringComparerEx_AccentedStringAreEqual2()
        {
            string a = "écoûtèr", b = "écoûtèr";
            var stringComparer = new StringComparerEx(CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
            bool result = stringComparer.Equals(a, b);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void StringComparerEx_AccentedStringAreEqual3()
        {
            string a = "écoûtèr", b = "ecouter";
            var stringComparer = new StringComparerEx(CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
            bool result = stringComparer.Equals(a, b);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void StringComparerEx_DifferentStringAreDifferent()
        {
            string a = "acoutur", b = "ëcöutêr";
            var stringComparer = new StringComparerEx(CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
            bool result = stringComparer.Equals(a, b);
            Assert.IsFalse(result);
        }
    }
}
