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
