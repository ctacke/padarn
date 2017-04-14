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
using System.Threading;
using System.Collections;
using System.ComponentModel;
using SBSSLCommon;
using SBSSLConstants;
using SBUtils;
#if SECURE_BLACKBOX_DEBUG
using Microsoft.Win32;
using System.IO;
#endif

namespace SecureBlackbox.SSLSocket
{

#if SECURE_BLACKBOX_DEBUG
    public static class LowLevelDumpLock
    {
        public static void Acquire()
        {
            Monitor.Enter(m_Lock);
        }

        public static void Release()
        {
            Monitor.Exit(m_Lock);
        }

        private static object m_Lock = new object();
    }
#endif

	/// <summary>
	/// Summary description for SSLSocket.
    /// </summary>
#if (!WindowsCE)
    [ToolboxItem(false)]
#endif
    public abstract class ElSSLSocket : Component
	{
		//private static int MAX_RECEIVED_DATALEN	= 1024 * 500;

		protected Socket transport;
		private SocketFlags transportFlags = 0;

		private byte[] inBuffer = new byte[16384];
		private int inBufferIndex = 0;
		private bool inBufferDataPresent;
		private bool transportBeginReceiveCalled = false;

		private byte[] receivedDataBuf;

		private AsyncCallback socketSendCallback;
		private AsyncCallback socketReceiveCallback;

		private SSLSocketAsyncResult beginConnectAsyncResult;
		private SSLSocketAsyncSendResult beginSendAsyncResult;
		private SSLSocketAsyncReceiveResult beginReceiveAsyncResult;
        private SSLSocketAsyncResult beginOpenSSLSessionAsyncResult;

		protected bool opened = false;	//set to true when callback SecureConnectionOpened() called
		protected bool closed = false;  //set to true when callback SecureConnectionClosed() called

		protected bool fDisposed = false;

		protected bool fBlocking = true;
        protected bool fIgnoreClose = false;
        protected bool fSilentClose = false;

		protected Exception innerException;

		protected int handshakeTimeout = 0;
	
		public ElSSLSocket()
		{
			Init();
		}

#if SECURE_BLACKBOX_DEBUG

        private string m_LowLevelDumpFilename = "";
        private string m_LowLevelDumpObjID = "";

        protected void LoadLowLevelDumpConfiguration()
        {
            try
            {
                RegistryKey hkcu = Registry.CurrentUser;
                RegistryKey propkey = hkcu.OpenSubKey("Software\\EldoS\\SSLSocket");
                object o = propkey.GetValue("LowLevelDumpFilename");
                if ((o != null) && (o is string))
                {
                    m_LowLevelDumpFilename = (string)o;
                }
                else
                {
                    m_LowLevelDumpFilename = "";
                }
                propkey.Close();
                byte[] buf = new byte[4];
                SBRandom.__Global.SBRndGenerate(buf, 0, 4);
                m_LowLevelDumpObjID = SBUtils.Unit.BinaryToString(buf);
            }
            catch (Exception)
            {
            }
        }

        protected void LowLevelDump(string s)
        {
            try
            {
                if (m_LowLevelDumpFilename == "")
                {
                    return;
                }
                string logline = "[" + DateTime.Now.ToLongTimeString() + "] (" + m_LowLevelDumpObjID + ") " + s + "\r\n";
                byte[] logbuf = SBUtils.Unit.BytesOfString(logline);
                LowLevelDumpLock.Acquire();
                try
                {
                    FileStream f = new FileStream(m_LowLevelDumpFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    try
                    {
                        f.Position = f.Length;
                        f.Write(logbuf, 0, logbuf.Length);
                    }
                    finally
                    {
                        f.Close();
                    }
                }
                finally
                {
                    LowLevelDumpLock.Release();
                }
            }
            catch (Exception)
            {
            }
        }

        protected void LowLevelDumpBinary(string s, byte[] buf, int offset, int len)
        {
            string fmt = SBUtils.Unit.BinaryToString(SBUtils.Unit.CloneBuffer(buf, offset, len));
            fmt = SBUtils.Unit.BeautifyBinaryString(fmt, ' ');
            LowLevelDump(s + fmt);
        }
#endif

		protected virtual void Init()
		{
#if SECURE_BLACKBOX_DEBUG
            LoadLowLevelDumpConfiguration();
            LowLevelDump("ElSSLSocket.Init() {");
#endif
            transport = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			Reset();
			socketSendCallback = new AsyncCallback(OnSocketSendCallback);
			socketReceiveCallback = new AsyncCallback(OnSocketReceiveCallback);
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.Init() }");
#endif
        }

