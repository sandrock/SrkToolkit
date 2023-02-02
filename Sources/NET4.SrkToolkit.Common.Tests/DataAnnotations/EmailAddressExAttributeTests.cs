﻿
namespace SrkToolkit.Common.Tests.DataAnnotations
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [TestClass]
    public class EmailAddressExAttributeTests
    {
#if NET40
#else
#endif

#if NET40
        private static EmailAddressAttribute GetTarget()
        {
            return new EmailAddressAttribute();
        }
#else
        private static EmailAddressExAttribute GetTarget()
        {
            return new EmailAddressExAttribute();
        }
#endif

        [TestMethod]
        public void AllowOne_NullValue_Pass()
        {
            string input = null;
            var target = GetTarget();
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowOne_EmptyValue_Pass()
        {
            string input = string.Empty;
            var target = GetTarget();
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowOne_WhiteValue_Pass()
        {
            string input = "    ";
            var target = GetTarget();
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowOne_BasicAddress_Pass()
        {
            string input = "michel.salamander@yahoooo.com";
            var target = GetTarget();
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowOne_BasicAddressWithTag_Pass()
        {
            string input = "michel.salamander+hello-world@yahoooo.com";
            var target = GetTarget();
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowOne_BasicAddressWithDiacritics_Pass()
        {
            string input = "michél.salamânder@yahoooo.com";
            var target = GetTarget();
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowOne_InvalidAddress1_Fail()
        {
            string input = "michél salamânder@yahoooo.com";
            var target = GetTarget();
            var result = target.IsValid(input);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AllowOne_InvalidAddress2_Fail()
        {
            string input = "michel salamander@yahoooo.com";
            var target = GetTarget();
            var result = target.IsValid(input);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AllowOne_InvalidAddress3_Fail()
        {
            string input = "michel.salamander.yahoooo.com";
            var target = GetTarget();
            var result = target.IsValid(input);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AllowOne_InvalidAddress4_Fail()
        {
            string input = "hello";
            var target = GetTarget();
            var result = target.IsValid(input);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AllowOne_InvalidAddress5_Fail()
        {
            string input = ".@test.com";
            var target = GetTarget();
            var result = target.IsValid(input);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AllowOne_BasicAddressUntrimmed_Fail()
        {
            string input = "  michel.salamander@yahoooo.com  ";
            var target = GetTarget();
            var result = target.IsValid(input);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AllowMultiple_NullValue_Pass()
        {
            string input = null;
            var target = GetTarget();
            target.AllowMultiple = true;
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowMultiple_EmptyValue_Pass()
        {
            string input = string.Empty;
            var target = GetTarget();
            target.AllowMultiple = true;
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowMultiple_WhiteValue_Pass()
        {
            string input = "    ";
            var target = GetTarget();
            target.AllowMultiple = true;
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowMultiple_BasicAddress1_Pass()
        {
            string input = "michel.salamander@yahoooo.com";
            var target = GetTarget();
            target.AllowMultiple = true;
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowMultiple_BasicAddress2_Pass()
        {
            string input = "michel.salamander@yahoooo.com michel.salamander@yahoooo.com";
            var target = GetTarget();
            target.AllowMultiple = true;
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowMultiple_BasicAddress2Untrimmed_Pass()
        {
            string input = "  michel.salamander@yahoooo.com \r\nmichel.salamander@yahoooo.com  ";
            var target = GetTarget();
            target.AllowMultiple = true;
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowMultiple_BelowMinimum_Fail()
        {
            string input = "michel.salamander@yahoooo.com";
            var target = GetTarget();
            target.AllowMultiple = true;
            target.MinimumAddresses = 2;
            var result = target.IsValid(input);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AllowMultiple_AtMinimum_Pass()
        {
            string input = "  michel.salamander@yahoooo.com \r\nmichel.salamander@yahoooo.com  ";
            var target = GetTarget();
            target.AllowMultiple = true;
            target.MinimumAddresses = 2;
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowMultiple_AboveMinimum_Pass()
        {
            string input = "  michel.salamander@yahoooo.com \r\nmichel.salamander@yahoooo.com test@test.com";
            var target = GetTarget();
            target.AllowMultiple = true;
            target.MinimumAddresses = 2;
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowMultiple_AboveMaximum_Fail()
        {
            string input = "  michel.salamander@yahoooo.com \r\nmichel.salamander@yahoooo.com test@test.com";
            var target = GetTarget();
            target.AllowMultiple = true;
            target.MaximumAddresses = 2;
            var result = target.IsValid(input);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AllowMultiple_AtMaximum_Pass()
        {
            string input = "  michel.salamander@yahoooo.com \r\nmichel.salamander@yahoooo.com  ";
            var target = GetTarget();
            target.AllowMultiple = true;
            target.MaximumAddresses = 2;
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AllowMultiple_BelowMaximum_Pass()
        {
            string input = "michel.salamander@yahoooo.com";
            var target = GetTarget();
            target.AllowMultiple = true;
            target.MaximumAddresses = 2;
            var result = target.IsValid(input);
            Assert.IsTrue(result);
        }
    }
}
