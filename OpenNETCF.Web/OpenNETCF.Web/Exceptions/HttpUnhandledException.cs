using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace OpenNETCF.Web
{
    /// <summary>
    /// The exception that is thrown when a generic exception occurs.
    /// </summary>
    public sealed class HttpUnhandledException : HttpException
    {

        /// <summary>
        /// Initializes a new instance of the HttpUnhandledException class with the specified error messages.
        /// </summary>
        /// <param name="message">The message displayed to the client when the exception is thrown.</param>
        public HttpUnhandledException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the HttpUnhandledException class with the specified error message and inner exception.
        /// </summary>
        /// <param name="message">The message displayed to the client when the exception is thrown.</param>
        /// <param name="innerException">The InnerException, if any, that threw the current exception.</param>
        public HttpUnhandledException(string message, Exception innerException)
            : base(message, innerException)
        {
            //TODO base.SetFormatter(new UnhandledErrorFormatter(innerException, message, null));
        }

        internal HttpUnhandledException(string message, string postMessage, Exception innerException)
            : base(message, innerException)
        {
            //TODO base.SetFormatter(new UnhandledErrorFormatter(innerException, message, postMessage));
        }
    }
}