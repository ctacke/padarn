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
using System.IO;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Serves as the base class for classes that provides HTTP-response information from an ASP.NET operation.
    /// </summary>
    /// <remarks>
    /// The HttpResponseBase class is an abstract class that contains the same members as the HttpResponse class. The HttpResponseBase class enables you to create derived classes that are like the HttpResponse class, but that you can customize and that work outside the ASP.NET pipeline. When you perform unit testing, you typically use a derived class to implement members that have customized behavior that fulfills the scenario you are testing.
    /// The HttpResponseWrapper class derives from the HttpResponseBase class. The HttpResponseWrapper class serves as a wrapper for the HttpResponse class. At run time, you typically use an instance of the HttpResponseWrapper class to call members of the HttpResponse object.
    /// </remarks>
    public abstract class HttpResponseBase
    {
        protected HttpResponseBase()
        {
        }

        public virtual TextWriter Output
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual int StatusCode
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual void Redirect(string url)
        {
            throw new NotImplementedException();
        }

        public virtual void Redirect(string url, bool endResponse)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, gets the caching policy (such as expiration time, privacy settings, and vary clauses) of the current Web page.
        /// </summary>
        /// <value>The caching policy of the current response.</value>
        public virtual HttpCachePolicyBase Cache
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// When overridden in a derived class, adds a session ID to the virtual path if the session is using Cookieless session state, and returns the combined path. 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public virtual string ApplyAppPathModifier(string virtualPath)
        {
            throw new NotImplementedException();
        }
    }
}
