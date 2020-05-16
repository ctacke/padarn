﻿using System.IO;

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
