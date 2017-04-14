#region License
// Copyright ©2017 Tacke Consulting (dba OpenNETCF)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or 
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
// ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion
using System;

using System.Collections.Generic;
using System.Text;
using System.Collections;
using OpenNETCF.Collections.Specialized;

namespace OpenNETCF.Web.Routing
{
    /// <summary>
    /// Represents a case-insensitive collection of key/value pairs that you use in various places in the routing framework, such as when you define the default values for a route or when you generate a URL that is based on a route.
    /// </summary>
    public class RouteValueDictionary : CaseInsensitiveDictionary<object>
    {
        /// <summary>
        /// Initializes a new instance of the RouteValueDictionary class that is empty. 
        /// </summary>
        public RouteValueDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the RouteValueDictionary class and adds values that are based on properties from the specified object. 
        /// </summary>
        /// <param name="values"></param>
        public RouteValueDictionary(Object values)
        {
            throw new NotImplementedException();
        }
    }
}
//        private Dictionary<string, object> m_items = new Dictionary<string, object>(new CaselessStringComparer());

//        public void Add(string key, object value)
//        {
//            m_items.Add(key, value);
//        }

//        public bool ContainsKey(string key)
//        {
//            return m_items.ContainsKey(key);
//        }

//        public ICollection<string> Keys
//        {
//            get { return m_items.Keys; }
//        }

//        public bool Remove(string key)
//        {
//            return m_items.Remove(key);
//        }

//        public bool TryGetValue(string key, out object value)
//        {
//            return m_items.TryGetValue(key, out value);
//        }

//        public ICollection<object> Values
//        {
//            get { return m_items.Values; }
//        }

//        public object this[string key]
//        {
//            get { return m_items[key]; }
//            set { m_items[key] = value; }
//        }

//        public void Add(KeyValuePair<string, object> item)
//        {
//            m_items.Add(item.Key, item.Value);
//        }

//        public void Clear()
//        {
//            m_items.Clear();
//        }

//        public bool Contains(KeyValuePair<string, object> item)
//        {
//            if (!ContainsKey(item.Key)) return false;

//            foreach (var i in m_items.Values)
//            {
//                if (i.Equals(item)) return true;
//            }

//            return false;
//        }

//        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
//        {
//            int count = array.Length - arrayIndex;
//            if(count > m_items.Count) count = m_items.Count;

//            using (var e = m_items.GetEnumerator())
//            {
//                for (int i = 0; i < count; i++)
//                {
//                    array[i] = new KeyValuePair<string, object>(e.Current.Key, e.Current.Value);
//                    e.MoveNext();
//                }
//            }
//        }

//        public int Count
//        {
//            get { return m_items.Count; }
//        }

//        public bool IsReadOnly
//        {
//            get { return false; }
//        }

//        public bool Remove(KeyValuePair<string, object> item)
//        {
//            if (!ContainsKey(item.Key)) return false;

//            return m_items.Remove(item.Key);
//        }

//        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
//        {
//            return m_items.GetEnumerator();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return m_items.GetEnumerator();
//        }
//    }
//}
