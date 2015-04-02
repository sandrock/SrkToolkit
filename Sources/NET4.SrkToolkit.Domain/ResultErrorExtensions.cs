﻿
namespace SrkToolkit.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Resources;
    using System.Text;
    using SrkToolkit.Domain.Internals;

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
            if (collection == null)
                throw new ArgumentNullException("collection");

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
            if (collection == null)
                throw new ArgumentNullException("collection");

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
            if (collection == null)
                throw new ArgumentNullException("collection");

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
            if (collection == null)
                throw new ArgumentNullException("collection");

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
            if (collection == null)
                throw new ArgumentNullException("collection");

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
            if (collection == null)
                throw new ArgumentNullException("collection");

            collection.Add(new ResultError<TEnum>(code, enumResourceManager, args));
            return collection;
        }

        /// <summary>
        /// Determines whether the specified collection contains the specified error code.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="code">The code.</param>
        /// <returns>true if the specified error code is found; otherwise, false</returns>
        /// <exception cref="System.ArgumentNullException">collection</exception>
        public static bool ContainsError<TEnum>(this IList<ResultError<TEnum>> collection, TEnum code)
            where TEnum : struct
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            return collection.Any(e => e.Code.Equals(code));
        }

        public static PostProcessWrapper<TEnum> WithPostProcess<TEnum>(this IList<ResultError<TEnum>> collection, Func<string, string> postProcess)
            where TEnum : struct
        {
            return new PostProcessWrapper<TEnum>(collection, postProcess);
        }

        /// <typeparam name="TEnum">The type of the enum.</typeparam>
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
                var message = this.postProcess(EnumTools.GetDescription(code, enumResourceManager, culture));
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
                var message = this.postProcess(EnumTools.GetDescription(code, enumResourceManager));
                collection.Add(new ResultError<TEnum>(code, message));
                return this;
            }

            /// <summary>
            /// Adds the specified collection.
            /// </summary>
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
                var message = this.postProcess(string.Format(EnumTools.GetDescription(code, enumResourceManager, culture), args));
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
                var message = this.postProcess(string.Format(EnumTools.GetDescription(code, enumResourceManager), args));
                collection.Add(new ResultError<TEnum>(code, message));
                return this;
            }
        }
    }
}