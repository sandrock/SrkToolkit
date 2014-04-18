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
        /////// <summary>
        /////// Replaces or adds a value in the query string of the specified url.
        /////// </summary>
        /////// <param name="helper">The helper.</param>
        /////// <param name="url">The original URL.</param>
        /////// <param name="key">The key to add or set.</param>
        /////// <param name="value">The value for the specified key.</param>
        /////// <returns>the modified URL</returns>
        ////public static string SetQueryString(this UrlHelper helper, string url, string key, string value)
        ////{
        ////    bool found = false;
        ////    string sep = "?";
        ////    string path = url;
        ////    string query = "";
        ////    var values = new Dictionary<string, string>();
        ////    var markPos = url.IndexOf('?');
        ////    if (markPos >= 0)
        ////    {
        ////        path = url.Substring(0, markPos);
        ////        var parts = url.Substring(markPos + 1).Split(new char[] { '?', '&', }, StringSplitOptions.RemoveEmptyEntries);
        ////        foreach (var part in parts)
        ////        {
        ////            var subparts = part.Split(new char[] { '=', });
        ////            values.Add(subparts[0], subparts.Length > 1 ? subparts[1] : string.Empty);
        ////        }
        ////    }

        ////    foreach (string item in values.Keys)
        ////    {
        ////        string itemValue = values[item];
        ////        if (key == item && !found)
        ////        {
        ////            found = true;
        ////            itemValue = value;
        ////        }

        ////        query += sep + Uri.EscapeDataString(item) + "=" + Uri.EscapeDataString(itemValue);
        ////        sep = "&";
        ////    }

        ////    if (!found)
        ////    {
        ////        query += sep + Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(value);
        ////    }

        ////    return path + query; ;
        ////}

        /// <summary>
        /// Replaces or adds a value in the query string of the specified url.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="url">The original URL.</param>
        /// <param name="keysAndValues">Pairs of key and value to add/replace.</param>
        /// <returns>
        /// the modified URL
        /// </returns>
        /// <exception cref="System.ArgumentException">keysAndValues must be contain pairs of key and value;keysAndValues</exception>
        public static string SetQueryString(this UrlHelper helper, string url, params string[] keysAndValues)
        {
            if (keysAndValues.Length % 2 != 0)
                throw new ArgumentException("keysAndValues must be contain pairs of key and value", "keysAndValues");

            // The plan:
            // - parse queyr and strings from url into dictionary
            // - update dictionary from keysAndValues
            // - build url

            string path = url;
            var values = new Dictionary<string, string>();

            // parse query
            string query = "";
            var markPos = url.IndexOf('?');
            if (markPos >= 0)
            {
                path = url.Substring(0, markPos);
                query = url.Substring(markPos + 1);
                var parts = query.Split(new char[] { '?', '&', }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    var subparts = part.Split(new char[] { '=', });
                    values.Add(subparts[0], subparts.Length > 1 ? subparts[1] : string.Empty);
                }
            }

            // make a dictionary out of keysAndValues
            var kvs = new Dictionary<string, string>();
            string theKey = null;
            for (int i = 0; i < keysAndValues.Length; i++)
            {
                if (theKey == null)
                {
                    theKey = keysAndValues[i];
                }
                else
                {
                    kvs.Add(theKey, keysAndValues[i]);
                    theKey = null;
                }
            }

            // update dictionary from keysAndValues
            foreach (var kv in kvs)
            {
                string key = kv.Key;
                string value = kv.Value;
                if (value != null)
                {
                    values[key] = value;
                }
                else
                {
                    if (values.ContainsKey(key))
                        values.Remove(key);
                }
            }

            // build url
            string sep = "?";
            var builder = new StringBuilder();
            builder.Append(path);
            
            foreach (string item in values.Keys)
            {
                string itemValue = values[item];
                builder.Append(sep);
                builder.Append(Uri.EscapeDataString(item));
                builder.Append("=");
                builder.Append(Uri.EscapeDataString(itemValue));
                sep = "&";
            }

            return builder.ToString();
        }
    }
}
