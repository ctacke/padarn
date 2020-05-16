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
