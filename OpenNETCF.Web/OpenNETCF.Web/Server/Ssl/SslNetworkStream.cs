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
using System.IO;
using System.Collections.Generic;
using System.Text;
using SBSSLServer;
using SecureBlackbox.SSLSocket.Server;
using OpenNETCF.Web.Server.Ssl;

namespace OpenNETCF.Web.Server
{
    class SslNetworkStream : NetworkStreamWrapperBase
    {
        private SslStream m_stream;
        private object m_syncRoot = new object();

        public SslNetworkStream(ElServerSSLSocket socket)
        {
            m_stream = new SslStream(socket);
        }

        protected override void Dispose(bool disposing)
        {
            lock (m_syncRoot)
            {
                m_stream.Dispose();
            }
        }

        public override bool CanRead
        {
            get
            {
                lock (m_syncRoot)
                {
                    return m_stream.CanRead;
                }
            }
        }
        public override bool CanSeek
        {
            get
            {
                lock (m_syncRoot)
                {
                    return m_stream.CanSeek;
                }
            }
        }

        public override bool CanWrite
        {
            get
            {
                lock (m_syncRoot)
                {
                    return m_stream.CanWrite;
                }
            }
        }

        public override bool DataAvailable
        {
            get
            {
                lock (m_syncRoot)
                {
                    //TODO
                    return CanRead;
                    //                    return m_stream.DataAvailable;
                }
            }
        }

        public override long Length
        {
            get
            {
                lock (m_syncRoot)
                {
                    return m_stream.Length;
                }
            }
        }

        public override long Position
        {
            get
            {
                lock (m_syncRoot)
                {
                    return m_stream.Position;
                }
            }
            set
            {
                lock (m_syncRoot)
                {
                    m_stream.Position = value; ;
                }
            }
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            lock (m_syncRoot)
            {
                return m_stream.BeginRead(buffer, offset, size, callback, state);
            }
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            lock (m_syncRoot)
            {
                return m_stream.BeginWrite(buffer, offset, size, callback, state);
            }
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            lock (m_syncRoot)
            {
                return m_stream.EndRead(asyncResult);
            }
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            lock (m_syncRoot)
            {
                m_stream.EndWrite(asyncResult);
            }
        }

        public override void Flush()
        {
            lock (m_syncRoot)
            {
                m_stream.Flush();
            }
        }

        public override int Read(byte[] buffer, int offset, int size)
        {
            lock (m_syncRoot)
            {
                return m_stream.Read(buffer, offset, size);
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            lock (m_syncRoot)
            {
                return m_stream.Seek(offset, origin);
            }
        }

        public override void SetLength(long value)
        {
            lock (m_syncRoot)
            {
                m_stream.SetLength(value);
            }
        }

        public override void Write(byte[] buffer, int offset, int size)
        {
            lock (m_syncRoot)
            {
                m_stream.Write(buffer, offset, size);
            }
        }

        protected override Stream GetStream()
        {
            return m_stream;
        }
    }
}
