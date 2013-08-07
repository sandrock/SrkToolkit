
namespace SrkToolkit.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Globalization;
    using SrkToolkit.Web.Mvc;
    using System.Web.Mvc;

    public class DecimalModelBinderTests
    {
        [TestClass]
        public class ConvertFrenchStyleDecimals
        {
            static List<Expectation<double>> doubleExpectations;

            static ConvertFrenchStyleDecimals()
            {
                doubleExpectations = new List<Expectation<double>>()
                {
                    // both ',' and '.' -> one of them alone is the decimal separator
                    new Expectation<double>("1,123.456", C("en-US"), 1123.456),
                    new Expectation<double>("1,123.456", C("fr-FR"), 1123.456),

                    // both ',' and '.' -> one of them alone is the decimal separator
                    new Expectation<double>("1.123,456", C("en-US"), 1.123456),
                    new Expectation<double>("1.123,456", C("fr-FR"), 1123.456),
                    
                    // both ',' and '.' -> one of them alone is the decimal separator
                    new Expectation<double>("1,222,123.456", C("en-US"), 1222123.456),
                    new Expectation<double>("1,222,123.456", C("fr-FR"), 1222123.456),

                    // both ',' and '.' -> one of them alone is the decimal separator
                    new Expectation<double>("1.222.123,456", C("en-US"), 1222123.456),
                    new Expectation<double>("1.222.123,456", C("fr-FR"), 1222123.456),

                    // both ' ' and '.' -> . separates
                    new Expectation<double>("1 123.456", C("en-US"), 1123.456),
                    new Expectation<double>("1 123.456", C("fr-FR"), 1123.456),

                    // both ' ' and ',' -> both useless
                    new Expectation<double>("1 123,456", C("en-US"), 1123456),
                    // both ' ' and ',' -> ',' is the dec-sep
                    new Expectation<double>("1 123,456", C("fr-FR"), 1123.456),

                    // only ' ' -> useless
                    new Expectation<double>("123 456", C("en-US"), 123456),
                    new Expectation<double>("123 456", C("fr-FR"), 123456),

                    // only '.' -> . separates
                    new Expectation<double>("123.456", C("en-US"), 123.456),
                    new Expectation<double>("123.456", C("fr-FR"), 123.456),

                    // only ',' -> useless
                    new Expectation<double>("123,456", C("en-US"), 123456),
                    // only ',' -> ',' is the dec-sep
                    new Expectation<double>("123,456", C("fr-FR"), 123.456),
                };
            }

            public static CultureInfo C(string name)
            {
                return new CultureInfo(name);
            }

            [TestMethod]
            public void DoubleNotFrenchStyle()
            {
                var target = new DecimalModelBinder<double>();
                foreach (var expectation in doubleExpectations.Where(c => c.Culture.NumberFormat.NumberDecimalSeparator == "."))
                {
                    object value;
                    var valueProvider = new ValueProviderResult(expectation.Input, expectation.Input, expectation.Culture);
                    var result = target.BindModelImpl(valueProvider, out value);
                    Assert.AreEqual(expectation.Expected, value, "In: '" + expectation.Input + "' " + expectation.Culture.Name);
                }
            }

            [TestMethod]
            public void DoubleIsFrenchStyle()
            {
                var target = new DecimalModelBinder<double>();
                foreach (var expectation in doubleExpectations.Where(c => c.Culture.NumberFormat.NumberDecimalSeparator == ","))
                {
                    object value;
                    var valueProvider = new ValueProviderResult(expectation.Input, expectation.Input, expectation.Culture);
                    var result = target.BindModelImpl(valueProvider, out value);
                    Assert.AreEqual(expectation.Expected, value, "In: '" + expectation.Input + "' " + expectation.Culture.Name);
                }
            }

            [TestMethod]
            public void DoubleBug1()
            {
                // typical bug we don't want to see
                string input = "1.2";
                var culture = C("fr-FR");
                double expected = 1.2D; // instead of validation error

                var target = new DecimalModelBinder<double>();
                object value;
                var valueProvider = new ValueProviderResult(input, input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.AreEqual(expected, value);
                Assert.AreEqual(0, result.Errors.Count);
            }

            [TestMethod]
            public void DoubleNotNullable()
            {
                // typical bug we don't want to see
                string input = "";
                var culture = C("fr-FR");
                ////double expected = 1.2D; // instead of validation error

                var target = new DecimalModelBinder<double>();
                object value;
                var valueProvider = new ValueProviderResult(input, input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.AreEqual(1, result.Errors.Count);
            }

            [TestMethod]
            public void DoubleIsNullable()
            {
                // typical bug we don't want to see
                string input = "";
                var culture = C("fr-FR");
                double? expected = null; // instead of validation error

                var target = new DecimalModelBinder<double?>();
                object value;
                var valueProvider = new ValueProviderResult(input, input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.AreEqual(expected, value);
                Assert.AreEqual(0, result.Errors.Count);
            }

            [TestMethod]
            public void DecimalNotFrenchStyle()
            {
                var target = new DecimalModelBinder<decimal>();
                foreach (var expectation in doubleExpectations.Where(c => c.Culture.NumberFormat.NumberDecimalSeparator == "."))
                {
                    decimal expectedValue = (decimal)expectation.Expected;
                    object value;
                    var valueProvider = new ValueProviderResult(expectation.Input, expectation.Input, expectation.Culture);
                    var result = target.BindModelImpl(valueProvider, out value);
                    Assert.AreEqual(expectedValue, value, "In: '" + expectation.Input + "' " + expectation.Culture.Name);
                }
            }

            [TestMethod]
            public void DecimalIsFrenchStyle()
            {
                var target = new DecimalModelBinder<decimal>();
                foreach (var expectation in doubleExpectations.Where(c => c.Culture.NumberFormat.NumberDecimalSeparator == ","))
                {
                    decimal expectedValue = (decimal)expectation.Expected;
                    object value;
                    var valueProvider = new ValueProviderResult(expectation.Input, expectation.Input, expectation.Culture);
                    var result = target.BindModelImpl(valueProvider, out value);
                    Assert.AreEqual(expectedValue, value, "In: '" + expectation.Input + "' " + expectation.Culture.Name);
                }
            }

            [TestMethod]
            public void DecimalBug1()
            {
                // typical bug we don't want to see
                string input = "1.2";
                var culture = C("fr-FR");
                decimal expected = 1.2M; // instead of validation error

                var target = new DecimalModelBinder<decimal>();
                object value;
                var valueProvider = new ValueProviderResult(input, input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.AreEqual(expected, value);
                Assert.AreEqual(0, result.Errors.Count);
            }

            [TestMethod]
            public void DecimalNotNullable()
            {
                // typical bug we don't want to see
                string input = "";
                var culture = C("fr-FR");
                ////double expected = 1.2D; // instead of validation error

                var target = new DecimalModelBinder<decimal>();
                object value;
                var valueProvider = new ValueProviderResult(input, input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.AreEqual(1, result.Errors.Count);
            }

            [TestMethod]
            public void DecimalIsNullable()
            {
                // typical bug we don't want to see
                string input = "";
                var culture = C("fr-FR");
                decimal? expected = null; // instead of validation error

                var target = new DecimalModelBinder<decimal?>();
                object value;
                var valueProvider = new ValueProviderResult(input, input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.AreEqual(expected, value);
                Assert.AreEqual(0, result.Errors.Count);
            }

            [TestMethod]
            public void Bug1()
            {
                string input = "123.45";
                var culture = C("fr-FR");
                decimal? expected = 123.45M;

                var target = new DecimalModelBinder<decimal?>();
                object value;
                var valueProvider = new ValueProviderResult(input, input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.AreEqual(expected, value);
                Assert.AreEqual(0, result.Errors.Count);
            }
        }

        [TestClass]
        public class RegisterMethod
        {
            [TestMethod]
            public void Works()
            {
                var binders = new ModelBinderDictionary();
                DecimalModelBinder.Register(binders);
                Assert.AreEqual(6, binders.Count);
            }
        }

        public class Expectation<T>
        {
            public Expectation(string input, CultureInfo culture, T expected)
            {
                this.Input = input;
                this.Culture = culture;
                this.Expected = expected;
            }

            public string Input { get; set; }
            public CultureInfo Culture { get; set; }
            public T Expected { get; set; }
        }
    }
}
