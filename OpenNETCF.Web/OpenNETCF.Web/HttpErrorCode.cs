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
    /// 
    /// </summary>
    public enum HttpErrorCode
    {
        /// <summary>
        /// The request could not be understood by the server due to malformed syntax.
        /// </summary>
        BadRequest = 400,
        /// <summary>
        /// The request requires user authentication.
        /// </summary>
        Unauthorized = 401,
        /// <summary>
        /// This code is reserved for future use.
        /// </summary>
        PaymentRequired = 402,
        /// <summary>
        /// The server understood the request, but is refusing to fulfill it. Authorization will not help and the request SHOULD NOT be repeated. 
        /// </summary>
        Forbidden = 403,
        /// <summary>
        /// The server has not found anything matching the Request-URI. No indication is given of whether the condition is temporary or permanent. 
        /// </summary>
        NotFound = 404,
        /// <summary>
        /// The method specified in the Request-Line is not allowed for the resource identified by the Request-URI.
        /// </summary>
        MethodNotAllowed = 405,
        /// <summary>
        /// The resource identified by the request is only capable of generating response entities which have content characteristics not acceptable according to the accept headers sent in the request. 
        /// </summary>
        NotAcceptable = 406,
        /// <summary>
        /// This code is similar to 401 (Unauthorized), but indicates that the client must first authenticate itself with the proxy.
        /// </summary>
        ProxyAuthenticationRequired = 407,
        /// <summary>
        /// The client did not produce a request within the time that the server was prepared to wait.
        /// </summary>
        RequestTimeout = 408,
        /// <summary>
        /// The request could not be completed due to a conflict with the current state of the resource.
        /// </summary>
        Conflict = 409,
        /// <summary>
        /// The requested resource is no longer available at the server and no forwarding address is known. This condition is expected to be considered permanent.
        /// </summary>
        Gone = 410,
        /// <summary>
        /// The server refuses to accept the request without a defined Content- Length.
        /// </summary>
        LengthRequired = 411,
        /// <summary>
        /// The precondition given in one or more of the request-header fields evaluated to false when it was tested on the server.
        /// </summary>
        PreconditionFailed = 412,
        /// <summary>
        /// The server is refusing to process a request because the request entity is larger than the server is willing or able to process.
        /// </summary>
        RequestEntityTooLarge = 413,
        /// <summary>
        /// The server is refusing to service the request because the Request-URI is longer than the server is willing to interpret.
        /// </summary>
        RequestURITooLong = 414,
        /// <summary>
        /// The server is refusing to service the request because the entity of the request is in a format not supported by the requested resource for the requested method.
        /// </summary>
        UnsupportedMediaType = 415,
        /// <summary>
        ///  server SHOULD return a response with this status code if a request included a Range request-header field (section 14.35), and none of the range-specifier values in this field overlap the current extent of the selected resource, 
        ///  and the request did not include an If-Range request-header field. (For byte-ranges, this means that the first- byte-pos of all of the byte-range-spec values were greater than the current length of the selected resource.) 
        /// </summary>
        RequestedRangeNotSatisfiable = 416,
        /// <summary>
        /// The expectation given in an Expect request-header field (see section 14.20) could not be met by this server, or, if the server is a proxy, the server has unambiguous evidence that the request could not be met by the next-hop server. 
        /// </summary>
        ExpectationFailed = 417,
        /// <summary>
        /// 
        /// </summary>
        WebDAVUnprocessableEntity = 422,
        /// <summary>
        /// 
        /// </summary>
        WebDAVLocked = 423,
        /// <summary>
        /// 
        /// </summary>
        WebDAVFailedDependency = 424,
        /// <summary>
        /// 
        /// </summary>
        WebDAVUnorderedCollection = 425,
        /// <summary>
        /// 
        /// </summary>
        WebDAVUpgradeRequired = 426,
        /// <summary>
        /// 
        /// </summary>
        WebDAVRetryWith = 449,
        /// <summary>
        /// The server encountered an unexpected condition which prevented it from fulfilling the request.
        /// </summary>
        InternalServerError = 500,
        /// <summary>
        /// The server does not support the functionality required to fulfill the request.
        /// </summary>
        NotImplemented = 501,
        /// <summary>
        /// The server, while acting as a gateway or proxy, received an invalid response from the upstream server it accessed in attempting to fulfill the request. 
        /// </summary>
        BadGateway = 502,
        /// <summary>
        /// The server is currently unable to handle the request due to a temporary overloading or maintenance of the server. The implication is that this is a temporary condition which will be alleviated after some delay.
        /// </summary>
        ServiceUnavailable = 503,
        /// <summary>
        /// The server, while acting as a gateway or proxy, did not receive a timely response from the upstream server specified by the URI (e.g. HTTP, FTP, LDAP) or some other auxiliary server (e.g. DNS) it needed to access in attempting to complete the request
        /// </summary>
        GatewayTimeout = 504,
        /// <summary>
        /// The server does not support, or refuses to support, the HTTP protocol version that was used in the request message.
        /// </summary>
        HTTPVersionNotSupported = 505,
        /// <summary>
        /// 
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        VariantAlsoNegotiates = 506,
        /// <summary>
        /// 
        /// </summary>
        InsufficientStorage = 507,
        /// <summary>
        /// 
        /// </summary>
        BandwidthLimitExceeded = 509
    }
}
