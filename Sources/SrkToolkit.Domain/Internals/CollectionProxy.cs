
namespace SrkToolkit.Domain.Internals
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Proxies a collection of <typeparamref name="TSource"/> into a collection of <typeparamref name="T"/> using a direct cast method.
    /// </summary>
    /// <typeparam name="TSource">source collection item type</typeparam>
    /// <typeparam name="T">new collection item type</typeparam>
    public sealed class CollectionProxy<TSource, T> : IList<T>
        where TSource : T
    {
        private readonly IList<TSource> source;

        public CollectionProxy(IList<TSource> source)
        {
            this.source = source;
        }

        public T this[int index]
        {
            get { return this.source[index]; }
            set { this.source[index] = (TSource)value; }
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
            this.source.Add((TSource)item);
        }

        public void Clear()
        {
            this.source.Clear();
        }

        public bool Contains(T item)
        {
            return this.source.Contains((TSource)item);
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
            return this.source.IndexOf((TSource)item);
        }

        public void Insert(int index, T item)
        {
            this.source.Add((TSource)item);
        }

        public bool Remove(T item)
        {
            return this.source.Remove((TSource)item);
        }

        public void RemoveAt(int index)
        {
            this.source.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.source.GetEnumerator();
        }
    }
}