		virtual protected void Reset()
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.Reset() {");
#endif
            lock (this)
			{
				innerException = null;
				opened = false;
				closed = false;
				inBufferDataPresent = false;
				inBufferIndex = 0;
				transportBeginReceiveCalled = false;

				beginConnectAsyncResult = null;
				beginSendAsyncResult = null;
				beginReceiveAsyncResult = null;
			}
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.Reset() }");
#endif
        }

		public new bool Equals(object o)
		{
			if (this == o)
				return true;
			else
				return false;
		}

		public static new bool Equals(object objA, object objB)
		{
			if (objA is ElSSLSocket && objA == objB)
				return true;
			else
				return false;
		}

		public abstract void Close(bool Silent);

		protected bool IsExceptionOccured()
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.IsExceptionOccured() {}, returning " + (innerException != null ? true : false).ToString());
#endif
            lock (this)
			{
				return innerException != null ? true : false;
			}		
		}

		protected void ExceptionOccured(Exception ex)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.ExceptionOccured(" + ex.Message + ") {}");
#endif
            lock (this)
			{
				if (innerException == null)
					innerException = ex;
			}
		}

		protected void SetConnectAsyncResult(SSLSocketAsyncResult ar)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.SetConnectAsyncResult() {}");
#endif
            lock (this)
			{
				beginConnectAsyncResult = ar;
			}
		}

		protected void CompleteConnectAsyncResult()
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.CompleteConnectAsyncResult() {}");
#endif
            lock (this)
			{
				if (beginConnectAsyncResult != null)
				{
					beginConnectAsyncResult.Completed(innerException);
					beginConnectAsyncResult = null;
					innerException = null;
				}
			}
		}

		protected void SetSendAsyncResult(SSLSocketAsyncSendResult ar)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.SetSendAsyncResult() {}");
#endif
            lock (this)
			{
				beginSendAsyncResult = ar;
			}
		}

		protected void CompleteSendAsyncResult()
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.CompleteSendAsyncResult() {}");
#endif
            lock (this)
			{
                SSLSocketAsyncResult res = beginSendAsyncResult;
				if (res != null)
				{
                    beginSendAsyncResult = null;
					res.Completed(innerException);
					res = null;
					innerException = null;
				}
                else if (beginConnectAsyncResult != null)
                {
                    res = beginConnectAsyncResult;
                    beginConnectAsyncResult = null;
                    res.Completed(innerException);
                    res = null;
                    innerException = null;
                }
                else if (beginOpenSSLSessionAsyncResult != null)
                {
                    res = beginOpenSSLSessionAsyncResult;
                    beginOpenSSLSessionAsyncResult = null;
                    res.Completed(innerException);
                    res = null;
                    innerException = null;
                }
            }
		}

		protected void SetReceiveAsyncResult(SSLSocketAsyncReceiveResult ar)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.SetReceiveAsyncResult() {}");
