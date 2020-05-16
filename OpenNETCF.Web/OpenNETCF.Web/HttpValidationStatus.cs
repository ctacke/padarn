using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Provides enumerated values that indicate cache validation status. 
    /// </summary>
    public enum HttpValidationStatus
    {
        /// <summary>
        /// Indicates that the request is treated as a cache miss and the page is executed. The cache is not invalidated.
        /// </summary>
        IgnoreThisRequest,
        /// <summary>
        /// Indicates that the cache is invalid. The item is evicted from the cache and the request is handled as a cache miss.
        /// </summary>
        Invalid,
        /// <summary>
        /// Indicates that the cache is valid.   
        /// </summary>
        Valid
    }
}
