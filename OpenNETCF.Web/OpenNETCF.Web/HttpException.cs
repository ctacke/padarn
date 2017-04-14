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
using System.Runtime.InteropServices;
using System.Security;
//using OpenNETCF.Configuration;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Describes an exception that occurred during the processing of HTTP requests.
    /// </summary>
    public class HttpException : ExternalException
    {
        private HttpErrorCode httpCode;
 
        /// <summary>
        /// Creates a new <see cref="HttpException"/> exception based on the error code that 
        /// is returned from the Win32 API GetLastError() method.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static HttpException CreateFromLastError(string message)
        {
            return new HttpException(message, HResultFromLastError(Marshal.GetLastWin32Error()));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class using a supplied error message.
        /// </summary>
        /// <param name="message">The error message displayed to the client when the exception is thrown.</param>
        public HttpException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates an instance of the <see cref="HttpException" /> type
        /// </summary>
        /// <param name="httpCode"></param>
        /// <param name="message"></param>
        public HttpException(int httpCode, string message)
            : base(message)
        {
            this.httpCode = (HttpErrorCode)httpCode;
        }

        /// <summary>
        /// Creates an instance of the <see cref="HttpException" /> type
        /// </summary>
        /// <param name="httpCode"></param>
        /// <param name="message"></param>
        public HttpException(HttpErrorCode httpCode, string message)
            : base(message)
        {
            this.httpCode = httpCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class using an error message and the InnerException property.
        /// </summary>
        /// <param name="message">The error message displayed to the client when the exception is thrown.</param>
        /// <param name="innerException">The <see cref="System.Exception.InnerException"/>, if any, that threw the current exception.</param>
        public HttpException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates an instance of the <see cref="HttpException" /> type
        /// </summary>
        /// <param name="httpCode"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public HttpException(HttpErrorCode httpCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.httpCode = httpCode;
        }

        /// <summary>
        /// Creates an instance of the <see cref="HttpException" /> type
        /// </summary>
        /// <param name="message"></param>
        /// <param name="hr"></param>
        public HttpException(string message, int hr)
            : base(message)
        {
            base.HResult = hr;
        }

        /// <summary>
        /// Creates an instance of the <see cref="HttpException" /> type
        /// </summary>
        /// <param name="httpCode"></param>
        /// <param name="message"></param>
        /// <param name="hr"></param>
        public HttpException(HttpErrorCode httpCode, string message, int hr)
            : base(message)
        {
            base.HResult = hr;
            this.httpCode = httpCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetHttpCode()
        {
            return (int)GetHttpCodeForException(this);
        }

        internal static HttpErrorCode GetHttpCodeForException(Exception e)
        {
            if(e is HttpException)
            {
                HttpException httpException = (HttpException)e;
                if(httpException.httpCode > 0)
                {
                    return httpException.httpCode;
                }
            }

            return HttpErrorCode.InternalServerError;
        }

        internal static int HResultFromLastError(int lastError)
        {
            if (lastError < 0)
            {
                return lastError;
            }
            return (((lastError & 0xffff) | 0x70000) | -2147483648);
        }
    }
}
