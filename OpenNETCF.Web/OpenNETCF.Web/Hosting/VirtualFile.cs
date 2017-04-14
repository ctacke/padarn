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
using System.IO;

namespace OpenNETCF.Web.Hosting
{
    /// <summary>
    /// Represents a file object in a virtual file or resource space.
    /// </summary>
    public abstract class VirtualFile : VirtualFileBase
    {
        protected VirtualFile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the VirtualFile class. 
        /// </summary>
        protected VirtualFile(string virtualPath)
        {
            _virtualPath = OpenNETCF.Web.VirtualPath.Create(virtualPath);
        }

        /// <summary>
        /// Gets a value that indicates that this is a virtual resource that should be treated as a file.
        /// </summary>
        /// <value>Always <b>false</b>.</value>
        public override bool IsDirectory
        {
            get { return false; }
        }

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource. 
        /// </summary>
        /// <returns>A read-only stream to the virtual file.</returns>
        /// <remarks>The Open method returns a stream containing the data treated as a file by the VirtualPathProvider class. The stream is read-only, and can be seekable.</remarks>
        public abstract Stream Open();
    }
}
