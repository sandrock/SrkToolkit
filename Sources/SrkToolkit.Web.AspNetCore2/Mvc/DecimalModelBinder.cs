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
#if ASPMVCCORE
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.Threading.Tasks;
#endif
#if ASPMVC
    using System.Web.Mvc;
#endif
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Carries the result of a <see cref="DecimalModelBinder{T}.BindModelImpl"/> call.
    /// Exposed publicly so unit tests can inspect errors without depending on MVC internals.
    /// </summary>
    public class DecimalModelBinderState
    {
        /// <summary>Gets the binding errors collected during parsing.</summary>
        public List<Exception> Errors { get; } = new List<Exception>();
    }

    /// <summary>
    /// Helps resolve culture-specific nuances in decimal inputs.
    /// Plays with decimal and group separators in numbers based on the request culture.
    /// See the unit tests for a description of the handled cases.
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
                    {
                        this.valueType = type;
                    }
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

#if ASPMVCCORE
        /// <summary>
        /// Binds the model to a value by using the specified binding context.
        /// </summary>
        /// <param name="bindingContext">The binding context.</param>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);

            object actualValue;
            var state = this.BindModelImpl(valueResult, out actualValue);

            if (state.Errors.Count > 0)
            {
                foreach (var error in state.Errors)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, error.Message);
                }

                bindingContext.Result = ModelBindingResult.Failed();
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(actualValue);
            }

            return Task.CompletedTask;
        }
#endif

#if ASPMVC
        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>The bound value.</returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            object actualValue;
            var state = this.BindModelImpl(valueResult, out actualValue);

            var modelState = new ModelState { Value = valueResult };
            foreach (var error in state.Errors)
            {
                modelState.Errors.Add(error);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
#endif

        /// <summary>
        /// The core parsing implementation, separated for unit testing.
        /// </summary>
        /// <param name="valueResult">The raw value from the value provider.</param>
        /// <param name="actualValue">The parsed value, or null on failure.</param>
        /// <returns>A state object containing any parse errors.</returns>
        public DecimalModelBinderState BindModelImpl(ValueProviderResult valueResult, out object actualValue)
        {
#if ASPMVCCORE
            if (valueResult == ValueProviderResult.None)
#else
            if (valueResult == null)
#endif
            {
                actualValue = null;
                return new DecimalModelBinderState();
            }

            var state = new DecimalModelBinderState();

#if ASPMVCCORE
            var attemptedValue = valueResult.FirstValue;
#else
            var attemptedValue = valueResult.AttemptedValue;
#endif

            if (string.IsNullOrEmpty(attemptedValue))
            {
                if (this.IsNullable)
                {
                    actualValue = default(T);
                    return state;
                }
                else
                {
                    actualValue = null;
                    state.Errors.Add(new ArgumentException("The value cannot be empty"));
                    return state;
                }
            }

            if (valueResult.Culture.NumberFormat.NumberDecimalSeparator == ",")
            {
                try
                {
                    var attemp = attemptedValue;

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
                                state.Errors.Add(new ArgumentException("Invalid number"));
                                return state;
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
                }
                catch (FormatException e)
                {
                    actualValue = null;
                    state.Errors.Add(e);
                }
            }
            else
            {
                try
                {
                    var attemp = attemptedValue;

                    if (valueResult.Culture.NumberFormat.NumberGroupSeparator != " "
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
                        {
                            decSeps = 0;
                        }
                        else if (firstDecSep != attemp.LastIndexOf(valueResult.Culture.NumberFormat.NumberDecimalSeparator))
                        {
                            decSeps = 2;
                        }

                        var commas = attemp.Count(c => c == ',');

                        if (decSeps > 1)
                        {
                            if (commas > 1)
                            {
                                actualValue = null;
                                state.Errors.Add(new ArgumentException("Invalid number"));
                                return state;
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
                }
                catch (FormatException e)
                {
                    actualValue = null;
                    state.Errors.Add(e);
                }
            }

            return state;
        }
    }

#if ASPMVCCORE
    /// <summary>
    /// Registers <see cref="DecimalModelBinder{T}"/> for <see cref="decimal"/>,
    /// <see cref="double"/> and <see cref="float"/> (including nullable variants).
    /// Insert at index 0 so it takes precedence over the built-in numeric binders:
    /// <code>
    /// services.AddControllersWithViews(options =>
    ///     options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider()));
    /// </code>
    /// </summary>
    public class DecimalModelBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc />
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var type = context.Metadata.ModelType;
            var underlying = Nullable.GetUnderlyingType(type) ?? type;

            if (underlying == typeof(decimal) || underlying == typeof(double) || underlying == typeof(float))
            {
                return (IModelBinder)Activator.CreateInstance(typeof(DecimalModelBinder<>).MakeGenericType(type));
            }

            return null;
        }
    }
#endif

#if ASPMVC
    /// <summary>
    /// Registration helper for MVC5.
    /// </summary>
    public static class DecimalModelBinder
    {
        /// <summary>
        /// Registers <see cref="DecimalModelBinder{T}"/> for <see cref="decimal"/>,
        /// <see cref="double"/> and <see cref="float"/> (including nullable variants).
        /// </summary>
        /// <param name="binders">The binder dictionary.</param>
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
#endif
}
