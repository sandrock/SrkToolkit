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

namespace SrkToolkit.Web.Mvc
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Helps resolve culture-specific nuances in decimal inputs.
    /// This object will play with decimal and group separators in numbers based on the user's culture.
    /// The unit tests explains what is happaning in here.
    /// </summary>
    public class DecimalModelBinder<T> : IModelBinder
    {
        private Type valueType;
        private bool? isNullable;

        private Type ValueType
        {
            get
            {
                if (this.valueType == null)
                {
                    var type = typeof(T);
                    if (type.IsValueType)
                    {
                        var nullableUnder = Nullable.GetUnderlyingType(type);
                        if (nullableUnder != null)
                        {
                            this.valueType = nullableUnder;
                            this.isNullable = true;
                        }
                        else
                        {
                            this.valueType = type;
                            this.isNullable = false;
                        }
                    }
                    else
                    {
                        this.isNullable = true;

                        var nullableUnder = Nullable.GetUnderlyingType(type);
                        if (nullableUnder != null)
                        {
                            this.valueType = nullableUnder;
                        }
                    }

                    if (this.valueType == null)
                        this.valueType = type;
                }

                return this.valueType;
            }
        }

        private bool IsNullable
        {
            get
            {
                this.valueType = this.ValueType;

                return this.isNullable.GetValueOrDefault();
            }
        }

        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>
        /// The bound value.
        /// </returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            object actualValue;
            var modelState = this.BindModelImpl(valueResult, out actualValue);

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }

        /// <summary>
        /// The implementation of the <see cref="BindModel"/> method (separated for unit tests).
        /// </summary>
        /// <param name="valueResult">The value result.</param>
        /// <param name="actualValue">The actual value.</param>
        /// <returns>the model state</returns>
        public ModelState BindModelImpl(ValueProviderResult valueResult, out object actualValue)
        {
            if (valueResult == null)
            {
                actualValue = null;
                return new ModelState();
            }

            var modelState = new ModelState { Value = valueResult, };

            if (string.IsNullOrEmpty(valueResult.AttemptedValue))
            {
                if (this.IsNullable)
                {
                    actualValue = default(T);
                    return modelState;
                }
                else
                {
                    actualValue = null;
                    modelState.Errors.Add(new ArgumentException("The value cannot be empty"));
                    return modelState;
                }
            }

            if (valueResult.Culture.NumberFormat.NumberDecimalSeparator == ",")
            {
                try
                {
                    var attemp = valueResult.AttemptedValue;

                    attemp = attemp.Replace(" ", "");

                    var commas = attemp.Count(c => c == ',');
                    var points = attemp.Count(c => c == '.');
                    if (commas > 0)
                    {
                        if (points == 1)
                        {
                            if (commas > 1)
                            {
                                attemp = attemp.Replace(",", "");
                            }
                            else
                            {
                                if (attemp.IndexOf(',') < attemp.IndexOf('.'))
                                {
                                    attemp = attemp.Replace(",", "");
                                }
                                else
                                {
                                    attemp = attemp.Replace(".", "");
                                    attemp = attemp.Replace(",", ".");
                                }
                            }
                        }
                        else if (points > 1)
                        {
                            if (commas > 1)
                            {
                                actualValue = null;
                                modelState.Errors.Add(new ArgumentException("Invalid number"));
                                return modelState;
                            }
                            else
                            {
                                attemp = attemp.Replace(".", "");
                                attemp = attemp.Replace(",", ".");
                            }
                        }
                        else if (commas > 1)
                        {
                            attemp = attemp.Replace(",", "");
                        }
                        else
                        {
                            attemp = attemp.Replace(",", ".");
                        }
                    }

                    actualValue = Convert.ChangeType(attemp, this.ValueType, CultureInfo.InvariantCulture);
                    ////actualValue = Convert.ToDouble(attemp, CultureInfo.InvariantCulture);
                }
                catch (FormatException e)
                {
                    actualValue = null;
                    modelState.Errors.Add(e);
                }
            }
            else
            {
                try
                {
                    var attemp = valueResult.AttemptedValue;

                    if (valueResult.Culture.NumberFormat.NumberGroupSeparator != " "
                     ////&& valueResult.Culture.NumberFormat.NumberDecimalSeparator == "."
                     && attemp.Contains(' '))
                    {
                        attemp = attemp.Replace(" ", valueResult.Culture.NumberFormat.NumberGroupSeparator);
                    }

                    if (valueResult.Culture.NumberFormat.NumberDecimalSeparator != ","
                     && attemp.Contains(','))
                    {
                        int decSeps = 1;
                        int firstDecSep = attemp.IndexOf(valueResult.Culture.NumberFormat.NumberDecimalSeparator);
                        if (firstDecSep < 0)
                            decSeps = 0;
                        else if (firstDecSep != attemp.LastIndexOf(valueResult.Culture.NumberFormat.NumberDecimalSeparator))
                            decSeps = 2;
                        var commas = attemp.Count(c => c == ',');

                        if (decSeps > 1)
                        {
                            if (commas > 1)
                            {
                                actualValue = null;
                                modelState.Errors.Add(new ArgumentException("Invalid number"));
                                return modelState;
                            }
                            else
                            {
                                attemp = attemp.Replace(valueResult.Culture.NumberFormat.NumberDecimalSeparator, "");
                                attemp = attemp.Replace(",", valueResult.Culture.NumberFormat.NumberDecimalSeparator);
                            }
                        }
                        else
                        {
                            attemp = attemp.Replace(",", "");
                        }
                    }

                    actualValue = Convert.ChangeType(attemp, this.ValueType, valueResult.Culture);
                    ////actualValue = Convert.ToDouble(attemp, valueResult.Culture);
                }
                catch (FormatException e)
                {
                    actualValue = null;
                    modelState.Errors.Add(e);
                }
            }

            return modelState;
        }
    }

    /// <summary>
    /// Helps resolve culture-specific nuances in decimal inputs.
    /// This object will play with decimal and group separators in numbers based on the user's culture.
    /// The unit tests explains what is happaning in here.
    /// </summary>
    public static class DecimalModelBinder
    {
        /// <summary>
        /// Registers this binder for types <see cref="double"/>, <see cref="float"/> and <see cref="decimal"/>.
        /// </summary>
        /// <param name="binders">The binders.</param>
        public static void Register(ModelBinderDictionary binders)
        {
            Bind<double>(binders);
            Bind<float>(binders);
            Bind<decimal>(binders);
        }

        private static void Bind<T>(ModelBinderDictionary binders)
            where T : struct
        {
            binders.Add(typeof(T), new DecimalModelBinder<T>());
            binders.Add(typeof(T?), new DecimalModelBinder<T?>());
        }
    }
}
