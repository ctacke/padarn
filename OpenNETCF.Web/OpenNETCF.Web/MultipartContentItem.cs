using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web
{
    internal sealed class MultipartContentItem
    {
        // Fields
        private string m_contentType;
        private HttpRawRequestContent m_data;
        private string m_filename;
        private int m_length;
        private string m_name;
        private int m_offset;

        // Methods
        internal MultipartContentItem(string name, string filename, string contentType, HttpRawRequestContent data, int offset, int length)
        {
            this.m_name = name;
            this.m_filename = filename;
            this.m_contentType = contentType;
            this.m_data = data;
            this.m_offset = offset;
            this.m_length = length;
        }

        internal HttpPostedFile GetAsPostedFile()
        {
            return new HttpPostedFile(this.m_filename, this.m_contentType, new HttpInputStream(this.m_data, this.m_offset, this.m_length));
        }

        internal string GetAsString(Encoding encoding)
        {
            if (this.m_length > 0)
            {
                byte[] data = this.m_data.GetAsByteArray(this.m_offset, this.m_length);
                return encoding.GetString(data, 0, data.Length);
            }
            return string.Empty;
        }

        // Properties
        internal bool IsFile
        {
            get
            {
                return (this.m_filename != null);
            }
        }

        internal bool IsFormItem
        {
            get
            {
                return (this.m_filename == null);
            }
        }

        internal string Name
        {
            get
            {
                return this.m_name;
            }
        }
    }
}
