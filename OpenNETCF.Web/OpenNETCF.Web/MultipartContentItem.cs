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
