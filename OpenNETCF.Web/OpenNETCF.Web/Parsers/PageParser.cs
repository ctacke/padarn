using System;

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Collections.Specialized;
using OpenNETCF.Web.UI.WebControls;
using OpenNETCF.Web.UI;

namespace OpenNETCF.Web.Parsers
{
    internal class PageParser
    {
        private Dictionary<string, ControlBuilder> m_controlBuilders = new Dictionary<string, ControlBuilder>();

        private static PageParser m_instance;

        private const string CodeBehind = "CodeBehind";
        private const string Inherits = "Inherits";
        private const string AutoEventWireup = "AutoEventWireup";

        private PageParser()
        {
            // TODO: load up all of the control builders
            m_controlBuilders.Add("button", new BasicControlBuilder<Button>("input"));
            m_controlBuilders.Add("label", new BasicControlBuilder<Label>("span"));
            m_controlBuilders.Add("textbox", new BasicControlBuilder<TextBox>("input"));
            m_controlBuilders.Add("linkbutton", new BasicControlBuilder<LinkButton>("a"));
        }

        internal static PageParser GetParser()
        {
            if (m_instance == null)
            {
                m_instance = new PageParser();
            }

            return m_instance;
        }

        internal string PreParse(string content, out string docType, out AspxInfo info)
        {
            content = StripDocType(content, out docType);
            content = ParseAspTags(content, out info);

            return content;
        }

        private object m_syncRoot = new object();
        private Page m_currentPage;

        internal string Parse(Page page, string content)
        {
            lock (m_syncRoot)
            {
                m_currentPage = page;

                if (content.Length > 0)
                {
                    XmlDocument doc = new XmlDocument();

                    NameTable nt = new NameTable();
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
                    nsmgr.AddNamespace("asp", "http://www.w3.org/1999/xhtml");
                    XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);
                    XmlReaderSettings xset = new XmlReaderSettings();
                    xset.ConformanceLevel = ConformanceLevel.Fragment;
                    XmlReader rd = XmlReader.Create(new StringReader(content), xset, context);
                    doc.Load(rd);

                    //            Dump(doc.FirstChild, 0);

                    content = ParseDocument(page, doc);

                    //page = ParseAspControls(page);

                    return string.Format("\r\n\r\n{0}\r\n\r\n{1}", page.DTDHeader, content);
                }

                return string.Empty;
            }
        }

        private string ParseDocument(Page page, XmlDocument doc)
        {
            var htmlNode = doc.FirstChild;
            // TODO: validate this is the "html" node?

            StringBuilder sb = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };                 

            using(XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartElement(htmlNode.LocalName, htmlNode.NamespaceURI);
                ParseNode(page, htmlNode, false, writer);
                writer.WriteFullEndElement();
                writer.Flush();
            }

