//                                                                   
// Copyright (c) 2007-2008 OpenNETCF Consulting, LLC                        
//    

using OpenNETCF.Web.Helpers;
using OpenNETCF.Web.Security;
using System.Runtime.InteropServices;
using System.Linq;

namespace OpenNETCF.Web.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using Configuration;
    using OpenNETCF.WindowsCE;
    using OpenNETCF.Web.Logging;
    using System.Net;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Default handler for ASP.NET page requests for the Web Server
    /// </summary>
    public sealed class AsyncWorkerRequest : HttpWorkerRequest
    {
#if TRACE
        private const string TRACE = "TRACE";
#endif

        private bool  EnableTracing { get; set; }

        #region Fields

        private Server.SocketWrapperBase m_client;
        private Stream m_output;
        private bool m_headersSent;
        private string m_serverHeader;
        private StringBuilder m_responseHeaders;
        private HttpRawRequestContent m_httpRawRequestContent;
        private MemoryStream m_response;
        private bool m_partialDownload = true;
        private ILogProvider m_logProvider;
        private bool m_headersCleared = false;

        private static Dictionary<Type, IHttpHandler> m_httpHandlerCache = new Dictionary<Type, IHttpHandler>();

        internal NameValueCollection m_headers;

        private static readonly char[] s_ColonOrNL = new char[] { ':', '\n' };

        #endregion // Fields

        internal override string Status { get; set; }

        /// <summary>
        /// Returns a value indicating whether the connection uses SSL.
        /// </summary>
        /// <returns>true if the connection is an SSL connection; otherwise, false.</returns>
        public override bool IsSecure()
        {
#if CORE
            return false;
#else
            return m_client is OpenNETCF.Web.Server.HttpsSocket;
#endif
        }

        /// <summary>
        /// Initializes an instance of <see cref="DefaultWorkerRequest"/>
        /// </summary>
        /// <param name="client"></param>
        internal AsyncWorkerRequest(Server.SocketWrapperBase client, ILogProvider logProvider)
        {
            m_logProvider = logProvider;
            m_logProvider.LogRuntimeInfo(ZoneFlags.WorkerRequest, "+AsyncWorkerRequest");

            this.m_client = client;
            m_logProvider.LogRuntimeInfo(ZoneFlags.WorkerRequest, string.Format("Creating network stream to {0}", m_client.RemoteEndPoint));
            m_output = client.CreateNetworkStream();

            if (m_output != null)
            {
                SetDefaultServerHeaderAndStatus();

                InitializeResponse();
            }
            else
            {
                m_logProvider.LogRuntimeInfo(ZoneFlags.WorkerRequest, "Network stream is null!");
            }
            m_logProvider.LogRuntimeInfo(ZoneFlags.WorkerRequest, "-AsyncWorkerRequest");
        }

        /// <summary>
        /// Returns the virtual path to the requested URI
        /// </summary>
        /// <returns>The path to the requested URI.</returns>
        public override string GetUriPath() { return Path; }

        /// <summary>
        /// Process the incoming HTTP request
        /// </summary>
        public override void ProcessRequest()
        {
            int et = Environment.TickCount;

            try
            {
                //Get the request binary contents
                try
                {
                    if (m_client.Connected)
                    {
                        m_httpRawRequestContent = GetPartialRawRequestContent(m_client);
                        if (m_client.Connected && m_httpRawRequestContent.Length == 0  && m_client.Available > 0 )
                        {
                            //try again since we should not have a 0 length on the request
                            int retries = 5;
                            while (retries-- != 0 && m_client.Connected)
                            {
                                m_httpRawRequestContent = GetPartialRawRequestContent(m_client);
                                if (m_httpRawRequestContent.Length > 0)
                                {
                                    break;
                                }
                                Debug.WriteLineIf(EnableTracing, "! AsyncWorkerRequest::ProcessRequest timeout getting partial content");
                                Thread.Sleep(100);
                            }
                        }
                    }
                }
                catch (SocketException e)
                {
                    if (e.ErrorCode == 10054)
                    {
                        //An existing connection was forcibly closed by the remote host
                        CloseConnection();
                        return;
                    }
                }

                // TODO; ctacke 2/4/10 - need to vet this isVirtualPath
                bool isVirtualPath = UrlPath.IsVirtualDirectory(m_httpRawRequestContent.Path);


                //if the raw content is null or we have no data just close the connection
                if (m_httpRawRequestContent == null 
                    || m_httpRawRequestContent.Length == 0
                    || m_httpRawRequestContent.Path == null )
                {
                    CloseConnection();
                }
                else if ((!m_httpRawRequestContent.Path.EndsWith("/")) && (m_httpRawRequestContent.Path.LastIndexOf('.') == -1) && (isVirtualPath))
                {
                    //first check to see if we have a forward slash
                    //this is needed because if a url is hit for example http://site/virtualDir without the slash the header for subsequent
                    //requests for images, css etc will return GET HTTP/1.1 /images/image.png instead of GET HTTP/1.1 /virtualDir/images/image.png
                    //if a slash is added at the end of the url there is no issue
                    //Even after implementing keep alive this is the only workaround i can see at this point
                    //The following describes persistent connections http://www.w3.org/Protocols/rfc2616/rfc2616-sec8.html#sec8 and is implemented in
                    //FlushResponse()
                    HttpContext.Current.Response.Redirect(m_httpRawRequestContent.Path + "/");
                    CloseConnection();
                }
                else
                {
                    ProcessRequestInternal();
                }
            }
            finally
            {
#if DEBUG
                et = Environment.TickCount - et;
                if (et > 10)
                {
                    Debug.WriteLineIf(EnableTracing, string.Format("AsyncWorkerRequest::ProcessRequest took {0}ms", et));
                }
#endif
            }
        }

#region Overriden Members

        /// <summary>
        /// 
        /// </summary>
        public override void EndOfRequest()
        {
            CloseConnection();
        }

        /// <summary>
        /// Returns the local address of the web server
        /// </summary>
        /// <returns></returns>
        public override string GetLocalAddress()
        {
            return ((System.Net.IPEndPoint)m_client.LocalEndPoint).Address.ToString();
        }

        /// <summary>
        /// Returns the local port of the web server
        /// </summary>
        /// <returns></returns>
        public override int GetLocalPort()
        {
            return ServerConfig.GetConfig().Port;
        }

        /// <summary>
        /// Return the HTTP version of the request
        /// </summary>
        /// <returns></returns>
        public override string GetHttpVersion()
        {
            return m_httpRawRequestContent.HttpVersion;
        }

        /// <summary>
        /// Returns the HTTP verb specified in the request
        /// </summary>
        /// <returns></returns>
        public override string GetHttpVerbName()
        {
            return m_httpRawRequestContent.HttpMethod.ToString();
        }

        /// <summary>
        /// Returns the remote address of the request
        /// </summary>
        /// <returns></returns>
        public override string GetRemoteAddress()
        {
            return ((System.Net.IPEndPoint)m_client.RemoteEndPoint).Address.ToString();
        }

        /// <summary>
        /// Flush the response stream to the client
        /// </summary>
        /// <param name="finalFlush"></param>
        public override void FlushResponse(bool finalFlush)
        {
            if (!m_headersSent)
            {

                // http://www.w3.org/Protocols/rfc2616/rfc2616-sec6.html
                m_responseHeaders.Insert(0, m_serverHeader);
                // status line
                m_responseHeaders.Insert(0, Status);

                // general header fields - see http://www.w3.org/Protocols/rfc2616/rfc2616-sec4.html#sec4.5
                m_responseHeaders.Append(HttpContext.Current.Response.Cache.GetHeaderString());

                // entity header fields - see http://www.w3.org/Protocols/rfc2616/rfc2616-sec7.html#sec7.1
                m_responseHeaders.Append(string.Format("Content-Type: {0}\r\n", HttpContext.Current.Response.ContentType));

                // Cookies
                if (HttpContext.Current.Response.Cookies != null)
                {
                    int count = HttpContext.Current.Response.Cookies.Count;
                    for (int i = 0; i < count; i++)
                    {
                        m_responseHeaders.AppendFormat(CultureInfo.CurrentCulture, "{0}\r\n", HttpContext.Current.Response.Cookies[i].GetSetCookieHeader(HttpContext.Current));
                    }
                }

                // append the content-length unless it was explicitly cleared
                if ((!m_headersCleared) && (m_responseHeaders.ToString().IndexOf("Content-Length:") == -1))
                {
                    int contentLength = 0;

                    if ((!HttpContext.Current.Response.ForcedContentLength.HasValue) || (HttpContext.Current.Response.ForcedContentLength >= 0))
                    {
                        if (HttpContext.Current.Response.ForcedContentLength.HasValue)
                        {
                            contentLength = (int)HttpContext.Current.Response.ForcedContentLength.Value;
                        }
                        else
                        {
                            contentLength += (int)m_response.Length;
                            //SendCalculatedContentLength(contentLength);
                        }

                        SendCalculatedContentLength(contentLength);
                        m_responseHeaders.Append("\r\n");
                    }
                }

                // ensure the headers are terminated with \r\n\r\n
                var index = m_responseHeaders.Length - 4;

                while ((index < m_responseHeaders.Length) && (m_responseHeaders[index] != '\r')) index++;
                m_responseHeaders.Length = index;
                m_responseHeaders.Append("\r\n\r\n");

                byte[] buffer = Encoding.UTF8.GetBytes(m_responseHeaders.ToString());
                try
                {
                    int retry = 3;
                    while (retry-- > 0)
                    {
                        if (m_output.CanWrite)
                            break;

                        Thread.Sleep(250);
                    }
                    if (retry < 0) throw new IOException("Unable to write to underlying stream.");

                    m_output.Write(buffer, 0, buffer.Length);
                }
                catch (IOException ioEx)
                {
                    // this is seen occasionally - need to protect it
                    // todo: what do we do when this occurs?  for now rethrow
                    if (System.Runtime.InteropServices.Marshal.GetHRForException(ioEx) == -2146232800)
                    {
                        //An existing connection was forcibly closed by the remote host
                        //Unable to write data to the transport connection.
                        CloseConnection();
                        return;
                    }
                    throw;
                }

                HttpContext.Current.Response.HeadersWritten = m_headersSent = true;
            }
            
            if (Path.EndsWith(".aspx"))
            {
                // TranslateResponseASP();
            }

            try
            {
                // coalesce output
                if (m_response.Length > 0)
                {
                    m_response.WriteTo(m_output);
                }
                m_response.SetLength(0);
                if (finalFlush)
                {
                    CloseConnection();
                }
            }
            catch (Exception e)
            {
                m_logProvider.LogPadarnError("DefaultWorkerRequest.FlushResponse: " + e.Message, null);
                CloseConnection();
            }
        }

        internal override void ClearHeaders()
        {
            m_headers.Clear();
            m_headersCleared = true;
        }

        public override bool HeadersSent()
        {
            return m_headersSent;
        }

        private void WriteResponse(IAsyncResult ar)
        {
            try
            {
                bool finalFlush = (bool)ar.AsyncState;
                m_output.EndWrite(ar);
                m_output.Flush();
                m_response.SetLength(0);
                if (finalFlush)
                {
                    CloseConnection();
                }
            }
            catch
            {
            }
        }

        private void TranslateResponseASP()
        {
            string regex = "<%=(.*?)%>";
            StringBuilder input = new StringBuilder();

            input.Append(Encoding.UTF8.GetString(m_response.ToArray(), 0, (int)m_response.Length));

            string source = input.ToString();
            var matches = Regex.Matches(source, regex, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            if (matches.Count == 0) return;

            foreach (Match match in matches)
            {
                source = source.Replace(match.Value, ParseASP(match.Groups[1].Value));
            }

            m_response.SetLength(0);
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            m_response.Write(bytes, 0, bytes.Length);
        }

        private string ParseASP(string asp)
        {
            asp = asp.Trim();

            switch (asp.ToLower())
            {
                case "server.getlasterror()":
                    return LastError.Message;
            }

            return "PADARN:UNSUPPORTED";
        }

        /// <summary>
        /// Close the connection to the client.
        /// </summary>
        public override void CloseConnection()
        {
            //Dispose of the raw request content
            if (m_httpRawRequestContent != null)
            {
                m_httpRawRequestContent.Dispose();
            }

            if (m_client.Connected)
            {
                try
                {
                    m_client.Shutdown(SocketShutdown.Both);
                }
                catch { }
            }
        }

        /// <summary>
        /// Reads the HTTP headers from the request
        /// </summary>
        protected override void GetRequestHeaders()
        {
            ReadRequestHeaders();
        }

        /// <summary>
        /// Reads the HTTP headers from the request
        /// </summary>
        protected override void ReadRequestHeaders()
        {
            m_headers = m_httpRawRequestContent.Headers;
        }

        /// <summary>
        /// Writes the specifide byte array to the response stream
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public override void SendResponseFromMemory(byte[] data, int length)
        {
            if (length <= 0)
            {
                return;
            }

            m_response.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Calculates the length of the response and then writes to the response
        /// </summary>
        /// <param name="contentLength"></param>
        public override void SendCalculatedContentLength(int contentLength)
        {
            m_responseHeaders.AppendFormat(CultureInfo.CurrentCulture, "Content-Length: {0}\r\n", contentLength);
            //SendKnownResponseHeader("Content-Length", contentLength.ToString());
        }

        /// <summary>
        /// Sends a well-known HTTP header to the response
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public override void SendKnownResponseHeader(string name, string value)
        {
            m_responseHeaders.AppendFormat(CultureInfo.CurrentCulture, "{0}: {1}\r\n", name, value);
        }

        internal override byte[] GetQueryStringRawBytes()
        {
            if (m_httpRawRequestContent.RawQueryString != null)
                return Encoding.UTF8.GetBytes(m_httpRawRequestContent.RawQueryString);
            else
                return new byte[0];
        }

        /// <summary>
        /// Returns the query string from the request
        /// </summary>
        /// <returns></returns>
        public override string GetQueryString()
        {
            return m_httpRawRequestContent.RawQueryString;
        }

        /// <summary>
        /// Returns a value indicating whether the client connection is still active.
        /// </summary>
        /// <returns>true if the client connection is still active; otherwise, false.</returns>
        public override bool IsClientConnected()
        {
            if (m_client == null) return false;

            return m_client.Connected;
        }

#endregion

#region Private Methods

        /// <summary>
        /// initializes the resposen.  Called from ctor and before closing the connection to see if keep alive is available
        /// </summary>
        internal void InitializeResponse()
        {
            m_response = new MemoryStream();
            m_responseHeaders = new StringBuilder();
        }

        private static string m_versionString = null;

        /// <summary>
        /// Sets the default headers.  Called from ctor and before closing the connection to see if keep alive is available
        /// </summary>
        private void SetDefaultServerHeaderAndStatus()
        {
            if(m_versionString == null)
            {
                m_versionString = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            }

            m_serverHeader = string.Format("Server: Padarn Web Server v{0}\r\n", m_versionString);
            Status = "HTTP/1.1 200 OK\r\n";
        }

        /// <summary>
        /// Determins if the connection should be kept alive
        /// </summary>
        private bool KeepConnectionAlive
        {
            get
            {
                return (m_httpRawRequestContent.Headers["HTTP_CONNECTION"] != null &&
                    m_httpRawRequestContent.Headers["HTTP_CONNECTION"].Equals("keep-alive", StringComparison.OrdinalIgnoreCase));
            }
        }

        private IHttpHandler GetHandlerForFilename(string fileName, string mimeType, HttpMethod method)
        {
            string extension = System.IO.Path.GetExtension(fileName).ToLower();

            // Load the correct file handler
            IHttpHandler handler = null;

            handler = GetCustomHandler(fileName, method);

            // TODO: ** check for custon HttpHandlers **
            if (handler == null)
            {

                switch (extension)
                {
                    case ".aspx":
                        handler = new PageHandler(fileName, mimeType);
                        break;
                    case ".asmx":
                        // TODO: Web Service call
                        LogError("XML Web Services not supported: " + fileName, null);
                        break;
                    default:
                        if (!fileName.StartsWith("about:"))
                        {
                            handler = new StaticFileHandler(fileName, mimeType);
                            //handler = new StaticFileHandler(fileName, MimeMapping.GetMimeMapping(extension));
                        }
                        break;
                }
            }

            return handler;
        }

        private static Dictionary<string, Type> m_handlerTypeCache = new Dictionary<string, Type>();

        private IHttpHandler GetCustomHandler(string fileName, HttpMethod method)
        {
            IHttpHandler returnValue = null;
            ServerConfig c = ServerConfig.GetConfig();

            foreach (HttpHandler h in c.HttpHandlers)
            {
                if (((h.Verb & method) == method) && FileNameIsInPath(fileName, h.Path))
                {
                    Type t;

                    lock (m_handlerTypeCache)
                    {
                        if (m_handlerTypeCache.ContainsKey(h.TypeName))
                        {
                            t = m_handlerTypeCache[h.TypeName];
                        }
                        else
                        {
                            t = Type.GetType(h.TypeName);

                            if (t == null)
                            {
                                t = c.GetType(h.TypeName);

                    if (t == null)
                    {
                        throw new HttpException(HttpErrorCode.InternalServerError,
                            string.Format("Unable To load type '{0}'", h.TypeName));
                    }
                            }

                            m_handlerTypeCache.Add(h.TypeName, t);
                        }
                    }

                    try
                    {
                        lock (m_httpHandlerCache)
                        {
                            if (m_httpHandlerCache.ContainsKey(t))
                            {
                                returnValue = m_httpHandlerCache[t];
                            }
                            else
                            {
                                returnValue = (IHttpHandler)Activator.CreateInstance(t);

                                if (returnValue.IsReusable)
                                {
                                    m_httpHandlerCache.Add(t, returnValue);
                                }
                            }
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);

                        if (Environment.OSVersion.Platform == PlatformID.Unix)
                        {
                            Console.WriteLine(ex);
                        }

                        throw new HttpException(HttpErrorCode.InternalServerError,
                            string.Format("Unable to create '{0}' handler for '{1}' method: {2}",
                            t.Name,
                            method.ToString(),
                            ex.ToString()));
                    }
                }
            }

            return returnValue;
        }

        private bool FileNameIsInPath(string fileName, string path)
        {
            if (path == "*") return true;

            return Regex.IsMatch(fileName, path, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal override string GetAppPathTranslated()
        {
            // TODO: cache these
            ServerConfig config = ServerConfig.GetConfig();
            string virtualRoot = String.Empty;

            if (config.VirtualDirectories != null)
            {
                virtualRoot = config.VirtualDirectories.FindPhysicalDirectoryForVirtualUrlPath(Path);
            }

            if (String.IsNullOrEmpty(virtualRoot))
            {
                return System.IO.Path.GetFullPath(ServerConfig.GetConfig().DocumentRoot);
            }

            return virtualRoot;
        }

        private void LogPageAccess(LogDataItem ldi)
        {
            if (m_logProvider != null)
            {
                try
                {
                    m_logProvider.LogPageAccess(ldi);
                }
                catch (Exception ex)
                {
                    // swallow logging exceptions to prevent a bad logging plug-in from tearing us down
                    // TODO: maybe log these errors somewhere?
                    LogError("Exception trying to log page access: " + ex.Message, ldi);
                }
            }
        }

        private HttpCachePolicy CheckGlobalCachePolicy(string fileName)
        {
            string fileExtension = System.IO.Path.GetExtension(fileName).ToLower();

            foreach (CachingProfile profile in ServerConfig.GetConfig().Caching.Profiles)
            {
                if (profile.Extension == fileExtension)
                {
                    HttpCachePolicy policy = new HttpCachePolicy();

                    policy.SetMaxAge(profile.Duration);
                    
                    switch(profile.Location)
                    {
                        case CacheLocation.Client:
                            policy.SetCacheability(HttpCacheability.Private);
                            break;
                        case CacheLocation.Downstream:
                            policy.SetCacheability(HttpCacheability.Public);
                            break;
                        case CacheLocation.None:
                            policy.SetCacheability(HttpCacheability.NoCache);
                            break;
                    }

                    return new HttpCachePolicy(profile);
                }
            }
            return null;
        }

        private static string m_lastRequestFrom;
        private static int m_requestCount;

        private void HandleNonFormsAuthentication()
        {
            if ((!HasAuthorizationHeader) || (!AuthenticateRequest()))
            {
                if (!HasAuthorizationHeader)
                {
                    m_requestCount = 0;
                }
                else if (m_lastRequestFrom == HttpContext.Current.Request.UserHostAddress)
                {
                    m_requestCount++;
                    Debug.WriteLineIf(EnableTracing, string.Format("{0} requests from {1}", m_requestCount, m_lastRequestFrom));
                    if (m_requestCount >= 3)
                    {
                        m_requestCount = 0;
                        m_lastRequestFrom = string.Empty;
                        throw new HttpException(HttpErrorCode.Unauthorized, "Unauthorized");
                    }

                }
                else
                {
                    m_lastRequestFrom = HttpContext.Current.Request.UserHostAddress;
                }

                SendAuthRequest();
                FlushResponse(true);
                return;
            }
        }

        private void HandleFormsAuthentication()
        {
            bool redirectToLogin = true;

            // is this a page that requires auth?
            var absolutePath = HostingEnvironment.MapPath(Path);
            if (string.Compare(absolutePath, FormsAuthentication.LoginUrlServerPath, true) == 0)
            {
                redirectToLogin = false;
            }
            else
            {
                // are we already authenticated
                var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie != null)
                {
                    // verify the cookie domain
                    if (authCookie.Domain == FormsAuthentication.CookieDomain)
                    {
                        // verify it hasn't expired
                        if (authCookie.Expires > DateTime.Now)
                        {
                            redirectToLogin = false;

                            // reset expiration
                            if (FormsAuthentication.SlidingExpiration)
                            {
                                FormsAuthentication.SetAuthCookie(authCookie["UID"], false);
//                                HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName]
                                authCookie.Expires = DateTime.Now.AddMinutes(30);
                            }
                        }
                    }
                }
            }

            if (redirectToLogin)
            {
                if (string.IsNullOrEmpty(Path))
                {
                    FormsAuthentication.ReturnUrl = FormsAuthentication.DefaultUrl;
                }
                else
                {
                    FormsAuthentication.ReturnUrl = Path;
                }
                var authUrl = string.Format("{0}?ReturnURL={1}", FormsAuthentication.LoginUrl, Path);
                HttpContext.Current.Response.Redirect(authUrl);
                FlushResponse(true);
                return;
            }

        }

        private void HandleRequestException(Exception e, LogDataItem ldi)
        {
            LastError = e;

            // TODO: Convert this to use an ErrorFormatter
            LogError("Exception handling HTTP Request: " + e.Message, ldi);

            string exceptionPage = StringHelper.ConvertVerbatimLineEndings(Resources.ErrorPage);

            m_response.SetLength(0);
            Status = StringHelper.ConvertVerbatimLineEndings(Resources.HttpOK);

            if (e is HttpException)
            {
                HttpException err = e as HttpException;

                // see if we have a custom error page:
                exceptionPage = GetCustomErrorPage((HttpErrorCode)err.GetHttpCode());

                if (err.GetHttpCode() == (int)HttpErrorCode.NotFound)
                {
                    Status = StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusFileNotFound);
                    if (exceptionPage == null)
                    {
                        exceptionPage = string.Format(StringHelper.ConvertVerbatimLineEndings(Resources.ContextualErrorTemplate), Resources.FileNotFoundTitle, e.Message, Resources.FileNotFoundDesc);
                    }
                }
                else if (err.GetHttpCode() == (int)HttpErrorCode.Unauthorized)
                {
                    Status = StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusUnauthorized);
                    if (exceptionPage == null)
                    {
                        exceptionPage = string.Format(StringHelper.ConvertVerbatimLineEndings(Resources.ContextualErrorTemplate), Resources.UnauthorizedTitle, Resources.UnuthorizedMessage, Resources.UnauthorizedDesc);
                    }
                }
                else if (e.InnerException != null)
                {
                    if (e.InnerException.Message == Resources.Max_request_length_exceeded)
                    {
                        Status = StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusInternalServerError);
                        if (exceptionPage == null)
                        {
                            exceptionPage = string.Format(StringHelper.ConvertVerbatimLineEndings(Resources.ContextualErrorTemplate), Resources.MaxRequestLengthErrorTitle, e.Message, Resources.MaxRequestLengthErrorDesc);
                        }
                    }
                    else if (e.InnerException.Message == Resources.DiskError)
                    {
                        Status = StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusInternalServerError);
                        if (exceptionPage == null)
                        {
                            exceptionPage = string.Format(StringHelper.ConvertVerbatimLineEndings(Resources.ContextualErrorTemplate), Resources.DiskErrorTitle, e.Message, Resources.DiskErrorDesc);
                        }
                    }
                    else if (e.InnerException is OutOfMemoryException)
                    {
                        Status = StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusInternalServerError);
                        if (exceptionPage == null)
                        {
                            exceptionPage = string.Format(StringHelper.ConvertVerbatimLineEndings(Resources.ContextualErrorTemplate), Resources.OutOfMemoryErrorTitle, e.Message, Resources.OutOfMemoryErrorDesc);
                        }
                    }
                }
                else
                {
                    // pass along the error
                    Status = GetStatusForErrorCode((HttpErrorCode)((e as HttpException).GetHttpCode()));
                    if (exceptionPage == null)
                    {
                        exceptionPage = string.Format(Resources.ErrorPage, Path, String.Format(Resources.UnhandledExceptionDesc, Path), String.Format("{0}: {1}", e.GetType().FullName, e.Message), ParseStackTrace(e.StackTrace));
                    }
                }
            }
            else if (e is System.IO.FileNotFoundException)
            {
                Status = StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusFileNotFound);
                exceptionPage = string.Format(StringHelper.ConvertVerbatimLineEndings(Resources.ContextualErrorTemplate), Resources.CodeBehindNotFoundTitle, e.Message, Resources.CodeBehindNotFoundDesc);
            }
            else if (e is TypeLoadException)
            {
                Status = StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusInternalServerError);
                exceptionPage = String.Format(StringHelper.ConvertVerbatimLineEndings(Resources.ContextualErrorTemplate),
                                              Resources.TypeLoadTitle, e.Message, Resources.TypeLoadDesc);
            }
            else // Unhandled Exception
            {
                Status = StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusInternalServerError);
                // see if we have a custom error page:
                exceptionPage = GetCustomErrorPage(HttpErrorCode.InternalServerError);
                if (exceptionPage == null)
                {
                    exceptionPage = string.Format(Resources.ErrorPage, Path, String.Format(Resources.UnhandledExceptionDesc, Path), String.Format("{0}: {1}", e.GetType().FullName, e.Message), ParseStackTrace(e.StackTrace));
                }
            }

            byte[] b = Encoding.UTF8.GetBytes(exceptionPage);

            m_responseHeaders.AppendFormat(CultureInfo.CurrentCulture, StringHelper.ConvertVerbatimLineEndings(Resources.HeaderFormat), Resources.HeaderContentType, "text/html");
            m_responseHeaders.AppendFormat(CultureInfo.CurrentCulture, StringHelper.ConvertVerbatimLineEndings(Resources.HeaderFormat), Resources.HeaderContentLength, b.Length);

            SendResponseFromMemory(b, b.Length);

            // let custom errors be translated
            TranslateResponseASP();

            FlushResponse(true);
        }

        private void ProcessRequestInternal()
        {
            // ctacke - lock added to see if it addresses concurrency NativeException
            // but doesn't appear to fix anything
            lock (m_client)
            {
                LogDataItem ldi = null;

                try
                {
                    try
                    {
                        //Get the content info (determines if it's a POST or GET
                        if (!GetContentInfo())
                        {
                            CloseConnection();
                        }

                        //Get the headers
                        GetRequestHeaders();

                        // set the header
                        HttpContext.Current.Request.Headers = m_headers;

                        // set the URI
                        HttpContext.Current.Request.Url = new Uri(string.Format("{0}://{1}{2}{3}",
                            this.IsSecure() ? "https" : "http",
                            this.m_client.LocalEndPoint.ToString(),
                            this.m_httpRawRequestContent.Path,
                            string.IsNullOrEmpty(this.m_httpRawRequestContent.RawQueryString) ? string.Empty : "?" + this.m_httpRawRequestContent.RawQueryString));

                        //Read the content data
                        if (HttpContext.Current.Request.ContentLength > 0)
                        {
                            HttpContext.Current.Request.RawPostContent = GetEntireRawContent();
                        }
                    }
                    catch (IOException e)
                    {
                        // we had a stream issue - most likely we're out of space trying to write the temp file
                        throw new HttpException(Resources.DiskError, e);
                    }
                    catch (OutOfMemoryException oom)
                    {
                        throw new HttpException(Resources.OutOfMemoryException, oom);
                    }
                    catch (ObjectDisposedException)
                    {
                        // if the underlying socket has been disposed
                        Debug.WriteLine("Socket Disposed - aborting request");
                        return;
                    }
                    catch (Exception e)
                    {
                        throw new HttpException(Resources.HttpErrorParsingHeader, e);
                    }

                    // create the session now (we needed the headers for the cookies)
//                    HttpContext.Current.InitializeSession();

                    if (AuthenticationEnabled || RequestRequiresAuthentication())
                    {
                        if (FormsAuthentication.IsEnabled)
                        {
                            HandleFormsAuthentication();
                        }
                        else
                        {
                            HandleNonFormsAuthentication();
                        }
                    }

                    HttpMethod method = (HttpMethod)Enum.Parse(typeof(HttpMethod), HttpContext.Current.Request.HttpMethod, true);

                    Debug.WriteLineIf(EnableTracing, string.Format("{0}: {1}", method, Path));

                    // do we have a custom Httphandler handler for the path?
                    IHttpHandler customHandler = GetCustomHandler(Path, method);
                    if (customHandler != null)
                    {
                        customHandler.ProcessRequest(HttpContext.Current);

                        //Do the final flush to the server
                        FlushResponse(true);

                        var disposableHandler = customHandler as IDisposable;
                        if (disposableHandler != null)
                        {
                            disposableHandler.Dispose();
                        }

                        return;
                    }

                    // check for virtual file
                    if (!ProcessRequestForVirtualFile(Path))
                    {
                        string localFile, mime, defaultDoc = null;

                        if (!Path.ToLower().StartsWith("about:"))
                        {
                            string physicalPath = HostingEnvironment.MapPath(Path);

                            if ((Path.EndsWith("/")) || (string.IsNullOrEmpty(System.IO.Path.GetExtension(physicalPath))))
                            {
                                defaultDoc = GetDefaultDocument(physicalPath);
                                localFile = System.IO.Path.Combine(physicalPath, defaultDoc);
                            }
                            else
                            {
                                localFile = physicalPath;
                            }

                            localFile = ((Path.EndsWith("/")) || (string.IsNullOrEmpty(System.IO.Path.GetExtension(physicalPath))))
                                            ? System.IO.Path.Combine(physicalPath, GetDefaultDocument(physicalPath))
                                            : physicalPath;

                            var r = System.IO.Path.GetFullPath(ServerConfig.GetConfig().DocumentRoot);
                            localFile = GetCasedFileNameFromCaselessName(localFile);

                            mime = MimeMapping.GetMimeMapping(localFile);

                            if (!File.Exists(localFile))
                            {
                                var name = UrlPath.FixVirtualPathSlashes(System.IO.Path.Combine(Path, defaultDoc ?? string.Empty));
                                throw new HttpException(404, String.Format("The file '{0}' cannot be found.", name));
                            }

                            // validate the requested file is *beneath* the server root (no navigating above the root)
                            if (!IsSubDirectoryOf(localFile, ServerConfig.GetConfig().DocumentRoot))
                            {
                                throw new HttpException(HttpErrorCode.NotFound, "Not found");
                            }
                        }
                        else
                        {
                            localFile = Path.Substring(1);
                            mime = "text/html";
                        }

                        ldi = new LogDataItem(m_headers, localFile, m_client.RemoteEndPoint.ToString(),
                                              ServerConfig.GetConfig());

                        var handler = GetHandlerForFilename(localFile, mime, method);

                        LogPageAccess(ldi);

                        HttpCachePolicy globalPolicy = CheckGlobalCachePolicy(localFile);
                        if (globalPolicy != null) HttpContext.Current.Response.Cache = globalPolicy;

                        // Now pass the request processing onto the relevant handler
                        if (handler == null)
                        {
                            throw new HttpException(Resources.NoHttpHandler);
                        }

                        handler.ProcessRequest(HttpContext.Current);

                        //Do the final flush to the server
                        FlushResponse(true);

                        EndOfRequest();

                        var disposableHandler = handler as IDisposable;
                        if (disposableHandler != null)
                        {
                            disposableHandler.Dispose();
                        }
                    }
                }
                catch (Exception e)
                {
                    if (Environment.OSVersion.Platform == PlatformID.Unix)
                    {
                        Console.WriteLine("Request exception: " + e.Message);
                    }

                    HandleRequestException(e, ldi);
                    return;
                }
                finally
                {
                    //Get rid of the uploaded content if available.  This will delete any temp files
                    if (HttpContext.Current.Request.RawPostContent != null)
                    {
                        HttpContext.Current.Request.RawPostContent.Dispose();
                        HttpContext.Current.Request.RawPostContent = null;
                    }
                }
            }
        }

        private bool IsSubDirectoryOf(string candidate, string other)
        {
            var isChild = false;
            try
            {
                var candidateInfo = new DirectoryInfo(candidate);
                var otherInfo = new DirectoryInfo(other);

                while (candidateInfo.Parent != null)
                {
                    if (string.Compare(candidateInfo.Parent.FullName, otherInfo.FullName, true) == 0)
                    {
                        isChild = true;
                        break;
                    }
                    else candidateInfo = candidateInfo.Parent;
                }
            }
            catch (Exception error)
            {
                var message = String.Format("Unable to check directories {0} and {1}: {2}", candidate, other, error);
                Trace.WriteLine(message);
            }

            return isChild;
        }

        public static string GetCasedFileNameFromCaselessName(string caselessName)
        {
            // TODO: skip on Windows (since the case is not relevent)

            // remove any relative pathing
            caselessName = System.IO.Path.GetFullPath(caselessName);

            var searchFile = System.IO.Path.GetFileName(caselessName);
            var searchPath = System.IO.Path.GetDirectoryName(caselessName);

            // crop any drive name
            string driveName;

            var index = searchPath.IndexOf(':');
            if (index > 0)
            {
                // the colon plus the first backslash
                driveName = caselessName.Substring(0, index + 2);
                searchPath = searchPath.Substring(index + 1);
            }
            else
            {
                // there is no drive (i.e. Linux)
                driveName = "/";
            }

            // get the directory list - being platform-agnostic (works on linux or windows)
            var directoryList = searchPath.Split(new char[] { System.IO.Path.DirectorySeparatorChar }).Where(s => !string.IsNullOrEmpty(s));

            // put in the drive name if the OS supports it
            string buildPath = driveName;

            // build up a case-sensitive path
            foreach (var dir in directoryList)
            {
                var casedDirectory = Directory.GetDirectories(buildPath).FirstOrDefault(d => string.Compare(System.IO.Path.GetFileName(d), dir, true) == 0);

                if (casedDirectory == null)
                {
                    return null;
                }

                buildPath = casedDirectory;
            }

            var casedFile = Directory.GetFiles(buildPath).FirstOrDefault(f => string.Compare(System.IO.Path.GetFileName(f), searchFile, true) == 0);

            if (casedFile == null)
            {
                return null;
            }

            return casedFile;
        }

        private Exception LastError { get; set; }

        private string GetCustomErrorPage(HttpErrorCode errorCode)
        {
            string[] extensions = new string[] { "htm", "html", "aspx" };
             
            var folder = ServerConfig.GetConfig().CustomErrorFolder;
            if (folder == null) return null;
            if (!Directory.Exists(folder)) return null;

            string path = System.IO.Path.Combine(folder, string.Format("{0}.", (int)errorCode));

            string returnPage = null;
            foreach (var ext in extensions)
            {
                string checkPath = path + ext;

                if (!File.Exists(checkPath)) continue;

                try
                {
                    using (TextReader reader = File.OpenText(checkPath))
                    {
                        returnPage = reader.ReadToEnd();
                        break;
                    }
                }
                catch
                {
                    returnPage = null;
                }
            }

            return returnPage;
        }

        private string GetStatusForErrorCode(HttpErrorCode error)
        {
            switch (error)
            {
                case HttpErrorCode.BadRequest: // 400
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusBadRequest);
                case HttpErrorCode.Unauthorized: // 401
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusUnauthorized);
                case HttpErrorCode.PaymentRequired: // 402
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusPaymentRequired);
                case HttpErrorCode.Forbidden: // 403
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusForbidden);
                case HttpErrorCode.NotFound: // 404
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusFileNotFound);
                case HttpErrorCode.MethodNotAllowed: // 405
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusNotAllowed);
                case HttpErrorCode.NotAcceptable: // 406
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusNotAcceptable);
                case HttpErrorCode.ProxyAuthenticationRequired: // 407
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusProxyAuthRequired);
                case HttpErrorCode.RequestTimeout: // 408
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusRequestTimeout);
                case HttpErrorCode.Conflict: // 409
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusConflict);
                case HttpErrorCode.Gone: // 410
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusGone);
                case HttpErrorCode.LengthRequired: // 411
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusLengthRequired);
                case HttpErrorCode.PreconditionFailed: // 412
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusPreconditionFailed);
                case HttpErrorCode.RequestEntityTooLarge: // 413
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusRequestEntityTooLarge);
                case HttpErrorCode.RequestURITooLong: // 414
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusRequestURITooLong);
                case HttpErrorCode.UnsupportedMediaType: // 415
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusUnsupportedMediaType);
                case HttpErrorCode.RequestedRangeNotSatisfiable: // 416
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusRequestedRangeNotSatisfiable);
                case HttpErrorCode.ExpectationFailed: // 417
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusExpectationFailed);

                case HttpErrorCode.InternalServerError: // 500
                    return StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusInternalServerError);
                default:
                    return string.Format(StringHelper.ConvertVerbatimLineEndings(Resources.HttpStatusGeneric), ((int)error).ToString());
            }
        }

        private bool ProcessRequestForVirtualFile(string requestPath)
        {
            if (HostingEnvironment.VirtualPathProvider == null)
            {
                return false;
            }

            VirtualFile vf = null;
            if (HostingEnvironment.VirtualPathProvider.FileExists(requestPath))
            {
                vf = HostingEnvironment.VirtualPathProvider.GetFile(requestPath);
            }
            else
            {
                return false;
            }

            if (vf == null)
            {
                throw new HttpException(404, Resources.HttpFileNotFound);
            }

            HttpContext.Current.Response.ContentType = MimeMapping.GetMimeMapping(requestPath);
            HttpContext.Current.Response.WriteVirtualFile(vf);
            FlushResponse(true);

            return true;
        }

        private string ParseStackTrace(string stackTrace)
        {
            StringBuilder builder = new StringBuilder();

            string[] calls = stackTrace.Split('\n');
            foreach (string call in calls)
            {
                string line = call.Trim('\r');
                builder.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{0}<br/>", line);

                if (line.EndsWith("Page_Load()"))
                {
                    break;
                }
            }

            return builder.ToString();
        }

        private void LogError(string errorInfo, LogDataItem ldi)
        {
            if (m_logProvider != null)
            {
                try
                {
                    m_logProvider.LogPadarnError(errorInfo, ldi);
                }
                catch
                {
                    // swallow logging exceptions to prevent a bad logging plug-in from tearing us down
                    // TODO: maybe log these errors somewhere?
                }
            }
        }

        private bool RequestRequiresAuthentication()
        {
            // Crawl the request path and check each virtual directory 
            string normalizedPath = this.Path.Trim('/');

            if (String.IsNullOrEmpty(normalizedPath))
            {
                return false;
            }

            string[] directories = normalizedPath.Split('/');

            foreach (string directory in directories)
            {
                if (UrlPath.IsVirtualDirectory(directory))
                {
                    VirtualDirectoryMapping dir =
                        ServerConfig.GetConfig().VirtualDirectories[directory];
                    if (dir.RequiresAuthentication)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static string GetDefaultDocument(string physicalPath)
        {
            string defaultDocument = "default.html";

            var conf = ServerConfig.GetConfig();
            if (conf == null)
            {
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    Console.WriteLine("No server config");
                }
            }
            else if (conf.DefaultDocuments == null)
            {
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    Console.WriteLine("No default documents");
                }
            }
            else
            {
                foreach (string document in conf.DefaultDocuments)
                {
                    string localFile = GetCasedFileNameFromCaselessName(System.IO.Path.Combine(physicalPath, document));
                    if(localFile != null)
                    {
                        defaultDocument = document;
                        break;
                    }
                }
            }
            return defaultDocument;
        }

        private bool AuthenticateRequest()
        {
            string challengeResponse = m_headers["HTTP_AUTHORIZATION"];
            if (challengeResponse == null)
                return false;
            int separator = challengeResponse.IndexOf(' ');
            string mode = challengeResponse.Substring(0, separator);
            if (string.Compare(mode, ServerConfig.GetConfig().Authentication.Mode, true) != 0)
            {
                return false;
            }

            string credentials = challengeResponse.Substring(separator + 1);
            Authentication auth;

            switch (mode.ToLower())
            {
                case "basic":
                    auth = new BasicAuthentication();
                    break;
                case "digest":
                    auth = new DigestAuthentication();
                    break;
                default:
                    throw new NotSupportedException(String.Format("Authorization type {0} is not supported.", mode));
            }

            return auth.AcceptCredentials(HttpContext.Current, credentials);
        }

        private void SendAuthRequest()
        {
            /*
             * "HTTP/1.0 401 UNAUTHORIZED " +
                                "Server: SokEvo/1.0 " +
                                "Date: Sat, 27 Nov 2004 10:18:15 GMT " +
                                "WWW-Authenticate: Basic realm=\"SokEvo\" " +
                                "Content-Type: text/html " +
                                "Content-Length: 311 " +
                                "     " +
             */
            //string resp = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/1999/REC-html401-19991224/loose.dtd\"> " +
            //                    "<HTML> " +
            //                    "  <HEAD> " +
            //                    "    <TITLE>Error</TITLE> " +
            //                    "    <META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=ISO-8859-1\"> " +
            //                    "  </HEAD> " +
            //                    "  <BODY><H1>401 Unauthorised.</H1></BODY> " +
            //                    "</HTML> ";
            Status = "HTTP/1.1 401 UNAUTHORIZED";
            Authentication auth;

            switch (ServerConfig.GetConfig().Authentication.Mode.ToLower())
            {
                case "basic":
                    //responseHeaders.Append(String.Format("WWW-Authenticate: Basic realm=\"{0}\"\r\n", ServerConfig.GetConfig().Authentication.Realm));
                    auth = new BasicAuthentication();
                    break;
                case "digest":
                    auth = new DigestAuthentication();
                    break;
                default:
                    throw new Exception();
            }

            auth.OnEndRequest(HttpContext.Current, EventArgs.Empty);
            HttpContext.Current.Response.SendStatus(401, "Access Denied", true);
        }

        private bool AuthenticationEnabled
        {
            get
            {
                AuthenticationConfiguration authCfg;
                if ((authCfg = ServerConfig.GetConfig().Authentication) != null)
                {
                    return authCfg.Enabled;
                }

                return false;
            }
        }

        private bool HasAuthorizationHeader
        {
            get { return !String.IsNullOrEmpty(m_headers["HTTP_AUTHORIZATION"]); }
        }

        /// <summary>
        /// Gets the remaing request content.  Primarly used for posted data
        /// </summary>
        /// <returns></returns>
        private HttpRawRequestContent GetEntireRawContent()
        {
            int length = HttpContext.Current.Request.ContentLength;
            Debug.WriteLineIf(EnableTracing, String.Format("Content-Length: {0}\nMax Request Length: {1}", length, HttpRuntimeConfig.GetConfig().MaxRequestLengthBytes), TRACE);
            //check to max sure we have not exceeded the maxlength
            if (length > HttpRuntimeConfig.GetConfig().MaxRequestLengthBytes)
            {
                throw new HttpException(500, Resources.Max_request_length_exceeded);
            }
            //See if we only downloaded partial data
            if (m_partialDownload)
            {
                Debug.WriteLineIf(EnableTracing, "Partial data download.", TRACE);

                int totalLength = Int32.Parse(m_httpRawRequestContent.Headers["HTTP_CONTENT_LENGTH"]) + m_httpRawRequestContent.LengthOfHeaders;
                //Create a new raw content to download the data
                m_httpRawRequestContent = new HttpRawRequestContent(HttpRuntimeConfig.GetConfig().RequestLengthDiskThresholdBytes,
                                3024,
                              ((System.Net.IPEndPoint)m_client.RemoteEndPoint).Address,
                              m_httpRawRequestContent);

                //Temp buffer
                byte[] buffer;

                //See if the browser wants a response code of 100 to continue sending posted data
                if (m_headers["HTTP_EXPECT"] != null && m_headers["HTTP_EXPECT"] == "100" && HttpContext.Current.Request.ContentType.StartsWith("multipart/form-data"))
                {
                    Debug.WriteLineIf(EnableTracing, "Request contains multi-part form data", TRACE);

                    //Tell the browser to continue sending data if required.  This is needed when uploading bigger files.
                    //HTTP/1.1 100 Continue
                    //TODO do we have to send the server headers for example?
                    /*
                       HTTP/1.1 100 Continue
                       Server: Microsoft-IIS/4.0
                       Date: Mon, 15 Apr 2002 00:49:27 GMT

                     */
                    buffer = Encoding.ASCII.GetBytes(Resources.HttpContinue);
                    m_output.Write(buffer, 0, buffer.Length);
                    m_output.Flush();
                }

                //Loop until we get all the content downloaded
                int received = 0, totalReceived = 0;
                buffer = new byte[10240];
                while (m_httpRawRequestContent.Length < totalLength)
                {
                    if (!m_client.Connected) break;

                    if (m_client.Available > 0)
                    {
                        received = m_client.Receive(buffer);
                        m_httpRawRequestContent.AddBytes(buffer, 0, received);
                        totalReceived += received;
                        Debug.WriteLineIf(EnableTracing, String.Format("Bytes received: {0}", totalReceived), TRACE);
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                m_httpRawRequestContent.DoneAddingBytes();
                m_partialDownload = false;
            }

            return m_httpRawRequestContent;
        }

        /// <summary>
        /// Retreives the entire Http request and stores it in a HttpRawRequestContent
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private HttpRawRequestContent GetPartialRawRequestContent(Server.SocketWrapperBase client)
        {
            HttpRawRequestContent rawContent = new HttpRawRequestContent(
                  HttpRuntimeConfig.GetConfig().RequestLengthDiskThresholdBytes,
                  3072/*Use 3k of memory then go to file if we go out of this threshold*/,
                  ((System.Net.IPEndPoint)client.RemoteEndPoint).Address);

            int received = -1;

            //TODO: I don't like this magic number, needs to get cleaned up
            // was fixed in 12259, but may have introduced other bugs (reverted in 13136)
            byte[] buffer = new byte[10240];

            received = client.Receive(buffer);

            if (received > 0)
            {
                rawContent.AddBytes(buffer, 0, received);
                rawContent.DoneAddingBytes();

                if (rawContent.HttpMethod == HttpMethod.POST && rawContent.Headers["HTTP_CONTENT_LENGTH"] == null)
                {
                    //it's a post but the content length has not been downloaded yet.
                    int retryCount = 5;
                    rawContent = DownloadUntilContentLengthHeader(rawContent, client, ref retryCount);
                }
                //Check for the content length header to see if there is more data to download
                if (rawContent.Headers["HTTP_CONTENT_LENGTH"] != null)
                {
                    m_partialDownload = true;
                }
                else
                {
                    m_partialDownload = false;
                }
            }
            else
            {
                //set the rawcontent as done
                rawContent.DoneAddingBytes();
            }

            return rawContent;
        }

        private HttpRawRequestContent DownloadUntilContentLengthHeader(HttpRawRequestContent content, Server.SocketWrapperBase client, ref int retryCount)
        {
            HttpRawRequestContent newContent = null;
            if (retryCount < 0)
            {
                newContent = new HttpRawRequestContent(HttpRuntimeConfig.GetConfig().RequestLengthDiskThresholdBytes,
                3072/*Use 3k of memory then go to file if we go out of this threshold*/,
                ((System.Net.IPEndPoint)client.RemoteEndPoint).Address);
                newContent.DoneAddingBytes();
            }
            else
            {
                //Some browsers like taking their time to send the post data so just wait a bit
                Thread.Sleep(50);
                if (content.Headers["HTTP_CONTENT_LENGTH"] == null)
                {
                    newContent = new HttpRawRequestContent(HttpRuntimeConfig.GetConfig().RequestLengthDiskThresholdBytes,
                      3072/*Use 3k of memory then go to file if we go out of this threshold*/,
                      ((System.Net.IPEndPoint)client.RemoteEndPoint).Address);
                    byte[] buffer = content.GetAsByteArray();
                    newContent.AddBytes(buffer, 0, buffer.Length);
                    content.Dispose();
                    if (client.Available > 0)
                    {
                        buffer = new byte[10240];
                        int received = -1;
                        received = client.Receive(buffer);
                        if (received != -1)
                        {
                            newContent.AddBytes(buffer, 0, received);
                            newContent.DoneAddingBytes();
                            if (newContent.Headers["HTTP_CONTENT_LENGTH"] == null)
                            {
                                --retryCount;
                                newContent = DownloadUntilContentLengthHeader(newContent, client, ref retryCount);
                            }
                        }
                    }
                    else
                    {
                        newContent.DoneAddingBytes();
                        --retryCount;
                        newContent = DownloadUntilContentLengthHeader(newContent, client, ref retryCount);
                    }
                }
            }

            return newContent;
        }

        private bool GetContentInfo()
        {
            string requestLine = m_httpRawRequestContent.ReadContentInfo();
            if (requestLine == null)
                throw new InvalidOperationException(String.Format(Resources.UnsupportedMethod, requestLine));
            if (m_httpRawRequestContent.HttpMethod == HttpMethod.Unknown ||
                m_httpRawRequestContent.HttpVersion == null ||
                m_httpRawRequestContent.Path == null)
                return false;
            else
                return true;

        }

        internal string Path
        {
            get
            {
                return m_httpRawRequestContent.Path;
            }
            set
            {
                m_httpRawRequestContent.Path = value;
            }
        }

        internal HttpRawRequestContent HttpRawRequestContent
        {
            get { return m_httpRawRequestContent; }
        }


#endregion // Private Methods
    }
}
