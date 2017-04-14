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

namespace OpenNETCF.Web.Server
{
    internal delegate void RequestEventHandler(object sender, RequestEventArgs e);
    internal delegate void ListenerStateChange(bool listening);

    internal class HttpRequestListener : IRequestListener
    {
        private event RequestEventHandler OnReceiveRequest;
        public event ListenerStateChange OnStateChange;

        private const int MAX_BUFFER = 32768;

        private int m_port;
        private int m_maxConnections;
        private SocketWrapperBase m_serverSocket;
        private ManualResetEvent requestDone = new ManualResetEvent(false);
        private bool m_shutDown = false;
        private ILogProvider m_logProvider;

        /// <summary>
        /// Create an instance of the listener on the specified port.
        /// </summary>
        /// <param name="port">The port to listen on for incoming requests.</param>
        /// <param name="maxConnections">The maximum number of clients that can concurrently connect to listener.</param>
        /// <param name="localIP"></param>
        public HttpRequestListener(IPAddress localIP, int port, int maxConnections, ILogProvider logProvider)
        {
            m_logProvider = logProvider;

            if (port <= 0) throw new ArgumentException("port");
            if (maxConnections <= 0) throw new ArgumentException("maxConnections");

            m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener | ZoneFlags.Startup, string.Format("Creating HttpRequestListener at {0}:{1} Max connections = {2}", localIP, port, maxConnections));

            m_maxConnections = maxConnections;
            m_port = port;

            IPEndPoint localEndpoint = new IPEndPoint(localIP, m_port);

            if (ServerConfig.GetConfig().UseSsl == true)
            {
                m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener | ZoneFlags.Startup, "SSL Enabled");
                m_serverSocket = new HttpsSocket(m_logProvider);
            }
            else
            {
                m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener | ZoneFlags.Startup, "SSL Disabled");
                m_serverSocket = new HttpSocket();
            }
            m_serverSocket.Create(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener | ZoneFlags.Startup, string.Format("Binding to {0}:{1}", localEndpoint.Address, localEndpoint.Port));
                m_serverSocket.Bind(localEndpoint);
            }
            catch(Exception ex)
            {
                m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener | ZoneFlags.Startup, string.Format("Failed to binding to {0}:{1}: {2}", localEndpoint.Address, localEndpoint.Port, ex.Message));
                throw;
            }

            OnReceiveRequest += ProcessRequest;
        }

        /// <summary>
        /// Listen for incoming HTTP requests
        /// </summary>
        public void StartListening()
        {
            m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener | ZoneFlags.Startup, "+HttpRequestListener.StartListening");

            try
            {
                m_shutDown = false;
                m_serverSocket.Listen(m_maxConnections);

                m_serverSocket.BeginAccept(AcceptRequest, m_serverSocket);

                RaiseStateChanged(true);
            }
            catch
            {
                RaiseStateChanged(false);
            }
            finally
            {
                m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener, "-HttpRequestListener.StartListening");
            }
        }

        private void RaiseStateChanged(bool running)
        {
            if (OnStateChange != null)
            {
                try
                {
                    OnStateChange(running);
                }
                catch { }
            }
        }

        public void Shutdown()
        {
            RaiseStateChanged(false);
            OnReceiveRequest -= ProcessRequest;
            m_shutDown = true;
            m_serverSocket.Close();
        }

        public void Dispose()
        {
            if ((m_serverSocket != null) && (!m_serverSocket.IsDisposed))
            {
                m_serverSocket.Dispose();
            }
            RaiseStateChanged(false);
        }

        private void AcceptRequest(IAsyncResult result)
        {
            m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener, "+HttpRequestListener.AcceptRequest");

            try
            {
                // connected will be false if we've called for an explicit Stop to the server
                //TODO : check why the socket is not connected but it works
                //if ((Environment.OSVersion.Platform == PlatformID.WinCE) && (!m_serverSocket.Connected))
                //{
                //    // NOTE:
                //    // this appears to only be true under CE.  The desktop Connected state is false when the request is accepted.
                //    return;
                //}

                SocketWrapperBase listener = (SocketWrapperBase)result.AsyncState;
                if (listener.IsDisposed) return;

                if (m_serverSocket.IsDisposed || m_shutDown)
                    return;

                SocketWrapperBase handler = listener.EndAccept(result);
                if ((handler == null) || (!handler.Connected)) return;

                RequestEventArgs e = new RequestEventArgs(handler);
                var orr = OnReceiveRequest;
                if (orr != null)
                {
                    orr(this, e);
                }
            }
            catch (Exception ex)
            {
                string text = string.Format("HttpRequestListener.AcceptRequest threw {0}: {1}", ex.GetType().Name, ex.Message);
                m_logProvider.LogPadarnError(text, null);
            }
            finally
            {
                try
                {
                    m_serverSocket.BeginAccept(AcceptRequest, m_serverSocket);
                }
                catch (ObjectDisposedException)
                {
                    // this may occur on server shutdown - swallow it and move on
                }

                m_logProvider.LogRuntimeInfo(ZoneFlags.RequestListener, "-HttpRequestListener.AcceptRequest");
            }
        }

        public bool ProcessingRequest
        {
            get { return m_clients.Count > 0; }
        }

        private void ProcessRequest(object sender, RequestEventArgs e)
        {
            if (m_shutDown) return;
            ProcessRequestWorker(e.Socket);
        }

        private List<IPAddress> m_clients = new List<IPAddress>();

        private void ProcessRequestWorker(object parm)
        {
            SocketWrapperBase sock = (SocketWrapperBase)parm;
            var et = Environment.TickCount;
            try
            {
                var client = (sock.RemoteEndPoint as IPEndPoint).Address;
                
                lock (m_clients)
                {
                    if (!m_clients.Contains(client))
                    {
                        if (m_clients.Count >= m_maxConnections)
                        {
                            var errorHandler = new DefaultWorkerRequest(sock, m_logProvider);
                            HttpRuntime.ProcessError(errorHandler, HttpErrorCode.Forbidden, "Maximum Connections Exceeded");
                            return;
                        }
                        else
                        {
                            m_clients.Add(client);
                        }
                    }
                }

                ThreadPool.QueueUserWorkItem(delegate
                {
                    try
                    {
                        HttpWorkerRequest wr = new AsyncWorkerRequest(sock, m_logProvider);
                        HttpRuntime.ProcessRequest(wr, m_logProvider);
                    }
                    catch (Exception ex)
                    {
                        string text = string.Format("HttpRuntime.ProcessRequest thread threw {0}: {1}", ex.GetType().Name, ex.Message);
                        m_logProvider.LogPadarnError(text, null);
                    }
                    finally
                    {
                        lock (m_clients)
                        {
                            m_clients.Remove(client);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                string text = string.Format("HttpRequestListener.ProcessRequest threw {0}: {1}", ex.GetType().Name, ex.Message);
                m_logProvider.LogPadarnError(text, null);
            }
            finally
            {
                et = Environment.TickCount - et;
            }
        }

        private string ReadAvailableData(NetworkStream ns)
        {
            if (ns.CanRead)
            {
                int read = 0;
                byte[] buffer = new byte[MAX_BUFFER];
                do
                {
                    try
                    {
                        read += ns.Read(buffer, 0, 1024);
                    }
                    catch (IOException)
                    {
                        break;
                    }
                } while (read <= MAX_BUFFER && ns.DataAvailable);

                return Encoding.UTF8.GetString(buffer, 0, read);
            }
            else
                return string.Empty;
        }
    }
}