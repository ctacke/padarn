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
//====================================================
//
//          EldoS SecureBlackbox Library            
//                                                    
//      Copyright (c) 2002-2013 EldoS Corporation     
//           http://www.secureblackbox.com            
//====================================================
using System;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using SecureBlackbox;
using SecureBlackbox.SSLSocket;
using SBSSLCommon;
using SBSSLConstants;
using SBSSLServer;
using SBX509;
//using SecureBlackbox.SSLServer.SBSSLServer;

namespace SecureBlackbox.SSLSocket.Server
{
	/// <summary>
	/// Summary description for SSLSocketServer.
    /// </summary>
#if (!WindowsCE)
    [ToolboxItem(true)]
#if (!MONO)
    [System.Drawing.ToolboxBitmap(typeof(ElServerSSLSocket), "SecureBlackbox.SSLSocket.Server.ElServerSSLSocket.bmp")]
#endif
#endif
    public class ElServerSSLSocket : ElSSLSocket 
	{
		protected SBSSLServer.TElSSLServer SBSSLServer;
        private TSBCloseReason closeDescription = 0;
		private bool errorOccured = false;
        private bool isListener = true;

		private SSLSocketAsyncAcceptResult beginAcceptAsyncResult;
				
		public void InternalValidate(ref SBX509.TSBCertificateValidity Validity, 
			ref int Reason)
		{
			SBSSLServer.InternalValidate(ref Validity, ref Reason);		
		}

		public ElServerSSLSocket() : base()
		{
		}

		public ElServerSSLSocket(Socket s) : base()
		{
			s.Blocking = true;
			Socket = s;
		}

        public ElServerSSLSocket(Socket s, bool isConnectionSocket)
            : base()
        {
            Socket = s;
            if (isConnectionSocket)
            {
                InitializeAcceptedSSLSocket(null);
            }
            else
            {
                Socket.Blocking = true;
            }
        }

		private ElServerSSLSocket(SSLSocketAsyncAcceptResult ar) : base()
		{
			beginAcceptAsyncResult = ar;
		}

		protected override void Dispose(bool Disposing)
		{
			if (!fDisposed)
			{
				if (Disposing)
				{
					Close(true);
					SBSSLServer.Dispose();
				}
				fDisposed = true;
			}
			base.Dispose(Disposing);
		}

		~ElServerSSLSocket()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		#region Properties and events
		public bool Active 
		{ 
			get
			{
				return SBSSLServer.Active;
			}		
		}

		public override bool get_CipherSuites(short Index)
		{
#if (CHROME_LIB && MONO)
			return SBSSLServer.GetCipherSuite(Index);
#else
			return SBSSLServer.get_CipherSuites(Index);
#endif
		}

		public override void set_CipherSuites(short Index, bool Value)
		{
#if (CHROME_LIB && MONO)
                        // bug in MONO-1.1.9.2 ?
			SBSSLServer.SetCipherSuite(Index, Value);
#else
			SBSSLServer.set_CipherSuites(Index, Value);
#endif
		}

		public override short CompressionAlgorithm 
		{ 
			get
			{
				return SBSSLServer.CompressionAlgorithm;
			}
		}	  

		public override bool get_CompressionAlgorithms(short Index)
		{
#if (CHROME_LIB && MONO)
                        // bug in MONO-1.1.9.2 ?
			return SBSSLServer.GetCompressionAlgorithms(Index);
#else
			return SBSSLServer.get_CompressionAlgorithms(Index);
#endif
		}

		public override void set_CompressionAlgorithms(short Index, bool Value)
		{
#if (CHROME_LIB && MONO)
                        // bug in MONO-1.1.9.2 ?
			SBSSLServer.SetCompressionAlgorithms(Index, Value);
#else
			SBSSLServer.set_CompressionAlgorithms(Index, Value);
#endif
		}

        public override int GetCipherSuitePriority(short Index)
        {
#if (CHROME_LIB && MONO)
                        // bug in MONO-1.1.9.2 ?
			return SBSSLServer.GetCipherSuitePriority(Index);
#else
            return SBSSLServer.get_CipherSuitePriorities(Index);
#endif
        }

        public override void SetCipherSuitePriority(short Index, int Value)
        {
#if (CHROME_LIB && MONO)
                        // bug in MONO-1.1.9.2 ?
			SBSSLServer.SetCipherSuitePriority(Index, Value);
#else
            SBSSLServer.set_CipherSuitePriorities(Index, Value);
#endif
        }

