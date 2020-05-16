namespace OpenNETCF.Web.Hosting
{
    /// <summary>
    /// Provides the core implementation for the VirtualFile and VirtualDirectory objects. An abstract class, it cannot be instantiated. 
    /// </summary>
    public abstract class VirtualFileBase
    {
        internal VirtualPath _virtualPath;

        /// <summary>
        /// Initializes the class for use by an inherited class instance. This constructor can be called only by an inherited class. 
        /// </summary>
        protected VirtualFileBase()
        {
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the VirtualFileBase instance represents a virtual file or a virtual directory.
        /// </summary>
        public abstract bool IsDirectory { get; }
        

        /// <summary>
        /// Gets the display name of the virtual resource. 
        /// </summary>
        public virtual string Name
        {
            get { return this._virtualPath.FileName; }
        }

        /// <summary>
        /// Gets the virtual file path. 
        /// </summary>
        public string VirtualPath
        {
            get { return this._virtualPath.VirtualPathString; }
        }
    }
}
