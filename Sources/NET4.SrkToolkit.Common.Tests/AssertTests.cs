
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AssertTests
    {
        [TestMethod]
        public void BothNull_Ok()
        {
            string expected = null;
            string actual = null;
            SrkToolkit.Testing.Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BothEmpty_Ok()
        {
            string expected = "";
            string actual = "";
            SrkToolkit.Testing.Assert.AreEqual(expected, actual);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void NullVsEmpty_Throws()
        {
            string expected = null;
            string actual = "";
            SrkToolkit.Testing.Assert.AreEqual(expected, actual);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void EmptyVsNull_Throws()
        {
            string expected = "";
            string actual = null;
            SrkToolkit.Testing.Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SameString_Ok()
        {
            string expected = "Tyuhn/*";
            string actual = "Tyuhn/*";
            SrkToolkit.Testing.Assert.AreEqual(expected, actual);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void ShorterResult_Ok()
        {
            string expected = "Tyuhn/*";
            string actual = "Tyuhn/";
            SrkToolkit.Testing.Assert.AreEqual(expected, actual);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void LongerResult_Ok()
        {
            string expected = "Tyuhn/*";
            string actual = "Tyuhn/*-";
            SrkToolkit.Testing.Assert.AreEqual(expected, actual);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void DiffAt0_Ok()
        {
            string expected = "Tyuhn/*";
            string actual = "7yuhn/*";
            SrkToolkit.Testing.Assert.AreEqual(expected, actual);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void DiffAt1_Ok()
        {
            string expected = "Tyuhn/*";
            string actual = "T0uhn/*";
            SrkToolkit.Testing.Assert.AreEqual(expected, actual);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void LongDiffAt15_Ok()
        {
            string expected = "aaaaaaaaaaaaaaaTyuhn/*";
            string actual = "aaaaaaaaaaaaaaa7yuhn/*";
            SrkToolkit.Testing.Assert.AreEqual(expected, actual);
        }
    }
}
