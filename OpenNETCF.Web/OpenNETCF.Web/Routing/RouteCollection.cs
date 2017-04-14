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
