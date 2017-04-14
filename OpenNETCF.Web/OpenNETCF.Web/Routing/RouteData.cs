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
    /// Encapsulates information about a route.
    /// </summary>
    public class RouteData
    {
        /// <summary>
        /// Initializes a new instance of the RouteData class. 
        /// </summary>
        public RouteData()
        {
        }

        /// <summary>
        /// Retrieves the value with the specified identifier.
        /// </summary>
        /// <param name="valueName"></param>
        /// <returns></returns>
        public string GetRequiredString(string valueName)
        {
            return Values[valueName].ToString();
        }

        /// <summary>
        /// Gets a collection of URL parameter values and default values for the route.
        /// </summary>
        public RouteValueDictionary Values 
        {
            get { throw new NotImplementedException(); }
            private set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a collection of custom values that are passed to the route handler but are not used when ASP.NET routing determines whether the route matches a request.
        /// </summary>
        public RouteValueDictionary DataTokens 
        {
            get { throw new NotImplementedException(); }
            private set { throw new NotImplementedException(); } 
        }

        /// <summary>
        /// Gets or sets the object that represents a route.
        /// </summary>
        public RouteBase Route
        {
            get { throw new NotImplementedException(); }
            private set { throw new NotImplementedException(); } 
        }
    }
}
