
namespace SrkToolkit.Common.Tests.DataAnnotations
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.DataAnnotations;

    [TestClass]
    public class CultureInfoAttributeTests
    {
        [TestMethod]
        public void ReturnsTrueWithEmptyValue()
        {
            var attr = new CultureInfoAttribute();
            string value = string.Empty;
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ReturnsTrueWithNullValue()
        {
            var attr = new CultureInfoAttribute();
            string value = null;
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ReturnsTrueWithValidValue()
        {
            var attr = new CultureInfoAttribute();
            string value = "fr-FR";
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AllowNeutral_ReturnsTrueWithNeutral()
        {
            var attr = new CultureInfoAttribute();
            attr.AllowNeutralCulture = true;
            string value = "fr";
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AllowNeutral_ReturnsTrueWithNonNeutral()
        {
            var attr = new CultureInfoAttribute();
            attr.AllowNeutralCulture = true;
            string value = "fr-CA";
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DisallowNeutral_ReturnsFalseWithNeutral()
        {
            var attr = new CultureInfoAttribute();
            attr.AllowNeutralCulture = false;
            string value = "fr";
            bool expected = false;
            bool result = attr.IsValid(value);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DisallowNeutral_ReturnsTrueWithNonNeutral()
        {
            var attr = new CultureInfoAttribute();
            attr.AllowNeutralCulture = false;
            string value = "fr-CA";
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DoNotAllowNeutralByDefault()
        {
            var attr = new CultureInfoAttribute();
            Assert.IsFalse(attr.AllowNeutralCulture);
        }
    }
}
