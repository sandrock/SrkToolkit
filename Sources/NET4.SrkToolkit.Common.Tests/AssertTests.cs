
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class AssertTests
    {
        [TestClass]
        public class AreEqualMethod
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

        [TestClass]
        public class ContainsMethod
        {
            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void BothNull_Throws()
            {
                string expected = null;
                string actual = null;
                SrkToolkit.Testing.Assert.Contains(expected, actual);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void BothEmpty_Throws()
            {
                string expected = "";
                string actual = "";
                SrkToolkit.Testing.Assert.Contains(expected, actual);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void NullVsEmpty_Throws()
            {
                string expected = null;
                string actual = "";
                SrkToolkit.Testing.Assert.Contains(expected, actual);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void EmptyVsNull_Throws()
            {
                string expected = "";
                string actual = null;
                SrkToolkit.Testing.Assert.Contains(expected, actual);
            }

            [TestMethod]
            public void SameString_Ok()
            {
                string expected = "Tyuhn/*";
                string actual = "Tyuhn/*";
                SrkToolkit.Testing.Assert.Contains(expected, actual);
            }

            [TestMethod]
            public void Contains_Ok()
            {
                string expected = "Tyuhn/*";
                string actual = "xxxTyuhn/*yyy";
                SrkToolkit.Testing.Assert.Contains(expected, actual);
            }

            [TestMethod]
            public void DoesNotContain_Ok()
            {
                string expected = "ioioioi";
                string actual = "xxxTyuhn/*yyy";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    Assert.Fail("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.AreEqual("Searched string <"+expected+"> was not found.", ex.Message);
                }
            }

            [TestMethod]
            public void PartialContains2In3_Longer_Throws()
            {
                string expected = "rty";
                string actual = "ert_______";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    Assert.Fail("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.AreEqual("Searched string was not found. Best partial match:\r\nV> ert_______\r\nS> -rty\r\n    ^^", ex.Message);
                }
            }

            [TestMethod]
            public void PartialContains2In3_Shorter_Throws()
            {
                string expected = "rty";
                string actual = "ert";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> ert\r\nS> -rty\r\n    ^^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    Assert.Fail("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.AreEqual(expectedMessage, ex.Message);
                }
            }

            [TestMethod]
            public void PartialContains2In3Prefixed_Shorter_Throws()
            {
                string expected = "rty";
                string actual = "__________________________ert";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> _______ert\r\nS> --------rty\r\n           ^^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    Assert.Fail("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.AreEqual(expectedMessage, ex.Message);
                }
            }

            [TestMethod]
            public void PartialContains2In3Prefixed_Longer_Throws()
            {
                string expected = "rty";
                string actual = "__________________________ert_______";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> _______ert_______\r\nS> --------rty\r\n           ^^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    Assert.Fail("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.AreEqual(expectedMessage, ex.Message);
                }
            }

            [TestMethod]
            public void PartialContains2In3Prefixed_MuchLonger_Throws()
            {
                string expected = "rty";
                string actual = "__________________________ert______________________________________________";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> _______ert_____________________________\r\nS> --------rty\r\n           ^^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    Assert.Fail("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.AreEqual(expectedMessage, ex.Message);
                }
            }

            [TestMethod]
            public void PartialContains3In4_Longer_Throws()
            {
                string expected = "rtyu";
                string actual = "erty_______";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> erty_______\r\nS> -rtyu\r\n    ^--^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    Assert.Fail("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.AreEqual(expectedMessage, ex.Message);
                }
            }

            [TestMethod]
            public void PartialContains3In4_Shorter_Throws()
            {
                string expected = "rtyu";
                string actual = "erty";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> erty\r\nS> -rtyu\r\n    ^--^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    Assert.Fail("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.AreEqual(expectedMessage, ex.Message);
                }
            }

            [TestMethod]
            public void PartialContains3In4Prefixed_Shorter_Throws()
            {
                string expected = "rtyu";
                string actual = "__________________________erty";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> ______erty\r\nS> -------rtyu\r\n          ^--^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    Assert.Fail("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.AreEqual(expectedMessage, ex.Message);
                }
            }

            [TestMethod]
            public void PartialContains3In4Prefixed_Longer_Throws()
            {
                string expected = "rtyu";
                string actual = "__________________________erty_______";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> ______erty_______\r\nS> -------rtyu\r\n          ^--^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    Assert.Fail("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.AreEqual(expectedMessage, ex.Message);
                }
            }

            [TestMethod]
            public void PartialContains3In4Prefixed_MuchLonger_Throws()
            {
                string expected = "rtyu";
                string actual = "__________________________erty______________________________________________";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> ______erty______________________________\r\nS> -------rtyu\r\n          ^--^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    Assert.Fail("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.AreEqual(expectedMessage, ex.Message);
                }
            }
        }
    }
}
