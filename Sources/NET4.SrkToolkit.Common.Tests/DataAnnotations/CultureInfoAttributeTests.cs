
namespace SrkToolkit.Common.Tests.DataAnnotations
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.DataAnnotations;
    using System.ComponentModel.DataAnnotations;

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
            string value = "fr", name = "Culture";
            var context = new ValidationContext(new object(), null, null);
            context.MemberName = name;
            var result = attr.GetValidationResult(value, context);
            Assert.IsNotNull(result);
            Assert.AreEqual("The culture is not valid.", result.ErrorMessage);
            Assert.AreEqual(1, result.MemberNames.Count());
            Assert.AreEqual(name, result.MemberNames.Single());
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
