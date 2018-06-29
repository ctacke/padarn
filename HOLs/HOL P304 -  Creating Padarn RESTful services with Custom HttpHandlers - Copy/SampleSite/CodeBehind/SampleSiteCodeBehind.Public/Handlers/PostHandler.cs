using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web;
using System.Xml;

namespace SampleSite.Handlers
{
    // CREATE
    public class PostHandler : BaseHandler
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

            string xml = GetInputData(context);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.SelectSingleNode("book");

            Book book = Book.FromXml(node.OuterXml);

            DataConnector.GetInstance().InsertBook(book);
        }
    }
}
