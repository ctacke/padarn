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
using OpenNETCF.Web;
using OpenNETCF.Web.SessionState;
using System.IO;
using OpenNETCF.Web.Parsers;

namespace OpenNETCF.Web.UI
{
    /// <summary>
    /// Represents an .aspx file, also known as a Web Forms page, 
    /// requested from a server that hosts a Padarn Web application.
    /// </summary>
    public class Page : TemplateControl
    {
        /// <summary>
        /// Occurs at the beginning of page initialization.
        /// </summary>
        public event EventHandler PreInit;

        /// <summary>
        /// Occurs when page initialization is complete.
        /// </summary>
        public event EventHandler InitComplete;

        /// <summary>
        /// Occurs before the page Load event.
        /// </summary>
        public event EventHandler PreLoad;

        /// <summary>
        /// Occurs at the end of the load stage of the page's life cycle.
        /// </summary>
        public event EventHandler LoadComplete;

        /// <summary>
        /// Occurs after the Control object is loaded but prior to rendering.
        /// </summary>
        public event EventHandler PreRender;

        /// <summary>
        /// Occurs before the page content is rendered.
        /// </summary>
        public event EventHandler PreRenderComplete;

        /// <summary>
        /// Gets the <see cref="HttpRequest"/> object for the requested page.
        /// </summary>
        /// <returns>The current <see cref="HttpRequest"/> associated with the page.</returns>
        /// <exception cref="HttpException">Occurs when the <see cref="HttpRequest"/> object is not available.</exception>
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }   
        }

        /// <summary>
        /// Gets the <see cref="HttpResponse"/> object associated with the <see cref="Page"/> object. This object 
        /// allows you to send HTTP response data to a client and contains information 
        /// about that response.
        /// </summary>
        /// <returns>The current <see cref="HttpResponse"/> associated with the page</returns>
        /// <exception cref="HttpException">The <see cref="HttpResponse"/> object is not available. </exception>
        public HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

        /// <summary>
        /// Gets the current Session object provided by Padarn.
        /// </summary>
        public HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Page"/> class.
        /// </summary>
        public Page()
        {
        }

        /// <summary>
        /// Notifies the server control that caused the postback that it should handle an incoming post back event.
        /// </summary>
        /// <param name="sourceControl"></param>
        /// <param name="eventArgument"></param>
        internal virtual void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            // TODO:
        }

        // called from the page handler - this circumvents the accessibility for OnPreLoad
        internal void CallOnPreLoad(EventArgs e)
        {
            OnPreLoad(e);
        }

        /// <summary>
        /// Raises the PreLoad event after postback data is loaded into the page server controls but before the OnLoad event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPreLoad(EventArgs e)
        {
            var handler = PreLoad;

            if (handler == null) return;
            handler(this, null);
        }

        // called from the page handler - this circumvents the accessibility for OnLoadComplete
        internal void CallOnLoadComplete(EventArgs e)
        {
            OnLoadComplete(e);
        }

        /// <summary>
        /// Raises the LoadComplete event at the end of the page load stage.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLoadComplete(EventArgs e)
        {
            var handler = LoadComplete;

            if (handler == null) return;
            handler(this, null);
        }

        // called from the page handler - this circumvents the accessibility for OnLoadComplete
        internal void CallOnPreInit(EventArgs e)
        {
            OnPreInit(e);
        }

        /// <summary>
        /// Raises the PreInit event at the beginning of page initialization.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPreInit(EventArgs e)
        {
            var handler = PreInit;

            if (handler == null) return;
            handler(this, null);
        }

        // called from the page handler - this circumvents the accessibility for OnLoadComplete
        internal void CallOnInitComplete(EventArgs e)
        {
            OnInitComplete(e);
        }

        /// <summary>
        /// Raises the InitComplete event after page initialization.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnInitComplete(EventArgs e)
        {
            var handler = InitComplete;

            if (handler == null) return;
            handler(this, null);
        }

        internal void RenderInternal()
        {
            using(var sw = new StringWriter())
            {
                var writer = new HtmlTextWriter(sw);

                Render(writer);

                sw.Flush();
                var pageData = sw.ToString();

                if (!string.IsNullOrEmpty(pageData))
                {
                    Response.Write(pageData);
                }
            }
        }

        /// <summary>
        /// Raises the PreRender event.
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnPreRender(EventArgs e)
        {
            var handler = PreRender;

            if (handler == null) return;
            handler(this, null);
        }

        // called from the page handler - this circumvents the accessibility for OnPreRenderComplete
        internal void CallOnPreRenderComplete(EventArgs e)
        {
            OnPreRenderComplete(e);
        }

        /// <summary>
        /// Raises the PreRenderComplete event after the OnPreRenderComplete event and before the page is rendered.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPreRenderComplete(EventArgs e)
        {
            var handler = PreRenderComplete;

            if (handler == null) return;
            handler(this, null);
        }

        /// <summary>
        /// Initializes the HtmlTextWriter object and calls on the child controls of the Page to render.
        /// </summary>
        /// <param name="writer"></param>
        protected internal override void Render(HtmlTextWriter writer)
        {
            // TODO call children to render
            writer.Write(Content);
        }

        /// <summary>
        /// Gets a value that indicates whether the page is being rendered for the first time or is being loaded in response to a postback.
        /// </summary>
        public bool IsPostBack { get; internal set; }

        internal string Content { get; set; }
        internal string DTDHeader { get; set; }
        internal AspxInfo AspxInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is reusable.
        /// </summary>
        /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
        public virtual bool IsReusable 
        { 
            get { return false; }
        }
    }
}