#endif
            lock (this)
			{
				beginReceiveAsyncResult = ar;
			}
		}

		protected void CompleteReceiveAsyncResult()
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.CompleteReceiveAsyncResult() {}");
#endif
            lock (this)
			{
                SSLSocketAsyncResult res = beginReceiveAsyncResult;
				if (res != null)
				{
                    beginReceiveAsyncResult = null;
					res.Completed(innerException);
					res = null;
					innerException = null;
                }
                else if (beginConnectAsyncResult != null)
                {
                    res = beginConnectAsyncResult;
                    beginConnectAsyncResult = null;
                    res.Completed(innerException);
                    res = null;
                    innerException = null;
                }
                else if (beginOpenSSLSessionAsyncResult != null)
                {
                    res = beginOpenSSLSessionAsyncResult;
                    beginOpenSSLSessionAsyncResult = null;
                    res.Completed(innerException);
                    res = null;
                    innerException = null;
                }
			}
		}

        protected void SetOpenSSLSessionAsyncResult(SSLSocketAsyncResult ar)
        {
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.SetOpenSSLSessionAsyncResult() {}");
#endif
            lock (this)
            {
                beginOpenSSLSessionAsyncResult = ar;
            }
        }

        protected void CompleteOpenSSLSessionAsyncResult()
        {
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.CompleteOpenSSLSessionAsyncResult() {}");
#endif
            lock (this)
            {
                if (beginOpenSSLSessionAsyncResult != null)
                {
                    beginOpenSSLSessionAsyncResult.Completed(innerException);
                    beginOpenSSLSessionAsyncResult = null;
                    innerException = null;
                }
            }
        }

		#region EventHandlers

		protected void OnSecureOpenConnection()
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.OnSecureOpenConnection() {}");
#endif
            opened = true;
            if (!transport.Blocking)
            {
                if (beginOpenSSLSessionAsyncResult != null)
                {
                    CompleteOpenSSLSessionAsyncResult();
                }
                else
                {
                    CompleteConnectAsyncResult();
                }
            }
		}

		protected void OnSecureCloseConnection()
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.OnSecureCloseConnection() {}");
#endif
            if (fIgnoreClose == false)
            {
                receivedDataBuf = null;
                closed = true;
                try
                {
                    if (!fSilentClose)
                        Shutdown(SocketShutdown.Both);
                    transport.Close();
                }
                catch (Exception ex)
                {
                    ExceptionOccured(ex);
                }

                if (!opened)
                {
                    if (beginOpenSSLSessionAsyncResult != null)
                    {
                        CompleteOpenSSLSessionAsyncResult();
                    }
                    else
                    {
                        CompleteConnectAsyncResult();
                    }
                }
                else
                {
                    Exception ex = innerException;
                    CompleteSendAsyncResult();
                    innerException = ex;
                    CompleteReceiveAsyncResult();
                }
            }
		}

		protected void OnSecureOnData(byte[] buffer)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.OnSecureOnData(" + buffer.Length.ToString() + " bytes) {}");
#endif
            if (!transport.Blocking && beginReceiveAsyncResult != null 
					&& receivedDataBuf != null)
			{
				//in the next function beginReceiveAsyncResult can be set to null
				//but user can call BeginReceive() again and 
				//beginReceiveAsyncResult initialized again
				CopyDataFromReceivedDataBuffer();
			}
			
			try
			{
				if (receivedDataBuf == null)
				{
					receivedDataBuf = new byte[buffer.Length];
					Buffer.BlockCopy(buffer, 0, receivedDataBuf, 0, buffer.Length);
				}
				else
				{
					byte[] newbuf = new byte[buffer.Length + receivedDataBuf.Length];
					Buffer.BlockCopy(receivedDataBuf, 0, newbuf, 0, receivedDataBuf.Length);
					Buffer.BlockCopy(buffer, 0, newbuf, receivedDataBuf.Length, buffer.Length);
					receivedDataBuf = newbuf;
				}
			}
			catch(Exception ex)
			{
				ExceptionOccured(ex);
			}
			
			if (!transport.Blocking && beginReceiveAsyncResult != null)
			{
				CopyDataFromReceivedDataBuffer();
			}
		}

		protected void OnSecureSend(byte[] buffer)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.OnSecureSend(" + buffer.Length.ToString() + " bytes) {");
            LowLevelDumpBinary("Buffer: ", buffer, 0, buffer.Length);
#endif
            if (transport.Blocking)
			{
				int offset = 0;
				int size = buffer.Length;
				while (size > 0) 
				{
					//transport.Send(buffer);
					int sent = transport.Send(buffer, offset, size, SocketFlags.None);
                    offset += sent;
					size -= sent;
				}
			}
			else
			{
				try
				{
					byte[] buf = new byte[buffer.Length];
					Buffer.BlockCopy(buffer, 0, buf, 0, buffer.Length);
					if (beginSendAsyncResult != null) 
					{
						beginSendAsyncResult.AddBufferForSend(buf);
						transport.BeginSend(buffer, 0, buffer.Length, transportFlags,
							socketSendCallback, 
							new SSLSocketBeginSendStateObject(beginSendAsyncResult, buf));
					}
					else
					{
						transport.BeginSend(buffer, 0, buffer.Length,
							transportFlags,	socketSendCallback, 
							new SSLSocketBeginSendStateObject(null, buf));
					}
				}
				catch(Exception ex)
				{
					ExceptionOccured(ex);
					CompleteSendAsyncResult();
				}
			}
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.OnSecureSend() }");
#endif
		}

		protected void OnSecureReceive(byte[] buffer, 
			int maxSize, out int written)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.OnSecureReceive(" + maxSize.ToString() + " bytes requested) {");
#endif
            written = 0;

			if (transport.Blocking)
			{
                written = transport.Receive(buffer, 0, maxSize, transportFlags);
                if (written == 0) // remote closed the connection
                {
                    try
                    {
                        transport.Shutdown(SocketShutdown.Both);
                        transport.Close();
                    }
                    catch (Exception)
                    {
                    }
                    closed = true;
                }
			}
			else
			{
				written = Math.Min(maxSize, inBufferIndex);
				if (written > 0) 
				{
                    Array.Copy(inBuffer, 0, buffer, 0, written);
					Array.Copy(inBuffer, written, inBuffer, 0, inBufferIndex - written);
					inBufferIndex -= written;
				}
				/*
				int count = 0;
				try
				{
					if (inBufferDataPresent)
					{
						inBufferDataPresent = false;
						buffer[0] = inBuffer[0];
						written = 1;
						maxSize--;
					}

					if (maxSize > 0)
					{
						count = Math.Min(transport.Available, maxSize);
						if (count > 0)
							written += transport.Receive(buffer, written, count, transportFlags);
					}
					if (receivedDataBuf == null || receivedDataBuf.Length < MAX_RECEIVED_DATALEN)
						BeginReceive();
				}
				catch(Exception ex)
				{
					//System.Console.WriteLine(ex.StackTrace);
					ExceptionOccured(ex);
				}*/
			}
#if SECURE_BLACKBOX_DEBUG
            LowLevelDumpBinary("Result: ", buffer, 0, written);
            LowLevelDump("ElSSLSocket.OnSecureReceive(" + written.ToString() + " bytes written) {");
#endif
        }

		#endregion

		public void Bind(EndPoint localEP)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.Bind() {}");
