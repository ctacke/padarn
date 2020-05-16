using System;

using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace OpenNETCF.Web.SessionState
{
    /// <summary>
    /// A collection of objects stored in session state. This class cannot be inherited.
    /// </summary>
    public sealed class SessionStateItemCollection : NameObjectCollectionBase,
        ISessionStateItemCollection, ICollection, IEnumerable
    {
        private object m_syncRoot = new object();

        /// <summary>
        /// Gets or sets a value indicating whether the collection has been marked as changed.
        /// </summary>
        public bool Dirty { get; set; }

        /// <summary>
        /// Creates a new, empty SessionStateItemCollection object.
        /// </summary>
        public SessionStateItemCollection()
        {
        }

        /// <summary>
        /// Removes all values and keys from the session-state collection.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// Deletes an item from the collection.
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            BaseRemove(name);
            Dirty = true;
        }

        /// <summary>
        /// Deletes an item at a specified index from the collection.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
            Dirty = true;
        }

        private bool ContainsKey(string name)
        {
            foreach (var key in BaseGetAllKeys())
            {
                if (key == name) return true;
            }

            return false;
        }

        /// <summary>
        /// Gets or sets a value in the collection by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get { return BaseGet(name); }
            set 
            {
                lock(m_syncRoot)
                {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException();
                }

                // if it's already in the collection, set the value
                var item = BaseGet(name);
                if(item != null)
                {
                    BaseSet(name, value);
                    return;
                }

                // a null means either it's not there or the value is null - we need to know which
                if (!this.ContainsKey(name))
                {
                    this.BaseAdd(name, value);
                }
                else
                {
                    BaseSet(name, value);
                }

                BaseSet(name, value);
                Dirty = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value in the collection by numerical index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get { return BaseGet(index); }
            set 
            { 
                BaseSet(index, value);
                Dirty = true;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        int ICollection.Count
        {
            get { return base.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        object ICollection.SyncRoot
        {
            get { return m_syncRoot; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return base.GetEnumerator();
        }
    }
}
