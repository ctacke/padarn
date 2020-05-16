using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web
{
    /// <summary>
    /// The exception that is thrown when a potentially malicious input string is received from the client as part of the request data. This class cannot be inherited.
    /// </summary>
    public sealed class HttpRequestValidationException : HttpException
    {
        /// <summary>
        /// Creates a new HttpRequestValidationException exception with the specified error message.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        public HttpRequestValidationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the HttpRequestValidationException class with a specified error message and a reference to the inner exception that is the cause of the exception.
        /// </summary>
        /// <param name="message">An error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If this parameter is not null, the current exception is raised in a catch block that handles the inner exception.</param>
        public HttpRequestValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
