
namespace System.Collections.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class SrkIDictionaryExtensions
    {
        /// <summary>
        /// Gets a value or null. Does not throw KeyNotFoundException.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns>the value or null</returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            TValue value;
            if (dictionary.TryGetValue(key, out value))
                value = default(TValue);

            return value;
        }
    }
}
