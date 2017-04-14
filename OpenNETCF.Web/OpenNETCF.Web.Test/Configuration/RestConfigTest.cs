using OpenNETCF.Web.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;

namespace OpenNETCF.Web.Test
{
    
    
    /// <summary>
    ///This is a test class for RestConfigTest and is intended
    ///to contain all RestConfigTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RestConfigTest
    {
        /// <summary>
        ///A test for RestConfig Constructor
        ///</summary>
        [TestMethod()]
        public void RestConfigConstructorTest()
        {
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement("restServices");
            d.AppendChild(root);
            XmlNode assemblies = d.CreateElement("Assemblies");
            root.AppendChild(assemblies);

            XmlNode add;
            XmlAttribute name;
            add = d.CreateElement("add");
            assemblies.AppendChild(add);
            name = d.CreateAttribute("name");
            name.Value = this.GetType().Assembly.FullName ;
            add.Attributes.Append(name);



            RestConfig target = new RestConfig(d);

            Assert.AreEqual(1, target.Count);
        }
    }
}
