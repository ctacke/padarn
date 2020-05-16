//                                                                   
// Copyright (c) 2007-2008 OpenNETCF Consulting, LLC                        
//                                                                     

using System.Text;
using OpenNETCF.Web.Helpers;

namespace OpenNETCF.Web
{
    using System.Globalization;
    using Configuration;
    using System;
    using System.Reflection;
    using System.IO;

    /// <summary>
    /// This abstract class defines the base worker methods and enumerations used by PS managed code to process requests.
    /// </summary>
    public abstract class HttpWorkerRequest
    {
        #region Abstract methods

        /// <summary>
        /// Used by the runtime to notify the HttpWorkerRequest that request processing for the current request is complete.
        /// </summary>
        public abstract void EndOfRequest();

        /// <summary>
        /// Sends all pending response data to the client.
        /// </summary>
        /// <param name="finalFlush">true if this is the last time response data will be flushed; otherwise, false.</param>
        public abstract void FlushResponse(bool finalFlush);

        /// <summary>
        /// Read the headers from the request
        /// </summary>
        protected abstract void ReadRequestHeaders();

        /// <summary>
        /// Process the HTTP request
        /// </summary>
        public abstract void ProcessRequest();

        /// <summary>
        /// Returns the virtual path to the requested URI
        /// </summary>
        /// <returns>The path to the requested URI.</returns>
        public abstract string GetUriPath();

        /// <summary>
        /// Provides access to the specified member of the request header.
        /// </summary>
        /// <returns>The server IP address returned in the request header.</returns>
        public abstract string GetLocalAddress();

        /// <summary>
        /// Provides access to the specified member of the request header.
        /// </summary>
        /// <returns>The server port number returned in the request header.</returns>
        public abstract int GetLocalPort();

        /// <summary>
        /// Provides access to the HTTP version of the request (for example, "HTTP/1.1").
        /// </summary>
        /// <returns>The HTTP version returned in the request header.</returns>
        public abstract string GetHttpVersion();

        /// <summary>
        /// Returns the specified member of the request header.
        /// </summary>
        /// <returns>The HTTP verb returned in the request header.</returns>
        public abstract string GetHttpVerbName();

        /// <summary>
        /// Returns the query string specified in the request URL.
        /// </summary>
        /// <returns>The request query string.</returns>
        public abstract string GetQueryString();

        /// <summary>
        /// Provides access to the specified member of the request header.
        /// </summary>
        /// <returns>The client&apos;s IP address.</returns>
        public abstract string GetRemoteAddress();

        /// <summary>
        /// Adds the specified number of bytes from a byte array to the response.
        /// </summary>
        /// <param name="data">The byte array to send.</param>
        /// <param name="length">The number of bytes to send, starting at the first byte.</param>
        public abstract void SendResponseFromMemory(byte[] data, int length);

        /// <summary>
        /// Adds a standard HTTP header to the response.
        /// </summary>
        /// <param name="name">The header name. For example, Accept-Range</param>
        /// <param name="value">The value of the header.</param>
        public abstract void SendKnownResponseHeader(string name, string value);

        /// <summary>
        /// Returns a value indicating whether the client connection is still active.
        /// </summary>
        /// <returns>true if the client connection is still active; otherwise, false.</returns>
        public abstract bool IsClientConnected();

        #endregion

        #region Virtual methods

        /// <summary>
        /// When overridden in a derived class, returns the name of the client computer.
        /// </summary>
        /// <returns>The name of the client computer.</returns>
        public virtual string GetRemoteName()
        {
            return GetRemoteAddress();
        }

        /// <summary>
        /// Returns the physical path to the currently executing server application.
        /// </summary>
        /// <returns>The physical path of the current application.</returns>
        internal virtual string GetAppPathTranslated()
        {
            return Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
        }

        /// <summary>
        /// Returns a single server variable from a dictionary of server variables associated with the request.
        /// </summary>
        /// <param name="name">The name of the requested server variable.</param>
        /// <returns>The requested server variable.</returns>
        public virtual string GetServerVariable(string name)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void GetRequestHeaders()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual string GetKnownRequestHeader(int index)
        {
            return null;
        }

        /// <summary>
        /// When overridden in a derived class, returns the name of the local server.
        /// </summary>
        /// <returns>The name of the local server.</returns>
        public virtual string GetServerName()
        {
            return GetLocalAddress();
        }

        /// <summary>
        /// Maps a virtual path to a physical path on the server.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        [Obsolete("Use HostingEnvironment.MapPath instead.", false)]
        public virtual string MapPath(string virtualPath)
        {
            return Hosting.HostingEnvironment.MapPath(virtualPath);
        }

        internal virtual string GetLocalPortAsString()
        {
            return GetLocalPort().ToString(NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Returns a value indicating whether the connection uses SSL.
        /// </summary>
        /// <returns>true if the connection is an SSL connection; otherwise, false. The default is false.</returns>
        public virtual bool IsSecure()
        {
            return false;
        }

        internal virtual byte[] GetQueryStringRawBytes()
        {
            return null;
        }

        /// <summary>
        /// Terminates the connection with the client.
        /// </summary>
        public virtual void CloseConnection()
        {
        }

        /// <summary>
        /// Adds a Content-Length HTTP header to the response.
        /// </summary>
        /// <param name="contentLength">The length of the response, in bytes.</param>
        public virtual void SendCalculatedContentLength(int contentLength)
        {
        }

        internal virtual string Status
        {
            get { return string.Empty;}
            set { string s = value; } // TODO: Check this is correct
        }

        /// <summary>
        /// Returns a value indicating whether HTTP response headers have been sent to the client for the current request.
        /// </summary>
        /// <returns></returns>
        public virtual bool HeadersSent()
        {
            return false;
        }

        internal virtual void ClearHeaders()
        {
        }

        #endregion

    }
}
