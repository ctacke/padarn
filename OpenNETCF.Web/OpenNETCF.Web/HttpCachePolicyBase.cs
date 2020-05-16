using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Serves as the base class for classes that contain methods for setting cache-specific HTTP headers and for controlling the ASP.NET page output cache.
    /// </summary>
    public abstract class HttpCachePolicyBase
    {
        /// <summary>
        /// When overridden in a derived class, sets the Cache-Control: s-maxage HTTP header to the specified time span.
        /// </summary>
        /// <param name="delta"></param>
        public virtual void SetProxyMaxAge(TimeSpan delta)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, registers a validation callback for the current response.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="data"></param>
        public virtual void AddValidationCallback(HttpCacheValidateHandler handler, Object data)
        {
            throw new NotImplementedException();
        }
    }
}
