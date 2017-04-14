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
