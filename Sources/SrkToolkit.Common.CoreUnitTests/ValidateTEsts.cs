// -----------------------------------------------------------------------
// <copyright file="ValidateTEsts.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Common.Tests
{
    using SrkToolkit.Common.Validation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ValidateTEsts
    {
        public class EmailAddressMethod
        {
            [Fact]
            public void SimpleAddress()
            {
                string input = "antoine@gmail.com";
                var result = Validate.EmailAddress(input);
                Assert.Equal(input, result);
            }

            [Fact]
            public void Loweryfies()
            {
                string input = "Antoine@Gmail.com";
                string expected = "antoine@gmail.com";
                var result = Validate.EmailAddress(input);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void FirstAndLastNameAddress()
            {
                string input = "antoine.sottiau@gmail.com";
                var result = Validate.EmailAddress(input);
                Assert.Equal(input, result);
            }

            [Fact]
            public void FirstAndLastNameAndPlusAddress()
            {
                string input = "antoine.sottiau+something-special@gmail.com";
                var result = Validate.EmailAddress(input);
                Assert.Equal(input, result);
            }

            [Fact]
            public void NoAtSign()
            {
                string input = "Antoine.Gmail.com";
                string expected = null;
                var result = Validate.EmailAddress(input);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void EmailCharacterInternational()
            {
                string input = "testspecïalchar@toto.com";
                var result = Validate.EmailAddress(input);
                Assert.Equal(input, result);
            }

            [Fact]
            public void EmailCharacterSingleQuote()
            {
                string input = "testspec'alchar@toto.com";
                var result = Validate.EmailAddress(input);
                Assert.Equal(input, result);
            }
        }

        public class ManyEmailAddressesMethod
        {
            [Fact]
            public void ReturnsEmptyEnumerationOnNullInput()
            {
                string input = null;
                var result = Validate.ManyEmailAddresses(input).ToArray();
                Assert.Equal(0, result.Length);
            }

            [Fact]
            public void Works()
            {
                string input = "blah antoine.sottiau+something-special@gmail.com Antoine@Gmail.com xxxxxx";
                var result = Validate.ManyEmailAddresses(input).ToArray();
                Assert.Equal(2, result.Length);
                Assert.Equal("antoine.sottiau+something-special@gmail.com", result[0]);
                Assert.Equal("antoine@gmail.com", result[1]);
            }
        }

        public class PhoneNumberMethod
        {
            [Fact]
            public void ValidPerfectNumber()
            {
                string input = "+33123456789";
                string expected = input;
                string result = Validate.PhoneNumber(input);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void ValidPerfectTrimmedNumber()
            {
                string input = "\t+33123456789   \r\n ";
                string expected = "+33123456789";
                string result = Validate.PhoneNumber(input);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void ValidPerfectSpacedNumber()
            {
                string input = "+33 123 456 789";
                string expected = "+33123456789";
                string result = Validate.PhoneNumber(input);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void ValidPerfectSeparatedNumber()
            {
                string input = "+33 123/456-789.012";
                string expected = "+33123456789012";
                string result = Validate.PhoneNumber(input);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void ZeroZeroNumber()
            {
                string input = "0033123456789";
                string expected = "+33123456789";
                string result = Validate.PhoneNumber(input);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void NationalReturnsNull()
            {
                string input = "0123456789";
                string expected = null;
                string result = Validate.PhoneNumber(input);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void NationalAccepted()
            {
                string input = "0123456789";
                string expected = "0123456789";
                string result = Validate.PhoneNumber(input, true);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void NationalNonZeroAccepted()
            {
                string input = "123456789";
                string expected = "123456789";
                string result = Validate.PhoneNumber(input, true);
                Assert.Equal(expected, result);
            }

            [Fact]
            public void IntlWithOptionalZero()
            {
                string input = "+33 (0) 123456789";
                string expected = "+33123456789";
                string result = Validate.PhoneNumber(input);
                Assert.Equal(expected, result);
            }
        }
    }
}
