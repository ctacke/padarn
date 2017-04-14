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
using System.Collections.Specialized;
using System.Text;
using OpenNETCF.Web.Configuration;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Provides a type-safe way to create and manipulate individual HTTP cookies.
    /// </summary>
    public sealed class HttpCookie
    {
        private bool added;
        private bool changed;
        private string domain;
        private bool expirationSet;
        private DateTime expires;
        private bool httpOnly;
        private HttpValueCollection multiValue;
        private string name;
        private string path;
        private bool secure;
        private string stringValue;

        internal HttpCookie()
        {
            this.changed = true;
            this.path = "/";
        }

        /// <summary>
        /// Creates and names a new cookie.
        /// </summary>
        /// <param name="name"></param>
        public HttpCookie(string name)
        {
            this.changed = true;
            this.path = "/";
            this.SetDefaultsFromConfig();
            this.name = name;
        }

        /// <summary>
        /// Creates, names, and assigns a value to a new cookie.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public HttpCookie(string name, string value)
        {
            this.path = "/";
            this.name = name;
            this.stringValue = value;
            this.SetDefaultsFromConfig();
            this.changed = true;
        }

        internal bool Added
        {
            get { return this.added; }
            set { this.added = value; }
        }

        internal bool Changed
        {
            get { return this.changed; }
            set { this.changed = value; }
        }

        /// <summary>
        /// Gets or sets the domain to associate the cookie with.
        /// </summary>
        public string Domain
        {
            get { return this.domain; }
            set
            {
                this.domain = value;
                this.changed = true;
            }
        }

        /// <summary>
        /// Gets or sets the expiration date and time for the cookie.
        /// </summary>
        public DateTime Expires
        {
            get
            {
                if (!this.expirationSet)
                {
                    return DateTime.MinValue;

                }
                return this.expires;
            }
            set
            {
                this.expires = value;
                this.expirationSet = true;
                this.changed = true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a cookie has subkeys.
        /// </summary>
        public bool HasKeys
        {
            get { return this.Values.HasKeys(); }
        }

        /// <summary>
        /// Gets or sets a value that specifies whether a cookie is accessible by client-side script.
        /// </summary>
        public bool HttpOnly
        {
            get { return this.httpOnly; }
            set
            {
                this.httpOnly = value;
                this.changed = true;
            }
        }

        public string this[string key]
        {
            get { return this.Values[key]; }
            set
            {
                this.Values[key] = value;
                this.changed = true;
            }
        }

        /// <summary>
        /// Gets or sets the name of a cookie.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.changed = true;
            }
        }

        /// <summary>
        /// Gets or sets the virtual path to transmit with the current cookie.
        /// </summary>
        public string Path
        {
            get { return this.path; }
            set
            {
                this.path = value;
                this.changed = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to transmit the cookie using Secure Sockets Layer (SSL)--that is, over HTTPS only.
        /// </summary>
        public bool Secure
        {
            get { return this.secure; }
            set
            {
                this.secure = value;
                this.changed = true;
            }
        }

        /// <summary>
        /// Gets or sets an individual cookie value.
        /// </summary>
        public string Value
        {
            get
            {
                if (this.multiValue != null)
                {
                    return this.multiValue.ToString(false);
                }
                return this.stringValue;
            }
            set
            {
                if (this.multiValue != null)
                {
                    this.multiValue.Reset();
                    this.multiValue.Add(null, value);
                }
                else
                {
                    this.stringValue = value;
                }
                this.changed = true;
            }
        }

        /// <summary>
        /// Gets a collection of key/value pairs that are contained within a single cookie object.
        /// </summary>
        public NameValueCollection Values
        {
            get
            {
                if (this.multiValue == null)
                {
                    this.multiValue = new HttpValueCollection();
                    if (this.stringValue != null)
                    {
                        if ((this.stringValue.IndexOf('&') >= 0) || (this.stringValue.IndexOf('=') >= 0))
                        {
                            this.multiValue.FillFromString(this.stringValue);
                        }
                        else
                        {
                            this.multiValue.Add(null, this.stringValue);
                        }
                        this.stringValue = null;
                    }
                }
                this.changed = true;
                return this.multiValue;
            }
        }

        internal string GetSetCookieHeader(HttpContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Set-Cookie: ");

            if (!string.IsNullOrEmpty(this.name))
            {
                builder.Append(this.name);
                builder.Append('=');
            }
            if (this.multiValue != null)
            {
                builder.Append(this.multiValue.ToString(false));
            }
            else if (this.stringValue != null)
            {
                builder.Append(this.stringValue);
            }
            if (!string.IsNullOrEmpty(this.domain))
            {
                builder.Append("; domain=");
                builder.Append(this.domain);
            }
            if (this.expirationSet && (this.expires != DateTime.MinValue))
            {
                builder.Append("; expires=");
                builder.Append(HttpUtility.FormatHttpCookieDateTime(this.expires));
            }
            if (!string.IsNullOrEmpty(this.path))
            {
                builder.Append("; path=");
                builder.Append(this.path);
            }
            if (this.secure)
            {
                builder.Append("; secure");
            }
            if (this.httpOnly && this.SupportsHttpOnly(context))
            {
                builder.Append("; HttpOnly");
            }
            return builder.ToString();
        }

        private void SetDefaultsFromConfig()
        {
            CookiesConfiguration cookiesConfig = ServerConfig.GetConfig().Cookies;
            if (cookiesConfig != null)
            {
                this.secure = cookiesConfig.RequireSSL;
                this.httpOnly = cookiesConfig.HttpOnlyCookies;

                if (!string.IsNullOrEmpty(cookiesConfig.Domain))
                {
                    this.domain = cookiesConfig.Domain;
                }
            }
        }

        private bool SupportsHttpOnly(HttpContext context)
        {
            if ((context == null) || context.Request == null)
            {
                return false;
            }

            HttpBrowserCapabilities userAgent = context.Request.Browser;
            if (userAgent == null)
            {
                return false;
            }

            if (!(userAgent.Type != "IE5"))
            {
                return (userAgent.Platform != "MacPPC");
            }

            return true;
        }
    }
}
