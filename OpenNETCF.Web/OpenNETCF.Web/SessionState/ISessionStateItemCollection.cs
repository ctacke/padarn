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
