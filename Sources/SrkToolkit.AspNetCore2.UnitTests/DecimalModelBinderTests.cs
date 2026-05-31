//
// Copyright 2014 SandRock
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

namespace SrkToolkit.Web.Tests
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using SrkToolkit.Web.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Xunit;

    public class DecimalModelBinderTests
    {
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

            [Fact]
            public void DoubleNotFrenchStyle()
            {
                var target = new DecimalModelBinder<double>();
                foreach (var expectation in doubleExpectations.Where(c => c.Culture.NumberFormat.NumberDecimalSeparator == "."))
                {
                    object value;
                    var valueProvider = new ValueProviderResult(expectation.Input, expectation.Culture);
                    var result = target.BindModelImpl(valueProvider, out value);
                    Assert.True(Equals(expectation.Expected, value), "In: '" + expectation.Input + "' " + expectation.Culture.Name);
                }
            }

            [Fact]
            public void DoubleIsFrenchStyle()
            {
                var target = new DecimalModelBinder<double>();
                foreach (var expectation in doubleExpectations.Where(c => c.Culture.NumberFormat.NumberDecimalSeparator == ","))
                {
                    object value;
                    var valueProvider = new ValueProviderResult(expectation.Input, expectation.Culture);
                    var result = target.BindModelImpl(valueProvider, out value);
                    Assert.True(Equals(expectation.Expected, value), "In: '" + expectation.Input + "' " + expectation.Culture.Name);
                }
            }

            [Fact]
            public void DoubleBug1()
            {
                // typical bug we don't want to see
                string input = "1.2";
                var culture = C("fr-FR");
                double expected = 1.2D; // instead of validation error

                var target = new DecimalModelBinder<double>();
                object value;
                var valueProvider = new ValueProviderResult(input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.Equal(expected, value);
                Assert.Equal(0, result.Errors.Count);
            }

            [Fact]
            public void DoubleNotNullable()
            {
                string input = "";
                var culture = C("fr-FR");

                var target = new DecimalModelBinder<double>();
                object value;
                var valueProvider = new ValueProviderResult(input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.Equal(1, result.Errors.Count);
            }

            [Fact]
            public void DoubleIsNullable()
            {
                string input = "";
                var culture = C("fr-FR");
                double? expected = null;

                var target = new DecimalModelBinder<double?>();
                object value;
                var valueProvider = new ValueProviderResult(input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.Equal(expected, value);
                Assert.Equal(0, result.Errors.Count);
            }

            [Fact]
            public void DecimalNotFrenchStyle()
            {
                var target = new DecimalModelBinder<decimal>();
                foreach (var expectation in doubleExpectations.Where(c => c.Culture.NumberFormat.NumberDecimalSeparator == "."))
                {
                    decimal expectedValue = (decimal)expectation.Expected;
                    object value;
                    var valueProvider = new ValueProviderResult(expectation.Input, expectation.Culture);
                    var result = target.BindModelImpl(valueProvider, out value);
                    Assert.True(Equals(expectedValue, value), "In: '" + expectation.Input + "' " + expectation.Culture.Name);
                }
            }

            [Fact]
            public void DecimalIsFrenchStyle()
            {
                var target = new DecimalModelBinder<decimal>();
                foreach (var expectation in doubleExpectations.Where(c => c.Culture.NumberFormat.NumberDecimalSeparator == ","))
                {
                    decimal expectedValue = (decimal)expectation.Expected;
                    object value;
                    var valueProvider = new ValueProviderResult(expectation.Input, expectation.Culture);
                    var result = target.BindModelImpl(valueProvider, out value);
                    Assert.True(Equals(expectedValue, value), "In: '" + expectation.Input + "' " + expectation.Culture.Name);
                }
            }

            [Fact]
            public void DecimalBug1()
            {
                // typical bug we don't want to see
                string input = "1.2";
                var culture = C("fr-FR");
                decimal expected = 1.2M; // instead of validation error

                var target = new DecimalModelBinder<decimal>();
                object value;
                var valueProvider = new ValueProviderResult(input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.Equal(expected, value);
                Assert.Equal(0, result.Errors.Count);
            }

            [Fact]
            public void DecimalNotNullable()
            {
                string input = "";
                var culture = C("fr-FR");

                var target = new DecimalModelBinder<decimal>();
                object value;
                var valueProvider = new ValueProviderResult(input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.Equal(1, result.Errors.Count);
            }

            [Fact]
            public void DecimalIsNullable()
            {
                string input = "";
                var culture = C("fr-FR");
                decimal? expected = null;

                var target = new DecimalModelBinder<decimal?>();
                object value;
                var valueProvider = new ValueProviderResult(input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.Equal(expected, value);
                Assert.Equal(0, result.Errors.Count);
            }

            [Fact]
            public void DecimalIsNullableNull()
            {
                decimal? expected = null;

                var target = new DecimalModelBinder<decimal?>();
                object value;
                var result = target.BindModelImpl(ValueProviderResult.None, out value);
                Assert.Equal(expected, value);
                Assert.Equal(0, result.Errors.Count);
            }

            [Fact]
            public void Bug1()
            {
                string input = "123.45";
                var culture = C("fr-FR");
                decimal? expected = 123.45M;

                var target = new DecimalModelBinder<decimal?>();
                object value;
                var valueProvider = new ValueProviderResult(input, culture);
                var result = target.BindModelImpl(valueProvider, out value);
                Assert.Equal(expected, value);
                Assert.Equal(0, result.Errors.Count);
            }
        }

        public class ProviderMethod
        {
            [Fact]
            public void ReturnsBinderForDecimal() => Assert.NotNull(new DecimalModelBinderProvider());

            [Fact]
            public void BinderCoversDecimalDoubleFloat()
            {
                // Smoke-test that the binder types can be instantiated for all covered types.
                Assert.IsAssignableFrom<IModelBinder>(new DecimalModelBinder<decimal>());
                Assert.IsAssignableFrom<IModelBinder>(new DecimalModelBinder<decimal?>());
                Assert.IsAssignableFrom<IModelBinder>(new DecimalModelBinder<double>());
                Assert.IsAssignableFrom<IModelBinder>(new DecimalModelBinder<double?>());
                Assert.IsAssignableFrom<IModelBinder>(new DecimalModelBinder<float>());
                Assert.IsAssignableFrom<IModelBinder>(new DecimalModelBinder<float?>());
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