#endif
            if (transport == null)
				throw new NullReferenceException("Set \"Socket\" property first");
			transport.Bind(localEP);
		}

		public void CloseSocket()
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.CloseSocket() {}");
#endif
            transport.Close();
		}

		public bool Poll(int microSeconds, System.Net.Sockets.SelectMode mode)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.Poll() {}");
#endif
            if (receivedDataBuf != null)
			{
				return true;
			}
			else
				return transport.Poll(microSeconds, mode);
		}

		public void Shutdown(System.Net.Sockets.SocketShutdown how)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.Shutdown() {}");
#endif
            try 
			{
				transport.Shutdown(how);
			} 
			catch 
			{
				;
			}
		}


		protected void InnerConnect(EndPoint remoteEP)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.InnerConnect() {}");
#endif
            if (transport == null)
				throw new NullReferenceException("Set \"Socket\" property first");
			Reset();
			transport.Blocking = true;
			transport.Connect(remoteEP);
			Open();
			while ((!opened) && (!closed) && (!SSLErrorOccured()))
				DataAvailable();
		}

		protected IAsyncResult InnerBeginConnect(EndPoint remoteEP, 
			AsyncCallback callback, object state)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.InnerBeginConnect() {}");
#endif
            if (transport == null)
				throw new NullReferenceException("Set \"Socket\" property first");
			if (beginConnectAsyncResult != null)
				throw new InvalidOperationException("Always in the \"connection\" state.");
			Reset();
			transport.Blocking = false;
			SetConnectAsyncResult(new SSLSocketAsyncResult(callback, state));
			transport.BeginConnect(remoteEP, 
				new AsyncCallback(OnSocketConnectCallback), null);
			return beginConnectAsyncResult;
		}

		protected virtual void InnerEndConnect(IAsyncResult asyncResult)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.InnerEndConnect() {}");
#endif
            SSLSocketAsyncResult ar = (SSLSocketAsyncResult)asyncResult;
			ar.AsyncWaitHandle.WaitOne();
			if (ar.Exception != null)
				throw ar.Exception;
		}

		public int Send(byte[] buffer)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.Send() {}");
#endif
            if (!opened)
				throw new InvalidOperationException("Connection not opened");
			if (closed)
				throw new InvalidOperationException("Connection already closed");
			transport.Blocking = true;
			SendData(buffer);
			return buffer.Length;
		}

		public int Send(byte[] buffer, SocketFlags socketFlags)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.Send(" + buffer.Length.ToString() + " bytes) {}");
