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

using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Globalization;

namespace OpenNETCF.Web.Server
{
    internal class SecureSocket : SocketWrapperBase
    {
        private Socket m_socket;

        public override void Create(SocketWrapperBase sock)
        {
            m_socket = ((SecureSocket)sock).m_socket;
        }

        public override void Create(System.Net.Sockets.AddressFamily af, System.Net.Sockets.SocketType type, System.Net.Sockets.ProtocolType proto)
        {
            throw new NotImplementedException();
        }

        public override void Bind(System.Net.IPEndPoint ep)
        {
            throw new NotImplementedException();
        }

        public override void Listen(int numConn)
        {
            throw new NotImplementedException();
        }

        public override IAsyncResult BeginAccept(AsyncCallback cb, object state)
        {
            throw new NotImplementedException();
        }

        public override SocketWrapperBase EndAccept(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override bool Connected
        {
            get { throw new NotImplementedException(); }
        }

        public override NetworkStreamWrapperBase CreateNetworkStream()
        {
            throw new NotImplementedException();
        }

        public override System.Net.EndPoint RemoteEndPoint
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Net.EndPoint LocalEndPoint
        {
            get { throw new NotImplementedException(); }
        }

        public override int Available
        {
            get { throw new NotImplementedException(); }
        }

        public override int Receive(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public override void Shutdown(System.Net.Sockets.SocketShutdown how)
        {
            throw new NotImplementedException();
        }
    }

    class SslHelper : IDisposable
    {

        private const ushort SO_SECURE = 0x2001;

        private const ushort SO_SEC_SSL = 0x2004;

        private const int _SO_SSL_FLAGS = 0x02;

        private const int _SO_SSL_VALIDATE_CERT_HOOK = 0x08;

        private const int SO_SSL_FAMILY = 0x00730000;

        private const long _SO_SSL = ((2L << 27) | SO_SSL_FAMILY);

        private const uint IOC_IN = 0x80000000;

        private const long SO_SSL_SET_VALIDATE_CERT_HOOK = (IOC_IN | _SO_SSL | _SO_SSL_VALIDATE_CERT_HOOK);

        private const long SO_SSL_SET_FLAGS = (IOC_IN | _SO_SSL | _SO_SSL_FLAGS);



        private const int SSL_CERT_X59 = 1;

        private const int SSL_ERR_OKAY = 0;

        private const int SSL_ERR_FAILED = 2;

        private const int SSL_ERR_BAD_LEN = 3;

        private const int SSL_ERR_BAD_TYPE = 4;

        private const int SSL_ERR_BAD_DATA = 5;

        private const int SSL_ERR_NO_CERT = 6;

        private const int SSL_ERR_BAD_SIG = 7;

        private const int SSL_ERR_CERT_EXPIRED = 8;

        private const int SSL_ERR_CERT_REVOKED = 9;

        private const int SSL_ERR_CERT_UNKNOWN = 10;

        private const int SSL_ERR_SIGNATURE = 11;

        private const int SSL_CERT_FLAG_ISSUER_UNKNOWN = 0x0001;





        public delegate int SSLVALIDATECERTFUNC(uint dwType, IntPtr pvArg, uint dwChainLen, IntPtr pCertChain, uint dwFlags);

        private IntPtr ptrHost;

        private IntPtr hookFunc;



        public SslHelper(Socket socket, string host)
        {

            //The managed SocketOptionName enum doesn't have SO_SECURE so here we cast the integer value

            socket.SetSocketOption(SocketOptionLevel.Socket, (SocketOptionName)SO_SECURE, SO_SEC_SSL);



            //We need to pass a function pointer and a pointer to a string containing the host

            //to unmanaged code

            hookFunc = Marshal.GetFunctionPointerForDelegate(new SSLVALIDATECERTFUNC(ValidateCert));



            //Allocate the buffer for the string

            ptrHost = Marshal.AllocHGlobal(host.Length + 1);

            WriteASCIIString(ptrHost, host);



            //Now put both pointers into a byte[]

            var inBuffer = new byte[8];

            var hookFuncBytes = BitConverter.GetBytes(hookFunc.ToInt32());

            var hostPtrBytes = BitConverter.GetBytes(ptrHost.ToInt32());

            Array.Copy(hookFuncBytes, inBuffer, hookFuncBytes.Length);

            Array.Copy(hostPtrBytes, 0, inBuffer, hookFuncBytes.Length, hostPtrBytes.Length);



            unchecked
            {

                socket.IOControl((int)SO_SSL_SET_VALIDATE_CERT_HOOK, inBuffer, null);

            }

        }



        private static void WriteASCIIString(IntPtr basePtr, string s)
        {

            byte[] bytes = Encoding.ASCII.GetBytes(s);

            for (int i = 0; i < bytes.Length; i++)

                Marshal.WriteByte(basePtr, i, bytes[i]);



            //null terminate the string

            Marshal.WriteByte(basePtr, bytes.Length, 0);

        }



        #region IDisposable Members



        ~SslHelper()
        {

            ReleaseHostPointer();

        }



        public void Dispose()
        {

            GC.SuppressFinalize(this);

            ReleaseHostPointer();

        }



        private void ReleaseHostPointer()
        {

            if (ptrHost != IntPtr.Zero)
            {

                Marshal.FreeHGlobal(ptrHost);

                ptrHost = IntPtr.Zero;

            }

        }



        #endregion



        private int ValidateCert(uint dwType, IntPtr pvArg, uint dwChainLen, IntPtr pCertChain, uint dwFlags)
        {

            //According to http://msdn.microsoft.com/en-us/library/ms940451.aspx:

            //

            //- dwChainLen is always 1

            //- Windows CE performs the cert chain validation

            //- pvArg is the context data we passed into the SO_SSL_SET_VALIDATE_CERT_HOOK call so in our

            //- case is the host name

            //

            //So here we are responsible for validating the dates on the certificate and the CN





            if (dwType != SSL_CERT_X59)

                return SSL_ERR_BAD_TYPE;



            //When in debug mode let self-signed certificates through ...

#if !DEBUG

            if ((dwFlags & SSL_CERT_FLAG_ISSUER_UNKNOWN) != 0)

                return SSL_ERR_CERT_UNKNOWN;

#endif



            Debug.Assert(dwChainLen == 1);



            //Note about the note: an unmanaged long is 32 bits, unlike a managed long which is 64. I was missing 

            //this fact when I wrote the comment. So the docs are accurate.


            //NOTE: The documentation says pCertChain is a pointer to a LPBLOB struct:

            //

            // {ulong size, byte* data} 

            //

            //in reality the size is a 32 bit integer (not 64).

            int certSize = Marshal.ReadInt32(pCertChain);

            IntPtr pData = Marshal.ReadIntPtr(new IntPtr(pCertChain.ToInt32() + sizeof(int)));



            byte[] certData = new byte[certSize];



            for (int i = 0; i < certSize; i++)

                certData[i] = Marshal.ReadByte(pData, (int)i);



            X509Certificate2 cert;

            try
            {

                cert = new X509Certificate2(certData);

            }

            catch (ArgumentException) { return SSL_ERR_BAD_DATA; }

            catch (CryptographicException) { return SSL_ERR_BAD_DATA; }



            //Validate the expiration date

            if (DateTime.Now > DateTime.Parse(cert.GetExpirationDateString(), CultureInfo.CurrentCulture))

                return SSL_ERR_CERT_EXPIRED;



            //Validate the effective date

            if (DateTime.Now < DateTime.Parse(cert.GetEffectiveDateString(), CultureInfo.CurrentCulture))

                return SSL_ERR_FAILED;



            string certName = cert.GetName();

            Debug.WriteLine(certName);



            //Validate the CN

            string host = ReadAnsiString(pvArg);

            if (!certName.Contains("CN=" + host))

                return SSL_ERR_FAILED;



            return SSL_ERR_OKAY;

        }



        private static string ReadAnsiString(IntPtr pvArg)
        {

            byte[] buffer = new byte[1024];

            int j = 0;

            do
            {

                buffer[j] = Marshal.ReadByte(pvArg, j);

                j++;

            } while (buffer[j - 1] != 0);

            string host = Encoding.ASCII.GetString(buffer, 0, j - 1);

            return host;

        }
    }
}
