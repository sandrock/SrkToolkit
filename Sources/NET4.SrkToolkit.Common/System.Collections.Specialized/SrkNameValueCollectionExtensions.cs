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

namespace System.Collections.Specialized
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

#if !NSTD
    /// <summary>
    /// Extension methods for the <see cref="NameValueCollection"/> class.
    /// </summary>
    public static class SrkNameValueCollectionExtensions
    {
        /// <summary>
        /// Creates a <see cref="IDictionary{string, string}"/> containing the values from the current <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static IDictionary<string, string> ToDictionary(this NameValueCollection collection)
        {
            var list = new Dictionary<string, string>(collection.Count);
            foreach (var item in collection.AllKeys)
            {
                list.Add(item, collection[item]);
            }

            return list;
        }

        /// <summary>
        /// Creates an enumerable from a <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> AsEnumerable(this NameValueCollection collection)
        {
            foreach (var item in collection.AllKeys)
            {
                yield return new KeyValuePair<string, string>(item, collection[item]);
            }
        }
    }
#endif
}
