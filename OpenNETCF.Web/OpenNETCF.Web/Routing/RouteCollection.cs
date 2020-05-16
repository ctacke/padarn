using System;

using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace OpenNETCF.Web.Routing
{
    /// <summary>
    /// Provides a collection of routes for Padarn routing.
    /// </summary>
    public class RouteCollection : Collection<RouteBase>
    {
        /// <summary>
        /// Initializes a new instance of the RouteCollection class. 
        /// </summary>
        public RouteCollection()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns information about the URL path that is associated with the named route, given the specified context, route name, and parameter values.
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public VirtualPathData GetVirtualPath(RequestContext requestContext, string name, RouteValueDictionary values)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns information about the URL path that is associated with the route, given the specified context and parameter values.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the requested route.</param>
        /// <param name="values">An object that contains the parameters for a route.</param>
        /// <returns>An object that contains information about the URL path that is associated with the route.</returns>
        public VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Provides an object for managing thread safety when you retrieve an object from the collection.
        /// </summary>
        /// <returns>An object that manages thread safety</returns>
        public IDisposable GetReadLock()
        {
            throw new NotImplementedException();
        }
    }
}
