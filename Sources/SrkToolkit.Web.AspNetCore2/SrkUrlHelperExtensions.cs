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

namespace SrkToolkit.Web
{
    using Microsoft.AspNetCore.Mvc.Routing;
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
        /// Replaces or adds values in the query string of the specified url.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="keysAndValues">Pairs of key and value to add/replace.</param>
        /// <exception cref="System.ArgumentException">keysAndValues must be contain pairs of key and value;keysAndValues</exception>
        public static Uri ResetQueryString(this Uri uri, params string[] keysAndValues)
        {
            var builder = new UriBuilder();
            builder.Host = uri.Host;
            builder.Path = SetQueryString(uri.PathAndQuery, true, keysAndValues);
            builder.Port = uri.Port;
            builder.Scheme = uri.Scheme;
            return builder.Uri;
        }

#if !NSTD
        /// <summary>
        /// Replaces or adds values in the query string of the specified url.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="url">The original URL.</param>
        /// <param name="keysAndValues">Pairs of key and value to add/replace.</param>
        /// <returns>
        /// the modified URL
        /// </returns>
        /// <exception cref="System.ArgumentException">keysAndValues must be contain pairs of key and value;keysAndValues</exception>
        public static string ResetQueryString(this UrlHelper helper, string url, params string[] keysAndValues)
        {
            return SetQueryString(url, true, keysAndValues);
        }
#endif

        /// <summary>
        /// Replaces or adds values in the query string of the specified url.
        /// </summary>
        /// <param name="pathAndQuery"></param>
        /// <param name="keysAndValues">Pairs of key and value to add/replace.</param>
        /// <exception cref="System.ArgumentException">keysAndValues must be contain pairs of key and value;keysAndValues</exception>
        public static string ResetQueryString(string pathAndQuery, params string[] keysAndValues)
        {
            return SetQueryString(pathAndQuery, true, keysAndValues);
        }

        /// <summary>
        /// Replaces or adds values in the query string of the specified url.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="keysAndValues">Pairs of key and value to add/replace.</param>
        /// <exception cref="System.ArgumentException">keysAndValues must be contain pairs of key and value;keysAndValues</exception>
        public static Uri SetQueryString(this Uri uri, params string[] keysAndValues)
        {
            var builder = new UriBuilder();
            builder.Host = uri.Host;
            builder.Path = SetQueryString(uri.PathAndQuery, keysAndValues);
            builder.Port = uri.Port;
            builder.Scheme = uri.Scheme;
            return builder.Uri;
        }

#if !NSTD
        /// <summary>
        /// Replaces or adds values in the query string of the specified url.
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
            return SetQueryString(url, keysAndValues);
        } 
#endif

        /// <summary>
        /// Replaces or adds values in the query string of the specified url.
        /// </summary>
        /// <param name="pathAndQuery"></param>
        /// <param name="keysAndValues">Pairs of key and value to add/replace.</param>
        /// <exception cref="System.ArgumentException">keysAndValues must be contain pairs of key and value;keysAndValues</exception>
        public static string SetQueryString(string pathAndQuery, params string[] keysAndValues)
        {
            return SetQueryString(pathAndQuery, false, keysAndValues);
        }

        /// <summary>
        /// Replaces or adds values in the query string of the specified url.
        /// </summary>
        /// <param name="pathAndQuery"></param>
        /// <param name="resetCurrentKey">if true, the presence of a key will remove previous value and replace it. If false, the new values will be append to previous ones </param>
        /// <param name="keysAndValues">Pairs of key and value to add/replace.</param>
        /// <exception cref="System.ArgumentException">keysAndValues must be contain pairs of key and value;keysAndValues</exception>
        private static string SetQueryString(string pathAndQuery, bool resetCurrentKey, params string[] keysAndValues)
        {
            if (keysAndValues.Length % 2 != 0)
                throw new ArgumentException("keysAndValues must be contain pairs of key and value", "keysAndValues");

            // The plan:
            // - parse query and strings from url into dictionary
            // - update dictionary from keysAndValues
            // - build url

            string path = pathAndQuery;
            var values = new List<KeyValuePair<string, string>>();
            var baseKeys = new List<string>();

            // parse query
            string query = "";
            var markPos = pathAndQuery.IndexOf('?');
            if (markPos >= 0)
            {
                path = pathAndQuery.Substring(0, markPos);
                query = pathAndQuery.Substring(markPos + 1);
                var parts = query.Split(new char[] { '?', '&', }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    var subparts = part.Split(new char[] { '=', });
                    values.Add(new KeyValuePair<string,string>(subparts[0], subparts.Length > 1 ? subparts[1] : string.Empty));

                    if (!baseKeys.Contains(subparts[0]))
                    {
                        baseKeys.Add(subparts[0]);
                    }
                }
            }

            // remove keys  
            if (resetCurrentKey)
            {
                for (int i = 0; i < keysAndValues.Length; i+=2)
                {
                    //read all keys
                    var key = Uri.EscapeDataString(keysAndValues[i]);
                    for (int j = 0; j < values.Count; j++)
                    {
                        if (values[j].Key == key)
                        {
                            values.RemoveAt(j);
                            j--;
                        }
                    }
                }
            }

            // read keysAndValues and alter the values
            string theKey = null, value;
            for (int i = 0; i < keysAndValues.Length; i++)
            {
                if (theKey == null)
                {
                    // the current item is a key
                    theKey = Uri.EscapeDataString(keysAndValues[i]);
                }
                else
                {
                    value = keysAndValues[i];
                    if (value != null)
                    {
                        // the current item is a non-null value: add the value
                        value = Uri.EscapeDataString(value);
                        values.Add(new KeyValuePair<string, string>(theKey, value));

                        if (!baseKeys.Contains(theKey))
                        {
                            baseKeys.Add(theKey);
                        }
                    }
                    else
                    {
                        // the current item is a null value: remove all values for this key
                        if (baseKeys.Contains(theKey))
                        {
                            for (int j = 0; j < values.Count; j++)
                            {
                                if (theKey.Equals(values[j].Key))
                                {
                                    values.RemoveAt(j--);
                                }
                            }

                            baseKeys.Remove(theKey);
                        }
                    }

                    theKey = null;
                }
            }

            // build url
            string sep = "?";
            var builder = new StringBuilder();
            builder.Append(path);
            
            foreach (var item in values)
            {
                builder.Append(sep);
                builder.Append((item.Key));
                builder.Append("=");
                builder.Append((item.Value));
                sep = "&";
            }

            return builder.ToString();
        }
    }
}
