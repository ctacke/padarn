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