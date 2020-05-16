using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web
{
    public class HttpResponseWrapper : HttpResponseBase
    {
        private HttpResponse m_response;

        public HttpResponseWrapper(HttpResponse httpResponse)
        {
            m_response = httpResponse;
        }

        public override void Redirect(string url)
        {
            m_response.Redirect(url);
        }

        /// <summary>
        /// Redirects a request to the specified URL and specifies whether execution of the current process should terminate.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="endResponse"></param>
        public override void Redirect(string url, bool endResponse)
        {
            m_response.Redirect(url, endResponse);
        }
    }
}
