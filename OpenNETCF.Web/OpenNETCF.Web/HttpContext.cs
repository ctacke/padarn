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
using System.Diagnostics;
using OpenNETCF.Security;
using OpenNETCF.Web.SessionState;
using System.Collections.Generic;
using System.Threading;
using OpenNETCF.Security.Principal;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Encapsulates all HTTP-specific information about an individual HTTP request.
    /// </summary>
    public sealed class HttpContext
    {
        private HttpWorkerRequest wr;
        private HttpRequest request;
        private HttpResponse response;

        private static LocalDataStoreSlot httpContextDataStoreSlot = Thread.AllocateDataSlot();

        internal HttpContext(HttpWorkerRequest wr, bool initResponseWriter)
        {
            this.wr = wr;
            Init(new HttpRequest(wr, this), new HttpResponse(wr, this));
        }

        private void Init(HttpRequest httpRequest, HttpResponse httpResponse)
        {
            request = httpRequest;
            response = httpResponse;

            // set up the default identity.  the DefaultWorkerRequest may change this
            GenericIdentity identity = new GenericIdentity("anonymous", "anonymous");
            User = new GenericPrincipal(identity);

            Thread.SetData(httpContextDataStoreSlot, this);
        }

        internal void InitializeSession()
        {
            // when called from the async handler, this is too late


            // this gets called from the DefaultWorkerRequest
            // we have to wait until then so we have the Cookies
//            Session = SessionManager.Instance.GetSession(this);

            //// create or get the session
            //// TODO: **allow this to be turned off for better perf**
            //SessionIDManager manager = new SessionIDManager();
            //var id = manager.GetSessionID(this);

            //if (id == null)
            //{
            //    id = manager.CreateSessionID(this);

            //    bool redirected;
            //    bool saved;
            //    manager.SaveSessionID(this, id, out redirected, out saved);
            //}

            //Session = SessionManager.Instance.GetSession(id);
            //if (m_sessions.ContainsKey(id))
            //{
            //    Session = m_sessions[id];
            //}
            //else
            //{
            //    Session = new HttpSessionState(id);
            //    m_sessions.Add(id, Session);
            //}
        }


        /// <summary>
        /// Gets the <see cref="HttpContext"/> object for the current HTTP request.
        /// </summary>
        /// <returns>The <see cref="HttpContext"/> for the current request.</returns>
        public static HttpContext Current
        {
            [DebuggerStepThrough]
            get
            {
                HttpContext ctx = Thread.GetData(httpContextDataStoreSlot) as HttpContext;
                if (ctx == null)
                    throw new ApplicationException("HttpContext does not exist in this thread.");
                return ctx;
            }
        }

        /// <summary>
        /// Gets the <see cref="HttpRequest"/> object for the current HTTP request.
        /// </summary>
        public HttpRequest Request
        {
            [DebuggerStepThrough]
            get { return request; }
        }

        /// <summary>
        /// Gets the <see cref="HttpResponse"/> object for the current HTTP response.
        /// </summary>
        public HttpResponse Response
        {
            [DebuggerStepThrough]
            get { return response; }
        }

        /// <summary>
        /// Gets the HttpSessionState object for the current HTTP request.
        /// </summary>
        public HttpSessionState Session 
        {
            get
            {
                return SessionManager.Instance.GetSession(this);
            }
        }

        internal HttpWorkerRequest WorkerRequest
        {
            [DebuggerStepThrough]
            get { return wr; }
        }

        /// <summary>
        /// Gets or sets security information for the current HTTP request.
        /// </summary>
        public IPrincipal User { get; internal set; }
    }
}
