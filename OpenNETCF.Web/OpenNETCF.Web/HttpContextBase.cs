using System;

using System.Collections.Generic;
using System.Text;
using System.Collections;
using OpenNETCF.Security;
using OpenNETCF.Security.Principal;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Serves as the base class for classes that contain HTTP-specific information about an individual HTTP request.
    /// </summary>
    public abstract class HttpContextBase : IServiceProvider
    {
        private List<Exception> m_exceptions = new List<Exception>();

        /// <summary>
        /// Initializes the class for use by an inherited class instance. This constructor can only be called by an inherited class.
        /// </summary>
        protected HttpContextBase()
        {
        }

        /// <summary>
        /// When overridden in a derived class, returns an object for the current service type.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns>
        /// The current service type, or null if no service is found.
        /// </returns>
        public virtual object GetService(Type serviceType)
        {
            return null;
        }

        /// <summary>
        /// When overridden in a derived class, adds an exception to the exception collection for the current HTTP request.
        /// </summary>
        /// <param name="errorInfo"></param>
        public virtual void AddError(Exception errorInfo)
        {
            m_exceptions.Add(errorInfo);
        }

        /// <summary>
        /// When overridden in a derived class, clears all errors for the current HTTP request.
        /// </summary>
        public virtual void ClearError()
        {
            m_exceptions.Clear();
        }

        /// <summary>
        /// When overridden in a derived class, gets an array of errors (if any) that accumulated when an HTTP request was being processed.
        /// </summary>
        public virtual Exception[] AllErrors 
        {
            get { return m_exceptions.ToArray(); } 
        }

        /// <summary>
        /// When overridden in a derived class, gets the first error (if any) that accumulated when an HTTP request was being processed.
        /// </summary>
        /// <value>
        /// The first exception for the current HTTP request/response process, or null if no errors accumulated during the HTTP request processing.
        /// </value>
        public virtual Exception Error 
        {
            get
            {
                if (m_exceptions.Count > 0) return m_exceptions[0];

                return null;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the HttpRequest object for the current HTTP request.
        /// </summary>
        public virtual HttpRequestBase Request 
        {
            get { return null; } 
        }

        /// <summary>
        /// When overridden in a derived class, gets the HttpResponse object for the current HTTP response.
        /// </summary>
        public virtual HttpResponseBase Response 
        {
            get { return null; } 
        }

        private Dictionary<string, object> m_items = new Dictionary<string, object>();
 
        public virtual IDictionary Items 
        {
            get { return m_items; } 
        }

        public virtual IPrincipal User 
        { 
            get { throw new NotImplementedException(); } 
            set { throw new NotImplementedException(); } 
        }
    }    
}
