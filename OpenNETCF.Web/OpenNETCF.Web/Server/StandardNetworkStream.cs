using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Server
{
    class StandardNetworkStream: NetworkStreamWrapperBase
    {
        private NetworkStream m_stream;
        private object m_syncRoot = new object();

        internal StandardNetworkStream(NetworkStream ns)
        {
            m_stream = ns;
        }

        protected override void Dispose(bool disposing)
        {
            lock (m_syncRoot)
            {
                m_stream.Close();
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
                    return m_stream.DataAvailable;
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
