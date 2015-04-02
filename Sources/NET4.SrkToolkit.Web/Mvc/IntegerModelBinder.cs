#if DEBUG
namespace SrkToolkit.Web.Mvc
{
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
    [Obsolete("DO NOT USE! Class under development.")]
    public class IntegerModelBinder<T> : IModelBinder
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

            try
            {
                var attemp = valueResult.AttemptedValue;
                attemp = attemp.Replace(" ", "");
                actualValue = Convert.ChangeType(attemp, this.ValueType, CultureInfo.InvariantCulture);
            }
            catch (FormatException e)
            {
                actualValue = null;
                modelState.Errors.Add(e);
            }

            return modelState;
        }
    }

    /// <summary>
    /// Helps resolve culture-specific nuances in decimal inputs.
    /// This object will play with decimal and group separators in numbers based on the user's culture.
    /// The unit tests explains what is happaning in here.
    /// </summary>
    [Obsolete("DO NOT USE! Class under development.")]
    public static class IntegerModelBinder
    {
        /// <summary>
        /// Registers this binder for types <see cref="byte"/>, <see cref="short"/>, <see cref="int"/> and <see cref="long"/>.
        /// </summary>
        /// <param name="binders">The binders.</param>
        public static void Register(ModelBinderDictionary binders)
        {
            Bind<byte>(binders);
            Bind<short>(binders);
            Bind<ushort>(binders);
            Bind<int>(binders);
            Bind<uint>(binders);
            Bind<long>(binders);
            Bind<ulong>(binders);
        }

        private static void Bind<T>(ModelBinderDictionary binders)
            where T : struct
        {
            binders.Add(typeof(T), new IntegerModelBinder<T>());
            binders.Add(typeof(T?), new IntegerModelBinder<T?>());
        }
    }
}
#endif
