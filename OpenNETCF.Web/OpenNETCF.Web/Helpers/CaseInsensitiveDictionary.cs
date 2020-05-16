using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace OpenNETCF.Collections.Specialized
{
    public class CaseInsensitiveDictionary<T> : IDictionary<string, T>,
                                         ICollection<KeyValuePair<string, T>>,
                                         IEnumerable<KeyValuePair<string, T>>,
                                         IEnumerable

    {
        internal class CaselessStringComparer : IEqualityComparer<string>
        {

            public bool Equals(string x, string y)
            {
                return (string.Compare(x, y, true) == 0);
            }

            public int GetHashCode(string obj)
            {
                return obj.GetHashCode();
            }
        }
        private Dictionary<string, T> m_items = new Dictionary<string, T>(new CaselessStringComparer());

        public void Add(string key, T value)
        {
            m_items.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return m_items.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return m_items.Keys; }
        }

        public bool Remove(string key)
        {
            return m_items.Remove(key);
        }

        public bool TryGetValue(string key, out T value)
        {
            return m_items.TryGetValue(key, out value);
        }

        public ICollection<T> Values
        {
            get { return m_items.Values; }
        }

        public T this[string key]
        {
            get { return m_items[key]; }
            set { m_items[key] = value; }
        }

        public void Add(KeyValuePair<string, T> item)
        {
            m_items.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            m_items.Clear();
        }

        public bool Contains(KeyValuePair<string, T> item)
        {
            if (!ContainsKey(item.Key)) return false;

            foreach (var i in m_items.Values)
            {
                if (i.Equals(item)) return true;
            }

            return false;
        }

        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            int count = array.Length - arrayIndex;
            if (count > m_items.Count) count = m_items.Count;

            using (var e = m_items.GetEnumerator())
            {
                for (int i = 0; i < count; i++)
                {
                    array[i] = new KeyValuePair<string, T>(e.Current.Key, e.Current.Value);
                    e.MoveNext();
                }
            }
        }

        public int Count
        {
            get { return m_items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, T> item)
        {
            if (!ContainsKey(item.Key)) return false;

            return m_items.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_items.GetEnumerator();
        }
    }
}
