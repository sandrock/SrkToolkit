
namespace SrkToolkit.Common.Tests.DataAnnotations
{
    using SrkToolkit.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.ComponentModel.DataAnnotations;
    using Xunit;

    public class CultureInfoAttributeTests
    {
        [Fact]
        public void ReturnsTrueWithEmptyValue()
        {
            var attr = new CultureInfoAttribute();
            string value = string.Empty;
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReturnsTrueWithNullValue()
        {
            var attr = new CultureInfoAttribute();
            string value = null;
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReturnsTrueWithValidValue()
        {
            var attr = new CultureInfoAttribute();
            string value = "fr-FR";
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AllowNeutral_ReturnsTrueWithNeutral()
        {
            var attr = new CultureInfoAttribute();
            attr.AllowNeutralCulture = true;
            string value = "fr";
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AllowNeutral_ReturnsTrueWithNonNeutral()
        {
            var attr = new CultureInfoAttribute();
            attr.AllowNeutralCulture = true;
            string value = "fr-CA";
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DisallowNeutral_ReturnsFalseWithNeutral()
        {
            var attr = new CultureInfoAttribute();
            attr.AllowNeutralCulture = false;
            string value = "fr", name = "Culture";
            var context = new ValidationContext(new object(), null, null);
            context.MemberName = name;
            var result = attr.GetValidationResult(value, context);
            Assert.NotNull(result);
            Assert.Equal("The culture is not valid.", result.ErrorMessage);
            Assert.Equal(1, result.MemberNames.Count());
            Assert.Equal(name, result.MemberNames.Single());
        }

        [Fact]
        public void DisallowNeutral_ReturnsTrueWithNonNeutral()
        {
            var attr = new CultureInfoAttribute();
            attr.AllowNeutralCulture = false;
            string value = "fr-CA";
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DoNotAllowNeutralByDefault()
        {
            var attr = new CultureInfoAttribute();
            Assert.False(attr.AllowNeutralCulture);
        }
    }
}
