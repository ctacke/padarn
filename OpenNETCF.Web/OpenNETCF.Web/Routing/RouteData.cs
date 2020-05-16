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
