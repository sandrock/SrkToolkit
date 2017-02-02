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
    /// Extension methods for <see cref="BasicResult"/>.
    /// </summary>
    public static class BasicResultExtensions
    {
        /// <summary>
        /// Adds the specified collection.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="displayMessage">The display message.</param>
        /// <returns></returns>
        public static IList<BasicResultError> Add<TEnum>(this IList<BasicResultError> collection, string displayMessage)
            where TEnum : struct
        {
            collection.Add(new BasicResultError(displayMessage));
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
        public static IList<BasicResultError> Add<TEnum>(this IList<BasicResultError> collection, TEnum code, ResourceManager enumResourceManager, CultureInfo culture)
            where TEnum : struct
        {
            collection.Add(new BasicResultError(EnumTools.GetDescription(code, enumResourceManager, culture)));
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
        public static IList<BasicResultError> Add<TEnum>(this IList<BasicResultError> collection, TEnum code, ResourceManager enumResourceManager)
            where TEnum : struct
        {
            collection.Add(new BasicResultError(EnumTools.GetDescription(code, enumResourceManager)));
            return collection;
        }

        /// <summary>
        /// Adds the specified collection.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="displayMessageFormat">The display message format.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static IList<BasicResultError> Add<TEnum>(this IList<BasicResultError> collection, string displayMessageFormat, params object[] args)
            where TEnum : struct
        {
            collection.Add(new BasicResultError(string.Format(displayMessageFormat, args)));
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
        public static IList<BasicResultError> Add<TEnum>(this IList<BasicResultError> collection, TEnum code, ResourceManager enumResourceManager, CultureInfo culture, params object[] args)
            where TEnum : struct
        {
            collection.Add(new BasicResultError(string.Format(EnumTools.GetDescription(code, enumResourceManager, culture), args)));
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
        public static IList<BasicResultError> Add<TEnum>(this IList<BasicResultError> collection, TEnum code, ResourceManager enumResourceManager, params object[] args)
            where TEnum : struct
        {
            collection.Add(new BasicResultError(string.Format(EnumTools.GetDescription(code, enumResourceManager), args)));
            return collection;
        }
    }
}
