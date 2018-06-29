using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web;
using System.Diagnostics;
using System.IO;

namespace SampleSite.Handlers
{
    public abstract class BaseHandler : IHttpHandler
    {
        protected const string XML_HEADER = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

        public virtual void ProcessRequest(HttpContext context)
        {
            Debug.WriteLine(string.Format("{0} received {1} at {2}",
                this.GetType().Name,
                context.Request.RequestType,
                context.Request.Path));
        }

        protected string GetEntityName(string path)
        {
            int index = path.IndexOf('/', 1);

            if (index > 0)
            {
                return path.Substring(1, index - 1);
            }

            index = path.IndexOf('/', 0);

            if (index < 0) return null;

            return path.Substring(index + 1);
        }
    
        protected string GetInputData(HttpContext context)
        {
            using (StreamReader reader = new StreamReader(context.Request.InputStream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
