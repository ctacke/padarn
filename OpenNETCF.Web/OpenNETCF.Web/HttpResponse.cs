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
//                                                                   
// Copyright (c) 2007-2010 OpenNETCF Consulting, LLC                        
//                                                                     

namespace OpenNETCF.Web
{
    using System;
    using System.IO;
    using System.Text;
    using Hosting;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents the response to a particular HTTP request.
    /// </summary>
    public sealed class HttpResponse
    {
        #region Fields

        private Encoding m_headerEncoding;
        private HttpWorkerRequest m_wr;
        private HttpContext m_context;
        private MemoryStream m_bufferedStream = null;
        private HttpCookieCollection m_cookies;

        internal long? ForcedContentLength { get; set; }

        internal HttpResponse(HttpWorkerRequest wr, HttpContext context)
        {
            ContentType = "text/html";
            Cache = new HttpCachePolicy();
            this.m_wr = wr;
            this.m_context = context;
        }

        #endregion // Fields

        #region Properties

        internal Stream BufferedStream
        {
            get { return m_bufferedStream; }
        }

        /// <summary>
        /// Specifies the length of the entity body in the HTTP response
        /// </summary>
        public long ContentLength
        {
            get { return m_bufferedStream.Length; }
            set { ForcedContentLength = value; }
        }

        /// <summary>
        /// Specifies the character encoding for the HTTP response
        /// </summary>
        public Encoding HeaderEncoding
        {
            get
            {
                if (m_headerEncoding == null)
                {
                    if ((m_headerEncoding == null) || m_headerEncoding.Equals(Encoding.Unicode))
                    {
                        m_headerEncoding = Encoding.UTF8;
                    }
                }
                return m_headerEncoding;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (value.Equals(Encoding.Unicode))
                {
                    throw new HttpException(string.Format("Invalid header encoding {0}", value.WebName));
                }
                if ((m_headerEncoding == null) || !m_headerEncoding.Equals(value))
                {
                    if (HeadersWritten)
                    {
                        throw new HttpException("Cannot set header encoding after headers sent");
                    }
                    m_headerEncoding = value;
                }
            }
        }

        private string m_contentType;
        /// <summary>
        /// Specifies the content type for the HTTP response
        /// </summary>
        public string ContentType
        {
            get { return m_contentType; }
            set
            {
                if (HeadersWritten)
                    throw new HttpException("Server cannot set content type after HTTP headers have been sent.");

                m_contentType = value;
            }
        }

        /// <summary>
        /// Gets the caching policy (expiration time, privacy, vary clauses) of a Web page.
        /// </summary>
        public HttpCachePolicy Cache { get; set; }

        internal bool HeadersWritten { get; set; }

        internal HttpRequest Request
        {
            get
            {
                if (this.m_context == null)
                {
                    return null;
                }
                return this.m_context.Request;
            }
        }

        /// <summary>
        /// Gets the response cookie collection.
        /// </summary>
        public HttpCookieCollection Cookies
        {
            get
            {
                if (this.m_cookies == null)
                {
                    this.m_cookies = new HttpCookieCollection(this, false);
                }
                return this.m_cookies;
            }
        }

        private int m_statusCode = 200;
        private string m_statusDescription = string.Empty;

        /// <summary>
        /// Gets or sets the HTTP status code of the output returned to the client.
        /// </summary>
        public int StatusCode 
        {
            get
            {
                var matches = Regex.Matches(m_wr.Status, @"HTTP/\d\.\d (?<code>\d{0,3}) (?<status>[\w\d ]*)", RegexOptions.IgnoreCase);
                return int.Parse(matches[0].Groups[1].Value);
            }
            set
            {
                m_statusCode = value;
                SetWorkerRequestStatus();
            }
        }

        /// <summary>
        /// Gets or sets the HTTP status string of the output returned to the client.
        /// </summary>
        public string StatusDescription 
        { 
            get
            {
                var matches = Regex.Matches(m_wr.Status, @"HTTP/\d\.\d (?<code>\d{0,3}) (?<status>[\w\d ]*)", RegexOptions.IgnoreCase);
                return matches[0].Groups[2].Value;
            }
            set
            {
                m_statusDescription = value;
                SetWorkerRequestStatus();
            }
        }

        private void SetWorkerRequestStatus()
        {
            m_wr.Status = string.Format("HTTP/1.1 {0} {1}\r\n", m_statusCode.ToString(), m_statusDescription);
        }

        #endregion // Properties

        /// <summary>
        /// Updates an existing cookie in the cookie collection.
        /// </summary>
        /// <param name="cookie"></param>
        public void SetCookie(HttpCookie cookie)
        {
            if (this.HeadersWritten)
            {
                throw new HttpException("Cannot modify cookies after headers are sent");
            }
            this.Cookies.AddCookie(cookie, false);
            this.OnCookieCollectionChange();
        }

        /// <summary>
        /// Adds an HTTP cookie to the intrinsic cookie collection.
        /// </summary>
        /// <param name="cookie"></param>
        public void AppendCookie(HttpCookie cookie)
        {
            if (this.HeadersWritten)
            {
                throw new HttpException("Cannot modify cookies after headers are sent");
            }
            this.Cookies.AddCookie(cookie, true);
            this.OnCookieAdd(cookie);
        }

