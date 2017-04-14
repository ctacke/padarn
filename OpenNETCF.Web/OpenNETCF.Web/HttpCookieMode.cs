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
