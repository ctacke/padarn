using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Routing
{
    /// <summary>
    /// Provides properties and methods for defining a route and for obtaining information about the route.
    /// </summary>
    public class Route : RouteBase
    {
        /// <summary>
        /// Gets or sets custom values that are passed to the route handler, but which are not used to determine whether the route matches a URL pattern.
        /// </summary>
        public RouteValueDictionary DataTokens { get; set; }

        /// <summary>
        /// Initializes a new instance of the Route class, by using the specified URL pattern and handler class. 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="routeHandler"></param>
        public Route(string url, IRouteHandler routeHandler)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns information about the requested route. (Overrides RouteBase.GetRouteData(HttpContextBase).)
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns information about the URL that is associated with the route. (Overrides RouteBase.GetVirtualPath(RequestContext, RouteValueDictionary).)
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            throw new NotImplementedException();
        }
    }
}