#endif
            return Send(buffer);
		}
		
		public int Send(byte[] buffer, int offset, int size)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.Send(" + size.ToString() + " bytes) {}");
#endif
            byte[] buf = new byte[size];
			Array.Copy(buffer, offset, buf, 0, size);
			return Send(buf);
		}

		public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.Send(" + size.ToString() + " bytes) {}");
#endif
            return Send(buffer, offset, size);
		}

		public IAsyncResult BeginSend(
			byte[] buffer,
			int offset,
			int size,
			AsyncCallback callback,
			object state
			)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.BeginSend(" + size.ToString() + " bytes) {}");
#endif
            if (!opened)
				throw new InvalidOperationException("Connection not opened");
			if (closed)
				throw new InvalidOperationException("Connection already closed");

			transport.Blocking = false;
			if (offset > 0 || size != buffer.Length)
			{
				byte[] buf = new byte[size];
				Buffer.BlockCopy(buffer, offset, buf, 0, size);
				buffer = buf;
				offset = 0;
			}
			SSLSocketAsyncSendResult ret = new SSLSocketAsyncSendResult(size, callback, state);
			SetSendAsyncResult(ret);
			SendData(buffer);
			CompleteSendAsyncResult();
			return ret;
		}

		public int EndSend(IAsyncResult asyncResult)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.EndSend() {}");
#endif
            SSLSocketAsyncSendResult asr = (SSLSocketAsyncSendResult)asyncResult;
			asr.AsyncWaitHandle.WaitOne();
			if (asr.Exception != null)
				throw asr.Exception;
            return asr.SendDataLen;
		}

		public int Receive(byte[] buffer)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.Receive() {}");
#endif
            if (!opened)
                throw new InvalidOperationException("Connection not opened");
			if (closed)
				throw new InvalidOperationException("Connection already closed");
			if (buffer.Length == 0)
				return 0;
			transport.Blocking = true;
			while ((receivedDataBuf == null ) && (!closed) && (!SSLErrorOccured()))
				DataAvailable();
			if (receivedDataBuf != null)
			{
				int len = Math.Min(buffer.Length, receivedDataBuf.Length);
				Buffer.BlockCopy(receivedDataBuf, 0, buffer, 0, len);
				int rest = receivedDataBuf.Length - len;
				if (rest > 0)
				{
					byte[] newbuf = new byte[rest];
					Buffer.BlockCopy(receivedDataBuf, len, newbuf, 0, rest);
					receivedDataBuf = newbuf;
				}
				else
					receivedDataBuf = null;
				return len;
			}
			return 0;
		}

		public int Receive(byte[] buffer, System.Net.Sockets.SocketFlags socketFlags)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.Receive() {}");
#endif
            return Receive(buffer);
		}

		public int Receive(byte[] buffer, int size, System.Net.Sockets.SocketFlags socketFlags)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.Receive() {}");
#endif
            byte[] buf = new byte[size];
			int result = Receive(buf);
			Array.Copy(buf, 0, buffer, 0, result);
			return result;
		}

		public int Receive(byte[] buffer, int offset, int size, System.Net.Sockets.SocketFlags socketFlags)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.Receive() {}");
#endif
            byte[] buf = new byte[size];
			int result = Receive(buf);
			Array.Copy(buf, 0, buffer, offset, result);
			return result;
		}

		public IAsyncResult BeginReceive(
			byte[] buffer,
			int offset,
			int size,
			AsyncCallback callback,
			object state
			)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.BeginReceive() {}");
#endif
            if (!opened)
				throw new InvalidOperationException("Connection not opened.");
			if (closed)
				throw new InvalidOperationException("Connection already closed.");
			if (beginReceiveAsyncResult != null)
				throw new InvalidOperationException("BeginReceive() cannot be called until the previous call is finished.");

			transport.Blocking = false;

			SetReceiveAsyncResult(new SSLSocketAsyncReceiveResult(buffer,
				offset, size, callback, state));
			
			if (receivedDataBuf != null)
			{
				IAsyncResult ret = beginReceiveAsyncResult;
				CopyDataFromReceivedDataBuffer();
				return ret;
			}
			else
			{
                // II 20100925: the following line has been commented out because of the following:
                // When the component works in asynchronous mode, it always runs its receive loop. It always calls
                // transport's BeginReceive() inside the OnSocketReceiveCallback handler, right after all the 
                // data received on the previous step has been passed to the component. That's why there is no
                // need in pushing the transport in response to user's BeginReceive() call.
				//BeginReceive(); 
				return beginReceiveAsyncResult;
			}
		}

		public int EndReceive(IAsyncResult asyncResult)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.EndReceive() {}");
