using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Routing
{
    /// <summary>
    /// Encapsulates information about an HTTP request that matches a defined route.
    /// </summary>
    public class RequestContext
    {
        /// <summary>
        /// Initializes a new instance of the RequestContext class.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="routeData"></param>
        public RequestContext(HttpContextBase httpContext, RouteData routeData)
        {
            HttpContext = httpContext;
            RouteData = routeData;
        }

        /// <summary>
        /// Gets information about the HTTP request.
        /// </summary>
        public HttpContextBase HttpContext { get; internal set; }

        /// <summary>
        /// Gets information about the requested route.
        /// </summary>
        public RouteData RouteData { get; internal set; }

    }
}