        public override long TotalBytesSent
        {
            get { return SBSSLServer.TotalBytesSent; }
            set { }
        }

        public override long TotalBytesReceived
        {
            get { return SBSSLServer.TotalBytesReceived; }
            set { }
        }

        public override short Options
        {
            get { return SBSSLServer.Options; }
            set { SBSSLServer.Options = value; }
        }

		public bool ClientAuthentication 
		{ 
			get
			{
				return SBSSLServer.ClientAuthentication;
			}
			set
			{
				SBSSLServer.ClientAuthentication = value;
			}
		}

        public SBCustomCertStorage.TElCustomCertStorage CustomCertStorage
        {
            get
            {
                return SBSSLServer.CertStorage;
            }
            set
            {
                SBSSLServer.CertStorage = value;
            }
        }

		public SBCustomCertStorage.TElCustomCertStorage ClientCertStorage 
		{ 
			get
			{
				return SBSSLServer.ClientCertStorage;
			}
			set
			{
				SBSSLServer.ClientCertStorage = value;
			}
		}

		public override short CurrentVersion 
		{ 
			get
			{
				return SBSSLServer.CurrentVersion;
			}
		}


		public bool ForceCertificateChain 
		{ 
			get
			{
				return SBSSLServer.ForceCertificateChain;	
			}
			set
			{
				SBSSLServer.ForceCertificateChain = value;
			}
		}

		public SBSessionPool.TElSessionPool SessionPool 
		{ 
			get
			{
				return SBSSLServer.SessionPool;
			}
			set
			{
				SBSSLServer.SessionPool = value;
			}
		}

		public override short Versions 
		{ 
			get
			{
				return SBSSLServer.Versions;
			}
			set
			{
				SBSSLServer.Versions = value;
			}
		}

		public override bool SSLEnabled
		{
			get
			{
				return SBSSLServer.Enabled;
			}
			set
			{
				if (SBSSLServer.Enabled != value)
				{
					if (!value || !transport.Connected)
						SBSSLServer.Enabled = value;
					else
					{
                        fIgnoreClose = true;
                        try
                        {
                            SBSSLServer.Close(true);
                        }
                        finally
                        {
                            fIgnoreClose = false;
                        }
						
						SBSSLServer.Enabled = true;
						opened = false;
						closed = false;
						if (fBlocking)
						{
							IntOpenSSLSession(true);
						}
						else
						{
                            Open();
							BeginReceive();
						}
					}
				}
			}
		}

        public SBSSLCommon.TElServerSSLExtensions Extensions
        {
            get { if (SBSSLServer != null) { return SBSSLServer.Extensions; } else { return null; } }
        }

        public SBSSLCommon.TElCustomSSLExtensions PeerExtensions
        {
            get { if (SBSSLServer != null) { return SBSSLServer.PeerExtensions; } else { return null; } }
        }

		public event SBSSLCommon.TSBCertificateValidateEvent OnCertificateValidate;
		public event SBUtils.TNotifyEvent OnCiphersNegotiated;
		public event SBSSLCommon.TSBErrorEvent OnError;
        public event SBSSLServer.TSBCertificateURLsEvent OnCertificateURLs;
        public event SBSSLCommon.TSBExtensionsReceivedEvent OnExtensionsReceived;
        public event SBSSLServer.TSBServerKeyNeededEvent OnKeyNeeded;
        public event SBSSLCommon.TSBRenegotiationStartEvent OnRenegotiationStart;

		#endregion

		#region Event handlers

		private void OnSecureServerOpenConnection(Object sender)
		{
			OnSecureOpenConnection();
			if (!transport.Blocking && beginAcceptAsyncResult != null)
			{
				beginAcceptAsyncResult.AcceptedSocket = this;
				beginAcceptAsyncResult.Parent.beginAcceptAsyncResult = null;
				CompleteAcceptAsyncResult();
			}
		}

        public TSBCloseReason CloseDescription
		{
			get
			{
				if (!closed)
					throw new InvalidOperationException("Socket not closed.");
				return closeDescription;
			}
		}

		private void OnSecureServerCloseConnection(Object sender, 
			TSBCloseReason closeDescription)
		{
			this.closeDescription = closeDescription;
			OnSecureCloseConnection();
		}

		private void OnSecureServerOnData(Object sender, byte[] buffer)
		{
			OnSecureOnData(buffer);
		}

