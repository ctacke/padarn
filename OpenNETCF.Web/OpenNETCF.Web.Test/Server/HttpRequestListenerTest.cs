using OpenNETCF.Web.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using System;

namespace OpenNETCF.Web.Server
{
    [TestClass()]
    public class HttpRequestListenerTest : TestBase
    {
      /*
        [TestMethod()]
        [Description("Ensures that the HttpRequestListener throws an argument exception for port numbers == 0")]
        public void HttpRequestListenerCTorBadPortTestZero()
        {
            ArgumentException expected = null;

            try
            {
                HttpRequestListener_Accessor target = new HttpRequestListener_Accessor(0, 5);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures that the HttpRequestListener throws an argument exception for port numbers < 0")]
        public void HttpRequestListenerCTorBadPortTestLTZero()
        {
            ArgumentException expected = null;

            try
            {
                HttpRequestListener_Accessor target = new HttpRequestListener_Accessor(-1, 5);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures that the HttpRequestListener throws an argument exception for max connections == 0")]
        public void HttpRequestListenerCTorBadMaxConnectionsZero()
        {
            ArgumentException expected = null;

            try
            {
                HttpRequestListener_Accessor target = new HttpRequestListener_Accessor(80, 0);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures that the HttpRequestListener throws an argument exception for max connections < 0")]
        public void HttpRequestListenerCTorBadMaxConnectionsLTZero()
        {
            ArgumentException expected = null;

            try
            {
                HttpRequestListener_Accessor target = new HttpRequestListener_Accessor(80, -1);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [Ignore]
        [TestMethod()]
        [Description("Ensures that the HttpRequestListener can be created when given a valid port and number of connections")]
        public void HttpRequestListenerCTorPositive()
        {
            HttpRequestListener_Accessor target = new HttpRequestListener_Accessor(80, 5);
            Assert.IsNotNull(target);
        }
       * */
    }
}
