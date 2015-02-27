
namespace SrkToolkit.Common.Tests.DataAnnotations
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.DataAnnotations;

    [TestClass]
    public class PhoneNumberAttributeTests
    {
        [TestClass]
        public class RegexTests
        {
            [TestMethod]
            public void StupidInputFails()
            {
                // prepare
                string input = "hey hi";
                var target = new Regex(PhoneNumberAttribute.PhoneRegex);
                bool result;

                // execute
                result = target.IsMatch(input);

                // verify
                Assert.IsFalse(result, "Should not allow stupid input");
            }

            [TestMethod]
            public void LocalNumberFails()
            {
                // prepare
                string input = "0123456789";
                var target = new Regex(PhoneNumberAttribute.PhoneRegex);
                bool result;

                // execute
                result = target.IsMatch(input);

                // verify
                Assert.IsFalse(result, "Should not allow local numbers");
            }

            [TestMethod]
            public void ValidNumberAreAllowed()
            {
                // prepare
                string input = "+33123456789";
                var target = new Regex(PhoneNumberAttribute.PhoneRegex);
                bool result;

                // execute
                result = target.IsMatch(input);

                // verify
                Assert.IsTrue(result, "Valid numbers should pass");
            }

            [TestMethod]
            public void SeparatorsAreAllowed()
            {
                // prepare
                string input = "+33 123.45(6-789) 123";
                var target = new Regex(PhoneNumberAttribute.PhoneRegex);
                bool result;

                // execute
                result = target.IsMatch(input);

                // verify
                Assert.IsTrue(result, "Separators should pass validation");
            }
        }

        [TestClass]
        public class ConvertNationalToInternationalMethod
        {
            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void EmptyIsInvalid()
            {
                string input = "", output = null;
                CultureInfo culture = CultureInfo.InvariantCulture;
                Assert.IsFalse(PhoneNumberAttribute.ConvertNationalToInternational(input, culture, out output));
                Assert.IsNull(output);
            }

            [TestMethod]
            public void InternationalBasicWorks()
            {
                string input = "+33123456789", output = null;
                CultureInfo culture = CultureInfo.InvariantCulture;
                Assert.IsTrue(PhoneNumberAttribute.ConvertNationalToInternational(input, culture, out output));
                Assert.AreEqual(input, output);
            }

            [TestMethod]
            public void InternationalSpacedWorks()
            {
                string input = "+33 123 456 789", output = null;
                CultureInfo culture = CultureInfo.InvariantCulture;
                Assert.IsTrue(PhoneNumberAttribute.ConvertNationalToInternational(input, culture, out output));
                Assert.AreEqual(input, output);
            }

            [TestMethod]
            public void InternationalDashedWorks()
            {
                string input = "+33-123-456-789", output = null;
                CultureInfo culture = CultureInfo.InvariantCulture;
                Assert.IsTrue(PhoneNumberAttribute.ConvertNationalToInternational(input, culture, out output));
                Assert.AreEqual(input, output);
            }

            [TestMethod]
            public void InternationalDottedWorks()
            {
                string input = "+33.123.456.789", output = null;
                CultureInfo culture = CultureInfo.InvariantCulture;
                Assert.IsTrue(PhoneNumberAttribute.ConvertNationalToInternational(input, culture, out output));
                Assert.AreEqual(input, output);
            }

            [TestMethod, ExpectedException(typeof(InvalidOperationException))]
            public void NationalInvariantFails()
            {
                string input = "123.456.789", output = null;
                CultureInfo culture = CultureInfo.InvariantCulture;
                Assert.IsFalse(PhoneNumberAttribute.ConvertNationalToInternational(input, culture, out output));
                Assert.IsNull(output);
            }
        }
    }
}