		private void OnSecureServerSend(Object sender, byte[] buffer)
		{
			OnSecureSend(buffer);
		}

		private void OnSecureServerReceive(Object sender, 
			ref byte[] buffer, int maxSize, out int written)
		{
			OnSecureReceive(buffer, maxSize, out written);
		}

		private void OnSecureServerCiphersNegotiated(Object sender)
		{
			if (OnCiphersNegotiated != null)
				OnCiphersNegotiated(this);
		}

		private void OnSecureServerError(Object sender, int errorCode, bool fatal, bool remote)
		{
			errorOccured = true;
			if (OnError != null)
				OnError(this, errorCode, fatal, remote);
		}		

		private void OnSecureServerCertificateValidate(Object sender,
			SBX509.TElX509Certificate certificate, ref bool validate)
		{
			if (OnCertificateValidate != null)
				OnCertificateValidate(this, certificate, ref validate);
		}

        private void  OnSecureServerRenegotiationStart(object Sender, bool Safe, ref bool Allow)
        {
 	        if (OnRenegotiationStart != null) 
                OnRenegotiationStart(this, Safe, ref Allow);
        }

        private void  OnSecureServerKeyNeeded(object Sender, string Identity, ref byte[] Key)
        {
 	        if (OnKeyNeeded != null) 
                OnKeyNeeded(this, Identity, ref Key);
        }

        private void  OnSecureServerExtensionsReceived(object Sender)
        {
 	        if (OnExtensionsReceived != null) 
                OnExtensionsReceived(this);
        }

        private void  OnSecureServerCertificateURLs(object Sender, TElClientCertURLsSSLExtension URLs, SBCustomCertStorage.TElMemoryCertStorage Certificates)
        {
 	        if (OnCertificateURLs != null) 
                OnCertificateURLs(this, URLs, Certificates);
        }

		private void OnSocketAcceptCallback(IAsyncResult asyncResult)
		{
			try
			{
				Socket s = transport.EndAccept(asyncResult);
				s.Blocking = false;
				ElServerSSLSocket sslsocket = new ElServerSSLSocket(beginAcceptAsyncResult);
				sslsocket.Blocking = false;
				sslsocket.Socket = s;
				sslsocket.InitializeAcceptedSSLSocket(SBSSLServer);
				sslsocket.OnCertificateValidate = this.OnCertificateValidate;
				sslsocket.OnCiphersNegotiated = this.OnCiphersNegotiated;
				sslsocket.OnError = this.OnError;
                sslsocket.OnCertificateURLs = this.OnCertificateURLs;
                sslsocket.OnExtensionsReceived = this.OnExtensionsReceived;
                sslsocket.OnKeyNeeded = this.OnKeyNeeded;
                sslsocket.OnRenegotiationStart = this.OnRenegotiationStart;
				sslsocket.Open();
				sslsocket.BeginReceive();
			}
			catch(Exception ex)
			{
				ExceptionOccured(ex);
				CompleteAcceptAsyncResult();
			}
		}

		#endregion

		override protected void Reset()
		{
			base.Reset();
			closeDescription = 0;
		}

		protected override void Init()
		{
			base.Init();

			SBSSLServer = new SBSSLServer.TElSSLServer(null);

			//SBSSLServer.OnOpenConnection += new SBSSLCommon.TSBOpenConnectionEvent(OnSecureServerOpenConnection);
			//SBSSLServer.OnCloseConnection += new SBSSLServer.TSBCloseConnectionEvent(OnSecureServerCloseConnection);
			//SBSSLServer.OnData += new SBSSLCommon.TSBDataEvent(OnSecureServerOnData);
			//SBSSLServer.OnSend += new SBSSLCommon.TSBSendEvent(OnSecureServerSend);
			
			//SBSSLServer.OnReceive += new SBSSLCommon.TSBReceiveEvent(OnSecureServerReceive);
			//SBSSLServer.OnCertificateValidate += new SBSSLCommon.TSBCertificateValidateEvent(OnSecureServerCertificateValidate);
			//SBSSLServer.OnError += new SBSSLCommon.TSBErrorEvent(OnSecureServerError);
			//SBSSLServer.OnCiphersNegotiated += new SBUtils.TNotifyEvent(OnSecureServerCiphersNegotiated);

			//default initialization
			SBSSLServer.ClientAuthentication = false;
			SBSSLServer.Enabled = true;
			SBSSLServer.ForceCertificateChain = false;
			SBSSLServer.Versions = SBSSLConstants.Unit.sbTLS1 | SBSSLConstants.Unit.sbTLS11 | SBSSLConstants.Unit.sbTLS12;
            /*
#if (CHROME_LIB && MONO)
                        // bug in MONO-1.1.9.2 ?
			SBSSLServer.SetCipherSuite(SBSSLConstants.Unit.SB_SUITE_NULL_NULL_NULL, true);
#else
			SBSSLServer.set_CipherSuites(SBSSLConstants.Unit.SB_SUITE_NULL_NULL_NULL, true);
#endif
             * */ // commented out by II 20110107
		}

