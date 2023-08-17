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

#if DEBUG
namespace SrkToolkit.Web.Tests
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
    using SrkToolkit.Web.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.Web.Mvc;
    using Xunit;

    public class IntegerModelBinderTests
    {
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

            [Fact]
            public void IntegerExpectations()
            {
                var target = new IntegerModelBinder<int>();
                foreach (var expectation in intExpectations)
                {
                    object value;
                    var valueProvider = new ValueProviderResult(expectation.Input, expectation.Input, expectation.Culture);
                    var result = target.BindModelImpl(valueProvider, out value);
                    Assert.Equal(expectation.Expected, value, "In: '" + expectation.Input + "' " + expectation.Culture.Name);
                }
            }

            [Fact]
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
                Assert.Equal(1, result.Errors.Count);
            }

            [Fact]
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
                Assert.Equal(expected, value);
                Assert.Equal(0, result.Errors.Count);
            }
        }

        public class RegisterMethod
        {
            [Fact]
            public void Works()
            {
                var binders = new ModelBinderDictionary();
                IntegerModelBinder.Register(binders);
                Assert.Equal(14, binders.Count);
                Assert.True(binders.All(b => b.Value.GetType().Name == "IntegerModelBinder`1"));
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
