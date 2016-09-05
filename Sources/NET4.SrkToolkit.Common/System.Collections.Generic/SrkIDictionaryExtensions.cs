
namespace System.Collections.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods for generic dictionaries.
    /// </summary>
    public static class SrkIDictionaryExtensions
    {
        public delegate TValue MergeConflictDelegate<TSource, TKey, TValue>(
            TKey key,
            TValue originalValue,
            TValue newValue);

        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="IDictionary{TKey, TSource}"/> with a key selector.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="collection"></param>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        public static void AddRange<TSource, TKey>(this IDictionary<TKey, TSource> collection, IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            AddRange<TSource, TKey, TSource>(collection, source, keySelector, x => x);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="IDictionary{TKey, TSource}"/> with a key and value selector.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="collection"></param>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="valueSelector"></param>
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
            if (!dictionary.TryGetValue(key, out value))
                value = default(TValue);

            return value;
        }

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