        protected override bool SSLErrorOccured()
        {
            return errorOccured;
        }

		private void InitializeAcceptedSSLSocket(SBSSLServer.TElSSLServer sbListenServer)
		{
            errorOccured = false;
            isListener = false;
			SBSSLServer.OnOpenConnection += new SBSSLCommon.TSBOpenConnectionEvent(OnSecureServerOpenConnection);
            SBSSLServer.OnCloseConnection += new SBSSLCommon.TSBCloseConnectionEvent(OnSecureServerCloseConnection);
			SBSSLServer.OnData += new SBSSLCommon.TSBDataEvent(OnSecureServerOnData);
			SBSSLServer.OnSend += new SBSSLCommon.TSBSendEvent(OnSecureServerSend);
			SBSSLServer.OnReceive += new SBSSLCommon.TSBReceiveEvent(OnSecureServerReceive);
			SBSSLServer.OnCertificateValidate += new SBSSLCommon.TSBCertificateValidateEvent(OnSecureServerCertificateValidate);
			SBSSLServer.OnError += new SBSSLCommon.TSBErrorEvent(OnSecureServerError);
			SBSSLServer.OnCiphersNegotiated += new SBUtils.TNotifyEvent(OnSecureServerCiphersNegotiated);
            SBSSLServer.OnCertificateURLs += new TSBCertificateURLsEvent(OnSecureServerCertificateURLs);
            SBSSLServer.OnExtensionsReceived += new TSBExtensionsReceivedEvent(OnSecureServerExtensionsReceived);
            SBSSLServer.OnKeyNeeded += new TSBServerKeyNeededEvent(OnSecureServerKeyNeeded);
            SBSSLServer.OnRenegotiationStart += new TSBRenegotiationStartEvent(OnSecureServerRenegotiationStart);
            if (sbListenServer != null)
            {
                SBSSLServer.ClientAuthentication = sbListenServer.ClientAuthentication;
                SBSSLServer.Enabled = sbListenServer.Enabled;
                SBSSLServer.ForceCertificateChain = sbListenServer.ForceCertificateChain;
                SBSSLServer.Versions = sbListenServer.Versions;
                SBSSLServer.CertStorage = sbListenServer.CertStorage;
                SBSSLServer.ClientCertStorage = sbListenServer.CertStorage;
                SBSSLServer.Extensions.Assign(sbListenServer.Extensions);
                SBSSLServer.Options = sbListenServer.Options;
                SBSSLServer.RenegotiationAttackPreventionMode = sbListenServer.RenegotiationAttackPreventionMode;

                for (short i = SBSSLConstants.Unit.SB_SUITE_FIRST;
                    i < SBSSLConstants.Unit.SB_SUITE_LAST; i++)
#if (CHROME_LIB && MONO)
				// bug in MONO-1.1.9.2 ?
				SBSSLServer.SetCipherSuite(i, 
					sbListenServer.GetCipherSuite(i));
#else
                    SBSSLServer.set_CipherSuites(i,
                        sbListenServer.get_CipherSuites(i));
#endif
                for (short i = SBSSLConstants.Unit.SSL_CA_FIRST;
                    i < SBSSLConstants.Unit.SSL_CA_LAST; i++)
#if (CHROME_LIB && MONO)
				// bug in MONO-1.1.9.2 ?
				SBSSLServer.SetCompressionAlgorithms(i, 
					sbListenServer.GetCompressionAlgorithms(i));
#else
                    SBSSLServer.set_CompressionAlgorithms(i,
                        sbListenServer.get_CompressionAlgorithms(i));
#endif
            }
		}

