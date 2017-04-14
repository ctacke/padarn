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
namespace OpenNETCF.Web
{
    using System;
    using System.IO;
    using System.Reflection;
    using Configuration;
    using UI;
    using System.Diagnostics;
    using System.Collections.Generic;
    using OpenNETCF.Web.Parsers;
    using OpenNETCF.Web.UI.WebControls;

    /// <summary>
    /// Padarn Page Handler
    /// </summary>
    public sealed class PageHandler : IHttpHandler
    {
        private const string CodeBehind = "CodeBehind";
        private const string Inherits = "Inherits";
        private const string AutoEventWireup = "AutoEventWireup";
        private const string Page = "Page";

        private readonly string m_filePath;
        private string m_mimeType;
        private HttpResponse m_response;

        private static Dictionary<string, Type> m_pageTypeCache = new Dictionary<string, Type>();
        private static Dictionary<Type, Page> m_pageCache = new Dictionary<Type, Page>();

        /// <summary>
        /// Creates an instance of the Padarn page handler
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="mimeType"></param>
        public PageHandler(string filePath, string mimeType)
        {
            m_filePath = filePath;
            m_mimeType = mimeType;
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the HttpHandler instance.
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Parses the Padarn Page and creates the HTTP response
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            m_response = context.Response;

            m_response.ContentType = MimeMapping.GetMimeMapping("default.aspx");

            ParsePage();

            m_response.Flush();
        }
            
        private void ParsePage()
        {
            var parser = PageParser.GetParser();

            AspxInfo aspx;
            string dtdHeader;

            string pageContent = string.Empty;

            using (var stream = File.OpenText(m_filePath))
            {
                pageContent = stream.ReadToEnd();
            }

            // strip the document type header and any ASP tags
            pageContent = parser.PreParse(pageContent, out dtdHeader, out aspx);
            pageContent = pageContent.Trim();
            #region Load the code-behind assembly and invoke the page load event
            // ----------------------------------------------------------------------
            var pageType = GetCodeBehindClass(aspx.CodeBehindAssemblyName, aspx.CodeBehindTypeName);

            if (pageType == null)
            {
                throw new TypeLoadException(String.Format("Could not load Page class '{0}!{1}'", aspx.CodeBehindAssemblyName, aspx.CodeBehindTypeName));
            }

            var instance = CreatePageInstance(pageType);
            instance.DTDHeader = dtdHeader;
            instance.AspxInfo = aspx;

            // parse for ASP controls
            string content;
            try
            {
                content = parser.Parse(instance, pageContent);
            }
            catch (System.Xml.XmlException ex)
            {
                // TODO: format a custom error screen for this (page parsing issue)
                throw new HttpException(HttpErrorCode.InternalServerError, ex.Message);
            }

            // for page life cycle see http://msdn.microsoft.com/en-us/library/ms178472.aspx
            instance.CallOnPreInit(EventArgs.Empty);

            instance.Content = content;
            instance.IsPostBack = instance.Request.Form["__EVENTTARGET"] != null;

            IPostBackEventHandler postbackSource = null;
            string postbackArg = null;
 
            if (instance.IsPostBack)
            {
                postbackSource = DeterminePostbackSource(instance, out postbackArg);
            }

            // TODO: controls raise their OnInit

            instance.OnInit(EventArgs.Empty);

            // TODO: load view state (only on postback)

            instance.CallOnInitComplete(EventArgs.Empty);

            // TODO: load postback data (only on postback)

            instance.CallOnPreLoad(EventArgs.Empty);

            if (aspx.AutoEventWireup)
            {
                WireupPage_Load(instance);
            }

            instance.OnLoad(EventArgs.Empty);

            // TODO: controls raise their Load events

            // TODO: controls raise any other events (click, etc)

            // TODO: RaisePostBackEvent (only on postback)
            if (instance.IsPostBack)
            {
                instance.RaisePostBackEvent(postbackSource, postbackArg);
            }

            instance.CallOnLoadComplete(EventArgs.Empty);

            instance.OnPreRender(EventArgs.Empty);

            // TODO: call PreRender() on page controls

            // TODO: call DataBind() on controls

            instance.CallOnPreRenderComplete(EventArgs.Empty);
            
            // TODO: SaveViewState

            // Render
            if (!instance.Response.IsRequestBeingRedirected)
            {
                // do not render on a redirect
                RenderPage(instance);
            }

            instance.Response.Flush();

            #endregion --------------------------------------------------------------
        }

