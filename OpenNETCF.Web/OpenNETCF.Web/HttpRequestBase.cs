﻿using System;

using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Serves as the base class for classes that enable ASP.NET to read the HTTP values sent by a client during a Web request.
    /// </summary>
    /// <remarks>
    /// The HttpRequestBase class is an abstract class that contains the same members as the HttpRequest class. The HttpRequestBase class enables you to create derived classes that are like the HttpRequest class, but that you can customize and that work outside the ASP.NET pipeline. When you perform unit testing, you typically use a derived class to implement members that have customized behavior that fulfills the scenario that you are testing.
    /// The HttpRequestWrapper class derives from the HttpRequestBase class. The HttpRequestWrapper class serves as a wrapper for the HttpRequest class. At run time, you typically use an instance of the HttpRequestWrapper class to invoke members of the HttpRequest object.
    /// </remarks>
    public abstract class HttpRequestBase
    {
        /// <summary>
        /// When overridden in a derived class, gets the collection of HTTP headers that were sent by the client.
        /// </summary>
        public virtual NameValueCollection Headers
        {
            get { throw new NotImplementedException(); } 
        }

        /// <summary>
        /// When overridden in a derived class, gets the collection of HTTP query-string variables.
        /// </summary>
        public virtual NameValueCollection QueryString
        {
            get { throw new NotImplementedException(); } 
        }

        /// <summary>
        /// When overridden in a derived class, gets the HTTP data-transfer method (such as GET, POST, or HEAD) that was used by the client.
        /// </summary>
        public virtual string HttpMethod
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// When overridden in a derived class, gets the collection of form variables that were sent by the client.
        /// </summary>
        public virtual NameValueCollection Form
        {
            get { throw new NotImplementedException(); } 
        }

        /// <summary>
        /// When overridden in a derived class, gets the virtual path of the current request.
        /// </summary>
        public virtual string Path 
        {
            get { throw new NotImplementedException(); } 
        }

        /// <summary>
        /// When overridden in a derived class, gets the virtual root path of the Padarn application on the server
        /// </summary>
        public virtual string ApplicationPath 
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// When overridden in a derived class, gets a collection of Web server variables.
        /// </summary>
        public virtual NameValueCollection ServerVariables
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// When overridden in a derived class, gets the complete URL of the current request.
        /// </summary>
        public virtual string RawUrl
        {
            get { throw new NotImplementedException(); }
        }

    }
}
