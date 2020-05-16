using System;

using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace OpenNETCF.Web.SessionState
{
    public interface ISessionStateItemCollection : ICollection, IEnumerable
    {
        /// <summary>
        /// Removes all values and keys from the session-state collection.
        /// </summary>
        void Clear();
        /// <summary>
        /// Deletes an item from the collection.
        /// </summary>
        /// <param name="name"></param>
        void Remove(string name);
        /// <summary>
        /// Deletes an item at a specified index from the collection.
        /// </summary>
        /// <param name="index"></param>
        void RemoveAt(int index);
        /// <summary>
        /// Gets or sets a value indicating whether the collection has been marked as changed.
        /// </summary>
        bool Dirty { get; set; }
        /// <summary>
        /// Gets or sets a value in the collection by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object this[string name] { get; set; }
        /// <summary>
        /// Gets or sets a value in the collection by numerical index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        object this[int index] { get; set; }
        /// <summary>
        /// Gets a collection of the variable names for all values stored in the collection.
        /// </summary>
        NameObjectCollectionBase.KeysCollection Keys { get; }
    }
}
