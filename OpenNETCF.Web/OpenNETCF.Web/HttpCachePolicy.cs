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
using OpenNETCF.Web.Configuration;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Contains methods for setting cache-specific HTTP headers and for controlling the ASP.NET page output cache.
    /// </summary>
    public class HttpCachePolicy
    {
        private bool m_noStore = false;
        private bool m_noTransform = false;
        private int m_maxAge = -1;
        private string m_cacheExtension = null;
        private HttpCacheability m_cacheability = HttpCacheability.Private;

        internal HttpCachePolicy()
        {
        }

        internal HttpCachePolicy(CachingProfile profile)
        {
            if (profile.Duration.Ticks > 0)
            {
                this.SetMaxAge(profile.Duration);
            }

            switch (profile.Location)
            {
                case CacheLocation.Client:
                    this.SetCacheability(HttpCacheability.Private);
                    break;
                case CacheLocation.Downstream:
                    this.SetCacheability(HttpCacheability.Public);
                    break;
                case CacheLocation.None:
                    this.SetCacheability(HttpCacheability.NoCache);
                    break;
            }
        }

        /// <summary>
        /// Sets the <b>Cache-Control: no-store</b> directive.
        /// </summary>
        public void SetNoStore()
        {
            m_noStore = true;
        }

        /// <summary>
        /// Sets the Cache-Control HTTP header. The Cache-Control HTTP header controls how documents are to be cached on the network.
        /// </summary>
        /// <param name="cacheability">The HttpCacheability enumeration value to set the header to.</param>
        public void SetCacheability(HttpCacheability cacheability)
        {
            SetCacheability(cacheability, null);
        }

        /// <summary>
        /// Sets the Cache-Control HTTP header. The Cache-Control HTTP header controls how documents are to be cached on the network.
        /// </summary>
        /// <param name="cacheability">The HttpCacheability enumeration value to set the header to.</param>
        /// <param name="field">The cache control extension to add to the header.</param>
        public void SetCacheability(HttpCacheability cacheability, string field)
        {
            switch (cacheability)
            {
                case HttpCacheability.Server:
                case HttpCacheability.ServerAndNoCache:
                case HttpCacheability.ServerAndPrivate:
                    throw new ArgumentOutOfRangeException("Server caching is not currently supported by Padarn");
            }

            m_cacheability = cacheability;
            m_cacheExtension = field;
        }

        /// <summary>
        /// Sets the <b>Cache-Control: no-transform</b> HTTP header. 
        /// </summary>
        /// <remarks>
        /// The Cache-Control: no-transform HTTP header instructs network caching applications not to modify the document. 
        /// The Cache-Control: no-transform HTTP header prevents downstream proxy servers from changing any header values specified by the Content-Encoding, Content-Range, or Content-Type headers (this includes the entity body). 
        /// For example, it prevents proxies from converting GIF images to PNG. 
        /// As with other restrictions on caching, once SetNoTransforms is called, the Cache-Control: no-transform HTTP header cannot be disabled through the HttpCachePolicy interface. 
        /// </remarks>
        public void SetNoTransforms()
        {
            m_noTransform = true; 
        }

        /// <summary>
        /// Sets the <b>Cache-Control: max-age HTTP header</b> based on the specified time span.
        /// </summary>
        /// <param name="delta">The time span used to set the <b>Cache-Control: max-age</b> header.</param>
        public void SetMaxAge(TimeSpan delta)
        {
            m_maxAge = (int)delta.TotalSeconds;
        }

        internal string GetHeaderString()
        {
            StringBuilder sb = new StringBuilder();

            switch (m_cacheability)
            {
                case HttpCacheability.NoCache:
                    AppendHeaderValue(sb, "no-cache");
                    break;
                case HttpCacheability.Private:
                    AppendHeaderValue(sb, "private");
                    break;
                case HttpCacheability.Public:
                    AppendHeaderValue(sb, "public");
                    break;
                case HttpCacheability.Server:
                case HttpCacheability.ServerAndNoCache:
                case HttpCacheability.ServerAndPrivate:
                    throw new ArgumentOutOfRangeException("Server caching is not currently supported by Padarn");
            }

            if(m_cacheExtension != null)
            {
                sb.Append("=" + m_cacheExtension);
            }

            if (m_noStore)
            {
                AppendHeaderValue(sb, "no-store");
            }

            if(m_noTransform)
            {
                AppendHeaderValue(sb, "no-transform");
            }

            if (m_maxAge >= 0)
            {
                AppendHeaderValue(sb, string.Format("max-age={0}", m_maxAge));
            }

            if (sb.Length > 0)
            {
                return string.Format("Cache-Control: {0}\r\n", sb.ToString());
            }

            return string.Empty;
        }

        private void AppendHeaderValue(StringBuilder headerBuilder, string value)
        {
            if (headerBuilder.Length > 0)
            {
                headerBuilder.Append(", ");
            }

            headerBuilder.Append(value);
        }
    }
}
