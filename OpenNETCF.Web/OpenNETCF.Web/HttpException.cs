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
