using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web.Security.Cryptography;

namespace OpenNETCF.Web
{
    public delegate bool AuthenticationCallbackHandler(IAuthenticationCallbackInfo info);

    /// <summary>
    /// Represents a method that is called to validate a cached item before the item is served from the cache. 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="data"></param>
    /// <param name="validationStatus"></param>
    public delegate void HttpCacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus);
}
