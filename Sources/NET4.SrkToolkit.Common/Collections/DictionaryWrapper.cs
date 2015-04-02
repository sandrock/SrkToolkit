
namespace SrkToolkit.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private IWrapper<TKey, TValue> wrapper;

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

        public DictionaryWrapper(Hashtable hashtable)
        {
            if (hashtable == null)
                throw new ArgumentNullException("hashtable");

            this.Set(hashtable);
        }

        private void Set(Hashtable hashtable)
        {
            this.wrapper = new HashtableWrapper<TKey, TValue>(hashtable);
        }

        #region IDictionary<TKey, TValue>

        public void Add(TKey key, TValue value)
        {
            this.wrapper.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return this.wrapper.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return this.wrapper.Keys;
            }
        }

        public bool Remove(TKey key)
        {
            return this.wrapper.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            TValue val;
            var result = this.wrapper.TryGetValue(key, out val);
            value = val;
            return result;
        }

        public ICollection<TValue> Values
        {
            get
            {
                return this.wrapper.Values;
            }
        }

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

        protected interface IWrapper<TKey1, TValue1>
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

        protected class HashtableWrapper<TKey1, TValue1> : IWrapper<TKey1, TValue1>
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
