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
    /// Defines the contract that a custom session-state identifier manager must implement.
    /// </summary>
    public interface ISessionIDManager
    {
        /// <summary>
        /// Initializes the SessionIDManager object with information from configuration files.
        /// </summary>
        void Initialize();
        /// <summary>
        /// Performs per-request initialization of the SessionIDManager object.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="suppressAutoDetectRedirect"></param>
        /// <param name="supportSessionIDReissue"></param>
        /// <returns></returns>
        bool InitializeRequest(HttpContext context, bool suppressAutoDetectRedirect, out bool supportSessionIDReissue);
        /// <summary>
        /// Gets the session-identifier value from the current Web request.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        string GetSessionID(HttpContext context);
        /// <summary>
        /// Creates a unique session identifier for the session.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        string CreateSessionID(HttpContext context);
        /// <summary>
        /// Deletes the session-identifier cookie from the HTTP response.
        /// </summary>
        /// <param name="context"></param>
        void RemoveSessionID(HttpContext context);
        /// <summary>
        /// Saves a newly created session identifier to the HTTP response.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="redirected"></param>
        /// <param name="cookieAdded"></param>
        void SaveSessionID(HttpContext context, string id, out bool redirected, out bool cookieAdded);
        /// <summary>
        /// Gets a value indicating whether a session identifier is valid.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Validate(string id);
    }
}
