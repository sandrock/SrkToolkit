
namespace System.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// Extension methods for the <see cref="IEnumerable"/> interface within MVC views.
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
