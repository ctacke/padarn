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
