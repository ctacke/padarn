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
// Copyright (c) 2007-2008 OpenNETCF Consulting, LLC                        
//                                                                     

namespace OpenNETCF.Web
{
    using System.Collections.Specialized;
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using OpenNETCF.WindowsCE;
    using System.IO;
    using OpenNETCF.Security;
    using OpenNETCF.Security.Principal;

    /// <summary>
    /// Enables Padarn to read the HTTP values sent by a client during a Web request.
    /// </summary>
    public sealed class HttpRequest
    {
        private HttpWorkerRequest m_wr;
        private HttpContext m_context;
        private HttpValueCollection m_queryString;
        private HttpBrowserCapabilities m_browser;
        private byte[] m_queryStringBytes;
        private string[] m_acceptTypes;
        private HttpValueCollection m_form;
        private HttpFileCollection m_files;
        private Encoding m_contentEncoding = null;
        private List<MultipartContentItem> m_multipartContentElements;
        private HttpCookieCollection m_cookies;

        /// <summary>
        /// Gets or sets information about the requesting client's browser capabilities.
        /// </summary>
        public HttpBrowserCapabilities Browser
        {
            get
            {
                if (m_browser == null)
                {
                    m_browser = new HttpBrowserCapabilities(Headers);
                }
                return m_browser;
            }
            internal set { m_browser = value; }
        }

        /// <summary>
        /// Returns the Headers associated with the HttpRequest
        /// </summary>
        public NameValueCollection Headers { get; set; }

        /// <summary>
        /// Returns the raw web request query string
        /// </summary>
        public string RawQueryString
        {
            get { return m_wr.GetQueryString(); }
        }

        /// <summary>
        /// Forcibly terminates the underlying TCP connection, causing any outstanding I/O to fail.
        /// </summary>
        public void Abort()
        {
            m_wr.CloseConnection();
        }

        /// <summary>
        /// Gets the WindowsIdentity type for the current user.
        /// </summary>
        public IIdentity LogonUserIdentity
        {
            get
            {
                return m_context.User.Identity;
            }
        }

        /// <summary>
        /// Gets information about the URL of the current request.
        /// </summary>
        /// <value>
        /// A Uri object containing information regarding the URL of the current request.
        /// </value>
        public Uri Url { get; internal set; }

