using OpenNETCF.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Web.Configuration;
using System.Xml;
using OpenNETCF.Web.Rest;
using System.Reflection;

namespace OpenNETCF.Web.Test
{
    
    
    /// <summary>
    ///This is a test class for RestHandlerTest and is intended
    ///to contain all RestHandlerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RestHandlerTest
    {

        /// <summary>
        ///A test for GetTypeForRequest
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OpenNETCF.Web.dll")]
        public void GetTypeForRequestTest()
        {
            Assert.Fail();
            //string path = "/myresthandler" ;
            
            //RestHandler_Accessor target = new RestHandler_Accessor(); // TODO: Initialize to an appropriate value
            //Type expected = typeof(RestServiceMock); // TODO: Initialize to an appropriate value
            //RestHandler_Accessor._restTypes.Add(path, expected);
            //Type actual;
            //actual = target.GetTypeForRequest(path);
            //Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [Description("Proves InitializeTypes adds the type with the correct path.")]
        public void InitializeTypesAddsTheTypeWithPath()
        {
            Assert.Fail();
            //string path = "/directory/path/blah" ;
            //ServerConfig config = new ServerConfig();
            //ServerConfig_Accessor a = ServerConfig_Accessor.AttachShadow(config);
            //ServerConfig_Accessor.m_instance = config;

            //XmlDocument d = new XmlDocument();
            //d.LoadXml(string.Format("<restServices><Assemblies><add name=\"{0}\" /></Assemblies></restServices>", this.GetType().Assembly.GetName().Name));
            //RestConfig_Accessor restConfig = new RestConfig_Accessor(d);
            //a.Rest = restConfig;

            //RestHandler target = new RestHandler();
            //RestHandler_Accessor ra = RestHandler_Accessor.AttachShadow(target);

            //RestHandler_Accessor.InitializeTypes();

            //Assert.IsNotNull(RestHandler_Accessor._restTypes);
            //Assert.AreNotEqual(0, RestHandler_Accessor._restTypes.Count);
            //RestMethodKey k = new RestMethodKey { UriTemplate = path, Method = "GET" };
            //Assert.IsTrue(RestHandler_Accessor._restTypes.ContainsKey(k));
        }

        [TestMethod]
        [Description("Proves GetMethodForRequest returns the correct MethodInfo.")]
        public void GetMethodForRequest()
        {
            ServerConfig config = new ServerConfig();
            ServerConfig_Accessor a = ServerConfig_Accessor.AttachShadow(config);
            ServerConfig_Accessor.m_instance = config;

            XmlDocument d = new XmlDocument();
            d.LoadXml(string.Format("<restServices><Assemblies><add name=\"{0}\" /></Assemblies></restServices>", this.GetType().Assembly.GetName().Name));
            RestConfig_Accessor restConfig = new RestConfig_Accessor(d);
            
            a.Rest = restConfig;

            RestHandler target = new RestHandler();
            RestHandler_Accessor ra = RestHandler_Accessor.AttachShadow(target);

            MethodInfo m = ra.GetMethodForRequest("/directory/path/blah", HttpMethod.GET);
            MethodInfo expected = typeof(RestServiceMock).GetMethod("GetSomething");

            Assert.IsNotNull(m);
            Assert.AreEqual(expected, m);
            
        }

        [TestMethod]
        [Description("Proves ProcessRequest invokes the correct method.")]
        public void ProcessRequestInvokes()
        {
            ServerConfig config = new ServerConfig();
            ServerConfig_Accessor a = ServerConfig_Accessor.AttachShadow(config);
            ServerConfig_Accessor.m_instance = config;

            XmlDocument d = new XmlDocument();
            d.LoadXml(string.Format("<restServices><Assemblies><add name=\"{0}\" /></Assemblies></restServices>", this.GetType().Assembly.GetName().Name));
            RestConfig_Accessor restConfig = new RestConfig_Accessor(d);

            a.Rest = restConfig;

            IHttpHandler target = new RestHandler();
            RestHandler_Accessor ra = RestHandler_Accessor.AttachShadow(target);

            
            HttpWorkerRequestMock r = new HttpWorkerRequestMock();
            r.HttpVerb = "GET";
            r.UriPath = "/directory/path/blah";
            HttpContext context = new HttpContext(r,true);
            HttpContext_Accessor hca = HttpContext_Accessor.AttachShadow(context);

            RestServiceMock.Clear();
            target.ProcessRequest(context);
            Assert.IsTrue(RestServiceMock.Instantiated);
            MethodInfo expected = typeof(RestServiceMock).GetMethod("GetSomething");
            Assert.AreEqual(expected, RestServiceMock.Invoked);



        }
    }

    [ServiceContract]
    public class RestServiceMock
    {
        public static bool Instantiated = false;
        public static MethodInfo Invoked = null;

        public RestServiceMock()
        {
            Instantiated = true;
        }

        public static void Clear()
        {
            Instantiated = false;
            Invoked = null;
        }

        [WebGet(UriTemplate = "/directory/path/blah")]
        public int GetSomething()
        {
            Invoked = this.GetType().GetMethod("GetSomething");
            return 0;
        }
    }

    public class HttpWorkerRequestMock : HttpWorkerRequest
    {
        public string HttpVerb = string.Empty;
        public string UriPath = string.Empty;

        public override void EndOfRequest()
        {
            throw new NotImplementedException();
        }

        public override void FlushResponse(bool finalFlush)
        {
            throw new NotImplementedException();
        }

        protected override void ReadRequestHeaders()
        {
            throw new NotImplementedException();
        }

        public override void ProcessRequest()
        {
            throw new NotImplementedException();
        }

        public override string GetUriPath()
        {
            return UriPath;
        }

        public override string GetLocalAddress()
        {
            throw new NotImplementedException();
        }

        public override int GetLocalPort()
        {
            throw new NotImplementedException();
        }

        public override string GetHttpVersion()
        {
            throw new NotImplementedException();
        }

        public override string GetHttpVerbName()
        {
            return HttpVerb;
        }

        public override string GetQueryString()
        {
            throw new NotImplementedException();
        }

        public override string GetRemoteAddress()
        {
            throw new NotImplementedException();
        }

        public override void SendResponseFromMemory(byte[] data, int length)
        {
            throw new NotImplementedException();
        }

        public override void SendKnownResponseHeader(string name, string value)
        {
            throw new NotImplementedException();
        }
    }
}
