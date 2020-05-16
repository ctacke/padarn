using System.Collections;

namespace OpenNETCF.Web.Hosting
{
    /// <summary>
    /// Provides a set of methods that enable a Web application to retrieve resources from a virtual file system. 
    /// </summary>
    public abstract class VirtualPathProvider
    {
        private VirtualPathProvider _previous;

        protected internal VirtualPathProvider Previous
        {
            get { return this._previous; }
        }

        /// <summary>
        /// Initializes the VirtualPathProvider instance. 
        /// </summary>
        protected virtual void Initialize() { }

        internal virtual void Initialize(VirtualPathProvider previous)
        {
            this._previous = previous;
            this.Initialize();
        }

        /// <summary>
        /// Gets a value that indicates whether a directory exists in the virtual file system. 
        /// </summary>
        /// <param name="virtualDir">The path to the virtual directory.</param>
        /// <returns><b>true</b> if the directory exists in the virtual file system; otherwise, <b>false</b>.</returns>
        public virtual bool DirectoryExists(string virtualDir)
        {
            if (this._previous == null)
            {
                return false;
            }

            return this._previous.DirectoryExists(virtualDir);
        }

        /// <summary>
        /// Gets a value that indicates whether a file exists in the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns><b>true</b> if the file exists in the virtual file system; otherwise, <b>false</b>.</returns>
        public virtual bool FileExists(string virtualPath)
        {
            if (this._previous == null)
            {
                return false;
            }

            return this._previous.FileExists(virtualPath);
        }

        /// <summary>
        /// Gets a virtual directory from the virtual file system.
        /// </summary>
        /// <param name="virtualDir">The path to the virtual directory.</param>
        /// <returns>A descendent of the VirtualDirectory class that represents a directory in the virtual file system.</returns>
        public virtual VirtualDirectory GetDirectory(string virtualDir)
        {
            if (this._previous == null)
            {
                return null;
            }

            return this._previous.GetDirectory(virtualDir);
        }

        /// <summary>
        /// Gets a virtual file from the virtual file system. 
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>A descendent of the VirtualFile class that represents a file in the virtual file system.</returns>
        public virtual VirtualFile GetFile(string virtualPath)
        {
            if (this._previous == null)
            {
                return null;
            }

            return this._previous.GetFile(virtualPath);
        }

        /// <summary>
        /// Gets a hash for the specified virtual file.
        /// </summary>
        /// <param name="virtualPath">Path to the virtual file</param>
        /// <param name="virtualPathDependencies"></param>
        /// <returns></returns>
        public virtual string GetFileHash(string virtualPath, IEnumerable virtualPathDependencies)
        {
            if (this._previous == null)
            {
                return null;
            }

            return this._previous.GetFileHash(virtualPath, virtualPathDependencies);
        }
    }
}
