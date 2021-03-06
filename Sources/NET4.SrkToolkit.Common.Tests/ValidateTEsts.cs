﻿// -----------------------------------------------------------------------
// <copyright file="ValidateTEsts.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.Common.Validation;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ValidateTEsts
    {
        [TestClass]
        public class EmailAddressMethod
        {
            [TestMethod]
            public void SimpleAddress()
            {
                string input = "antoine@gmail.com";
                var result = Validate.EmailAddress(input);
                Assert.AreEqual(input, result);
            }

            [TestMethod]
            public void Loweryfies()
            {
                string input = "Antoine@Gmail.com";
                string expected = "antoine@gmail.com";
                var result = Validate.EmailAddress(input);
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void FirstAndLastNameAddress()
            {
                string input = "antoine.sottiau@gmail.com";
                var result = Validate.EmailAddress(input);
                Assert.AreEqual(input, result);
            }

            [TestMethod]
            public void FirstAndLastNameAndPlusAddress()
            {
                string input = "antoine.sottiau+something-special@gmail.com";
                var result = Validate.EmailAddress(input);
                Assert.AreEqual(input, result);
            }

            [TestMethod]
            public void NoAtSign()
            {
                string input = "Antoine.Gmail.com";
                string expected = null;
                var result = Validate.EmailAddress(input);
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void EmailCharacterInternational()
            {
                string input = "testspecïalchar@toto.com";
                var result = Validate.EmailAddress(input);
                Assert.AreEqual(input, result);
            }

            [TestMethod]
            public void EmailCharacterSingleQuote()
            {
                string input = "testspec'alchar@toto.com";
                var result = Validate.EmailAddress(input);
                Assert.AreEqual(input, result);
            }
        }

        [TestClass]
        public class ManyEmailAddressesMethod
        {
            [TestMethod]
            public void ReturnsEmptyEnumerationOnNullInput()
            {
                string input = null;
                var result = Validate.ManyEmailAddresses(input).ToArray();
                Assert.AreEqual(0, result.Length);
            }

            [TestMethod]
            public void Works()
            {
                string input = "blah antoine.sottiau+something-special@gmail.com Antoine@Gmail.com xxxxxx";
                var result = Validate.ManyEmailAddresses(input).ToArray();
                Assert.AreEqual(2, result.Length);
                Assert.AreEqual("antoine.sottiau+something-special@gmail.com", result[0]);
                Assert.AreEqual("antoine@gmail.com", result[1]);
            }
        }

        [TestClass]
        public class PhoneNumberMethod
        {
            [TestMethod]
            public void ValidPerfectNumber()
            {
                string input = "+33123456789";
                string expected = input;
                string result = Validate.PhoneNumber(input);
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void ValidPerfectTrimmedNumber()
            {
                string input = "\t+33123456789   \r\n ";
                string expected = "+33123456789";
                string result = Validate.PhoneNumber(input);
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void ValidPerfectSpacedNumber()
            {
                string input = "+33 123 456 789";
                string expected = "+33123456789";
                string result = Validate.PhoneNumber(input);
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void ValidPerfectSeparatedNumber()
            {
                string input = "+33 123/456-789.012";
                string expected = "+33123456789012";
                string result = Validate.PhoneNumber(input);
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void ZeroZeroNumber()
            {
                string input = "0033123456789";
                string expected = "+33123456789";
                string result = Validate.PhoneNumber(input);
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void NationalReturnsNull()
            {
                string input = "0123456789";
                string expected = null;
                string result = Validate.PhoneNumber(input);
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void NationalAccepted()
            {
                string input = "0123456789";
                string expected = "0123456789";
                string result = Validate.PhoneNumber(input, true);
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void NationalNonZeroAccepted()
            {
                string input = "123456789";
                string expected = "123456789";
                string result = Validate.PhoneNumber(input, true);
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void IntlWithOptionalZero()
            {
                string input = "+33 (0) 123456789";
                string expected = "+33123456789";
                string result = Validate.PhoneNumber(input);
                Assert.AreEqual(expected, result);
            }
        }
    }
}
