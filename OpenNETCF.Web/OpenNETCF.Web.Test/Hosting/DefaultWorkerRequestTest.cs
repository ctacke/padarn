using OpenNETCF.Web.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Web;
using OpenNETCF.Web.Logging;
using OpenNETCF.Web.Server;
using System.IO;
using OpenNETCF.Web.Configuration;
using System;

namespace OpenNETCF.Web.Test
{
    
    
    /// <summary>
    ///This is a test class for DefaultWorkerRequestTest and is intended
    ///to contain all DefaultWorkerRequestTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DefaultWorkerRequestTest
    {



        /// <summary>
        ///A test for GetHandlerForFilename
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OpenNETCF.Web.dll")]
        public void GetHandlerForFilenameTest()
        {
            DefaultWorkerRequest target = new DefaultWorkerRequest(new SocketWrapperBaseMock(), new LogProviderMock());
            DefaultWorkerRequest_Accessor a = DefaultWorkerRequest_Accessor.AttachShadow(target);
            
            
            string fileName = "/mypath";
            string mimeType = string.Empty; // TODO: Initialize to an appropriate value
            IHttpHandler actual;
            
            ServerConfig cfg = new ServerConfig();
            cfg.HttpHandlers.Add(new HttpHandler(HttpMethod.ANY, fileName, "HttpHandlerMock"));
            ServerConfig_Accessor.m_instance = cfg;

            actual = a.GetHandlerForFilename(fileName, mimeType, HttpMethod_Accessor.ANY );
            Assert.AreEqual(typeof(HttpHandlerMock), actual.GetType());

        }

        [TestMethod]
        public void IsCustomHandlerPositive()
        {
            DefaultWorkerRequest target = new DefaultWorkerRequest(new SocketWrapperBaseMock(), new LogProviderMock());
            DefaultWorkerRequest_Accessor a = DefaultWorkerRequest_Accessor.AttachShadow(target);
            // fake out the config
            ServerConfig cfg = new ServerConfig();
            cfg.HttpHandlers.Add(new HttpHandler(HttpMethod.ANY, "/mypath", "HttpHandlerMock"));
            ServerConfig_Accessor.m_instance = cfg;

            Assert.IsNotNull( a.GetCustomHandler("/mypath", HttpMethod_Accessor.ANY));

        }

        [TestMethod]
        public void IsCustomHandlerChecksVerb()
        {
            DefaultWorkerRequest target = new DefaultWorkerRequest(new SocketWrapperBaseMock(), new LogProviderMock());
            DefaultWorkerRequest_Accessor a = DefaultWorkerRequest_Accessor.AttachShadow(target);
            // fake out the config
            ServerConfig cfg = new ServerConfig();
            cfg.HttpHandlers.Add(new HttpHandler(HttpMethod.GET, "/mypath", "HttpHandlerMock"));
            ServerConfig_Accessor.m_instance = cfg;

            Assert.IsNull(a.GetCustomHandler("/mypath", HttpMethod_Accessor.POST));
        }

        public class HttpHandlerMock : IHttpHandler
        {
            #region IHttpHandler Members

            void IHttpHandler.ProcessRequest(HttpContext context)
            {
                throw new System.NotImplementedException();
            }

            #endregion
        }

        public class LogProviderMock : ILogProvider
        {
            #region ILogProvider Members

            OpenNETCF.Web.Configuration.ServerConfig ILogProvider.ServerConfiguration
            {
                get
                {
                    throw new System.NotImplementedException();
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }

            void ILogProvider.LogPageAccess(LogDataItem dataItem)
            {
                
            }

            void ILogProvider.LogPadarnError(string errorInfo, LogDataItem dataItem)
            {
                
            }

            void ILogProvider.LogRuntimeInfo(ZoneFlags zoneMask, string info)
            {
                
            }

            #endregion
        }

        internal class SocketWrapperBaseMock : Server.SocketWrapperBase
        {
            public override void Create(OpenNETCF.Web.Server.SocketWrapperBase sock)
            {
                throw new System.NotImplementedException();
            }

            public override void Create(System.Net.Sockets.AddressFamily af, System.Net.Sockets.SocketType type, System.Net.Sockets.ProtocolType proto)
            {
                throw new System.NotImplementedException();
            }

            public override void Bind(System.Net.IPEndPoint ep)
            {
                throw new System.NotImplementedException();
            }

            public override void Listen(int numConn)
            {
                throw new System.NotImplementedException();
            }

            public override System.IAsyncResult BeginAccept(System.AsyncCallback cb, object state)
            {
                throw new System.NotImplementedException();
            }

            public override OpenNETCF.Web.Server.SocketWrapperBase EndAccept(System.IAsyncResult asyncResult)
            {
                throw new System.NotImplementedException();
            }

            public override void Close()
            {
                throw new System.NotImplementedException();
            }

            public override bool Connected
            {
                get { throw new System.NotImplementedException(); }
            }

            public override OpenNETCF.Web.Server.NetworkStreamWrapperBase CreateNetworkStream()
            {
                return new NetworkStreamWrapperBaseMock();
            }

            public override System.Net.EndPoint RemoteEndPoint
            {
                get { throw new System.NotImplementedException(); }
            }

            public override System.Net.EndPoint LocalEndPoint
            {
                get { throw new System.NotImplementedException(); }
            }

            public override int Available
            {
                get { throw new System.NotImplementedException(); }
            }

            public override int Receive(byte[] buffer)
            {
                throw new System.NotImplementedException();
            }

            public override void Shutdown(System.Net.Sockets.SocketShutdown how)
            {
                throw new System.NotImplementedException();
            }
        }

        internal class NetworkStreamWrapperBaseMock : NetworkStreamWrapperBase
        {
            public override bool CanRead
            {
                get { throw new System.NotImplementedException(); }
            }

            public override bool CanSeek
            {
                get { throw new System.NotImplementedException(); }
            }

            public override bool CanWrite
            {
                get { throw new System.NotImplementedException(); }
            }

            public override bool DataAvailable
            {
                get { throw new System.NotImplementedException(); }
            }

            public override long Length
            {
                get { throw new System.NotImplementedException(); }
            }

            public override long Position
            {
                get
                {
                    throw new System.NotImplementedException();
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }

            public override System.IAsyncResult BeginRead(byte[] buffer, int offset, int size, System.AsyncCallback callback, object state)
            {
                throw new System.NotImplementedException();
            }

            public override System.IAsyncResult BeginWrite(byte[] buffer, int offset, int size, System.AsyncCallback callback, object state)
            {
                throw new System.NotImplementedException();
            }

            protected override void Dispose(bool disposing)
            {
                throw new System.NotImplementedException();
            }

            public override int EndRead(System.IAsyncResult asyncResult)
            {
                throw new System.NotImplementedException();
            }

            public override void EndWrite(System.IAsyncResult asyncResult)
            {
                throw new System.NotImplementedException();
            }

            public override void Flush()
            {
                throw new System.NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int size)
            {
                throw new System.NotImplementedException();
            }

            public override long Seek(long offset, System.IO.SeekOrigin origin)
            {
                throw new System.NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new System.NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int size)
            {
                throw new System.NotImplementedException();
            }

            protected override System.IO.Stream GetStream()
            {
                return new MemoryStream();
            }
        }
    }
}
