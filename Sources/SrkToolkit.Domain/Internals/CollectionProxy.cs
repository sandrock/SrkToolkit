
namespace SrkToolkit.Domain.Internals
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Proxies a collection of <typeparamref name="TSource"/> into a collection of <typeparamref name="T"/> using a direct cast method.
    /// An optional converter allows writing items of type <typeparamref name="T"/> that are not already
    /// <typeparamref name="TSource"/> — useful when the interface is set via an assignee of a different concrete type.
    /// </summary>
    /// <typeparam name="TSource">source collection item type</typeparam>
    /// <typeparam name="T">new collection item type</typeparam>
    public sealed class CollectionProxy<TSource, T> : IList<T>
        where TSource : T
    {
        private readonly IList<TSource> source;
        private readonly Func<T, TSource> writeConverter;

        /// <summary>
        /// Proxies a collection of <typeparamref name="TSource"/> into a collection of <typeparamref name="T"/> using a direct cast method.
        /// An optional converter allows writing items of type <typeparamref name="T"/> that are not already
        /// <typeparamref name="TSource"/> — useful when the interface is set via an assignee of a different concrete type.
        /// </summary>
        /// <typeparam name="TSource">source collection item type</typeparam>
        /// <typeparam name="T">new collection item type</typeparam>
        public CollectionProxy(IList<TSource> source)
        {
            this.source = source;
        }

        /// <summary>
        /// Proxies a collection of <typeparamref name="TSource"/> into a collection of <typeparamref name="T"/> using a direct cast method.
        /// An optional <paramref name="writeConverter"/> allows writing items of type <typeparamref name="T"/> that are not already
        /// <typeparamref name="TSource"/> — useful when the interface is set via an assignee of a different concrete type.
        /// </summary>
        /// <typeparam name="TSource">source collection item type</typeparam>
        /// <typeparam name="T">new collection item type</typeparam>
        public CollectionProxy(IList<TSource> source, Func<T, TSource> writeConverter)
        {
            this.source = source;
            this.writeConverter = writeConverter;
        }

        public T this[int index]
        {
            get { return this.source[index]; }
            set { this.source[index] = this.ToSource(value); }
        }

        public int Count
        {
            get { return this.source.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.source.IsReadOnly; }
        }

        public void Add(T item)
        {
            this.source.Add(this.ToSource(item));
        }

        public void Clear()
        {
            this.source.Clear();
        }

        public bool Contains(T item)
        {
            return this.source.Contains(this.ToSource(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ////this.source.CopyTo(array, arrayIndex);
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)this.source.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return this.source.IndexOf(this.ToSource(item));
        }

        public void Insert(int index, T item)
        {
            this.source.Add(this.ToSource(item));
        }

        public bool Remove(T item)
        {
            return this.source.Remove(this.ToSource(item));
        }

        public void RemoveAt(int index)
        {
            this.source.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.source.GetEnumerator();
        }

        private TSource ToSource(T value)
        {
            if (this.writeConverter != null)
            {
                return this.writeConverter(value);
            }

            return (TSource)value;
        }
    }
}
