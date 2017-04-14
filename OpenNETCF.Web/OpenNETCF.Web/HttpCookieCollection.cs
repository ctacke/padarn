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
// <copyright company="OpenNETCF" file="HttpCookieCollection.cs">
//     Copyright (c) 2008-2010 OpenNETCF Consulting, LLC.
// </copyright>

namespace OpenNETCF.Web
{
    using System;
    using System.Collections.Specialized;

    /// <summary>
    /// Provides a type-safe way to manipulate HTTP cookies.
    /// </summary>
    public sealed class HttpCookieCollection : NameObjectCollectionBase
    {
        private HttpCookie[] all;
        private string[] allKeys;
        private bool changed;
        private HttpResponse response;

        /// <summary>
        /// Initializes a new instance of the HttpCookieCollection class.
        /// </summary>
        public HttpCookieCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        internal HttpCookieCollection(HttpResponse response, bool readOnly)
        {
            this.response = response;
            IsReadOnly = readOnly;
        }

        /// <summary>
        /// Gets a string array containing all the keys (cookie names) in the cookie collection.
        /// </summary>
        public string[] AllKeys
        {
            get
            {
                if (this.allKeys == null)
                {
                    this.allKeys = BaseGetAllKeys();
                }

                return this.allKeys;
            }
        }

        internal bool Changed
        {
            get { return this.changed; }
            set { this.changed = value; }
        }

        /// <summary>
        /// Gets the cookie with the specified name from the cookie collection.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HttpCookie this[string name]
        {
            get { return this.Get(name); }
        }

        /// <summary>
        /// Gets the cookie with the specified index from the cookie collection.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public HttpCookie this[int index]
        {
            get { return this.Get(index); }
        }

        /// <summary>
        /// Adds the specified cookie to the cookie collection.
        /// </summary>
        /// <param name="cookie"></param>
        public void Add(HttpCookie cookie)
        {
            if (this.response != null)
            {
                this.response.BeforeCookieCollectionChange();
            }

            this.AddCookie(cookie, true);

            if (this.response != null)
            {
                this.response.OnCookieAdd(cookie);
            }
        }

        /// <summary>
        /// Removes the cookie with the specified name from the collection.
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            this.all = null;
            this.allKeys = null;
            BaseRemove(name);
            this.changed = true;
        }

        /// <summary>
        /// Clears all cookies from the cookie collection.
        /// </summary>
        public void Clear()
        {
            this.Reset();
        }

        /// <summary>
        /// Copies members of the cookie collection to an Array beginning at the specified index of the array.
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="index"></param>
        public void CopyTo(Array dest, int index)
        {
            if (this.all == null)
            {
                int count = this.Count;
                this.all = new HttpCookie[count];
                for (int i = 0; i < count; i++)
                {
                    this.all[i] = this.Get(i);
                }
            }
            this.all.CopyTo(dest, index);
        }

        /// <summary>
        /// Returns the cookie with the specified index from the cookie collection.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public HttpCookie Get(int index)
        {
            return (HttpCookie)BaseGet(index);
        }

        /// <summary>
        /// Returns the cookie with the specified name from the cookie collection.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HttpCookie Get(string name)
        {
            HttpCookie cookie = (HttpCookie)base.BaseGet(name);
            if ((cookie == null) && (this.response != null))
            {
                cookie = new HttpCookie(name);
                this.AddCookie(cookie, true); 
                this.response.OnCookieAdd(cookie);
            }
            return cookie;
        }

        /// <summary>
        /// Returns the key (name) of the cookie at the specified numerical index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetKey(int index)
        {
            return BaseGetKey(index);
        }

        /// <summary>
        /// Updates the value of an existing cookie in a cookie collection.
        /// </summary>
        /// <param name="cookie"></param>
        public void Set(HttpCookie cookie)
        {
            if (this.response != null)
            {
                this.response.BeforeCookieCollectionChange();
            }
            this.AddCookie(cookie, false);
            if (this.response != null)
            {
                this.response.OnCookieCollectionChange();
            }
        }

        internal void Reset()
        {
            this.all = null;
            this.allKeys = null;
            BaseClear();
            this.changed = true;
        }

        internal void AddCookie(HttpCookie cookie, bool append)
        {
            this.all = null;
            this.allKeys = null;

            if (append)
            {
                cookie.Added = true;
                BaseAdd(cookie.Name, cookie);
            }
            else
            {
                if (BaseGet(cookie.Name) != null)
                {
                    cookie.Changed = true;
                }
                BaseSet(cookie.Name, cookie);
            }
        }
    }
}
