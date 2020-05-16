using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Server
{
  class RequestEventArgs : EventArgs
  {
    private readonly SocketWrapperBase m_socket;
    private readonly NetworkStreamWrapperBase m_stream;

    public NetworkStreamWrapperBase Stream
    {
      get { return m_stream; }
    }

    public SocketWrapperBase Socket
    {
      get { return m_socket; }
    }

    public RequestEventArgs(SocketWrapperBase socket)
    {
      m_socket = socket;
      m_stream = socket.CreateNetworkStream();
    }
  }
}
