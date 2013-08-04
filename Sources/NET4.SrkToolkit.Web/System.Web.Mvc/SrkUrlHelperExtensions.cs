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
    /// Extension methods for the <see cref="UrlHelper"/> class.
    /// </summary>
    public static class SrkUrlHelperExtensions
    {
        /// <summary>
        /// Replaces or adds a value in the query string of the specified url.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="url">The original URL.</param>
        /// <param name="key">The key to add or set.</param>
        /// <param name="value">The value for the specified key.</param>
        /// <returns>the modified URL</returns>
        public static string SetQueryString(this UrlHelper helper, string url, string key, string value)
        {
            bool found = false;
            string sep = "?";
            string path = url;
            string query = "";
            var values = new Dictionary<string, string>();
            var markPos = url.IndexOf('?');
            if (markPos >= 0)
            {
                path = url.Substring(0, markPos);
                var parts = url.Substring(markPos + 1).Split(new char[] { '?', '&', }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    var subparts = part.Split(new char[] { '=', });
                    values.Add(subparts[0], subparts.Length > 1 ? subparts[1] : string.Empty);
                }
            }

            foreach (string item in values.Keys)
            {
                string itemValue = values[item];
                if (key == item && !found)
                {
                    found = true;
                    itemValue = value;
                }

                query += sep + Uri.EscapeDataString(item) + "=" + Uri.EscapeDataString(itemValue);
                sep = "&";
            }

            if (!found)
            {
                query += sep + Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(value);
            }

            return path + query;;
        }
    }
}
