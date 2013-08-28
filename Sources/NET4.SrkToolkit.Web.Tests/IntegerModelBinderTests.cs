#if DEBUG
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

    public class IntegerModelBinderTests
    {
        [TestClass]
        public class ConvertFrenchStyleDecimals
        {
            static List<Expectation<int>> intExpectations;

            static ConvertFrenchStyleDecimals()
            {
                intExpectations = new List<Expectation<int>>()
                {
                    new Expectation<int>("1123456", C("en-US"), 1123456),
                    new Expectation<int>("1123456", C("fr-FR"), 1123456),
                };
            }

            public static CultureInfo C(string name)
            {
                return new CultureInfo(name);
            }

            [TestMethod]
            public void IntegerExpectations()
            {
                var target = new IntegerModelBinder<int>();
                foreach (var expectation in intExpectations)
                {
                    object value;
                    var valueProvider = new ValueProviderResult(expectation.Input, expectation.Input, expectation.Culture);
                    var result = target.BindModelImpl(valueProvider, out value);
                    Assert.AreEqual(expectation.Expected, value, "In: '" + expectation.Input + "' " + expectation.Culture.Name);
                }
            }

            [TestMethod]
            public void IntNotNullable()
            {
                // typical bug we don't want to see
                string input = "";
                var culture = C("fr-FR");
                ////int expected = 12; // instead of validation error

                var target = new DecimalModelBinder<int>();
                object value;
                var valueProvider = new ValueProviderResult(input, input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.AreEqual(1, result.Errors.Count);
            }

            [TestMethod]
            public void IntIsNullable()
            {
                // typical bug we don't want to see
                string input = "";
                var culture = C("fr-FR");
                int? expected = null; // instead of validation error

                var target = new DecimalModelBinder<int?>();
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
                IntegerModelBinder.Register(binders);
                Assert.AreEqual(14, binders.Count);
                Assert.IsTrue(binders.All(b => b.Value.GetType().Name == "IntegerModelBinder`1"));
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
#endif
