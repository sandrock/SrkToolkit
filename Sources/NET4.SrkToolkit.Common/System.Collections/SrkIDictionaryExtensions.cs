
namespace System.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension methods for the <see cref="IDictionary"/> interface.
    /// </summary>
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

        public static void AddRange<TSource, TKey>(this IDictionary<TKey, TSource> collection, IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            SrkIDictionaryExtensions.AddRange<TSource, TKey, TSource>(collection, source, keySelector, x => x);
        }

        public static void AddRange<TSource, TKey, TValue>(this IDictionary<TKey, TValue> collection, IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (valueSelector == null)
                throw new ArgumentNullException("valueSelector");

            foreach (var item in source)
            {
                var key = keySelector(item);
                if (collection.ContainsKey(key))
                {
                    var ex = new ArgumentException("The specified key already exists in the dictionary", "source");
                    ex.Data.Add("Key", key);
                    throw ex;
                }

                var value = valueSelector(item);
                collection.Add(key, value);
            }
        }

        public delegate TValue MergeConflictDelegate<TSource, TKey, TValue>(
            TKey key,
            TValue originalValue,
            TValue newValue);

        public static void Merge<TSource, TKey, TValue>(
            this IDictionary<TKey, TValue> collection,
            IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> valueSelector,
            MergeConflictDelegate<TSource, TKey, TValue> conflictDelegate)
            ////Func<TSource, TKey, TValue, IDictionary<TKey, TValue>, TValue> conflictAction)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (valueSelector == null)
                throw new ArgumentNullException("valueSelector");

            var comparer = EqualityComparer<TValue>.Default;
            foreach (var item in source)
            {
                var key = keySelector(item);
                var value = valueSelector(item);
                if (collection.ContainsKey(key))
                {
                    if (comparer.Equals(collection[key], value))
                    {
                        // values are same, continue
                        continue;
                    }
                    else if (conflictDelegate != null)
                    {
                        var conflictResult = conflictDelegate(key, collection[key], value);
                        collection[key] = conflictResult;
                    }
                    else
                    {
                        var ex = new InvalidOperationException("The specified key already exists in the dictionary and no resolver is defined");
                        ex.Data.Add("Key", key);
                        throw ex;
                    }
                }
                else
                {
                    collection.Add(key, value);
                }
            }
        }
    }
}
