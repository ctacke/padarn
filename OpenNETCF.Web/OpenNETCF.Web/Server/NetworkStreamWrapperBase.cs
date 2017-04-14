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

namespace OpenNETCF.Web.Server
{
    abstract class NetworkStreamWrapperBase
    {
        protected NetworkStreamWrapperBase()
        {
        }

        // Summary:
        //     Creates a new instance of the System.Net.Sockets.NetworkStream class for
        //     the specified System.Net.Sockets.Socket.
        //
        // Parameters:
        //   socket:
        //     The System.Net.Sockets.Socket that the System.Net.Sockets.NetworkStream will
        //     use to send and receive data.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     socket is null.
        //
        //   System.IO.IOException:
        //     socket is not connected.-or- The System.Net.Sockets.Socket.SocketType property
        //     of socket is not System.Net.Sockets.SocketType.Stream.-or- socket is in a
        //     nonblocking state.
        public NetworkStreamWrapperBase(SocketWrapperBase socket)
        {
        }
        //
        // Summary:
        //     Initializes a new instance of the System.Net.Sockets.NetworkStream class
        //     for the specified System.Net.Sockets.Socket with the specified System.Net.Sockets.Socket
        //     ownership.
        //
        // Parameters:
        //   socket:
        //     The System.Net.Sockets.Socket that the System.Net.Sockets.NetworkStream will
        //     use to send and receive data.
        //
        //   ownsSocket:
        //     true to indicate that the System.Net.Sockets.NetworkStream will take ownership
        //     of the System.Net.Sockets.Socket; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     socket is null.
        //
        //   System.IO.IOException:
        //     socket is not connected.-or- The value of the System.Net.Sockets.Socket.SocketType
        //     property of socket is not System.Net.Sockets.SocketType.Stream.-or- socket
        //     is in a nonblocking state.
        public NetworkStreamWrapperBase(SocketWrapperBase socket, bool ownsSocket)
        {
        }
        //
        // Summary:
        //     Creates a new instance of the System.Net.Sockets.NetworkStream class for
        //     the specified System.Net.Sockets.Socket with the specified access rights.
        //
        // Parameters:
        //   socket:
        //     The System.Net.Sockets.Socket that the System.Net.Sockets.NetworkStream will
        //     use to send and receive data.
        //
        //   access:
        //     A bitwise combination of the System.IO.FileAccess values that specify the
        //     type of access given to the System.Net.Sockets.NetworkStream over the provided
        //     System.Net.Sockets.Socket.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     socket is null.
        //
        //   System.IO.IOException:
        //     socket is not connected.-or- The System.Net.Sockets.Socket.SocketType property
        //     of socket is not System.Net.Sockets.SocketType.Stream.-or- socket is in a
        //     nonblocking state.
        public NetworkStreamWrapperBase(SocketWrapperBase socket, FileAccess access)
        {
        }
        //
        // Summary:
        //     Creates a new instance of the System.Net.Sockets.NetworkStream class for
        //     the specified System.Net.Sockets.Socket with the specified access rights
        //     and the specified System.Net.Sockets.Socket ownership.
        //
        // Parameters:
        //   socket:
        //     The System.Net.Sockets.Socket that the System.Net.Sockets.NetworkStream will
        //     use to send and receive data.
        //
        //   access:
        //     A bitwise combination of the System.IO.FileAccess values that specifies the
        //     type of access given to the System.Net.Sockets.NetworkStream over the provided
        //     System.Net.Sockets.Socket.
        //
        //   ownsSocket:
        //     true to indicate that the System.Net.Sockets.NetworkStream will take ownership
        //     of the System.Net.Sockets.Socket; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     socket is null.
        //
        //   System.IO.IOException:
        //     socket is not connected.-or- The System.Net.Sockets.Socket.SocketType property
        //     of socket is not System.Net.Sockets.SocketType.Stream.-or- socket is in a
        //     nonblocking state.
        public NetworkStreamWrapperBase(SocketWrapperBase socket, FileAccess access, bool ownsSocket)
        {
        }

