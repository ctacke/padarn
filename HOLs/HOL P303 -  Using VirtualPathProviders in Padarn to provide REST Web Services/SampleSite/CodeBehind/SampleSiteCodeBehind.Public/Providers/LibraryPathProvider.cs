using System;
using OpenNETCF.Web;
using OpenNETCF.Web.Hosting;

namespace SampleSite.Providers
{
    public class LibraryPathProvider : VirtualPathProvider
    {
        private LibraryVirtualFile m_file;
		private Library m_library;
        private bool isInitialized;

        /// <summary>
        /// Determines whether a specified virtual path is within the virtual file system.
        /// </summary>
        /// <param name="virtualPath">An absolute virtual path.</param>
        /// <returns>True if the virtual path is within the virtual file sytem; otherwise, false.</returns>
        private static bool IsPathVirtual(string virtualPath)
        {
            bool result = false;

            // If the virtualPath is one of the known virtual paths, return true.
            if (virtualPath.StartsWith("/library/show", StringComparison.InvariantCultureIgnoreCase) || 
				virtualPath.StartsWith("/library/destroy", StringComparison.InvariantCultureIgnoreCase))
            {
				// Sample only supports display and deletion of library contents (books)
				// Further enhancemnets would include modification and creation of books
                result = true;
			}

			return result;
		}

		public override bool FileExists(string virtualPath)
		{
			bool result = false;

			// If the path is virtual, see if the file exists.
			if (IsPathVirtual(virtualPath))
			{
				// Create the file and return the value of the Exists property.
				m_file = (LibraryVirtualFile)GetFile(virtualPath);
				result = m_file.Exists;
			}
			else
			{
				m_file = null;
			}

			return result;
		}

		public override VirtualFile GetFile(string virtualPath)
		{
			if (!isInitialized)
			{
                // Initialize the library, if not yet created
				m_library = LibraryManager.GetLibrary(null);
                isInitialized = true;
            }

            VirtualFile file;

            // If the path is virtual, get the file from the virtual file sytem.
            if (IsPathVirtual(virtualPath))
            {
                // If the file has already been created, return the existing instance.
                if (m_file != null && m_file.VirtualPath.Equals(virtualPath, StringComparison.InvariantCultureIgnoreCase))
                {
                    file = m_file;
                }
                else
                {
                    // If the file has not been created, instantiate it.
                    file = new LibraryVirtualFile(virtualPath);
                }
            }
            else
            {
                // If the file is not virtual, use the default path provider.
                file = Previous.GetFile(virtualPath);
            }

            return file;
        }
    }
}
