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
