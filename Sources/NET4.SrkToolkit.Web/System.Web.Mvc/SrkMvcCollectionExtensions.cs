
namespace System.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    public static class SrkMvcCollectionExtensions
    {
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
