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
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace OpenNETCF.Web.Parsers
{
    internal class AspxInfo
    {
        public AspxInfo()
        {
            // Padarn defaults to true for backward compatibility
            // IIS, I believe, defaults to false
            AutoEventWireup = true;
        }

        public string CodeBehindTypeName { get; set; }
        public string CodeBehindAssemblyName { get; set; }
        public bool AutoEventWireup { get; set; }
        public string Content { get; set; }

    }

    internal class AspxParser
    {
        private const string CodeBehind = "CodeBehind";
        private const string Inherits = "Inherits";
        private const string AutoEventWireup = "AutoEventWireup";
        private const string Page = "Page";

        private static AspxParser m_instance;
        private object m_syncRoot = new object();

        private AspxParser()
        {
        }

        internal static AspxParser GetParser()
        {
            if (m_instance == null)
            {
                m_instance = new AspxParser();
            }

            return m_instance;
        }

        internal AspxInfo Parse(string filePath)
        {
            var info = new AspxInfo();

            string line;

            using(var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                StreamReader r = new StreamReader(stream);

                // We're only interested in the first line
                line = r.ReadLine();

                if (!line.StartsWith("<%@") || !line.EndsWith("%>"))
                {
                    // TODO: Stub out InvalidPageException
                    throw new /*InvalidPage*/Exception();
                }

                string[] tokens = line.Split(' ');
                int index = 1;
                int count = tokens.Length;
                if (!tokens[index++].Equals(Page))
                {
                    throw new /*InvalidPage*/Exception();
                }

                for (int i = index; i < count; i++)
                {
                    if (tokens[i].IndexOf("=") > -1)
                    {
                        // We have a name-value pair
                        string name = tokens[i].Substring(0, tokens[i].IndexOf("="));
                        string value = tokens[i].Substring(tokens[i].IndexOf("=") + 2).Trim('"');


                        switch (name)
                        {
                            case Inherits:
                                info.CodeBehindTypeName = value;
                                break;
                            case CodeBehind:
                                info.CodeBehindAssemblyName = value;
                                break;
                            case AutoEventWireup:
                                try
                                {
                                    info.AutoEventWireup = bool.Parse(value);
                                }
                                catch
                                {
                                    info.AutoEventWireup = true;
                                }
                                break;
                        }
                    }
                }

                
                //StringBuilder content = new StringBuilder();

                //StringBuilder aspx = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
                //aspx.Append(r.ReadToEnd());
                
                //XmlTextReader reader = XmlReader.Create(
                //var doc = new XmlDocument();
                //doc.Load(aspx.ToString());

                //XmlParserContext ctx = new XmlParserContext(null, null, string.Empty, XmlSpace.Default);
                //XmlTextReader reader = new XmlTextReader(tmp, XmlNodeType.Element, ctx);
                //while (reader.Read())
                //{
                //    Debug.WriteLine(reader.Name);
                //}

                //var doc = new XmlDocument();
                //doc.Load(stream);

                //info.Content = content.ToString();
            }

            return info;
        }
    }
}