		protected void CompleteAcceptAsyncResult()
		{
			lock(this)
			{
				if (beginAcceptAsyncResult != null)
				{
					beginAcceptAsyncResult.Completed(innerException);
					beginAcceptAsyncResult = null;
					innerException = null;
				}
			}
		}

		public void RenegotiateCiphers()
		{
			SBSSLServer.RenegotiateCiphers();
		}
	
		public void Listen(int backlog)
		{
			transport.Listen(backlog);
		}

		public ElServerSSLSocket Accept()
		{
			if (transport == null)
				throw new NullReferenceException("Set \"Socket\" property first");
			
			transport.Blocking = true;
			Socket s = transport.Accept();
			s.Blocking = true;
			ElServerSSLSocket sslsocket = new ElServerSSLSocket();
			sslsocket.Socket = s;
			sslsocket.InitializeAcceptedSSLSocket(SBSSLServer);
			sslsocket.OnCertificateValidate = this.OnCertificateValidate;
			sslsocket.OnCiphersNegotiated = this.OnCiphersNegotiated;
			sslsocket.OnError = this.OnError;
            sslsocket.OnCertificateURLs = this.OnCertificateURLs;
            sslsocket.OnExtensionsReceived = this.OnExtensionsReceived;
            sslsocket.OnKeyNeeded = this.OnKeyNeeded;
            sslsocket.OnRenegotiationStart = this.OnRenegotiationStart;
			if (!SBSSLServer.Enabled) 
			{
				sslsocket.OpenSSLSession();
			}
			return sslsocket;
		}

		public IAsyncResult BeginAccept(AsyncCallback callback, object state)
		{
			if (transport == null)
				throw new NullReferenceException("Set \"Socket\" property first");
			if (beginAcceptAsyncResult != null)
				throw new InvalidOperationException();

			transport.Blocking = false;
			beginAcceptAsyncResult = new SSLSocketAsyncAcceptResult(this, callback, state);
			transport.BeginAccept(new AsyncCallback(OnSocketAcceptCallback), null);
			return beginAcceptAsyncResult;
		}

		public ElServerSSLSocket EndAccept(IAsyncResult asyncResult)
		{
			SSLSocketAsyncAcceptResult ar = (SSLSocketAsyncAcceptResult)asyncResult;
			ar.AsyncWaitHandle.WaitOne();
			if (ar.Exception != null)
				throw ar.Exception;
			if (!SBSSLServer.Enabled) 
			{
				ar.AcceptedSocket.OpenSSLSession();
			}
			return ar.AcceptedSocket;
		}

		public override void Close(bool Silent)
		{
            fSilentClose = Silent;
			SBSSLServer.Close(Silent);
			if (isListener)
            {
                try
                {
                    transport.Close();
                }
                catch (Exception ex)
                {
                    ExceptionOccured(ex);
                }
            }
		}

        protected void IntOpenSSLSession(bool CallOpen)
		{
			if (CallOpen)
				Open();
			int startTick = System.Environment.TickCount;
			while((!opened) && (!closed) && (!errorOccured)) 
			{
				if (transport.Poll(1000000, SelectMode.SelectRead)) 
				{
					DataAvailable();				
				}
				if ((this.handshakeTimeout != 0) && (System.Environment.TickCount - startTick > handshakeTimeout)) 
				{
					Close(true);
				}
				System.Threading.Thread.Sleep(0);
			}
			if (!opened) 
			{
				throw new Exception("SSL handshake failure");
			}
		}

		public void OpenSSLSession()
		{
			IntOpenSSLSession(true);
		}

		protected override void Open()
		{
			SBSSLServer.Open();
		}

		protected override void DataAvailable()
		{
			SBSSLServer.DataAvailable();
		}

		protected override void SendData( byte[] buffer)
		{
			SBSSLServer.SendData(buffer);
		}

		protected class SSLSocketAsyncAcceptResult : SSLSocketAsyncResult
		{
			ElServerSSLSocket acceptedSocket;
			ElServerSSLSocket parent;

			public SSLSocketAsyncAcceptResult(ElServerSSLSocket parent, AsyncCallback callback, Object asyncState) :
				base(callback, asyncState)
			{
				this.parent = parent;
			}

			public ElServerSSLSocket Parent
			{
				get
				{
					return parent;
				}
			}

			public ElServerSSLSocket AcceptedSocket
			{
				get 
				{
					return acceptedSocket;
				}
				set
				{
					acceptedSocket = value;
				}
			}
		}
	}
}
