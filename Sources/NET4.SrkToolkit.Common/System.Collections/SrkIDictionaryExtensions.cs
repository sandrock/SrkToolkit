
namespace System.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class SrkIDictionaryExtensions
    {
        /// <summary>
        /// Gets a value or creates it in the dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="factory">The factory to create the object if not present.</param>
        /// <param name="autoCreate">if set to <c>true</c> [auto create].</param>
        /// <returns></returns>
        public static T GetValue<T>(this IDictionary dictionary, string key, Func<T> factory, bool autoCreate)
            where T : class
        {
            var value = dictionary[key] as T;
            if (value == null && autoCreate)
            {
                value = factory();
                dictionary[key] = value;
            }

            return value;
        }
    }
}
