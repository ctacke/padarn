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
