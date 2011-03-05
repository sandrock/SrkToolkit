using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SrkToolkit.WildServiceRef {

    /// <summary>
    /// Helper class for HTTP-related manipulations.
    /// </summary>
    public static class HttpTools {

        /// <summary>
        /// Takes key/value pairs to create a dictionary.
        /// </summary>
        /// <param name="keyValues">pass keys and values alternatively: key1, value1, key2, value2</param>
        /// <exception cref="ArgumentException">if key/value count is not even</exception>
        /// <returns>return a dictionary if values exists</returns>
        public static Dictionary<string, string> GetDictionaryFromKeyValues(params string[] keyValues) {
            Dictionary<string, string> parameters = null;
            if (keyValues.Length > 0) {
                if (keyValues.Length % 2 != 0)
                    throw new ArgumentException("Invalid parameters count", "keyvalues");

                parameters = new Dictionary<string, string>();
                bool isKey = true;
                string key = null;
                foreach (var item in keyValues) {
                    if (isKey) {
                        key = item;
                    } else {
                        parameters.Add(key, item);
                    }
                    isKey = !isKey;
                }
            }
            return parameters;
        }

        /// <summary>
        /// Return a querystring out of key/value pairs.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="GetDictionaryFromKeyValues"/> internaly.
        /// </remarks>
        /// <param name="keyValues">pass keys and values alternatively: key1, value1, key2, value2</param>
        /// <returns>a querystring like "?key1=value1&amp;key2=value2"</returns>
        public static string GetParamsAsQueryString(params string[] keyValues) {
            return GetParamsAsQueryString(GetDictionaryFromKeyValues(keyValues));
        }

        /// <summary>
        /// Return a querystring out of a parameters collection.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>a querystring like "?key1=value1&amp;key2=value2"</returns>
        public static string GetParamsAsQueryString(Dictionary<string, string> parameters) {
            if (parameters == null || parameters.Count == 0)
                return null;

            var items = new List<string>();
            foreach (var item in parameters) {
                items.Add(string.Concat(item.Key, "=", HttpUtility.UrlEncode(item.Value)));
            }

            return string.Concat('?', string.Join("&", items));
        }

        internal static string PostEncode(string value) {
            return value.Replace("=", "%3D").Replace("&", "%26");
        }

    }
}