        // Summary:
        //     Gets a value that indicates whether the System.Net.Sockets.NetworkStream
        //     supports reading.
        //
        // Returns:
        //     true if data can be read from the stream; otherwise, false. The default value
        //     is true.
        public abstract bool CanRead { get; }
        //
        // Summary:
        //     Gets a value that indicates whether the stream supports seeking. This property
        //     is not currently supported.This property always returns false.
        //
        // Returns:
        //     false in all cases to indicate that System.Net.Sockets.NetworkStream cannot
        //     seek a specific location in the stream.
        public abstract bool CanSeek { get; }
        //
        // Summary:
        //     Gets a value that indicates whether the System.Net.Sockets.NetworkStream
        //     supports writing.
        //
        // Returns:
        //     true if data can be written to the System.Net.Sockets.NetworkStream; otherwise,
        //     false. The default value is true.
        public abstract bool CanWrite { get; }
        //
        // Summary:
        //     Gets a value that indicates whether data is available on the System.Net.Sockets.NetworkStream
        //     to be read.
        //
        // Returns:
        //     true if data is available on the stream to be read; otherwise, false.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Net.Sockets.NetworkStream is closed.
        //
        //   System.IO.IOException:
        //     The underlying System.Net.Sockets.Socket is closed.
        //
        //   System.Net.Sockets.SocketException:
        //     Use the System.Net.Sockets.SocketException.ErrorCode property to obtain the
        //     specific error code, and refer to the Windows�Sockets version 2 API error
        //     code documentation in MSDN for a detailed description of the error.
        public abstract bool DataAvailable { get; }
        //
        // Summary:
        //     Gets the length of the data available on the stream. This property is not
        //     currently supported and always throws a System.NotSupportedException.
        //
        // Returns:
        //     The length of the data available on the stream.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     Any use of this property.
        public abstract long Length { get; }
        //
        // Summary:
        //     Gets or sets the current position in the stream. This property is not currently
        //     supported and always throws a System.NotSupportedException.
        //
        // Returns:
        //     The current position in the stream.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     Any use of this property.
        public abstract long Position { get; set; }

