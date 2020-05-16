using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Encapsulates the HTTP intrinsic object that enables Padarn to read the HTTP values that are sent by a client during a Web request. 
    /// </summary>
    /// <remarks>
    /// The HttpRequestWrapper class derives from the HttpRequestBase class and serves as a wrapper for the HttpRequest class. This class exposes the functionality of the HttpRequest class and exposes the HttpRequestBase type. The HttpRequestBase class enables you to replace the original implementation of the HttpRequest class in your application with a custom implementation, such as when you perform unit testing outside the ASP.NET pipeline.
    /// </remarks>
    public class HttpRequestWrapper : HttpRequestBase
    {
        private HttpRequest m_request;

        /// <summary>
        /// Initializes a new instance of the HttpRequestWrapper class by using the specified request object.
        /// </summary>
        /// <param name="httpRequest"></param>
        public HttpRequestWrapper(HttpRequest httpRequest)
        {
            m_request = httpRequest;
        }

        public override System.Collections.Specialized.NameValueCollection Headers
        {
            get { return m_request.Headers; }
        }

        public override System.Collections.Specialized.NameValueCollection QueryString
        {
            get { return m_request.QueryString; }
        }

        public override System.Collections.Specialized.NameValueCollection Form
        {
            get { return m_request.Form; }
        }

        public override string HttpMethod
        {
            get { return m_request.HttpMethod; }
        }
    }
}
