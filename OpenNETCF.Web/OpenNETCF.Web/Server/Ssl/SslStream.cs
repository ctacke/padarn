using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SecureBlackbox.SSLSocket.Server;

namespace OpenNETCF.Web.Server.Ssl
{
    public class SslStream : MemoryStream
    {
        private ElServerSSLSocket m_socket;
        private object m_syncRoot = new object();

        public SslStream(ElServerSSLSocket socket)
        {
            m_socket = socket;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            m_socket.Send(buffer, offset, count);
        }
    }
}