#endif
            SSLSocketAsyncReceiveResult asr = (SSLSocketAsyncReceiveResult)asyncResult;
			asr.AsyncWaitHandle.WaitOne();
			beginReceiveAsyncResult = null;
			if (asr.Exception != null)
				throw asr.Exception;
			return asr.ReceivedDataLen;
		}

        public IAsyncResult BeginOpenSSLSession(AsyncCallback callback, object state)
        {
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.BeginOpenSSLSession() {}");
#endif
            if (transport == null)
                throw new NullReferenceException("Set \"Socket\" property first");
            if (!transport.Connected)
                throw new InvalidOperationException("Transport socket is not connected");
            if ((beginOpenSSLSessionAsyncResult != null) || (beginConnectAsyncResult != null) ||
                (beginSendAsyncResult != null) || (beginReceiveAsyncResult != null))
                throw new InvalidOperationException("Asynchronous operation pending");
            Reset();
            transport.Blocking = false;
            SetOpenSSLSessionAsyncResult(new SSLSocketAsyncResult(callback, state));
            Open();
            BeginReceive();
            return beginOpenSSLSessionAsyncResult;
        }

        public void EndOpenSSLSession(IAsyncResult asyncResult)
        {
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.EndOpenSSLSession() {}");
#endif
            SSLSocketAsyncResult ar = (SSLSocketAsyncResult)asyncResult;
            ar.AsyncWaitHandle.WaitOne();
            if (ar.Exception != null)
                throw ar.Exception;
        }

		private void OnSocketConnectCallback(IAsyncResult asyncResult)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.OnSocketConnectCallback() {}");
#endif
            try
			{
				transport.EndConnect(asyncResult);
			}
			catch(Exception ex)
			{
				ExceptionOccured(ex);
				CompleteConnectAsyncResult();
				return;
			}

			Open();
			BeginReceive();		
		}

		private void OnSocketReceiveCallback(IAsyncResult asyncResult)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.OnSocketReceiveCallback() {}");
