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
    /// Provides enumerated values that are used to set the Cache-Control HTTP header. 
    /// </summary>
    /// <remarks>
    /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for more information
    /// </remarks>
    public enum HttpCacheability
    {
        /// <summary>
        /// Default value. Sets <b>Cache-Control: private</b> to specify that the response is cacheable only on the client and not by shared (proxy server) caches. 
        /// </summary>
        Private,
        /// <summary>
        /// Sets the <b>Cache-Control: no-cache</b> header. Without a field name, the directive applies to the entire request and a shared (proxy server) cache must force a successful revalidation with the origin Web server before satisfying the request. With a field name, the directive applies only to the named field; the rest of the response may be supplied from a shared cache. 
        /// </summary>
        NoCache,
        /// <summary>
        /// Specifies that the response is cached only at the origin server. Similar to the NoCache option. Clients receive a <b>Cache-Control: no-cache</b> directive but the document is cached on the origin server. Equivalent to ServerAndNoCache. 
        /// </summary>
        /// <remarks><b>This is currently unsupported by Padarn</b></remarks>
        Server,
        /// <summary>
        /// Applies the settings of both Server and NoCache to indicate that the content is cached at the server but all others are explicitly denied the ability to cache the response. 
        /// </summary>
        /// <remarks><b>This is currently unsupported by Padarn</b></remarks>
        ServerAndNoCache,
        /// <summary>
        /// Sets <b>Cache-Control: public</b> to specify that the response is cacheable by clients and shared (proxy) caches. 
        /// </summary>
        Public,
        /// <summary>
        /// Indicates that the response is cached at the server and at the client but nowhere else. Proxy servers are not allowed to cache the response. 
        /// </summary>
        /// <remarks><b>This is currently unsupported by Padarn</b></remarks>
        ServerAndPrivate
    }
}
