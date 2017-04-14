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
using System.IO;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Provides access to individual files that have been uploaded by a client.
    /// </summary>
    public sealed class HttpPostedFile
    {
        // Fields
        private string m_contentType;
        private string m_filename;
        private HttpInputStream m_stream;

        // Methods
        internal HttpPostedFile(string filename, string contentType, HttpInputStream stream)
        {
            this.m_filename = filename;
            this.m_contentType = contentType;
            this.m_stream = stream;
        }

        /// <summary>
        /// Saves the contents of an uploaded file.
        /// </summary>
        /// <param name="filename">The name of the saved file. </param>
        public void SaveAs(string filename)
        {
            if (!Path.IsPathRooted(filename))
            {
                throw new HttpException("Not a rooted path: " + filename);
            }
            FileStream s = new FileStream(filename, FileMode.Create);
            try
            {
                this.m_stream.WriteTo(s);
                s.Flush();
            }
            finally
            {
                s.Close();
            }
        }

        /// <summary>
        /// Gets the size of an uploaded file, in bytes
        /// </summary>
        public int ContentLength
        {
            get
            {
                return (int)this.m_stream.Length;
            }
        }

        /// <summary>
        /// Gets the MIME content type of a file sent by a client.
        /// </summary>
        public string ContentType
        {
            get
            {
                return this.m_contentType;
            }
        }

        /// <summary>
        /// Gets the fully qualified name of the file on the client.
        /// </summary>
        public string FileName
        {
            get
            {
                return this.m_filename;
            }
        }

        /// <summary>
        /// Gets a Stream object that points to an uploaded file to prepare for reading the contents of the file.
        /// </summary>
        public Stream InputStream
        {
            get
            {
                return this.m_stream;
            }
        }
    }
}
