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

namespace OpenNETCF.Web.SessionState
{
    /// <summary>
    /// Manages unique identifiers for Padarn session state.
    /// </summary>
    public class SessionIDManager : ISessionIDManager
    {
        private const string SessionCookieName = "PadarnSessionCookie";
        private const string SessionCookieIDValueName = "SessionID";

        /// <summary>
        /// Creates an instance of the SessionIDManager class.
        /// </summary>
        public SessionIDManager()
        {
        }

        /// <summary>
        /// Initializes the SessionIDManager object with information from configuration files.
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// Performs per-request initialization of the SessionIDManager object.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="suppressAutoDetectRedirect"></param>
        /// <param name="supportSessionIDReissue"></param>
        /// <returns></returns>
        public bool InitializeRequest(HttpContext context,
                                      bool suppressAutoDetectRedirect,
                                      out bool supportSessionIDReissue)
        {
            supportSessionIDReissue = false;
            return false;
        }


        /// <summary>
        /// Gets the session-identifier value from the current Web request.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetSessionID(HttpContext context)
        {
            string id = null;

            if (context.Request.Cookies.Count != 0)
            {
                var cookie = context.Request.Cookies[SessionCookieName];
                if (cookie != null)
                {
                    id = cookie[SessionCookieIDValueName];
                }
            }

            if (id != null)
            {
                if (!Validate(id))
                {
                    id = null;
                }
            }

            return id;
        }

        /// <summary>
        /// Creates a unique session identifier for the session.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string CreateSessionID(HttpContext context)
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Deletes the session-identifier cookie from the HTTP response.
        /// </summary>
        /// <param name="context"></param>
        public void RemoveSessionID(HttpContext context)
        {
            context.Response.Cookies.Remove(SessionCookieName);
        }

        /// <summary>
        /// Saves a newly created session identifier to the HTTP response.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="redirected"></param>
        /// <param name="cookieAdded"></param>
        public void SaveSessionID(HttpContext context, string id, out bool redirected, out bool cookieAdded)
        {
            redirected = false;
            cookieAdded = false;

            HttpCookie sessionCookie = new HttpCookie(SessionCookieName);
            sessionCookie.Values[SessionCookieIDValueName] = id;
            context.Response.Cookies.Set(sessionCookie);
            cookieAdded = true;
        }

        /// <summary>
        /// Gets a value indicating whether a session identifier is valid.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Validate(string id)
        {
            try
            {
                Guid testGuid = new Guid(id);

                return (id == testGuid.ToString());
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ets the maximum length of a valid session identifier.
        /// </summary>
        /// <remarks>
        /// While session identifiers created by the CreateSessionID method are 24 characters long, the maximum length of a session identifier allowed by the SessionIDManager class is 80 characters.
        /// </remarks>
        public static int SessionIDMaxLength 
        {
            get { return 80; } 
        }

    }
}
