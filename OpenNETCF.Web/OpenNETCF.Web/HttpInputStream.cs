using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OpenNETCF.Web
{
    internal class HttpInputStream : Stream
    {
        // Fields
        private HttpRawRequestContent m_data;
        private int m_length;
        private int m_offset;
        private int m_pos;

        // Methods
        internal HttpInputStream(HttpRawRequestContent data, int offset, int length)
        {
            this.Init(data, offset, length);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    this.Uninit();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public override void Flush()
        {
        }

        internal byte[] GetAsByteArray()
        {
            if (this.m_length == 0)
            {
                return null;
            }
            return this.m_data.GetAsByteArray(this.m_offset, this.m_length);
        }

        protected void Init(HttpRawRequestContent data, int offset, int length)
        {
            this.m_data = data;
            this.m_offset = offset;
            this.m_length = length;
            this.m_pos = 0;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int length = this.m_length - this.m_pos;
            if (count < length)
            {
                length = count;
            }
            if (length > 0)
            {
                this.m_data.CopyBytes(this.m_offset + this.m_pos, buffer, offset, length);
            }
            this.m_pos += length;
            return length;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            int num = this.m_pos;
            int num2 = (int)offset;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    num = num2;
                    break;

                case SeekOrigin.Current:
                    num = this.m_pos + num2;
                    break;

                case SeekOrigin.End:
                    num = this.m_length + num2;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("origin");
            }
            if ((num < 0) || (num > this.m_length))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            this.m_pos = num;
            return (long)this.m_pos;
        }

        public override void SetLength(long length)
        {
            throw new NotSupportedException();
        }

        protected void Uninit()
        {
            this.m_data = null;
            this.m_offset = 0;
            this.m_length = 0;
            this.m_pos = 0;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        internal void WriteTo(Stream s)
        {
            if ((this.m_data != null) && (this.m_length > 0))
            {
                this.m_data.WriteBytes(this.m_offset, this.m_length, s);
            }
        }

        // Properties
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override long Length
        {
            get
            {
                return (long)this.m_length;
            }
        }

        public override long Position
        {
            get
            {
                return (long)this.m_pos;
            }
            set
            {
                this.Seek(value, SeekOrigin.Begin);
            }
        }
    }
}