#endif
            int i = 0;
			try
			{
				transportBeginReceiveCalled = false;
				i = transport.EndReceive(asyncResult);
#if SECURE_BLACKBOX_DEBUG
                LowLevelDump("ElSSLSocket.OnSocketReceiveCallback(): " + i.ToString() + " bytes received from socket");
#endif
            }
			catch(Exception ex)
			{
				ExceptionOccured(ex);
				CompleteReceiveAsyncResult();
				return;
			}
				
			if (i > 0)
			{
				inBufferIndex += i;
				inBufferDataPresent = true;
				while ((inBufferIndex > 0) && (!SSLErrorOccured()))
				{
                    int prevInBufferIndex = inBufferIndex;
					DataAvailable();
                    if (inBufferIndex == prevInBufferIndex)
                    {
                        break; // antifreeze
                    }
				}
			}
			if ((transport != null) && (transport.Connected)) 
			{
				BeginReceive();
			}
		}

		private void OnSocketSendCallback(IAsyncResult asyncResult)
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.OnSocketSendCallback() {}");
#endif
            SSLSocketBeginSendStateObject state = (SSLSocketBeginSendStateObject)asyncResult.AsyncState;
			SSLSocketAsyncSendResult asr = (SSLSocketAsyncSendResult)state.AsyncSendResult;
			try
			{
				int sent = transport.EndSend(asyncResult);
#if SECURE_BLACKBOX_DEBUG
                LowLevelDump("ElSSLSocket.OnSocketSendCallback(): " + sent.ToString() + " bytes sent to socket");
#endif
                byte[] oldbuf = state.Buffer;
				if (sent < oldbuf.Length)
				{
					byte[] newbuf = new byte[oldbuf.Length - sent];
					Buffer.BlockCopy(oldbuf, sent, newbuf, 0, newbuf.Length);
					if (asr != null)
						asr.ReplaceBufferForSend(oldbuf, newbuf);
					transport.BeginSend(newbuf, 0, newbuf.Length, transportFlags, 
						socketSendCallback, 
						new SSLSocketBeginSendStateObject(asr, newbuf));
				}
				else
				{
					if (asr != null)
						asr.RemoveBufferForSend(oldbuf);
				}
			}
			catch(Exception ex)
			{
				ExceptionOccured(ex);
				CompleteSendAsyncResult();
			}
		}

		protected abstract void Open();

		protected abstract void DataAvailable();

		protected abstract void SendData(byte[] buffer);

		private void CopyDataFromReceivedDataBuffer()
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.CopyDataFromReceivedDataBuffer() {}");
#endif
            if (innerException == null)
			{
				try
				{
					int len = beginReceiveAsyncResult.CopyBuffer(receivedDataBuf);
					if (len > 0)
					{
						if (receivedDataBuf.Length == len)
							receivedDataBuf = null;
						else
						{
							int newBufLen = receivedDataBuf.Length - len;
							byte[] newbuf = new byte[newBufLen];
							Buffer.BlockCopy(receivedDataBuf, len, newbuf, 0, newBufLen);
							receivedDataBuf = newbuf;
						}
					}
				}
				catch(Exception ex)
				{
					innerException = ex;
				}	
			}
			CompleteReceiveAsyncResult();
		}

		protected void BeginReceive()
		{
#if SECURE_BLACKBOX_DEBUG
            LowLevelDump("ElSSLSocket.BeginReceive() {}");
#endif
            if (!transportBeginReceiveCalled)
			{
				transport.BeginReceive(inBuffer, inBufferIndex, inBuffer.Length - inBufferIndex, 
					transportFlags, socketReceiveCallback, null);
				transportBeginReceiveCalled = true;
			}
		}

        protected virtual bool SSLErrorOccured()
        {
            return false;
        }

		public abstract bool get_CipherSuites(short Index);
		
		public abstract void set_CipherSuites(short Index, bool Value);

        public abstract int GetCipherSuitePriority(short Index);

        public abstract void SetCipherSuitePriority(short Index, int Value);

        public virtual long TotalBytesSent
        {
            get { return 0; }
            set { } 
        }

        public virtual long TotalBytesReceived
        {
            get { return 0; }
            set { }
        }

        public virtual short Options
        {
            get { return 0; }
            set { }
        }
		
		public virtual short CipherSuite
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}
		
		public virtual short CurrentVersion
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		
		public virtual short Versions
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public abstract bool get_CompressionAlgorithms(short Index);
		
		public abstract void set_CompressionAlgorithms(short Index, bool Value);
		
		public virtual short CompressionAlgorithm
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public virtual bool SSLEnabled
		{
			get
			{
				return true;
			}
			set
			{
			}
		}
		
		public bool Connected 
		{
			get 
			{
				return (transport.Connected && opened && !closed);
			}
		}

		public int Available 
		{
			get 
			{
				if (receivedDataBuf != null)
				{
					if (receivedDataBuf.Length != 0) 
						return receivedDataBuf.Length;
				}
				if (transport.Available > 0) 
					return 1; // with SSL we don't know the actual size of the data
				else
					return 0;
			}
		}

		public bool Blocking
		{
			get
			{
				if (transport != null)
					return transport.Blocking;
				else
				{
					return fBlocking;
				}
			}
			set
			{
				if (transport != null)
				{
					transport.Blocking = value;
				}
				fBlocking = value;				
			}
		}

		public Socket Socket
		{
			get
			{
				return transport;
			}

			set
			{
				this.transport = value;
				value.Blocking = fBlocking;
			}
		}

		public SocketFlags SocketFlags
		{
			get
			{
				return transportFlags;
			}
			set
			{
				transportFlags = value;
			}
		}


		public EndPoint RemoteEndPoint {
			get 
			{
				return transport.RemoteEndPoint;
			}
		}

		public EndPoint LocalEndPoint 
		{
			get 
			{
				return transport.LocalEndPoint;
			}
		}

		public int HandshakeTimeout 
		{
			get 
			{
				return this.handshakeTimeout;
			}
			set 
			{
				this.handshakeTimeout = value;
			}
		}

		
		protected class SSLSocketAsyncResult : IAsyncResult
		{
			private ManualResetEvent mrEvent = new ManualResetEvent(false);
			private Object asyncState;
			private bool completed = false;
			private AsyncCallback callback;
			protected Exception ex;

			public SSLSocketAsyncResult(AsyncCallback callback, Object asyncState)
			{
				this.asyncState = asyncState;
				this.callback = callback;
			}
	
			public Exception Exception
			{
				get
				{
					return ex;
				}
			}

			virtual public void Completed(Exception ex)
			{
				lock(this)
				{
					if (this.ex == null)
						this.ex = ex;
					if (!completed)
					{
						completed = true;
						mrEvent.Set();
						if (callback != null)
							callback(this);			
					}
				}
			}
				
			public AsyncCallback Callback
			{
				get
				{
					return callback;
				}
			}

			public object AsyncState 
			{
				get
				{
					return asyncState;
				}
			}

			public WaitHandle AsyncWaitHandle 
			{
				get
				{
					return mrEvent;
				}
			}

			public bool CompletedSynchronously 
			{
				get
				{
					return false;
				}
			}

			public bool IsCompleted 
			{
				get
				{
					return completed;
				}
			}
		}

		protected class SSLSocketAsyncSendResult : SSLSocketAsyncResult
		{
			private ArrayList buffersForSend = new ArrayList();
			private int sendDataLen;
			private bool completedCalled = false;

			public SSLSocketAsyncSendResult(int sendDataLen, AsyncCallback callback, Object asyncState) :
				base(callback, asyncState)
			{
				this.sendDataLen = sendDataLen;
			}

			override public void Completed(Exception ex)
			{
				lock(this)
				{
					completedCalled = true;
					if (this.ex == null)
						this.ex = ex;
					if (buffersForSend.Count == 0 || this.ex != null)
						base.Completed(ex);
				}
			}
			
			public void AddBufferForSend(byte[] buffer)
			{
				lock(this)
				{
					buffersForSend.Add(buffer);
				}
			}

			public void ReplaceBufferForSend(byte[] oldbuf, byte[] newbuf)
			{
				lock(this)
				{
					if (buffersForSend[0] != oldbuf)
						throw new InvalidOperationException();
					buffersForSend.RemoveAt(0);
					buffersForSend.Insert(0, newbuf);
				}
			}

			public void RemoveBufferForSend(byte[] buffer)
			{
				lock(this)
				{
					int count = buffersForSend.Count;
					for (int i = 0; i < count; i++)
					{
						if (buffersForSend[i] == buffer)
						{
							buffersForSend.RemoveAt(i);
							if (completedCalled)
								Completed(ex);
							return;
						}
					}
					throw new InvalidOperationException();
				}
			}

			/*public int BufferForSendCount
			{
				get
				{
					return buffersForSend.Count;
				}
			}*/

			public int SendDataLen
			{
				get
				{
					return this.sendDataLen;
				}
			}
		}

		protected class SSLSocketAsyncReceiveResult : SSLSocketAsyncResult
		{
			private byte[] buffer;
			private int offset;
			private int size;
			private int initialSize;

			public SSLSocketAsyncReceiveResult(
				byte[] buffer,
                int offset,
				int size,
				AsyncCallback callback, 
				Object asyncState) :
				base(callback, asyncState)
			{
				this.buffer = buffer;
				this.offset = offset;
				this.size = size;
				initialSize = size;
			}

			public int BufferSize
			{
				get
				{
					return size;
				}
			}

			public int CopyBuffer(byte[] inbuf)
			{
				int ret = Math.Min(size, inbuf.Length);
				Buffer.BlockCopy(inbuf, 0, buffer, offset, ret);
				offset += ret;
				size -= ret;
				return ret;
			}

			public int ReceivedDataLen
			{
				get
				{
					return initialSize - size;
				}
			}
		}

		protected class SSLSocketBeginSendStateObject
		{
			private SSLSocketAsyncSendResult asr;
			private byte[] buffer;

			public SSLSocketBeginSendStateObject(SSLSocketAsyncSendResult asr, byte[] buffer)
			{
				this.asr = asr;
				this.buffer = buffer;
			}

			public SSLSocketAsyncSendResult AsyncSendResult
			{
				get
				{
					return this.asr;
				}
			}

			public byte[] Buffer
			{
				get
				{
					return this.buffer;
				}
			}

		}
	}
}
