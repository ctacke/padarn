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
#if !WindowsCE
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using OpenNETCF.Web;
using OpenNETCF.Web.Hosting;
using OpenNETCF.Web.Configuration;
using OpenNETCF.Web.Logging;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Authentication;

namespace OpenNETCF.Web.Server
{
    internal class HttpsRequestListener : IRequestListener
    {
        public event ListenerStateChange OnStateChange;
    
        private ILogProvider m_logProvider;
        private static X509Certificate m_certificate = null;
        private TcpListener m_listener;
        private int m_port;
        private int m_maxConnections;
        private bool m_shutDown = false;


        /// <summary>
        /// Create an instance of the listener on the specified port.
        /// </summary>
        /// <param name="port">The port to listen on for incoming requests.</param>
        /// <param name="maxConnections">The maximum number of clients that can concurrently connect to listener.</param>
        /// <param name="localIP"></param>
        public HttpsRequestListener(IPAddress localIP, int port, int maxConnections, ILogProvider logProvider)
        {
            if (port <= 0) throw new ArgumentException("port");
            if (maxConnections <= 0) throw new ArgumentException("maxConnections");

            m_logProvider = logProvider;

            m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener | ZoneFlags.Startup, string.Format("Creating HttpsRequestListener at {0}:{1} Max connections = {2}", localIP, port, maxConnections));
            m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener | ZoneFlags.Startup, "SSL Enabled");

            m_maxConnections = maxConnections;
            m_port = port;

            if(m_certificate == null)
            {
                m_certificate = X509Certificate.CreateFromCertFile(@"..\..\..\..\..\..\Engine\Certs\padarn_server.cer");
            }
            m_listener = new TcpListener(localIP, port);
            m_listener.Start();

            // TODO: handle maxConnections
        }

        public bool ProcessingRequest
        {
            get { return true; } // TODO
        }

        /// <summary>
        /// Listen for incoming HTTP requests
        /// </summary>
        public void StartListening()
        {
            m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener | ZoneFlags.Startup, "+HttpsRequestListener.StartListening");

            try
            {
                m_shutDown = false;
                m_listener.BeginAcceptTcpClient(AcceptRequest, null);
                RaiseStateChanged(true);


            }
            catch
            {
                RaiseStateChanged(false);
            }
            finally
            {
                m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener, "-HttpsRequestListener.StartListening");
            }
        }

        private void RaiseStateChanged(bool running)
        {
            var handler = OnStateChange;

            if(handler != null)
            {
                try
                {
                    OnStateChange(running);
                }
                catch { }
            }
        }

        private X509Certificate LocalSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return null;
        }

        private bool RemoteValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void AcceptRequest(IAsyncResult result)
        {
            m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener, "+HttpsRequestListener.AcceptRequest");

            try
            {
                var client = m_listener.EndAcceptTcpClient(result);
                if (m_shutDown) return;

                SslStream sslStream = new SslStream(client.GetStream(), false, RemoteValidationCallback);
                
                try
                {
                    sslStream.AuthenticateAsServer(m_certificate, false, SslProtocols.Default, true);
                    sslStream.ReadTimeout = 5000;
                    sslStream.WriteTimeout = 5000;

                    try
                    {
                        HttpWorkerRequest wr = new SslWorkerRequest(client, sslStream, m_logProvider);
                        HttpRuntime.ProcessRequest(wr, m_logProvider);
                    }
                    catch (Exception ex)
                    {
                        string text = string.Format("HttpRuntime.ProcessRequest thread threw {0}: {1}", ex.GetType().Name, ex.Message);
                        m_logProvider.LogPadarnError(text, null);
                    }
                }
                catch (AuthenticationException e)
                {
                    Console.WriteLine("Exception: {0}", e.Message);
                    if (e.InnerException != null)
                    {
                        Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                    }
                    Console.WriteLine("Authentication failed - closing the connection.");
                    sslStream.Close();
                    client.Close();
                    return;
                }
                catch (IOException iox)
                {
                    // maybe http was used, not https
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    // The client stream will be closed with the sslStream
                    // because we specified this behavior when creating
                    // the sslStream.
                    sslStream.Close();
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                string text = string.Format("HttpsRequestListener.AcceptRequest threw {0}: {1}", ex.GetType().Name, ex.Message);
                m_logProvider.LogPadarnError(text, null);
            }
            finally
            {
                try
                {
                    m_listener.BeginAcceptTcpClient(AcceptRequest, null);
                }
                catch (ObjectDisposedException)
                {
                    // this may occur on server shutdown - swallow it and move on
                }

                m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener, "-HttpsRequestListener.AcceptRequest");
            }
        }

        public void Shutdown()
        {
            RaiseStateChanged(false);
            m_shutDown = true;
            if (m_listener != null)
            {
                m_listener.Stop();
            }
        }

        public void Dispose()
        {
            Shutdown();
        }

    }
}
#endif