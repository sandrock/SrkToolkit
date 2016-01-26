
namespace System.Collections.Specialized
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
}
