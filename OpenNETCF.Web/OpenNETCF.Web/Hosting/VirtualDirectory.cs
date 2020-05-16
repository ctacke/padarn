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
