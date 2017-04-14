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
