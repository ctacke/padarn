using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web;
using System.Diagnostics;

namespace SampleSite.Handlers
{
    public delegate void HttpEventHandler(IHttpHandler source, HttpContext context);

    // READ
    public class GetHandler : BaseHandler
    {
        public static event HttpEventHandler BeforeGet;

        public override void ProcessRequest(HttpContext context)
        {
            if (BeforeGet != null)
            {
                BeforeGet(this, context);
            }

            base.ProcessRequest(context);
            
            string entity = GetEntityName(context.Request.Path);

            // the only entity we support is "Books"
            if (string.Compare(entity, "books", true) != 0)
            {
                throw new HttpException(HttpErrorCode.NotFound, string.Format("Entity '{0}' not supported", entity));
            }

            Book[] books = DataConnector.GetInstance().GetAllBooks();

            StringBuilder sb = new StringBuilder(XML_HEADER);
            sb.Append("<books>");

            foreach (var b in books)
            {
                sb.Append(b.AsXml());
            }

            sb.Append("</books>");

            context.Response.Write(sb.ToString());
            context.Response.Flush();
        }
    }
}