        private IPostBackEventHandler DeterminePostbackSource(Page instance, out string argument)
        {
            var controlName = instance.Request.Form["__EVENTTARGET"];
            argument = instance.Request.Form["__EVENTARGUMENT"];  
            
            // TODO:
            // if controlName is empty, look for a submit button
            foreach (var c in instance.Controls)
            {
                if ((c is Button) && (!string.IsNullOrEmpty(instance.Request.Form[c.ID])))
                {
                    // a submit button was pressed
                    // TODO: look for a handler
                }
            }

            return null;
        }

        private Type GetCodeBehindClass(string assemblyName, string className)
        {
            Type page = null;

            string key = string.Format("{0}.{1}", assemblyName, className);

            if (m_pageTypeCache.ContainsKey(key))
            {
                return m_pageTypeCache[key];
            }

            string asmPath = Path.Combine(HttpContext.Current.WorkerRequest.GetAppPathTranslated(), Resources.BinFolderName);

            asmPath = Path.Combine(asmPath, assemblyName);
            asmPath = Path.GetFullPath(asmPath);

            if (!File.Exists(asmPath))
            {
                var c = Assembly.GetExecutingAssembly().GetName().CodeBase;
                var u = new Uri(c);
                // check application path
                var checkPath = Path.Combine(Path.GetDirectoryName(u.LocalPath), assemblyName);
                if (File.Exists(checkPath))
                {
                    asmPath = checkPath;
                }
                else
                {
                    throw new FileNotFoundException(string.Format("Cannot load code-behind assembly '{0}'", asmPath));
                }
            }

            var codeBehindAssembly = CodeBehindAssembly.LoadFrom(asmPath);

            if (codeBehindAssembly == null)
            {
                throw new TypeLoadException(string.Format("Cannot load code-behind assembly '{0}'", asmPath));
            }

            Type[] pages = codeBehindAssembly.GetTypesFromBaseType(typeof(Page));

            // TODO: handle a null array returned here (load failure from version mismatch, etc)
            // at a minimum we should log this

            if (pages == null)
            {
                throw new HttpException(HttpErrorCode.InternalServerError, 
                    string.Format("Unable to load type information from Code Behind assembly at '{0}'. Check the file, reference versions and dependencies",
                    asmPath));
            }

            int count = pages.Length;

            for (int i = 0; i < count; i++)
            {
                if (pages[i].FullName.Equals(className))
                {
                    page = pages[i];
                    break;
                }
            }

            if (page != null)
            {
                if (!m_pageTypeCache.ContainsKey(key))
                {
                    m_pageTypeCache.Add(key, page);
                }
            }

            return page;
        }
            
        private Page CreatePageInstance(Type pageType)
        {
            Page instance;

            /// ---- TODO: begin temp fix for incorrect page caching
            instance = (Page)Activator.CreateInstance(pageType);
            return instance;
            /// ---- TODO: end temp fix for incorrect page caching
            /// 
            // do we have a cached version
//            if (m_pageCache.ContainsKey(pageType))
//            {
//                instance = m_pageCache[pageType];
//                if (instance.IsReusable)
//                {
//                    // TODO: Response stream datais duplicated for each call - need to clear that up before enabling caching
//                    return instance;
//                }
//
//                // not reusable, so remove from the cache (it was changed at run time)
//                m_pageCache.Remove(pageType);
//            }
//
//            instance = (Page)Activator.CreateInstance(pageType);
//
//            if (instance.IsReusable)
//            {
//                m_pageCache.Add(pageType, instance);
//            }
//
//            return instance;
        }

        private void WireupPage_Load(Page instance)
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            MethodInfo pageLoadMethod = instance.GetType().GetMethod("Page_Load", flags);

            if (pageLoadMethod == null) return;

            instance.Load += delegate
            {
                pageLoadMethod.Invoke(instance, new object[] { null, EventArgs.Empty });
            };
        }

        private void RenderPage(Page page)
        {
            page.RenderInternal();
        }

    }
}
