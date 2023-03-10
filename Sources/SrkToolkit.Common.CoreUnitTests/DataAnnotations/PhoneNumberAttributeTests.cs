
namespace SrkToolkit.Common.Tests.DataAnnotations
{
    using SrkToolkit.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.ComponentModel.DataAnnotations;
    using Xunit;

    public class PhoneNumberAttributeTests
    {
        public class RegexTests
        {
            [Fact]
            public void StupidInputFails()
            {
                // prepare
                string input = "hey hi";
                var target = new Regex(PhoneNumberAttribute.PhoneRegex);
                bool result;

                // execute
                result = target.IsMatch(input);

                // verify
                Assert.False(result, "Should not allow stupid input");
            }

            [Fact]
            public void LocalNumberFails()
            {
                // prepare
                string input = "0123456789";
                var target = new Regex(PhoneNumberAttribute.PhoneRegex);
                bool result;

                // execute
                result = target.IsMatch(input);

                // verify
                Assert.False(result, "Should not allow local numbers");
            }

            [Fact]
            public void ValidNumberAreAllowed()
            {
                // prepare
                string input = "+33123456789";
                var target = new Regex(PhoneNumberAttribute.PhoneRegex);
                bool result;

                // execute
                result = target.IsMatch(input);

                // verify
                Assert.True(result, "Valid numbers should pass");
            }

            [Fact]
            public void SeparatorsAreAllowed()
            {
                // prepare
                string input = "+33 123.45(6-789) 123";
                var target = new Regex(PhoneNumberAttribute.PhoneRegex);
                bool result;

                // execute
                result = target.IsMatch(input);

                // verify
                Assert.True(result, "Separators should pass validation");
            }
        }

        public class ConvertNationalToInternationalMethod
        {
            [Fact]
            public void EmptyIsInvalid()
            {
                string input = "", output = null;
                CultureInfo culture = CultureInfo.InvariantCulture;
                Assert.Throws<ArgumentException>(() =>
                {
                    Assert.False(PhoneNumberAttribute.ConvertNationalToInternational(input, culture, out output));
                    Assert.Null(output);
                });
            }

            [Fact]
            public void InternationalBasicWorks()
            {
                string input = "+33123456789", output = null;
                CultureInfo culture = CultureInfo.InvariantCulture;
                Assert.True(PhoneNumberAttribute.ConvertNationalToInternational(input, culture, out output));
                Assert.Equal(input, output);
            }

            [Fact]
            public void InternationalSpacedWorks()
            {
                string input = "+33 123 456 789", output = null;
                CultureInfo culture = CultureInfo.InvariantCulture;
                Assert.True(PhoneNumberAttribute.ConvertNationalToInternational(input, culture, out output));
                Assert.Equal(input, output);
            }

            [Fact]
            public void InternationalDashedWorks()
            {
                string input = "+33-123-456-789", output = null;
                CultureInfo culture = CultureInfo.InvariantCulture;
                Assert.True(PhoneNumberAttribute.ConvertNationalToInternational(input, culture, out output));
                Assert.Equal(input, output);
            }

            [Fact]
            public void InternationalDottedWorks()
            {
                string input = "+33.123.456.789", output = null;
                CultureInfo culture = CultureInfo.InvariantCulture;
                Assert.True(PhoneNumberAttribute.ConvertNationalToInternational(input, culture, out output));
                Assert.Equal(input, output);
            }

            [Fact]
            public void NationalInvariantFails()
            {
                string input = "123.456.789", output = null;
                CultureInfo culture = CultureInfo.InvariantCulture;
                Assert.Throws<InvalidOperationException>(() =>
                {
                    Assert.False(PhoneNumberAttribute.ConvertNationalToInternational(input, culture, out output));
                    Assert.Null(output);
                });
            }
        }

        [Fact]
        public void ReturnsTrueWithEmptyValue()
        {
            var attr = new PhoneNumberAttribute();
            string value = string.Empty;
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReturnsTrueWithNullValue()
        {
            var attr = new PhoneNumberAttribute();
            string value = null;
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReturnsTrueWithValidValue()
        {
            var attr = new PhoneNumberAttribute();
            string value = "+33123456798";
            bool expected = true;
            bool result = attr.IsValid(value);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReturnsFalseWithObviousInvalid()
        {
            var attr = new PhoneNumberAttribute();
            string value = "foobar", name = "PhoneNumber";
            var context = new ValidationContext(new object(), null, null);
            context.MemberName = name;
            var result = attr.GetValidationResult(value, context);
            Assert.NotNull(result);
            Assert.Equal("Use the international phone number format (+xxyyyyyyyyyy).", result.ErrorMessage);
            Assert.Equal(1, result.MemberNames.Count());
            Assert.Equal(name, result.MemberNames.Single());
        }
    }
}
