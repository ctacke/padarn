using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web;
using System.Diagnostics;

namespace SampleSite.Handlers
{
    // READ
    public class GetHandler : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
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

            throw new HttpException(HttpErrorCode.Forbidden, "My Test Description");
            context.Response.StatusCode = 201;
            context.Response.StatusDescription = "WOOHOO";

            context.Response.Write(sb.ToString());
            context.Response.Flush();
        }
    }
}
