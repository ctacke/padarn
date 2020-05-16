using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Routing
{
    /// <summary>
    /// Represents information about the route and virtual path that are the result of generating a URL with the Padarn routing framework.
    /// </summary>
    public class VirtualPathData
    {
        /// <summary>
        /// Initializes a new instance of the VirtualPathData class. 
        /// </summary>
        /// <param name="route"></param>
        /// <param name="virtualPath"></param>
        public VirtualPathData(RouteBase route, string virtualPath)
        {
            Route = route;
            VirtualPath = virtualPath;
        }

        /// <summary>
        /// Gets or sets the route that is used to create the URL.
        /// </summary>
        public RouteBase Route { get; set; }
        
        /// <summary>
        /// Gets or sets the URL that was created from the route definition
        /// </summary>
        public string VirtualPath { get; set; }

        /// <summary>
        /// Gets the collection of custom values for the route definition.
        /// </summary>
        public RouteValueDictionary DataTokens { get; internal set; }

    }
}
