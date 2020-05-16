using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OpenNETCF.Web.Server.Ssl
{
    class SslAsyncResult : IAsyncResult
    {
        public SslAsyncResult(object state)
        {
            this.AsyncState = state;
        }

        public object AsyncState { get; private set; }

        public System.Threading.WaitHandle AsyncWaitHandle { get { throw new NotImplementedException(); } }

        public bool CompletedSynchronously { get { throw new NotImplementedException(); } }

        public bool IsCompleted { get { throw new NotImplementedException(); } }
    }
}
