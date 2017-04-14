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

namespace OpenNETCF.Web.Server
{
    abstract class SocketWrapperBase : IDisposable
    {
        public abstract void Create(SocketWrapperBase sock);
        public abstract void Create(AddressFamily af, SocketType type, ProtocolType proto);
        public abstract void Bind(IPEndPoint ep);
        public abstract void Listen(int numConn);
        public abstract IAsyncResult BeginAccept(AsyncCallback cb, object state);
        public abstract SocketWrapperBase EndAccept(IAsyncResult asyncResult);
        public abstract void Close();
        public abstract bool Connected { get;}
        public abstract NetworkStreamWrapperBase CreateNetworkStream();
        public abstract EndPoint RemoteEndPoint { get; }
        public abstract EndPoint LocalEndPoint { get; }
        public abstract int Available { get; }
        public abstract int Receive(byte[] buffer);
        public abstract IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state);
        public abstract void Shutdown(SocketShutdown how);

        public bool IsDisposed { get; protected set; }
        
        public virtual void Dispose() 
        {
            ReleaseManagedResources();
            ReleaseNativeResources();
            GC.SuppressFinalize(this);

            IsDisposed = true; 
        }
        protected virtual void ReleaseManagedResources()
        {
        }

        protected virtual void ReleaseNativeResources()
        {
        }

        ~SocketWrapperBase()
        {
            ReleaseNativeResources();
        }
    }
}
