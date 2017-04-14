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