        /// <summary>
        /// Gets the collection of HTTP query string variables.
        /// </summary>
        /// <remarks>A NameValueCollection containing the collection of query string variables sent by the client. For example, If the request URL is http://www.opennetcf.com/default.aspx?id=44 then the value of QueryString is "id=44".</remarks>
        public NameValueCollection QueryString
        {
            get
            {
                if (m_queryString == null)
                {
                    m_queryString = new HttpValueCollection();
                    byte[] queryStringBytes = this.QueryStringBytes;
                    if (queryStringBytes != null)
                    {
                        if (queryStringBytes.Length != 0)
                        {
                            this.m_queryString.FillFromEncodedBytes(queryStringBytes, this.QueryStringEncoding, true);
                        }
                    }
                }
                return m_queryString;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the request has been authenticated.
        /// </summary>
        public bool IsAuthenticated
        {
            get 
            {
                if (LogonUserIdentity != null)
                {
                    return LogonUserIdentity.IsAuthenticated;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicting whether the HTTP connection uses secure sockets (that is, HTTPS).
        /// </summary>
        public bool IsSecureConnection
        {
            get
            {
                return m_wr.IsSecure();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the request is from the local computer.
        /// </summary>
        public bool IsLocal
        {
            get
            {
                string remoteaddr = m_wr.GetRemoteAddress();

                System.Net.IPHostEntry hostent = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (System.Net.IPAddress address in hostent.AddressList)
                {
                    if (string.Compare(address.ToString(), remoteaddr) == 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the IP host address of the remote client.
        /// </summary>
        public string UserHostAddress
        {
            get { return Headers["HTTP_REMOTE_ADDR"]; }
        }

        /// <summary>
        /// Gets or sets the character set of the entity-body.
        /// </summary>
        public Encoding ContentEncoding
        {
            get
            {
                if (m_contentEncoding == null)
                {
                    if (Headers["HTTP_CONTENT_ENCODING"] != null)
                    {
                        m_contentEncoding = Encoding.GetEncoding(Headers["HTTP_CONTENT_ENCODING"]);
                    }
                    else
                        m_contentEncoding = Encoding.UTF8;
                }

                return m_contentEncoding;
            }
        }

        /// <summary>
        /// Gets or sets the MIME content type of the incoming request.
        /// </summary>
        /// <remarks>A string representing the MIME content type of the incoming request, for example, "text/html".</remarks>
        public string ContentType
        {
            get
            {
                return Headers["HTTP_CONTENT_TYPE"];
            }
        }

        /// <summary>
        /// Specifies the length, in bytes, of content sent by the client.
        /// </summary>
        public int ContentLength
        {
            get
            {
                try
                {
                    string length = Headers["HTTP_CONTENT_LENGTH"];
                    if (string.IsNullOrEmpty(length)) return 0;

                    return Int32.Parse(Headers["HTTP_CONTENT_LENGTH"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets a string array of client-supported MIME accept types.
        /// </summary>
        public string[] AcceptTypes
        {
            get
            {
                if (m_acceptTypes == null && m_wr != null)
                {
                    m_acceptTypes = ParseAcceptHeader(Headers["HTTP_ACCEPT"]);
                }
                return m_acceptTypes;
            }
        }

        /// <summary>
        /// Gets the HTTP data transfer method (such as GET, POST, or HEAD) used by the client.
        /// </summary>
        public string HttpMethod
        {
            get
            {
                return HttpContext.Current.WorkerRequest.GetHttpVerbName();
            }
        }

        /// <summary>
        /// Gets the HTTP data transfer method (GET or POST) used by the client.
        /// </summary>
        public string RequestType
        {
            get
            {
                return this.HttpMethod;
            }
        }

        /// <summary>
        /// Gets the virtual path of the current request.
        /// </summary>
        public string Path
        {
            get { return HttpContext.Current.WorkerRequest.GetUriPath(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public NameValueCollection Form
        {
            get
            {
                if (this.m_form == null)
                {
                    //extract all the fields from the form submitted
                    this.m_form = new HttpValueCollection();
                    if (this.m_wr != null)
                    {
                        this.FillInFormCollection();
                    }
                    this.m_form.MakeReadOnly();

                    //Validate the form for any cross scripting
                    ValidateNameValueCollection(this.m_form, "Request.Form");
                }


                return this.m_form;
            }
        }

        /// <summary>
        /// Gets the collection of files uploaded by the client, in multipart MIME format.
        /// </summary>
        public HttpFileCollection Files
        {
            get
            {
                if (this.m_files == null)
                {
                    this.m_files = new HttpFileCollection();
                    if (this.m_wr != null)
                    {
                        this.FillInFilesCollection();
                    }
                }
                return this.m_files;
            }
        }

        public HttpCookieCollection Cookies
        {
            get
            {
                if (this.m_cookies == null)
                {
                    this.m_cookies = new HttpCookieCollection(null, false);
                    if (this.m_wr != null)
                    {
                        this.FillInCookiesCollection(this.m_cookies, true);
                    }
                }
                return this.m_cookies;
            }
        }

        /// <summary>
        /// The raw http content uploaded by the browser
        /// </summary>
        internal HttpRawRequestContent RawPostContent { get; set; }

        private HttpInputStream _inputStream;

        public Stream InputStream
        {
            get
            {
                if (_inputStream == null)
                {
                    HttpRawRequestContent data = this.RawPostContent;
                    if (data != null)
                    {
                        _inputStream = new HttpInputStream(data, data.LengthOfHeaders, data.Length - data.LengthOfHeaders);
                    }
                    else
                    {
                        _inputStream = new HttpInputStream(null, 0, 0);
                    }
                }

                return _inputStream;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wr"></param>
        /// <param name="context"></param>
        public HttpRequest(HttpWorkerRequest wr, HttpContext context)
        {
            this.m_wr = wr;
            this.m_context = context;
        }

        internal string QueryStringText
        {
            get { return m_wr.GetQueryString(); }
        }

        internal byte[] QueryStringBytes
        {
            get
            {
                if ((m_queryStringBytes == null) && (m_wr != null))
                {
                    m_queryStringBytes = m_wr.GetQueryStringRawBytes();
                }
                return m_queryStringBytes;
            }
        }

        internal Encoding QueryStringEncoding
        {
            get
            {
                Encoding contentEncoding = this.ContentEncoding;
                if (!contentEncoding.Equals(Encoding.Unicode))
                {
                    return contentEncoding;
                }
                return Encoding.UTF8;
            }
        }

        private string[] ParseAcceptHeader(string s)
        {
            if (s == null || s.Length == 0)
            {
                return null;
            }

            //Parse the items which are comma delimited
            ArrayList list = new ArrayList();
            int startIndex = 0;
            int length = s.Length;
            while (startIndex < length)
            {
                int index = s.IndexOf(',', startIndex);
                if (index < 0)
                {
                    index = length;
                }
                list.Add(s.Substring(startIndex, index - startIndex));
                startIndex = index + 1;
                if ((startIndex < length) && (s[startIndex] == ' '))
                {
                    startIndex++;
                }
            }

            //Make sure there is something in the list
            int count = list.Count;
            if (count == 0)
            {
                return null;
            }

            //return a string[]
            string[] array = new string[count];
            list.CopyTo(0, array, 0, count);
            return array;
        }

        /// <summary>
        /// Validates the name value collection created for POST data
        /// </summary>
        /// <param name="nvc"></param>
        /// <param name="collectionName"></param>
        private void ValidateNameValueCollection(NameValueCollection nvc, string collectionName)
        {
            int count = nvc.Count;
            for (int i = 0; i < count; i++)
            {
                string key = nvc.GetKey(i);
                if ((key == null))
                {
                    string str2 = nvc.Get(i);
                    if (!string.IsNullOrEmpty(str2))
                    {
                        ValidateString(str2, key, collectionName);
                    }
                }
            }
        }

        /// <summary>
        /// Validates a string for any cross site scripting
        /// </summary>
        /// <param name="s"></param>
        /// <param name="valueName"></param>
        /// <param name="collectionName"></param>
        private void ValidateString(string s, string valueName, string collectionName)
        {
            s = StringHelper.RemoveNullCharacters(s);
            int matchIndex = 0;
            if (CrossSiteScriptingValidation.IsDangerousString(s, out matchIndex))
            {
                string str = valueName + "=\"";
                int startIndex = matchIndex - 10;
                if (startIndex <= 0)
                {
                    startIndex = 0;
                }
                else
                {
                    str = str + "...";
                }
                int length = matchIndex + 20;
                if (length >= s.Length)
                {
                    length = s.Length;
                    str = str + s.Substring(startIndex, length - startIndex) + "\"";
                }
                else
                {
                    str = str + s.Substring(startIndex, length - startIndex) + "...\"";
                }
                throw new HttpRequestValidationException(Resources.Dangerous_input_detected);
            }
        }

        private void FillInFormCollection()
        {
            if ((this.m_wr != null) && this.ContentLength > 0)
            {
                string contentType = this.ContentType;
                if (contentType != null)
                {
                    if (contentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
                    {
                        byte[] bytes = null;
                        if (this.RawPostContent != null)
                        {
                            bytes = this.RawPostContent.GetAsByteArray();
                            if (bytes != null)
                            {
                                try
                                {
                                    this.m_form.FillFromEncodedBytes(bytes, this.ContentEncoding, false);
                                }
                                catch (Exception exception)
                                {
                                    throw new HttpException(Resources.Invalid_urlencoded_form_data, exception);
                                }
                            }
                        }
                    }
                    else if (ContentType.StartsWith("multipart/form-data", StringComparison.OrdinalIgnoreCase))
                    {
                        List<MultipartContentItem> multipartContent = this.GetMultipartContent();
                        if (multipartContent != null)
                        {
                            for (int i = 0; i < multipartContent.Count; i++)
                            {
                                if (multipartContent[i].IsFormItem)
                                {
                                    this.m_form.Add(multipartContent[i].Name, multipartContent[i].GetAsString(this.ContentEncoding));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void FillInFilesCollection()
        {
            if (this.ContentType == null)
            {
                return;
            }
            if ((this.m_wr != null) && this.ContentType.StartsWith("multipart/form-data", StringComparison.OrdinalIgnoreCase))
            {
                List<MultipartContentItem> multipartContent = this.GetMultipartContent();
                if (multipartContent != null)
                {
                    for (int i = 0; i < multipartContent.Count; i++)
                    {
                        if (multipartContent[i].IsFile)
                        {
                            HttpPostedFile asPostedFile = multipartContent[i].GetAsPostedFile();
                            ValidateString(asPostedFile.FileName, "filename", "Request.Files");
                            this.m_files.AddFile(multipartContent[i].Name, asPostedFile);
                        }
                    }
                }
            }
        }

        internal void FillInCookiesCollection(HttpCookieCollection cookieCollection, bool includeResponse)
        {
            HttpCookie cookie = null;

            string cookieHeader = this.Headers["HTTP_COOKIE"];

            int startIndex = 0;
            int length = (cookieHeader == null) ? 0 : cookieHeader.Length;

            while (startIndex < length)
            {
                int pos = startIndex;

                while (pos < length)
                {
                    char ch = cookieHeader[pos];
                    if (ch == ';')
                    {
                        break;
                    }
                    pos++;
                }

                string cookieString = cookieHeader.Substring(startIndex, pos - startIndex).Trim();
                startIndex = pos + 1;
                if (cookieString.Length != 0)
                {
                    HttpCookie tmpCookie = CreateCookieFromString(cookieString);
                    if (cookie != null)
                    {
                        string name = tmpCookie.Name;
                        if (!String.IsNullOrEmpty(name) && name[0] == '$')
                        {
                            if (StringHelper.EqualsIgnoreCase(name, "$Path"))
                            {
                                cookie.Path = tmpCookie.Value;
                            }
                            else if (StringHelper.EqualsIgnoreCase(name, "$Domain"))
                            {
                                cookie.Domain = tmpCookie.Value;
                            }
                            continue;
                        }
                    }
                    cookieCollection.AddCookie(tmpCookie, true);
                    cookie = tmpCookie;
                }
            }

            // Handle includeResponse

        }

        internal void AddResponseCookie(HttpCookie cookie)
        {
            if (this.m_cookies != null)
            {
                this.m_cookies.AddCookie(cookie, true);
            }
        }

        internal void ResetCookies()
        {
            if (this.m_cookies != null)
            {
                this.m_cookies.Reset();
                this.FillInCookiesCollection(this.m_cookies, true);
            }
        }

        private List<MultipartContentItem> GetMultipartContent()
        {
            if (this.m_multipartContentElements == null)
            {
                byte[] multipartBoundary = this.GetMultipartBoundary();
                if (this.RawPostContent == null || multipartBoundary == null)
                {
                    return new List<MultipartContentItem>();
                }
                this.m_multipartContentElements = MultipartFormParser.Parse(this.RawPostContent, this.RawPostContent.Length, multipartBoundary, this.ContentEncoding);
            }
            return this.m_multipartContentElements;
        }

        private byte[] GetMultipartBoundary()
        {
            //Get the boundary as a byte[]
            var boundaryText = ExtractGetBoundaryTextFromContentType(this.ContentType);
            return Encoding.ASCII.GetBytes(boundaryText);
        }

        private const string BOUNDARY_ATTRIBUTE = "boundary=";
        private string ExtractGetBoundaryTextFromContentType(string contentType)
        {
            //Get the boundary as a byte[]
            //Sample boundary item
            //multipart/form-data; boundary=---------------------------5895116275398
            int boundaryIndex = contentType.IndexOf(BOUNDARY_ATTRIBUTE, 0, StringComparison.OrdinalIgnoreCase);
            if (boundaryIndex == -1)
            {
                return null;
            }

            //The beginning on the boundary items has two extra dashes
            var boundaryText = "--";
            boundaryIndex += BOUNDARY_ATTRIBUTE.Length;

            // boundary text *may* be quote delimited in some implementations
            if (contentType[boundaryIndex] == '"')
            {
                boundaryIndex++;
                var end = contentType.IndexOf('"', boundaryIndex);
                if (end > 0)
                {
                    boundaryText += contentType.Substring(boundaryIndex, end - boundaryIndex);
                }
                else
                {
                    boundaryText += contentType.Substring(boundaryIndex);
                }
            }
            else
            {
                boundaryText += contentType.Substring(boundaryIndex);
            }

            return boundaryText;
        }

        internal static HttpCookie CreateCookieFromString(string s)
        {
            HttpCookie cookie = new HttpCookie();

            int length = (s != null) ? s.Length : 0;
            int startIndex = 0;
            bool flag = true;
            int valueCount = 1;

            while (startIndex < length)
            {
                int endOfNameIndex;
                int index = s.IndexOf('&', startIndex);

                if (index < 0)
                {
                    index = length;
                }

                if (flag)
                {
                    endOfNameIndex = s.IndexOf('=', startIndex);
                    if ((endOfNameIndex >= 0) && (endOfNameIndex < index))
                    {
                        cookie.Name = s.Substring(startIndex, endOfNameIndex - startIndex);
                        startIndex = endOfNameIndex + 1;
                    }
                    else if (index == length)
                    {
                        cookie.Name = s;
                        return cookie;
                    }
                    flag = false;
                }
                
                endOfNameIndex = s.IndexOf('=', startIndex);

                if (((endOfNameIndex < 0) && (index == length)) && (valueCount == 0))
                {
                    cookie.Value = s.Substring(startIndex, length - startIndex);
                }
                else if ((endOfNameIndex >= 0) && (endOfNameIndex < index))
                {
                    cookie.Values.Add(s.Substring(startIndex, endOfNameIndex - startIndex), s.Substring(endOfNameIndex + 1, (index - endOfNameIndex) - 1));
                    valueCount++;
                }
                else
                {
                    cookie.Values.Add(null, s.Substring(startIndex, index - startIndex));
                    valueCount++;
                }
                startIndex = index + 1;
            }

            return cookie;
        }
    }
}