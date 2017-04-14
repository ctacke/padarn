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
//                                                                   
// Copyright (c) 2007-2009 OpenNETCF Consulting, LLC                        
//                                                                     
namespace OpenNETCF.Web
{
    using System;
    using OpenNETCF.Web.Logging;
    using System.Threading;
    using System.Diagnostics;

    /// <summary>
    /// Provides a set of Padarn run-time services for the current application.
    /// </summary>
    public static class HttpRuntime
    {
        /// <summary>
        /// Drives all Padarn Web processing execution.
        /// </summary>
        /// <param name="wr"></param>
        public static void ProcessRequest(HttpWorkerRequest wr)
        {
            // Developer note:  This method is actually never called inside Padarn, the internal version below is (since we need the log provider)

            if(wr == null)
            {
                throw new ArgumentNullException("wr");
            }

            // Queue up the request
            ProcessRequestInternal(wr, null);
        }

        internal static void ProcessError(HttpWorkerRequest wr, HttpErrorCode error, string errorMessage)
        {
            var ctx = new HttpContext(wr, false);
            ctx.Response.StatusCode = (int)error;
            ctx.Response.StatusDescription = errorMessage;
            ctx.Response.Write(errorMessage);
            ctx.Response.Flush();
        }

        internal static void ProcessRequest(HttpWorkerRequest wr, ILogProvider logProvider)
        {
            if (wr == null)
            {
                throw new ArgumentNullException("wr");
            }

            // Queue up the request
            ProcessRequestInternal(wr, logProvider);
        }

        private static void ProcessRequestInternal(HttpWorkerRequest wr, ILogProvider logProvider)
        {
            HttpContext ctx = null;
            try
            {
                ctx = new HttpContext(wr, false);
            }
            catch(Exception ex)
            {
                if (logProvider != null)
                {
                    logProvider.LogPadarnError(string.Format("HttpRuntime.ProcessRequest threw an {0} while creating an HttpContext: {1}", ex.GetType().Name, ex.Message), null); 
                }

                // TODO: Send 400 Bad Request (RFC 2616 10.4.1)
            }

            // Returns an instance of the DefaultHttpHandler
            IHttpHandler applicationInstance = HttpApplicationFactory.GetApplicationInstance(wr); 
            if(applicationInstance == null)
            {
                logProvider.LogPadarnError("HttpRuntime.ProcessRequest received a null IHttpHandler from the application factory", null);
                throw new NullReferenceException("applicationInstance");
            }

            try
            {
                applicationInstance.ProcessRequest(ctx);
                FinishRequest(ctx.WorkerRequest, ctx, null);
            }
            catch(Exception ex)
            {
                logProvider.LogPadarnError(string.Format("HttpRuntime.ProcessRequest threw an {0} while processing the request: {1}", ex.GetType().Name, ex.Message), null);
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    //System.Diagnostics.Debugger.Break();
                }
                // TODO: Send 500/400/etc
            }
        }

        private static void FinishRequest(HttpWorkerRequest wr, HttpContext ctx, Exception e)
        {
            // TODO: Flush the response to the caller
            // throw new Exception("The method or operation is not implemented.");

        }
    }
}
