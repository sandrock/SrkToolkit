// -----------------------------------------------------------------------
// <copyright file="SrkObservableCollectionExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace System.Collections.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension methods for <see cref="ObservableCollection{T}"/>.
    /// </summary>
    public static class SrkObservableCollectionExtensions
    {
        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items.</param>
        /// <returns>this</returns>
        /// <exception cref="System.ArgumentNullException">collection or items</exception>
        public static ObservableCollection<T> AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
            {
                collection.Add(item);
            }

            return collection;
        }

        /// <summary>
        /// Removes all items matching a condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>this</returns>
        /// <exception cref="System.ArgumentNullException">collection or items</exception>
        public static ObservableCollection<T> RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> predicate)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            var delete = collection.Where(predicate).ToArray();
            foreach (var item in delete)
            {
                if (collection.Contains(item))
                    collection.Remove(item);
            }

            return collection;
        }
    }
}
