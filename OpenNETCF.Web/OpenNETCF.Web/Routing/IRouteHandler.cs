using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Routing
{
    /// <summary>
    /// Defines the contract that a class must implement in order to process a request for a matching route pattern.
    /// </summary>
    public interface IRouteHandler
    {
        /// <summary>
        /// Provides the object that processes the request.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the request.</param>
        /// <returns>An object that processes the request.</returns>
        IHttpHandler GetHttpHandler(RequestContext requestContext);
    }
}
