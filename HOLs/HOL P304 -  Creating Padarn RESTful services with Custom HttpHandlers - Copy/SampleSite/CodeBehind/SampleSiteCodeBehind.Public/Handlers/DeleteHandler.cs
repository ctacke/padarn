using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web;

namespace SampleSite.Handlers
{
    // DELETE
    public class DeleteHandler : BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);

            string path = context.Request.Path;

            string entity = GetEntityName(path);

            // the only entity we support is "Books"
            if (string.Compare(entity, "books", true) != 0)
            {
                throw new HttpException(HttpErrorCode.NotFound, string.Format("Entity '{0}' not supported", entity));
            }

            // get the ID
            if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }

            string id = path.Substring(path.LastIndexOf('/') + 1);

            DataConnector.GetInstance().DeleteBook(int.Parse(id));
        }
    }
}
