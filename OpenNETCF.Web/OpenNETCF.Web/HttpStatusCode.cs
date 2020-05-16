﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenNETCF.Net
{
    public enum HttpStatusCode
    {
        /// <summary>
        /// Continue indicates that the client can continue with its request.
        /// </summary>
        Continue = 100,
        /// <summary>
        /// SwitchingProtocols indicates that the protocol version or protocol is being changed.
        /// </summary>        
        SwitchingProtocols = 101,
        /// <summary>
        /// OK indicates that the request succeeded and that the requested information is in the response.
        /// </summary>
        OK = 200,
        /// <summary>
        /// Created indicates that the request resulted in a new resource created before the response was sent.
        /// </summary>
        Created = 201,
        /// <summary>
        /// Accepted indicates that the request has been accepted for further processing.
        /// </summary>
        Accepted = 202,
        /// <summary>
        /// NonAuthoritativeInformation indicates that the returned metainformation is from a cached copy instead
        /// of the origin server and therefore may be incorrect.
        /// </summary>
        NonAuthoritativeInformation = 203,
        /// <summary>
        /// NoContent indicates that the request has been successfully processed and that the response is intentionally blank.
        /// </summary>
        NoContent = 204,
        /// <summary>
        /// ResetContent indicates that the client should reset (not reload) the current resource.
        /// </summary>
        ResetContent = 205,
        /// <summary>
        /// PartialContent indicates that the response is a partial response as requested by a GET request that includes a byte range.
        /// </summary>
        PartialContent = 206,
        /// <summary>
        /// MultipleChoices indicates that the requested information has multiple representations. The
        /// default action is to treat this status as a redirect and follow the contents of the Location header associated with this response.
        /// </summary>
        MultipleChoices = 300,
        /// <summary>
        /// Ambiguous indicatesthat the requested information has multiple representations. The default
        /// action is to treat this status as a redirect and follow the contents of the
        /// Location header associated with this response.
        /// </summary>
        Ambiguous = 300,
        /// <summary>
        /// MovedPermanently indicates that the requested information has been moved to the URI specified
        /// in the Location header. The default action when this status is received is
        /// to follow the Location header associated with the response.
        /// </summary>
        MovedPermanently = 301,
        /// <summary>
        /// Moved indicates that the requested information has been moved to the URI specified in the
        /// Location header. The default action when this status is received is to follow
        /// the Location header associated with the response. When the original request
        /// method was POST, the redirected request will use the GET method.
        /// </summary>
        Moved = 301,
        Found = 302,
        Redirect = 302,
        SeeOther = 303,
        RedirectMethod = 303,
        /// <summary>
        /// NotModified indicates that the client's cached copy is up to date. The contents of the resource
        /// are not transferred.
        /// </summary>
        NotModified = 304,
        UseProxy = 305,
        Unused = 306,
        TemporaryRedirect = 307,
        RedirectKeepVerb = 307,

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
