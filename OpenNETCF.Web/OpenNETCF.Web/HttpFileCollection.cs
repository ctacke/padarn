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
