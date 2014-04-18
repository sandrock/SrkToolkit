
namespace SrkToolkit.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Resources;
    using System.Text;
    
    /// <summary>
    /// A rich error identified by a code.
    /// </summary>
    /// <remarks>
    /// This object will use a ResourceManager (.resx file) to translate error codes to display messages.
    /// In your resource file, index you value using &lt;EnumTypeName&gt;_&lt;EnumKey&gt;
    /// </remarks>
    /// <typeparam name="TEnum"></typeparam>
    public class ResultError<TEnum>
        where TEnum : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultError{TEnum}"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="displayMessage">The display message.</param>
        public ResultError(TEnum code, string displayMessage)
        {
            this.Code = code;
            this.DisplayMessage = displayMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultError{TEnum}"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        public ResultError(TEnum code, ResourceManager enumResourceManager)
        {
            this.Code = code;
            this.DisplayMessage = ResultError.MessageFromEnum(code, enumResourceManager);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultError{TEnum}"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        /// <param name="culture">The culture.</param>
        public ResultError(TEnum code, ResourceManager enumResourceManager, CultureInfo culture)
        {
            this.Code = code;
            this.DisplayMessage = ResultError.MessageFromEnum(code, enumResourceManager, culture);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultError{TEnum}"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="displayMessageFormat">The display message format.</param>
        /// <param name="args">The args.</param>
        public ResultError(TEnum code, string displayMessageFormat, params object[] args)
        {
            this.Code = code;
            this.DisplayMessage = string.Format(displayMessageFormat, args);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultError{TEnum}"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="args">The args.</param>
        public ResultError(TEnum code, ResourceManager enumResourceManager, CultureInfo culture, params object[] args)
        {
            this.Code = code;
            this.DisplayMessage = string.Format(ResultError.MessageFromEnum(code, enumResourceManager, culture), args);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultError{TEnum}"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        /// <param name="args">The args.</param>
        public ResultError(TEnum code, ResourceManager enumResourceManager, params object[] args)
        {
            this.Code = code;
            this.DisplayMessage = string.Format(ResultError.MessageFromEnum(code, enumResourceManager), args);
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public TEnum Code { get; set; }

        /// <summary>
        /// Gets or sets the display message.
        /// </summary>
        /// <value>
        /// The display message.
        /// </value>
        public string DisplayMessage { get; set; }
    }

    public static class ResultError
    {
        internal static string MessageFromEnum(object code, ResourceManager enumResourceManager, CultureInfo culture)
        {
            var key = code.GetType().Name + "_" + code.ToString();
            var value = enumResourceManager.GetString(key, culture);
            return value ?? code.ToString();
        }

        internal static string MessageFromEnum(object code, ResourceManager enumResourceManager)
        {
            var key = code.GetType().Name + "_" + code.ToString();
            var value = enumResourceManager.GetString(key);
            return value ?? code.ToString();
        }

        public static string GetMessageFromEnum(object code, ResourceManager resourceManager, CultureInfo culture)
        {
            return MessageFromEnum(code, resourceManager, culture);
        }

        public static string GetMessageFromEnum(object code, ResourceManager resourceManager)
        {
            return MessageFromEnum(code, resourceManager);
        }
    }

    /// <summary>
    /// Extension methods for <see cref="ResultError"/>.
    /// </summary>
    public static class ResultErrorExtensions
    {
        /// <summary>
        /// Adds the specified collection.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="code">The code.</param>
        /// <param name="displayMessage">The display message.</param>
        /// <returns></returns>
        public static IList<ResultError<TEnum>> Add<TEnum>(this IList<ResultError<TEnum>> collection, TEnum code, string displayMessage)
            where TEnum : struct
        {
            collection.Add(new ResultError<TEnum>(code, displayMessage));
            return collection;
        }

        /// <summary>
        /// Adds the specified collection.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="code">The code.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static IList<ResultError<TEnum>> Add<TEnum>(this IList<ResultError<TEnum>> collection, TEnum code, ResourceManager enumResourceManager, CultureInfo culture)
            where TEnum : struct
        {
            collection.Add(new ResultError<TEnum>(code, enumResourceManager, culture));
            return collection;
        }

        /// <summary>
        /// Adds the specified collection.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="code">The code.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        /// <returns></returns>
        public static IList<ResultError<TEnum>> Add<TEnum>(this IList<ResultError<TEnum>> collection, TEnum code, ResourceManager enumResourceManager)
            where TEnum : struct
        {
            collection.Add(new ResultError<TEnum>(code, enumResourceManager));
            return collection;
        }

        /// <summary>
        /// Adds the specified collection.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="code">The code.</param>
        /// <param name="displayMessageFormat">The display message format.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static IList<ResultError<TEnum>> Add<TEnum>(this IList<ResultError<TEnum>> collection, TEnum code, string displayMessageFormat, params object[] args)
            where TEnum : struct
        {
            collection.Add(new ResultError<TEnum>(code, displayMessageFormat, args));
            return collection;
        }

        /// <summary>
        /// Adds the specified collection.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="code">The code.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static IList<ResultError<TEnum>> Add<TEnum>(this IList<ResultError<TEnum>> collection, TEnum code, ResourceManager enumResourceManager, CultureInfo culture, params object[] args)
            where TEnum : struct
        {
            collection.Add(new ResultError<TEnum>(code, enumResourceManager, culture, args));
            return collection;
        }

        /// <summary>
        /// Adds the specified collection.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="code">The code.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static IList<ResultError<TEnum>> Add<TEnum>(this IList<ResultError<TEnum>> collection, TEnum code, ResourceManager enumResourceManager, params object[] args)
            where TEnum : struct
        {
            collection.Add(new ResultError<TEnum>(code, enumResourceManager, args));
            return collection;
        }

        public static PostProcessWrapper<TEnum> WithPostProcess<TEnum>(this IList<ResultError<TEnum>> collection, Func<string, string> postProcess)
            where TEnum : struct
        {
            return new PostProcessWrapper<TEnum>(collection, postProcess);
        }

        public class PostProcessWrapper<TEnum>
            where TEnum : struct
        {
            private readonly IList<ResultError<TEnum>> collection;
            private readonly Func<string, string> postProcess;

            internal PostProcessWrapper(IList<ResultError<TEnum>> collection, Func<string, string> postProcess)
            {
                this.collection = collection;
                this.postProcess = postProcess;
            }

            /// <summary>
            /// Adds the specified collection.
            /// </summary>
            /// <param name="code">The code.</param>
            /// <param name="displayMessage">The display message.</param>
            /// <returns></returns>
            public PostProcessWrapper<TEnum> Add(TEnum code, string displayMessage)
            {
                collection.Add(new ResultError<TEnum>(code, this.postProcess(displayMessage)));
                return this;
            }

            /// <summary>
            /// Adds the specified collection.
            /// </summary>
            /// <param name="code">The code.</param>
            /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
            /// <param name="culture">The culture.</param>
            /// <returns></returns>
            public PostProcessWrapper<TEnum> Add(TEnum code, ResourceManager enumResourceManager, CultureInfo culture)
            {
                var message = this.postProcess(ResultError.GetMessageFromEnum(code, enumResourceManager, culture));
                collection.Add(new ResultError<TEnum>(code, message));
                return this;
            }

            /// <summary>
            /// Adds the specified collection.
            /// </summary>
            /// <param name="code">The code.</param>
            /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
            /// <returns></returns>
            public PostProcessWrapper<TEnum> Add(TEnum code, ResourceManager enumResourceManager)
            {
                var message = this.postProcess(ResultError.GetMessageFromEnum(code, enumResourceManager));
                collection.Add(new ResultError<TEnum>(code, message));
                return this;
            }

            /// <summary>
            /// Adds the specified collection.
            /// </summary>
            /// <typeparam name="TEnum">The type of the enum.</typeparam>
            /// <param name="collection">The collection.</param>
            /// <param name="code">The code.</param>
            /// <param name="displayMessageFormat">The display message format.</param>
            /// <param name="args">The args.</param>
            /// <returns></returns>
            public PostProcessWrapper<TEnum> Add(TEnum code, string displayMessageFormat, params object[] args)
            {
                var message = this.postProcess(string.Format(displayMessageFormat, args));
                collection.Add(new ResultError<TEnum>(code, message));
                return this;
            }

            /// <summary>
            /// Adds the specified collection.
            /// </summary>
            /// <param name="code">The code.</param>
            /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
            /// <param name="culture">The culture.</param>
            /// <param name="args">The args.</param>
            /// <returns></returns>
            public PostProcessWrapper<TEnum> Add(TEnum code, ResourceManager enumResourceManager, CultureInfo culture, params object[] args)
            {
                var message = this.postProcess(string.Format(ResultError.GetMessageFromEnum(code, enumResourceManager, culture), args));
                collection.Add(new ResultError<TEnum>(code, message));
                return this;
            }

            /// <summary>
            /// Adds the specified collection.
            /// </summary>
            /// <param name="code">The code.</param>
            /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
            /// <param name="args">The args.</param>
            /// <returns></returns>
            public PostProcessWrapper<TEnum> Add(TEnum code, ResourceManager enumResourceManager, params object[] args)
            {
                var message = this.postProcess(string.Format(ResultError.GetMessageFromEnum(code, enumResourceManager), args));
                collection.Add(new ResultError<TEnum>(code, message));
                return this;
            }
        }
    }
}
