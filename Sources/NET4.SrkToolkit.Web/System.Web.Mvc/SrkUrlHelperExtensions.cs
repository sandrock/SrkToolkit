// -----------------------------------------------------------------------
// <copyright file="SrkUrlHelperExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace System.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class SrkUrlHelperExtensions
    {
        /// <summary>
        /// Replaces or adds a value in the query string of the specified url.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string SetQueryString(this UrlHelper helper, string url, string key, string value)
        {
            bool found = false;
            string sep = "?";
            foreach (string item in helper.RequestContext.HttpContext.Request.QueryString.Keys)
            {
                string itemValue = helper.RequestContext.HttpContext.Request.QueryString[item];
                if (key == item && !found)
                {
                    found = true;
                    itemValue = value;
                }

                url += sep + Uri.EscapeDataString(item) + "=" + Uri.EscapeDataString(itemValue);
                sep = "&";
            }

            if (!found)
            {
                url += sep + Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(value);
            }

            return url;
        }
    }
}
