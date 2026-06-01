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

namespace System
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension methods for arrays.
    /// </summary>
    public static class SrkArrayExtensions
    {
        /// <summary>
        /// Creates a new array combining the current array with other arrays.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array1">The array1.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// array1
        /// or
        /// items
        /// </exception>
        public static T[] CombineWith<T>(this T[] array1, params T[][] items)
        {
            if (array1 == null)
                throw new ArgumentNullException("array1");
            if (items == null)
                throw new ArgumentNullException("items");

            int newSize = array1.Length;
            for (int i = 0; i < items.Length; i++)
            {
                newSize += items[i].Length;
            }

            var result = new T[newSize];
            Array.Copy(array1, 0, result, 0, array1.Length);
            int nextIndex = array1.Length;
            for (int i = 0; i < items.Length; i++)
            {
                Array.Copy(items[i], 0, result, nextIndex, items[i].Length);
                nextIndex += items[i].Length;
            }

            return result;
        }

        /// <summary>
        /// Creates a new array combining the current array and new values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array1">The array1.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">array1</exception>
        public static T[] CombineWith<T>(this T[] array1, params T[] item)
        {
            if (array1 == null)
                throw new ArgumentNullException("array1");
            if (item == null)
                throw new ArgumentNullException("item");

            int newSize = array1.Length + item.Length;

            var result = new T[newSize];
            Array.Copy(array1, 0, result, 0, array1.Length);
            for (int i = 0; i < item.Length; i++)
            {
                result[array1.Length + i] = item[i];
            }

            return result;
        }
    }
}
