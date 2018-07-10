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
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web.Configuration;
using OpenNETCF.Web.Logging;
using SBCustomCertStorage;
using SBX509;
using SecureBlackbox.SSLSocket.Server;
using System.Threading;
using OpenNETCF.Web.Server.Ssl;

namespace OpenNETCF.Web.Server
{
    class HttpsSocket : SocketWrapperBase
    {
        private ElServerSSLSocket m_socket;
        private ElServerSSLSocket m_clientSocket;
        private ILogProvider m_logProvider;
        private static SBSessionPool.TElSessionPool m_sessionPool;
#if WindowsCE
        private static SBWinCertStorage.TElWinCertStorage m_certStorage = new SBWinCertStorage.TElWinCertStorage();
#else
        private static TElMemoryCertStorage m_certStorage = new TElMemoryCertStorage();
#endif
        private object m_syncRoot = new object();
        private const string SSL_EVAL_LICENSE_KEY = "45AFCF5995D8921D98A50E0C2EBB83E65E2EC1CCFDCD0222B07F4D6CFD8C7D4F4088B4137D74C89C873D2AC07F567311DBF2568E84C2CCD1DB9D84E0C7B2F37FA5D9FB68A37446F71E4C1EBBD3B82D41F1CA8952872AF57128CD7199B9931ED8294CCC9EFEDBA255F0B2902F934E036475148C72B73996057F55DB8E0DF8540DF553DF9F33FF1A666FDB5AA9BACAE288DB06DD41C917679265387F32F6AD7532F80EE36C636D4D633C45B1CD890ED8DD4337E6567D9E91DD3AEE53672D17D08CD3A0D2D66385443A7A3A5F1928ED62796FBCE9C0F4B288687B269E3E27E2F9B4399E2C2E0825B919F56FAB3C1FD247A723D368723F00749932BB0F1C74BFB7A1";
        private static short m_protocols = 0;
        private static ServerConfig m_config;

        static HttpsSocket()
        {
            m_config = Configuration.ServerConfig.GetConfig();

            if (string.IsNullOrEmpty(m_config.SSLLicenseKey))
            {
                // Get default License key
                SBUtils.Unit.SetLicenseKey(SSL_EVAL_LICENSE_KEY);
            }
            else
            {
                SBUtils.Unit.SetLicenseKey(m_config.SSLLicenseKey);
            }

            // Set security protocols            
            if (m_config.Security.Tls10) m_protocols += SBSSLConstants.Unit.sbTLS1;
            if (m_config.Security.Tls11) m_protocols += SBSSLConstants.Unit.sbTLS11;
            if (m_config.Security.Tls12) m_protocols += SBSSLConstants.Unit.sbTLS12;

            if (m_config.Security.ResumeSession)
            {
                m_sessionPool = new SBSessionPool.TElSessionPool();
            }

#if WindowsCE
            TElX509Certificate cert = LoadCertificate(m_config.CertificateName, m_config.CertificatePassword);
            m_certStorage.Add(cert, "Root", true, false, false);

            TElX509Certificate a = m_certStorage.GetCertificates(0);
            int b = m_certStorage.Count;

            m_certStorage.SystemStores.Add("Root");
#else
            m_certStorage.Add(LoadCertificate(m_config.CertificateName, m_config.CertificatePassword), true);
#endif
        }

        public HttpsSocket(ILogProvider logProvider)
        {
            m_logProvider = logProvider;
        }

        private void m_socket_OnError(object sender, int errorCode, bool fatal, bool remote)
        {
            m_logProvider.LogPadarnError(string.Format("ErrorCode = {0}, fatal = {1}, remote = {2}", errorCode, fatal, remote), null);
        }

        private static TElX509Certificate LoadCertificate(string certificateName, string certificatePassword)
        {
            // We do not use LoadFromFileAuto or LoadFromBuffer here because it works only on desktop => Invalid Certificate Data on WinCe        
            // TODO : deal with PEM & SPC
            using (FileStream fs = new FileStream(certificateName, FileMode.Open))
            {
                TElX509Certificate cert = new TElX509Certificate();
                switch (cert.LoadFromStreamPFX(fs, certificatePassword, 0))
                {
                    case 0:
                        return cert;

                    case 7955: // SB_PKCS12_ERROR_INVALID_PASSWORD
                        throw new HttpException("Invalid certificate password");

                    default:
                        throw new HttpException(string.Format("Unable to load the certificate from '{0}'", certificateName));
                }
            }
        }

