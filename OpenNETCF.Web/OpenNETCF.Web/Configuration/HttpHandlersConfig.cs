using System;

using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenNETCF.Web.Configuration
{
    internal class HttpHandlersConfigSection : List<HttpHandler>
    {
        readonly XmlNode _section;
        public string[] AssemblyNames { get; private set; }

        internal HttpHandlersConfigSection(XmlNode section)
        {
            _section = section;
            ParseSection(_section);
        }

        private void ParseSection(XmlNode _section)
        {
            XmlNamespaceManager nsmgr = null;
            if (!string.IsNullOrEmpty(_section.NamespaceURI))
            {
                nsmgr = new XmlNamespaceManager(_section.OwnerDocument.NameTable);
                nsmgr.AddNamespace("padarn", _section.NamespaceURI);
            }

            List<string> assemblies = new List<string>();
            XmlNodeList l;

            if (nsmgr == null)
            {
                l = _section.SelectNodes("assembly");
            }
            else
            {
                l = _section.SelectNodes("padarn:assembly", nsmgr);
            }

            foreach (XmlNode n in l)
            {
                assemblies.Add(n.InnerText);
            }
            AssemblyNames = assemblies.ToArray();

            if (nsmgr == null)
            {
                l = _section.SelectNodes("add");
            }
            else
            {
                l = _section.SelectNodes("padarn:add", nsmgr);
            }

            foreach (XmlNode n in l)
            {
                string verb = n.Attributes["verb"].Value;
                HttpMethod method;
                if (verb != "*")
                {
                    method = (HttpMethod)Enum.Parse(typeof(HttpMethod), verb, true);
                }
                else
                {
                    method = HttpMethod.ANY;
                }

                this.Add(new HttpHandler(method, n.Attributes["path"].Value, n.Attributes["type"].Value));
            }
        }
    }


    internal class HttpHandler
    {

        readonly HttpMethod _verb;
        readonly string _path;
        readonly string  _type;
        
        internal HttpHandler(HttpMethod verb, string path, string typeName)
        {

            _verb = verb;
            _path = path;
            _type = typeName;
        }

        public string TypeName
        {
            get { return _type; }
        }
        public HttpMethod Verb
        {
            get { return _verb; }
        }
        public string Path
        {
            get { return _path; }
        }
    }
}