            return sb.ToString();
        }

        private void InsertServerSideFormInfrastructure(XmlWriter writer)
        {
            writer.WriteStartElement("div");
            writer.WriteRaw("<input type=\"hidden\" name=\"__EVENTTARGET\" id=\"__EVENTTARGET\" value=\"\" />");
            writer.WriteRaw("<input type=\"hidden\" name=\"__EVENTARGUMENT\" id=\"__EVENTARGUMENT\" value=\"\" />");
            writer.WriteEndElement();

            writer.WriteStartElement("script");
            writer.WriteAttributeString("type", "text/javascript");
            writer.WriteRaw("\r\n//<![CDATA[\r\n");
            writer.WriteRaw("var theForm = document.forms['form1'];\r\n");
            writer.WriteRaw("if (!theForm) {\r\n");
            writer.WriteRaw("    theForm = document.form1;\r\n");
            writer.WriteRaw("}\r\n");
            writer.WriteRaw("function __doPostBack(eventTarget, eventArgument) {\r\n");
            writer.WriteRaw("    if (!theForm.onsubmit || (theForm.onsubmit() != false)) {\r\n");
            writer.WriteRaw("        theForm.__EVENTTARGET.value = eventTarget;\r\n");
            writer.WriteRaw("        theForm.__EVENTARGUMENT.value = eventArgument;\r\n");
            writer.WriteRaw("        theForm.submit();\r\n");
            writer.WriteRaw("    }\r\n");
            writer.WriteRaw("}\r\n");
            writer.WriteRaw("//]]>\r\n");
            writer.WriteEndElement();
        }

        private void ParseNode(Page page, XmlNode parent, bool runatServer, XmlWriter writer)
        {
            foreach (XmlNode node in parent.ChildNodes)
            {
                Debug.WriteLine(node.Name);

                bool currentNodeIsServer = node.Attributes == null ? false : node.Attributes["runat"] != null;
                bool server = runatServer ? true : currentNodeIsServer;


                if (server && node.Prefix == "asp")
                {
                    ParseAspControl(page, node, writer);
                }
                else if (currentNodeIsServer && node.LocalName == "form")
                {
                    writer.WriteStartElement("form");
                    var id = node.Attributes["id"].Value;
                    writer.WriteAttributeString("name", id);
                    writer.WriteAttributeString("id", id);
                    writer.WriteAttributeString("method", "post");
                    writer.WriteAttributeString("action", m_currentPage.Request.Path);

                    InsertServerSideFormInfrastructure(writer);

                    if (node.HasChildNodes)
                    {
                        ParseNode(page, node, server, writer);
                    }

                    writer.WriteFullEndElement();
                }
                else
                {
                    if (node.NodeType == XmlNodeType.Text)
                    {
                        writer.WriteRaw(node.InnerText);
                        continue;
                    }
                    else
                    {
                        writer.WriteStartElement(node.Prefix, node.LocalName, null);

                        foreach (XmlAttribute attr in node.Attributes)
                        {
                            if (server && (attr.Name == "runat"))
                            {
                                continue;
                            }
                            writer.WriteAttributeString(attr.Name, attr.Value);
                        }

                        if (node.HasChildNodes)
                        {
                            ParseNode(page, node, server, writer);
                        }

                        writer.WriteFullEndElement();
                    }
                }
            }
        }

        private void ParseAspControl(Page page, XmlNode node, XmlWriter writer)
        {
            var name = node.LocalName.ToLower();
            if (m_controlBuilders.ContainsKey(name))
            {
                Hashtable parms = new Hashtable();

                foreach (XmlAttribute attrib in node.Attributes)
                {
                    parms.Add(attrib.Name.ToLower(), attrib.Value);
                }

                var builder = m_controlBuilders[name];

                var control = Activator.CreateInstance(builder.ControlType) as WebControl;
                control.SetParameters(parms);

                control.Content = node.InnerText;

                page.Controls.Add(control);

                control.Render(writer);
            }
            else
            {
                throw new NotSupportedException(string.Format("Unsupported server control: '{0}'", name));
            }
        }

        private void Dump(XmlNode parent, int level)
        {
            string spacer = new string(' ', level * 4);

            foreach (XmlNode node in parent.ChildNodes)
            {
                Debug.WriteLine(spacer + node.Name);

                if (node.Attributes["runat"] != null)
                {
                    Debug.WriteLine("server");
                }

                if (node.HasChildNodes)
                {
                    Dump(node, level + 1);
                }
            }
        }

        internal string StripDocType(string source, out string docType)
        {
            string regex = "<!DOCTYPE(.*?)>";
            var matches = Regex.Matches(source, regex, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            docType = string.Empty;

            foreach (Match match in matches)
            {
                docType = match.Value;
                source = source.Replace(docType, string.Empty);
            }

            return source;
        }

        private IDictionary GetControlParameters(string controlText)
        {
            return null;
        }

        internal string ParseAspTags(string source, out AspxInfo info)
        {
            info = new AspxInfo();

            string regex = "<%(.*?)%>";
            var matches = Regex.Matches(source, regex, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match match in matches)
            {
                if (match.Value.StartsWith("<%@ Page"))
                {
                    info = ParsePageInfo(match.Value);
                    source = source.Replace(match.Value, string.Empty);
                }
                else
                {
                    source = source.Replace(match.Value, ParseASP(match.Groups[1].Value));
                }
            }

            return source;
        }

        private AspxInfo ParsePageInfo(string tag)
        {
            var info = new AspxInfo();

            string[] tokens = tag.Split(' ');
            int index = 1;
            int count = tokens.Length;

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

            return info;
        }

        private string ParseASP(string tag)
        {
            // TODO:
            return "<pre>[translated asp]</pre>";
        }

    }
}
