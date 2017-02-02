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

namespace System.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// Extension methods for the <see cref="IEnumerable&lt;T&gt;"/> interface within MVC views.
    /// </summary>
    public static class SrkMvcCollectionExtensions
    {
        /// <summary>
        /// Gets a selectlist from a collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="valueSelector">The value selector.</param>
        /// <param name="displaySelector">The display selector.</param>
        /// <param name="isSelected">The "is selected" selected.</param>
        /// <returns>A selectlist</returns>
        public static IList<SelectListItem> ToSelectList<T>(this IEnumerable<T> collection, Func<T, string> valueSelector, Func<T, string> displaySelector, Func<T, bool> isSelected)
        {
            var list = collection
                .Select(i => new SelectListItem
                {
                    Value = valueSelector(i),
                    Text = displaySelector(i) ?? valueSelector(i),
                    Selected = isSelected != null ? isSelected(i) : false,
                })
                .ToArray();
            return list;
        }
    }
}