        public override void Create(System.Net.Sockets.AddressFamily af, System.Net.Sockets.SocketType type, System.Net.Sockets.ProtocolType proto)
        {
            lock (m_syncRoot)
            {
                m_socket = new ElServerSSLSocket(new Socket(af, type, proto));                
                m_socket.Versions = m_protocols;

                // To speed the answer, we're using only the fastest cipher suites (based on experimentation ...)
                for (short i = SBSSLConstants.__Global.SB_SUITE_FIRST; i < SBSSLConstants.__Global.SB_SUITE_LAST; i++)
                {
                    m_socket.set_CipherSuites(i, false);
                }

                // let the user overide cipher suites if they desire
                if (m_config.Security.CipherList != null && m_config.Security.CipherList.Count > 0)
                {
                    foreach (var c in m_config.Security.CipherList)
                    {
                        m_socket.set_CipherSuites(c, true);
                    }
                }
                else
                {
                    m_socket.set_CipherSuites(SBSSLConstants.__Global.SB_SUITE_RSA_3DES_SHA, true);
                    m_socket.set_CipherSuites(SBSSLConstants.__Global.SB_SUITE_RSA_AES128_SHA, true);
                    m_socket.set_CipherSuites(SBSSLConstants.__Global.SB_SUITE_RSA_AES256_SHA, true);
                }

                m_socket.CustomCertStorage = m_certStorage;
                m_socket.OnError += new SBSSLCommon.TSBErrorEvent(m_socket_OnError);
            }
        }

        protected override void ReleaseManagedResources()
        {
            Close();
        }

        public override void Create(SocketWrapperBase sock)
        {
            lock (m_syncRoot)
            {
                m_socket = ((HttpsSocket)sock).m_socket;
            }
        }

        public void Create(ElServerSSLSocket sock)
        {
            lock (m_syncRoot)
            {
                m_socket = sock;
            }
        }

        public override void Bind(System.Net.IPEndPoint ep)
        {
            lock (m_syncRoot)
            {
                m_socket.Bind(ep);
            }
        }

        public override void Listen(int numConn)
        {
            lock (m_syncRoot)
            {
                m_socket.Listen(numConn);
            }
        }

        public override IAsyncResult BeginAccept(AsyncCallback cb, object state)
        {
            lock (m_syncRoot)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    try
                    {
                        m_clientSocket = m_socket.Accept();
                        if (m_config.Security.ResumeSession)
                        {
                            // Setting here a session pool, allow to enable session resume capabilities on the server 
                            m_clientSocket.SessionPool = m_sessionPool;
                        }
                        m_clientSocket.OpenSSLSession();
                    }
                    catch (Exception ex)
                    {
                        m_logProvider.LogRuntimeInfo(ZoneFlags.SSL, "Exception during OpenSSLSession : " + ex.Message);
                        // Error during handshaking : closing all sockets
                        m_clientSocket.Dispose();
                        m_clientSocket = null; // setting this to null = error in EndAccept
                    }

                    cb(new SslAsyncResult(this));
                });

                return null;
            }
        }

        public override void Close()
        {
            lock (m_syncRoot)
            {
                m_socket.Close(true);
            }
        }

        public override bool Connected
        {
            get
            {
                lock (m_syncRoot)
                {
                    return m_socket.Connected;
                }
            }
        }

        public override SocketWrapperBase EndAccept(IAsyncResult asyncResult)
        {
            if (IsDisposed) return null;

            lock (m_syncRoot)
            {
                if (m_socket == null || m_clientSocket == null) return null;

                HttpsSocket result = new HttpsSocket(m_logProvider);
                result.Create(m_clientSocket);

                return result;
            }
        }

        public override NetworkStreamWrapperBase CreateNetworkStream()
        {
            lock (m_syncRoot)
            {
                return new SslNetworkStream(m_socket);
            }
        }

        public override EndPoint LocalEndPoint
        {
            get
            {
                lock (m_syncRoot)
                {

                    return m_socket.LocalEndPoint;
                }
            }
        }

        public override EndPoint RemoteEndPoint
        {
            get
            {
                lock (m_syncRoot)
                {
                    return m_socket.RemoteEndPoint;
                }
            }
        }

        public override int Available
        {
            get
            {
                lock (m_syncRoot)
                {
                    return m_socket.Available;
                }
            }
        }

        public override IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }


        public override int Receive(byte[] buffer)
        {
            lock (m_syncRoot)
            {
                if (!m_socket.Connected) return 0;
                return m_socket.Receive(buffer);
            }
        }

        public override void Shutdown(SocketShutdown how)
        {
            lock (m_syncRoot)
            {
                m_socket.Shutdown(how);
            }
        }

        public IntPtr Handle
        {
            get
            {
                return m_socket.Socket.Handle;
            }
        }
    }

}