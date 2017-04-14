using OpenNETCF.Web.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.IO;

namespace OpenNETCF.Web.Test
{
    
    
    /// <summary>
    ///This is a test class for ServerConfigurationHandlerTest and is intended
    ///to contain all ServerConfigurationHandlerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServerConfigurationHandlerTest
    {
        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        public void EmptyHandlers()
        {
            ServerConfigurationHandler target = new ServerConfigurationHandler(); // TODO: Initialize to an appropriate value
            object parent = null; // TODO: Initialize to an appropriate value
            object configContext = null; // TODO: Initialize to an appropriate value
            ServerConfig actual;
            XmlDocument d = new XmlDocument();
            d.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><WebServer/>");

            actual = target.Create(parent, configContext, d.SelectSingleNode("WebServer")) as ServerConfig;

            Assert.AreEqual(0,actual.HttpHandlers.Count);
        }

        [TestMethod()]
        public void HandlersParsed()
        {
            ServerConfigurationHandler target = new ServerConfigurationHandler(); // TODO: Initialize to an appropriate value
            object parent = null; // TODO: Initialize to an appropriate value
            object configContext = null; // TODO: Initialize to an appropriate value
            ServerConfig actual;
            XmlDocument d = new XmlDocument();
            d.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><WebServer><httpHandlers><add verb=\"*\" path=\"xyz\" type=\"System.String\"/></httpHandlers></WebServer>");

            actual = target.Create(parent, configContext, d.SelectSingleNode("WebServer")) as ServerConfig;

            Assert.AreEqual(1, actual.HttpHandlers.Count);
        }

        [TestMethod]
        [Description("Proves Create populates the Rest property")]
        public void CreateRest()
        {
            ServerConfigurationHandler target = new ServerConfigurationHandler(); // TODO: Initialize to an appropriate value
            object parent = null; 
            object configContext = null; 
            ServerConfig actual;
            XmlDocument d = new XmlDocument();
            d.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><WebServer><restServices/></WebServer>");

            actual = target.Create(parent, configContext, d.SelectSingleNode("WebServer")) as ServerConfig;

            Assert.IsNotNull(actual.Rest);

        }
    }
}
