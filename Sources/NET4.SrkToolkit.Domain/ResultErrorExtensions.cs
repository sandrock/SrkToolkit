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
    /// Extension methods for <see cref="ResultError{TResultCode}"/>.
    /// </summary>
    public static class ResultErrorExtensions
    {
        /// <summary>
        /// Adds an error to the collection.
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
        /// Adds an error to the collection.
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
        /// Adds an error to the collection.
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
        /// Adds an error to the collection.
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
        /// Adds an error to the collection.
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
        /// Adds a detailled error to the collection.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="code">The code.</param>
        /// <param name="detail">A detailled error message.</param>
        /// <param name="displayMessage">The display message.</param>
        /// <returns></returns>
        public static IList<ResultError<TEnum>> AddDetail<TEnum>(this IList<ResultError<TEnum>> collection, TEnum code, string detail, string displayMessage)
            where TEnum : struct
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            collection.Add(new ResultError<TEnum>(code, displayMessage) { Detail = detail, });
            return collection;
        }

        /// <summary>
        /// Adds a detailled error to the collection.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="code">The code.</param>
        /// <param name="detail">A detailled error message.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static IList<ResultError<TEnum>> AddDetail<TEnum>(this IList<ResultError<TEnum>> collection, TEnum code, string detail, ResourceManager enumResourceManager, CultureInfo culture)
            where TEnum : struct
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            collection.Add(new ResultError<TEnum>(code, enumResourceManager, culture) { Detail = detail, });
            return collection;
        }

        /// <summary>
        /// Adds a detailled error to the collection.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="code">The code.</param>
        /// <param name="detail">A detailled error message.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        /// <returns></returns>
        public static IList<ResultError<TEnum>> AddDetail<TEnum>(this IList<ResultError<TEnum>> collection, TEnum code, string detail, ResourceManager enumResourceManager)
            where TEnum : struct
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            collection.Add(new ResultError<TEnum>(code, enumResourceManager) { Detail = detail, });
            return collection;
        }

        /// <summary>
        /// Adds a detailled error to the collection.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="code">The code.</param>
        /// <param name="detail">A detailled error message.</param>
        /// <param name="displayMessageFormat">The display message format.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static IList<ResultError<TEnum>> AddDetail<TEnum>(this IList<ResultError<TEnum>> collection, TEnum code, string detail, string displayMessageFormat, params object[] args)
            where TEnum : struct
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            collection.Add(new ResultError<TEnum>(code, displayMessageFormat, args) { Detail = detail, });
            return collection;
        }

        /// <summary>
        /// Adds a detailled error to the collection.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="code">The code.</param>
        /// <param name="detail">A detailled error message.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static IList<ResultError<TEnum>> AddDetail<TEnum>(this IList<ResultError<TEnum>> collection, TEnum code, string detail, ResourceManager enumResourceManager, CultureInfo culture, params object[] args)
            where TEnum : struct
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            collection.Add(new ResultError<TEnum>(code, enumResourceManager, culture, args) { Detail = detail, });
            return collection;
        }

        /// <summary>
        /// Adds a detailled error to the collection.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="code">The code.</param>
        /// <param name="detail">A detailled error message.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static IList<ResultError<TEnum>> AddDetail<TEnum>(this IList<ResultError<TEnum>> collection, TEnum code, string detail, ResourceManager enumResourceManager, params object[] args)
            where TEnum : struct
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            collection.Add(new ResultError<TEnum>(code, enumResourceManager, args) { Detail = detail, });
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
