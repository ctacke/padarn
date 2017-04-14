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

namespace OpenNETCF.Web.UI
{
    /// <summary>
    /// Provides a collection container that enables Padarn server controls to maintain a list of their child controls.
    /// </summary>
    public class ControlCollection : IEnumerable<Control>
    {
        private List<Control> m_controls = new List<Control>();

        /// <summary>
        /// Adds the specified Control object to the collection.
        /// </summary>
        /// <param name="child"></param>
        public virtual void Add(Control child)
        {
            m_controls.Add(child);
        }

        /// <summary>
        /// Removes all controls from the current server control's ControlCollection object.
        /// </summary>
        public virtual void Clear()
        {
            m_controls.Clear();
        }

        /// <summary>
        /// Gets a reference to the server control at the specified index location in the ControlCollection object.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Control this[int index]
        {
            get { return m_controls[index]; }
        }

        /// <summary>
        /// Gets the number of server controls in the ControlCollection object for the specified ASP.NET server control.
        /// </summary>
        public virtual int Count
        {
            get { return m_controls.Count; }
        }

        public IEnumerator<Control> GetEnumerator()
        {
            return m_controls.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_controls.GetEnumerator();
        }
    }
}
