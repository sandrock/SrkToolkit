
namespace SrkToolkit.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Wraps non-generic dictionary-like collections as a generic dictionary.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private IWrapper<TKey, TValue> wrapper;

        /// <summary>
        /// Wraps a <see cref="IDictionary"/> as a generic dictionary.
        /// </summary>
        /// <param name="dictionary"></param>
        public DictionaryWrapper(IDictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            if (dictionary is Hashtable)
            {
                this.Set((Hashtable)dictionary);
            }
            else
            {
                throw new ArgumentException("Cannot wrap " + dictionary.GetType().Name + " as IDictionary<TKey, TValue>");
            }
        }

        /// <summary>
        /// Wraps a <see cref="Hashtable"/> as a generic dictionary.
        /// </summary>
        /// <param name="hashtable"></param>
        public DictionaryWrapper(Hashtable hashtable)
        {
            if (hashtable == null)
                throw new ArgumentNullException("hashtable");

            this.Set(hashtable);
        }

        #region IDictionary<TKey, TValue>

        /// <summary>
        /// Adds an element with the provided key and value to the wrapped dictionary.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(TKey key, TValue value)
        {
            this.wrapper.Add(key, value);
        }

        /// <summary>
        /// Determines whether the wrapped dictionary contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the wrapped dictionary</param>
        /// <returns>true if the wrapped dictionary contains an element with the key; otherwise, false.</returns>
        public bool ContainsKey(TKey key)
        {
            return this.wrapper.ContainsKey(key);
        }

        /// <summary>
        /// Gets a <see cref="System.Collections.Generic.ICollection{TKey}"/> containing the keys of the wrapped dictionary.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                return this.wrapper.Keys;
            }
        }

        /// <summary>
        /// Removes the element with the specified key from the wrapped dictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully removed; otherwise, false. This method also returns false if key was not found in the original wrapped dictionary.</returns>
        public bool Remove(TKey key)
        {
            return this.wrapper.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the object that implements wrapped dictionary contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            TValue val;
            var result = this.wrapper.TryGetValue(key, out val);
            value = val;
            return result;
        }

        /// <summary>
        /// Gets a <see cref="System.Collections.Generic.ICollection{TKey}"/> containing the values of the wrapped dictionary.
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                return this.wrapper.Values;
            }
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get or set.</param>
        /// <returns>The element with the specified key.</returns>
        public TValue this[TKey key]
        {
            get
            {
                return this.wrapper.Get(key);
            }
            set
            {
                this.wrapper.Set(key, value);
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.wrapper.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.wrapper.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.wrapper.ContainsKey(item.Key)
                && this.wrapper.Get(item.Key).Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public int Count
        {
            get
            {
                return this.wrapper.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.wrapper.ContainsKey(item.Key)
                && this.wrapper.Get(item.Key).Equals(item.Value)
                && this.wrapper.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException();
        }

        #endregion

        private void Set(Hashtable hashtable)
        {
            this.wrapper = new HashtableWrapper<TKey, TValue>(hashtable);
        }

        private interface IWrapper<TKey1, TValue1>
        {
            void Add(TKey1 key, TValue1 value);

            bool ContainsKey(TKey1 key);

            ICollection<TKey1> Keys { get; }

            bool Remove(TKey1 key);

            bool TryGetValue(TKey1 key, out TValue1 val);

            ICollection<TValue1> Values { get; }

            TValue1 Get(TKey1 key);

            void Set(TKey1 key, TValue1 value);

            void Clear();

            int Count { get; }
        }

        private class HashtableWrapper<TKey1, TValue1> : IWrapper<TKey1, TValue1>
        {
            private Hashtable hashtable;

            public HashtableWrapper(Hashtable hashtable)
            {
                this.hashtable = hashtable;
            }

            public void Add(TKey1 key, TValue1 value)
            {
                this.hashtable.Add(key, value);
            }

            public bool ContainsKey(TKey1 key)
            {
                return this.hashtable.ContainsKey(key);
            }

            public ICollection<TKey1> Keys
            {
                get
                {
                    return this.hashtable.Keys.Cast<TKey1>().ToArray();
                }
            }

            public bool Remove(TKey1 key)
            {
                if (this.hashtable.ContainsKey(key))
                {
                    this.hashtable.Remove(key);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public bool TryGetValue(TKey1 key, out TValue1 val)
            {
                if (this.hashtable.ContainsKey(key))
                {
                    val = (TValue1)this.hashtable[key];
                    this.hashtable.Remove(key);
                    return true;
                }
                else
                {
                    val = default(TValue1);
                    return false;
                }
            }

            public ICollection<TValue1> Values
            {
                get
                {
                    return this.hashtable.Values.Cast<TValue1>().ToArray();
                }
            }

            public TValue1 Get(TKey1 key)
            {
                if (!this.hashtable.ContainsKey(key))
                {
                    throw new ArgumentException("Key is not present in hashtable");
                }

                return (TValue1)this.hashtable[key];
            }

            public void Set(TKey1 key, TValue1 value)
            {
                this.hashtable[key] = value;
            }

            public void Clear()
            {
                this.hashtable.Clear();
            }

            public int Count
            {
                get
                {
                    return this.hashtable.Count;
                }
            }
        }
    }
}
