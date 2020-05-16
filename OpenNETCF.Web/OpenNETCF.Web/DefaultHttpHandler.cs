using System.Diagnostics;
using System;
namespace OpenNETCF.Web
{
    /// <summary>
    /// The default handler for HTTP requests
    /// </summary>
    public sealed class DefaultHttpHandler : IHttpHandler
    {
        /// <summary>
        /// Process the HTTP request
        /// </summary>
        /// <param name="context">The current context of the request</param>
        public void ProcessRequest(HttpContext context)
        {
            HttpWorkerRequest wr = context.WorkerRequest;
            wr.ProcessRequest();
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
