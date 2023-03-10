
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class AssertTests
    {
        public class AreEqualMethod
        {
            [Fact]
            public void BothNull_Ok()
            {
                string expected = null;
                string actual = null;
                SrkToolkit.Testing.Assert.AreEqual(expected, actual);
            }

            [Fact]
            public void BothEmpty_Ok()
            {
                string expected = "";
                string actual = "";
                SrkToolkit.Testing.Assert.AreEqual(expected, actual);
            }

            [Fact]
            public void NullVsEmpty_Throws()
            {
                string expected = null;
                string actual = "";
                Assert.Throws<ArgumentException>(() =>
                {
                    SrkToolkit.Testing.Assert.AreEqual(expected, actual);
                });
            }

            [Fact]
            public void EmptyVsNull_Throws()
            {
                string expected = "";
                string actual = null;
                Assert.Throws<ArgumentException>(() =>
                {
                    SrkToolkit.Testing.Assert.AreEqual(expected, actual);
                });
            }

            [Fact]
            public void SameString_Ok()
            {
                string expected = "Tyuhn/*";
                string actual = "Tyuhn/*";
                SrkToolkit.Testing.Assert.AreEqual(expected, actual);
            }

            [Fact]
            public void ShorterResult_Ok()
            {
                string expected = "Tyuhn/*";
                string actual = "Tyuhn/";
                Assert.Throws<ArgumentException>(() =>
                {
                    SrkToolkit.Testing.Assert.AreEqual(expected, actual);
                });
            }

            [Fact]
            public void LongerResult_Ok()
            {
                string expected = "Tyuhn/*";
                string actual = "Tyuhn/*-";
                Assert.Throws<ArgumentException>(() =>
                {
                    SrkToolkit.Testing.Assert.AreEqual(expected, actual);
                });
            }

            [Fact]
            public void DiffAt0_Ok()
            {
                string expected = "Tyuhn/*";
                string actual = "7yuhn/*";
                Assert.Throws<ArgumentException>(() =>
                {
                    SrkToolkit.Testing.Assert.AreEqual(expected, actual);
                });
            }

            [Fact]
            public void DiffAt1_Ok()
            {
                string expected = "Tyuhn/*";
                string actual = "T0uhn/*";
                Assert.Throws<ArgumentException>(() =>
                {
                    SrkToolkit.Testing.Assert.AreEqual(expected, actual);
                });
            }

            [Fact]
            public void LongDiffAt15_Ok()
            {
                string expected = "aaaaaaaaaaaaaaaTyuhn/*";
                string actual = "aaaaaaaaaaaaaaa7yuhn/*";
                Assert.Throws<ArgumentException>(() =>
                {
                    SrkToolkit.Testing.Assert.AreEqual(expected, actual);
                });
            }
        }

        public class ContainsMethod
        {
            [Fact]
            public void BothNull_Throws()
            {
                string expected = null;
                string actual = null;
                Assert.Throws<ArgumentException>(() =>
                {
                SrkToolkit.Testing.Assert.Contains(expected, actual);
                });
            }

            [Fact]
            public void BothEmpty_Throws()
            {
                string expected = "";
                string actual = "";
                Assert.Throws<ArgumentException>(() =>
                {
                SrkToolkit.Testing.Assert.Contains(expected, actual);
                });
            }

            [Fact]
            public void NullVsEmpty_Throws()
            {
                string expected = null;
                string actual = "";
                Assert.Throws<ArgumentException>(() =>
                {
                SrkToolkit.Testing.Assert.Contains(expected, actual);
                });
            }

            [Fact]
            public void EmptyVsNull_Throws()
            {
                string expected = "";
                string actual = null;
                Assert.Throws<ArgumentException>(() =>
                {
                SrkToolkit.Testing.Assert.Contains(expected, actual);
                });
            }

            [Fact]
            public void SameString_Ok()
            {
                string expected = "Tyuhn/*";
                string actual = "Tyuhn/*";
                SrkToolkit.Testing.Assert.Contains(expected, actual);
            }

            [Fact]
            public void Contains_Ok()
            {
                string expected = "Tyuhn/*";
                string actual = "xxxTyuhn/*yyy";
                SrkToolkit.Testing.Assert.Contains(expected, actual);
            }

            [Fact]
            public void DoesNotContain_Ok()
            {
                string expected = "ioioioi";
                string actual = "xxxTyuhn/*yyy";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    throw new InvalidOperationException("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal("Searched string <"+expected+"> was not found.", ex.Message);
                }
            }

            [Fact]
            public void PartialContains2In3_Longer_Throws()
            {
                string expected = "rty";
                string actual = "ert_______";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    throw new InvalidOperationException("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal("Searched string was not found. Best partial match:\r\nV> ert_______\r\nS> -rty\r\n    ^^", ex.Message);
                }
            }

            [Fact]
            public void PartialContains2In3_Shorter_Throws()
            {
                string expected = "rty";
                string actual = "ert";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> ert\r\nS> -rty\r\n    ^^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    throw new InvalidOperationException("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal(expectedMessage, ex.Message);
                }
            }

            [Fact]
            public void PartialContains2In3Prefixed_Shorter_Throws()
            {
                string expected = "rty";
                string actual = "__________________________ert";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> _______ert\r\nS> --------rty\r\n           ^^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    throw new InvalidOperationException("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal(expectedMessage, ex.Message);
                }
            }

            [Fact]
            public void PartialContains2In3Prefixed_Longer_Throws()
            {
                string expected = "rty";
                string actual = "__________________________ert_______";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> _______ert_______\r\nS> --------rty\r\n           ^^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    throw new InvalidOperationException("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal(expectedMessage, ex.Message);
                }
            }

            [Fact]
            public void PartialContains2In3Prefixed_MuchLonger_Throws()
            {
                string expected = "rty";
                string actual = "__________________________ert______________________________________________";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> _______ert_____________________________\r\nS> --------rty\r\n           ^^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    throw new InvalidOperationException("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal(expectedMessage, ex.Message);
                }
            }

            [Fact]
            public void PartialContains3In4_Longer_Throws()
            {
                string expected = "rtyu";
                string actual = "erty_______";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> erty_______\r\nS> -rtyu\r\n    ^--^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    throw new InvalidOperationException("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal(expectedMessage, ex.Message);
                }
            }

            [Fact]
            public void PartialContains3In4_Shorter_Throws()
            {
                string expected = "rtyu";
                string actual = "erty";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> erty\r\nS> -rtyu\r\n    ^--^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    throw new InvalidOperationException("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal(expectedMessage, ex.Message);
                }
            }

            [Fact]
            public void PartialContains3In4Prefixed_Shorter_Throws()
            {
                string expected = "rtyu";
                string actual = "__________________________erty";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> ______erty\r\nS> -------rtyu\r\n          ^--^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    throw new InvalidOperationException("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal(expectedMessage, ex.Message);
                }
            }

            [Fact]
            public void PartialContains3In4Prefixed_Longer_Throws()
            {
                string expected = "rtyu";
                string actual = "__________________________erty_______";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> ______erty_______\r\nS> -------rtyu\r\n          ^--^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    throw new InvalidOperationException("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal(expectedMessage, ex.Message);
                }
            }

            [Fact]
            public void PartialContains3In4Prefixed_MuchLonger_Throws()
            {
                string expected = "rtyu";
                string actual = "__________________________erty______________________________________________";
                string expectedMessage = "Searched string was not found. Best partial match:\r\nV> ______erty______________________________\r\nS> -------rtyu\r\n          ^--^";
                try
                {
                    SrkToolkit.Testing.Assert.Contains(expected, actual);
                    throw new InvalidOperationException("Expected exception");
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal(expectedMessage, ex.Message);
                }
            }
        }
    }
}
