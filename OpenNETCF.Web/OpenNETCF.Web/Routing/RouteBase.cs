using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Routing
{
    /// <summary>
    /// Serves as the base class for all classes that represent a Padarn route.
    /// </summary>
    public abstract class RouteBase
    {
        /// <summary>
        /// Initializes the class for use by an inherited class instance. This constructor can only be called by an inherited class.
        /// </summary>
        protected RouteBase()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, returns route information about the request.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public abstract RouteData GetRouteData(HttpContextBase httpContext);

        /// <summary>
        /// When overridden in a derived class, checks whether the route matches the specified values, and if so, generates a URL and retrieves information about the route.
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public abstract VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values);
    }
}
