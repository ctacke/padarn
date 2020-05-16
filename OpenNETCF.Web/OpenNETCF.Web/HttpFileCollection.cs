using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Provides access to and organizes files uploaded by a client.
    /// </summary>
    public sealed class HttpFileCollection : NameObjectCollectionBase
    {
        // Fields
        private HttpPostedFile[] _all;
        private string[] _allKeys;

        // Methods
        internal HttpFileCollection()
        { }

        internal void AddFile(string key, HttpPostedFile file)
        {
            this._all = null;
            this._allKeys = null;
            base.BaseAdd(key, file);
        }

        /// <summary>
        /// Copies members of the file collection to an Array beginning at the specified index of the array.
        /// </summary>
        /// <param name="dest">The destination Array. </param>
        /// <param name="index">The index of the destination array where copying starts. </param>
        public void CopyTo(Array dest, int index)
        {
            if (this._all == null)
            {
                int count = this.Count;
                this._all = new HttpPostedFile[count];
                for (int i = 0; i < count; i++)
                {
                    this._all[i] = this.Get(i);
                }
            }
            if (this._all != null)
            {
                this._all.CopyTo(dest, index);
            }
        }

        /// <summary>
        /// Returns the HttpPostedFile object with the specified numerical index from the file collection.
        /// </summary>
        /// <param name="index">The index of the object to be returned from the file collection.</param>
        /// <returns>An HttpPostedFile object.</returns>
        public HttpPostedFile Get(int index)
        {
            return (HttpPostedFile)base.BaseGet(index);
        }

        /// <summary>
        /// Returns the HttpPostedFile object with the specified name from the file collection.
        /// </summary>
        /// <param name="name">The name of the object to be returned from a file collection. </param>
        /// <returns>An HttpPostedFile object.</returns>
        public HttpPostedFile Get(string name)
        {
            return (HttpPostedFile)base.BaseGet(name);
        }

        /// <summary>
        /// Returns the name of the HttpFileCollection member with the specified numerical index.
        /// </summary>
        /// <param name="index">The index of the object name to be returned. </param>
        /// <returns>The name of the HttpFileCollection member specified by index.</returns>
        public string GetKey(int index)
        {
            return base.BaseGetKey(index);
        }

        /// <summary>
        /// Gets a string array containing the keys (names) of all members in the file collection.
        /// </summary>
        public string[] AllKeys
        {
            get
            {
                if (this._allKeys == null)
                {
                    this._allKeys = base.BaseGetAllKeys();
                }
                return this._allKeys;
            }
        }

        /// <summary>
        /// Gets the object with the specified name from the file collection.
        /// </summary>
        /// <param name="name">Name of item to be returned.</param>
        /// <returns>The HttpPostedFile specified by name.</returns>
        public HttpPostedFile this[string name]
        {
            get
            {
                return this.Get(name);
            }
        }

        /// <summary>
        /// Gets the object with the specified numerical index from the HttpFileCollection.
        /// </summary>
        /// <param name="index">The index of the item to get from the file collection. </param>
        /// <returns>The HttpPostedFile specified by index.</returns>
        public HttpPostedFile this[int index]
        {
            get
            {
                return this.Get(index);
            }
        }
    }

}
