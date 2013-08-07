// -----------------------------------------------------------------------
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
        }
    }
}
