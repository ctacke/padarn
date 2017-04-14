using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Web.Configuration;
using System.Xml;

namespace OpenNETCF.Web.Test.Configuration
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class HttpHandlersConfigSectionTest
    {

        [TestMethod]
        public void HttpHandlersCountCorrect()
        {
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement("httpHandlers");
            XmlAttribute verb;
            XmlAttribute path;
            XmlAttribute type;
            d.AppendChild(root);
            XmlNode add;

            add = d.CreateElement("add");
            verb = d.CreateAttribute("verb");
            add.Attributes.Append(verb);
            verb.Value = "*";
            path = d.CreateAttribute("path");
            add.Attributes.Append(path);
            path.Value = "MyPath";
            type = d.CreateAttribute("type");
            add.Attributes.Append(type);
            type.Value = "System.String";
            root.AppendChild(add);

            add = d.CreateElement("add");
            verb = d.CreateAttribute("verb");
            add.Attributes.Append(verb);
            verb.Value = "*";
            path = d.CreateAttribute("path");
            add.Attributes.Append(path);
            path.Value = "Bob";
            type = d.CreateAttribute("type");
            add.Attributes.Append(type);
            type.Value = "System.String";
            root.AppendChild(add);

            HttpHandlersConfigSection s = new HttpHandlersConfigSection(root);

            Assert.AreEqual(2, s.Count);

        }
    }
}
