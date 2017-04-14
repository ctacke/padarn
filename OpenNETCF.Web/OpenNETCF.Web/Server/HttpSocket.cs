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
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OpenNETCF.Web.Server
{
    class HttpSocket : SocketWrapperBase
    {
        private Socket m_socket;
        private object m_syncRoot = new object();

        public override void Create(System.Net.Sockets.AddressFamily af, System.Net.Sockets.SocketType type, System.Net.Sockets.ProtocolType proto)
        {
            lock (m_syncRoot)
            {
                m_socket = new Socket(af, type, proto);
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
                m_socket = ((HttpSocket)sock).m_socket;
            }
        }

        public void Create(Socket sock)
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
                try
                {
                    return m_socket.BeginAccept(cb, state);
                }
                catch (ObjectDisposedException)
                {
                    // can happen if the containing app is shut down
                    return null;
                }
            }
        }

        public override void Close()
        {
            lock (m_syncRoot)
            {
#if !WindowsCE
                if (m_socket.Connected) m_socket.Disconnect(false);
#endif

                m_socket.Close();
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
                if (m_socket == null) return null;

                HttpSocket result = new HttpSocket();
                result.Create(m_socket.EndAccept(asyncResult));
                return result;
            }
        }

        public override NetworkStreamWrapperBase CreateNetworkStream()
        {
            lock (m_syncRoot)
            {
                return new StandardNetworkStream(new NetworkStream(m_socket));
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
            lock (m_syncRoot)
            {
                return m_socket.BeginReceive(buffer, offset, size, socketFlags, callback, state);
            }
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
                lock (m_syncRoot)
                {
                    return m_socket.Handle;
                }
            }
        }
    }
}