        internal void BeforeCookieCollectionChange()
        {
            if (this.HeadersWritten)
            {
                throw new HttpException("Cannot modify cookies after headers are sent");
            }
        }

        internal void OnCookieAdd(HttpCookie cookie)
        {
            this.Request.AddResponseCookie(cookie);
        }

        internal void OnCookieCollectionChange()
        {
            this.Request.ResetCookies();
        }


        /// <summary>
        /// Append a header to the HttpResponse
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AppendHeader(string name, string value)
        {
            m_wr.SendKnownResponseHeader(name, value);
        }

        /// <summary>
        /// Clears all headers from the buffer stream.
        /// </summary>
        public void ClearHeaders()
        {
            m_wr.ClearHeaders();   
        }

        /// <summary>
        /// Writes a string of binary characters to the HTTP output stream.
        /// </summary>
        /// <param name="buffer">The bytes to write to the output stream.</param>
        public void BinaryWrite(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            m_wr.SendResponseFromMemory(buffer, buffer.Length);
        }

        /// <summary>
        /// Writes a string of binary characters to the HTTP output stream.
        /// </summary>
        /// <param name="buffer">The bytes to write to the output stream.</param>
        public void BinaryWrite(byte[] buffer, int offset, int length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            m_wr.SendResponseFromMemory(buffer, buffer.Length);
        }

        /// <summary>
        /// Write to the HttpResponse stream
        /// </summary>
        /// <param name="s"></param>
        public void Write(string s)
        {
            BinaryWrite(Encoding.UTF8.GetBytes(s));
        }

        /// <summary>
        /// Write a line to the HttpResponse stream
        /// </summary>
        /// <param name="s"></param>
        public void WriteLine(string s)
        {
            Write(string.Format("{0}\r\n", s));
        }

        /// <summary>
        /// Redirect the request to a different URL
        /// </summary>
        /// <param name="url"></param>
        public void Redirect(string url)
        {
            Redirect(url, true);
        }

        /// <summary>
        /// Flush the HttpResponse stream to the client.
        /// </summary>
        public void Flush()
        {
            m_wr.FlushResponse(false);
        }

        /// <summary>
        /// Gets a Boolean value indicating whether the client is being transferred to a new location.
        /// </summary>
        public bool IsRequestBeingRedirected { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the client is still connected to the server.
        /// </summary>
        /// <value>true if the client is currently connected; otherwise, false.</value>
        public bool IsClientConnected
        {
            get { return m_wr.IsClientConnected(); }
        }

        internal void Write(byte[] b)
        {
            m_wr.SendResponseFromMemory(b, b.Length);
        }

        internal void Redirect(string url, bool endResponse)
        {
            if (url == null)
            {
                throw new ArgumentException("url");
            }

            if (url.IndexOf('\n') >= 0)
            {
                throw new ArgumentException(Resources.CannotRedirectToNewLine);
            }

            IsRequestBeingRedirected = true;

            url = ConvertToFullyQualifiedRedirectUrlIfRequired(url);

            m_wr.Status = "HTTP/1.1 302 Found\r\n";
            string b = "<html><head><title>Object moved</title></head><body>\r\n" +
                      "<h2>Object moved to <a href=\"" + url + "\">here</a>.</h2>\r\n" +
                      "</body></html>\r\n";
            m_wr.SendKnownResponseHeader(Resources.HeaderContentType, "text/html");
            m_wr.SendKnownResponseHeader(Resources.HeaderContentLength, b.Length.ToString());
            m_wr.SendKnownResponseHeader(Resources.HeaderLocation, url);

            Write(b);
            if (endResponse)
            {
                Flush();
            }
        }

        internal void SendStatus(int status, string description, bool endResponse)
        {
            m_wr.Status = String.Format("HTTP/1.1 {0} {1}\r\n", status, description);

            if (endResponse)
                Flush();
        }

        private string ConvertToFullyQualifiedRedirectUrlIfRequired(string url)
        {
            //if (!url.StartsWith("http"))
            //{
            //    DefaultWorkerRequest ps = (DefaultWorkerRequest)m_wr;
            //    string host = ps.HttpRawRequestContent.Headers["HTTP_HOST"];
            //    url = UriCombine(host, url, ps.IsSecure());
            //}

            return url;
        }

        private static string UriCombine(string host, string virtualPath, bool isSSL)
        {
            StringBuilder uri = new StringBuilder();
            uri.Append("http");
            if (isSSL)
                uri.Append("s");
            uri.Append("://");
            uri.Append(host);

            if (!virtualPath.StartsWith("/"))
                uri.Append("/");

            uri.Append(virtualPath);

            return uri.ToString();
        }

        internal void WriteVirtualFile(VirtualFile vf)
        {
            using (Stream stream = vf.Open())
            {
                // Make sure all the bytes are written to the stream 
                // and move to the start
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                long length = stream.Length;
                if (length > 0)
                {
                    byte[] buffer = new byte[(int)length];
                    stream.Read(buffer, 0x0, (int)length);
                    BinaryWrite(buffer);
                    Flush();
                }
            }
        }
    }
}