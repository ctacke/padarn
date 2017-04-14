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
using System.Collections.Specialized;
using System.Threading;

namespace OpenNETCF.Web.SessionState
{
    /// <summary>
    /// Provides access to session-state values as well as session-level settings and lifetime management methods.
    /// </summary>
    public sealed class HttpSessionState : ICollection, IEnumerable
    {
        private SessionStateItemCollection m_objects = new SessionStateItemCollection();

        internal event EventHandler TimedOut;

        private object m_sycnRoot = new object();
        private int m_timeout;
        private Timer m_timer;

        /// <summary>
        /// Gets the unique identifier for the session.
        /// </summary>
        public string SessionID { get; private set; }
        internal HttpContext Context { get; private set; }

        internal HttpSessionState(string sessionID, HttpContext context)
        {
            SessionID = sessionID;
            Context = context;
        }

        ~HttpSessionState()
        {
            if (m_timer != null)
            {
                m_timer.Dispose();
                m_timer = null;
            }
        }

        /// <summary>
        /// Gets and sets the amount of time, in minutes, allowed between requests before the session-state provider terminates the session.
        /// </summary>
        public int Timeout 
        {
            get { return m_timeout; }
            set
            {
                if(value <= 0) throw new ArgumentException();

                int duration = value * 60000;

                if (m_timer == null)
                {
                    m_timer = new Timer(TimerCallbackProc, null, duration, System.Threading.Timeout.Infinite);
                }
                else
                {
                    m_timer.Change(duration, System.Threading.Timeout.Infinite);
                }

                m_timeout = value;
            }
        }

        private void TimerCallbackProc(object o)
        {
            var handler = TimedOut;
            if (handler == null) return;

            handler(this, null);
        }


        /// <summary>
        /// Cancels the current session.
        /// </summary>
        public void Abandon()
        {
            // TODO:
            throw new NotSupportedException();
        }

        /// <summary>
        /// Adds a new item to the session-state collection.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, object value)
        {
            this[name] = value;
        }

        /// <summary>
        /// Deletes an item from the session-state collection.
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            m_objects.Remove(name);
        }

        /// <summary>
        /// Returns an enumerator that can be used to read all the session-state variable names in the current session.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return m_objects.GetEnumerator();
        }

        /// <summary>
        /// Copies the collection of session-state values to a one-dimensional array, starting at the specified index in the array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            lock (m_sycnRoot)
            {
                int count = array.Length - index;
                if (count > Count) count = Count;

                for (int i = index; i < count; i++)
                {
                    array.SetValue(this[i], i);
                }
            }
        }

        /// <summary>
        /// Removes all keys and values from the session-state collection.
        /// </summary>
        public void Clear()
        {
            m_objects.Clear();
        }

        /// <summary>
        /// Gets a value that indicates whether the application is configured for cookieless sessions.
        /// </summary>
        /// <remarks>
        /// Padarn only supports cookie sessions
        /// </remarks>
        public HttpCookieMode CookieMode
        {
            get { return HttpCookieMode.UseCookies; }
        }

        /// <summary>
        /// Gets the current session-state mode.
        /// </summary>
        public SessionStateMode Mode
        {
            get { return SessionStateMode.InProc; }
        }

        /// <summary>
        /// Gets or sets a session value by numerical index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get 
            {
                lock (m_sycnRoot)
                {
                    return m_objects[index];
                }
            }
            set 
            {
                lock (m_sycnRoot)
                {
                    m_objects[index] = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a session value by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get 
            {
                lock (m_sycnRoot)
                {
                    return m_objects[name];
                }
            }
            set 
            {
                lock (m_sycnRoot)
                {
                    m_objects[name] = value;
                }                
            }
        }

        /// <summary>
        /// Gets the number of items in the session-state collection.
        /// </summary>
        public int Count
        {
            get { return m_objects.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the collection of session-state values is synchronized (thread safe).
        /// </summary>
        public bool IsSynchronized
        {
            get { return true; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the collection of session-state values.
        /// </summary>
        public object SyncRoot
        {
            get { return m_sycnRoot; }
        }
    }
}