        // Summary:
        //     Begins an asynchronous read from the System.Net.Sockets.NetworkStream.
        //
        // Parameters:
        //   buffer:
        //     An array of type System.Byte that is the location in memory to store data
        //     read from the System.Net.Sockets.NetworkStream.
        //
        //   offset:
        //     The location in buffer to begin storing the data.
        //
        //   size:
        //     The number of bytes to read from the System.Net.Sockets.NetworkStream.
        //
        //   callback:
        //     The System.AsyncCallback delegate that is executed when System.Net.Sockets.NetworkStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)
        //     completes.
        //
        //   state:
        //     An object that contains any additional user-defined data.
        //
        // Returns:
        //     An System.IAsyncResult that represents the asynchronous call.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     buffer is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     offset is less than 0.-or- offset is greater than the length of buffer.-or-
        //     size is less than 0.-or- size is greater than the length of buffer minus
        //     the value of the offset parameter.
        //
        //   System.IO.IOException:
        //     The underlying System.Net.Sockets.Socket is closed.-or- There was a failure
        //     while reading from the network. -or-An error occurred when accessing the
        //     socket. See the Remarks section for more information.
        //
        //   System.ObjectDisposedException:
        //     The System.Net.Sockets.NetworkStream is closed.
        public abstract IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state);
        //
        // Summary:
        //     Begins an asynchronous write to a stream.
        //
        // Parameters:
        //   buffer:
        //     An array of type System.Byte that contains the data to write to the System.Net.Sockets.NetworkStream.
        //
        //   offset:
        //     The location in buffer to begin sending the data.
        //
        //   size:
        //     The number of bytes to write to the System.Net.Sockets.NetworkStream.
        //
        //   callback:
        //     The System.AsyncCallback delegate that is executed when System.Net.Sockets.NetworkStream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)
        //     completes.
        //
        //   state:
        //     An object that contains any additional user-defined data.
        //
        // Returns:
        //     An System.IAsyncResult that represents the asynchronous call.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     buffer is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     offset is less than 0.-or- offset is greater than the length of buffer.-or-
        //     size is less than 0.-or- size is greater than the length of buffer minus
        //     the value of the offset parameter.
        //
        //   System.IO.IOException:
        //     The underlying System.Net.Sockets.Socket is closed.-or- There was a failure
        //     while writing to the network. -or-An error occurred when accessing the socket.
        //     See the Remarks section for more information.
        //
        //   System.ObjectDisposedException:
        //     The System.Net.Sockets.NetworkStream is closed.
        public abstract IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state);
        //
        // Summary:
        //     Releases the unmanaged resources used by the System.Net.Sockets.NetworkStream
        //     and optionally releases the managed resources.
        //
        // Parameters:
        //   disposing:
        //     true to release both managed and unmanaged resources; false to release only
        //     unmanaged resources.
        protected abstract void Dispose(bool disposing);
        //
        // Summary:
        //     Handles the end of an asynchronous read.
        //
        // Parameters:
        //   asyncResult:
        //     An System.IAsyncResult that represents an asynchronous call.
        //
        // Returns:
        //     The number of bytes read from the System.Net.Sockets.NetworkStream.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     asyncResult is null.
        //
        //   System.IO.IOException:
        //     The underlying System.Net.Sockets.Socket is closed.-or- An error occurred
        //     when accessing the socket. See the Remarks section for more information.
        //
        //   System.ObjectDisposedException:
        //     The System.Net.Sockets.NetworkStream is closed.
        public abstract int EndRead(IAsyncResult asyncResult);
        //
        // Summary:
        //     Handles the end of an asynchronous write.
        //
        // Parameters:
        //   asyncResult:
        //     The System.IAsyncResult that represents the asynchronous call.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     asyncResult is null.
        //
        //   System.IO.IOException:
        //     The underlying System.Net.Sockets.Socket is closed.-or- An error occurred
        //     while writing to the network. -or-An error occurred when accessing the socket.
        //     See the Remarks section for more information.
        //
        //   System.ObjectDisposedException:
        //     The System.Net.Sockets.NetworkStream is closed.
        public abstract void EndWrite(IAsyncResult asyncResult);
        //
        // Summary:
        //     Flushes data from the stream. This method is reserved for future use.
        public abstract void Flush();
        //
        // Summary:
        //     Reads data from the System.Net.Sockets.NetworkStream.
        //
        // Parameters:
        //   buffer:
        //     An array of type System.Byte that is the location in memory to store data
        //     read from the System.Net.Sockets.NetworkStream.
        //
        //   offset:
        //     The location in buffer to begin storing the data to.
        //
        //   size:
        //     The number of bytes to read from the System.Net.Sockets.NetworkStream.
        //
        // Returns:
        //     The number of bytes read from the System.Net.Sockets.NetworkStream.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     buffer is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     offset is less than 0.-or- offset is greater than the length of buffer.-or-
        //     size is less than 0.-or- size is greater than the length of buffer minus
        //     the value of the offset parameter. -or-An error occurred when accessing the
        //     socket. See the Remarks section for more information.
        //
        //   System.IO.IOException:
        //     The underlying System.Net.Sockets.Socket is closed.
        //
        //   System.ObjectDisposedException:
        //     The System.Net.Sockets.NetworkStream is closed.-or- There is a failure reading
        //     from the network.
        public abstract int Read(byte[] buffer, int offset, int size);
        //
        // Summary:
        //     Sets the current position of the stream to the given value. This method is
        //     not currently supported and always throws a System.NotSupportedException.
        //
        // Parameters:
        //   offset:
        //     This parameter is not used.
        //
        //   origin:
        //     This parameter is not used.
        //
        // Returns:
        //     The position in the stream.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     Any use of this property.
        public abstract long Seek(long offset, SeekOrigin origin);
        //
        // Summary:
        //     Sets the length of the stream. This method always throws a System.NotSupportedException.
        //
        // Parameters:
        //   value:
        //     This parameter is not used.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     Any use of this property.
        public abstract void SetLength(long value);
        //
        // Summary:
        //     Writes data to the System.Net.Sockets.NetworkStream.
        //
        // Parameters:
        //   buffer:
        //     An array of type System.Byte that contains the data to write to the System.Net.Sockets.NetworkStream.
        //
        //   offset:
        //     The location in buffer from which to start writing data.
        //
        //   size:
        //     The number of bytes to write to the System.Net.Sockets.NetworkStream.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     buffer is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     offset is less than 0.-or- offset is greater than the length of buffer.-or-
        //     size is less than 0.-or- size is greater than the length of buffer minus
        //     the value of the offset parameter.
        //
        //   System.IO.IOException:
        //     There was a failure while writing to the network. -or-An error occurred when
        //     accessing the socket. See the Remarks section for more information.
        //
        //   System.ObjectDisposedException:
        //     The System.Net.Sockets.NetworkStream is closed.-or- There was a failure reading
        //     from the network.
        public abstract void Write(byte[] buffer, int offset, int size);

        public static implicit operator Stream(NetworkStreamWrapperBase stm)
        {
            if (stm == null) return null;
            return stm.GetStream();
        }

        protected abstract Stream GetStream();
    }
}
