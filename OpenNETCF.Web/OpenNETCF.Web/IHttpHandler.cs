//                                                                   
// Copyright (c) 2007-2010 OpenNETCF Consulting, LLC                        
//                                                                     

namespace OpenNETCF.Web
{
    /// <summary>
    /// Defines the contract that ASP.NET implements to synchronously process HTTP Web 
    /// requests using custom HTTP handlers.
    /// </summary>
    public interface IHttpHandler
    {
        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that 
        /// implements the <see cref="IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="HttpContext"/> object that provides references to the 
        /// intrinsic server objects (for example, Request, Response, Session, and Server) 
        /// used to service HTTP requests.</param>
        void ProcessRequest(HttpContext context);

        /// <summary>
        /// Gets a value indicating whether another request can use the IHttpHandler instance.
        /// </summary>
        bool IsReusable { get; }
    }
}
