using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Specifies how cookies are used for a Web application.
    /// </summary>
    public enum HttpCookieMode
    {
        /// <summary>
        /// The calling feature uses the query string to store an identifier regardless of whether the browser or device supports cookies.
        /// </summary>
        UseUri,
        /// <summary>
        /// Cookies are used to persist user data regardless of whether the browser or device supports cookies.
        /// </summary>
        UseCookies,
        /// <summary>
        /// Padarn determines whether the requesting browser or device supports cookies. If the requesting browser or device supports cookies then AutoDetect uses cookies to persist user data; otherwise, an identifier is used in the query string. If the browser or device supports the use of cookies but cookies are currently disabled, cookies are still used by the requesting feature.
        /// </summary>
        AutoDetect,
        /// <summary>
        /// Padarn determines whether to use cookies based on System.Web.HttpBrowserCapabilities setting. If the setting indicates that the browser or device supports cookies, cookies are used; otherwise, an identifier is used in the query string.
        /// </summary>
        UseDeviceProfile
    }
}
