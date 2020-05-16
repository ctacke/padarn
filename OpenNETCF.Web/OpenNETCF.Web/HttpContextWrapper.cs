using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web
{
    public class HttpContextWrapper : HttpContextBase
    {
        private HttpContext m_context;

        /// <summary>
        /// Initializes a new instance of the HttpContextWrapper class by using the specified context object.
        /// </summary>
        /// <param name="httpContext"></param>
        public HttpContextWrapper(
                HttpContext httpContext
            )
        {
            m_context = httpContext;
        }
    }
}
