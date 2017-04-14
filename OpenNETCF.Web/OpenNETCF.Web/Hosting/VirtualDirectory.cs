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
using System.Collections;

namespace OpenNETCF.Web.Hosting
{
    /// <summary>
    /// Represents a directory object in a virtual file or resource space.
    /// </summary>
    public abstract class VirtualDirectory : VirtualFileBase
    {
        /// <summary>
        /// Initializes a new instance of the VirtualDirectory class. 
        /// </summary>
        protected VirtualDirectory(string virtualPath)
        {
            _virtualPath = OpenNETCF.Web.VirtualPath.CreateTrailingSlash(virtualPath);
        }

        /// <summary>
        /// Gets a value that indicates that this is a virtual resource that should be treated as a directory.
        /// </summary>
        /// <value>Always <b>true</b>.</value>
        public override bool IsDirectory
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a list of all files contained in this directory.
        /// </summary>
        /// <value>An object implementing the IEnumerable interface containing VirtualFile objects.</value>
        /// <remarks>The Files property contains VirtualFile objects that represent the files contained in this virtual directory. 
        /// To return the VirtualDirectory objects, use the Directories property. 
        /// To return both VirtualFile and VirtualDirectory objects, use the Children property.</remarks>
        public abstract IEnumerable Files { get; }

        /// <summary>
        /// Gets a list of all the subdirectories contained in this directory. 
        /// </summary>
        public abstract IEnumerable Directories { get; }

        /// <summary>
        /// Gets a list of the files and subdirectories contained in this virtual directory.
        /// </summary>
        public abstract IEnumerable Children { get; }

    }
}
