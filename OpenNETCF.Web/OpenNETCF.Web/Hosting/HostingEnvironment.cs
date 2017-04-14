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
using OpenNETCF.Web.Configuration;
using OpenNETCF.Web.Helpers;
using System.Globalization;
using System.Reflection;
using System.IO;

namespace OpenNETCF.Web.Hosting
{
    /// <summary>
    /// Provides application-management functions and application services to a managed application within its application domain. This class cannot be inherited.
    /// </summary>
    public sealed class HostingEnvironment
    {
        private static VirtualPathProvider _virtualPathProvider;

        internal static VirtualPathProvider VirtualPathProvider
        {
            get { return _virtualPathProvider; }
        }

        /// <summary>
        /// Registers a new VirtualPathProvider instance with the Padarn system.
        /// </summary>
        /// <param name="virtualPathProvider"></param>
        public static void RegisterVirtualPathProvider(VirtualPathProvider virtualPathProvider)
        {
            VirtualPathProvider previous = HostingEnvironment._virtualPathProvider;
            HostingEnvironment._virtualPathProvider = virtualPathProvider;
            virtualPathProvider.Initialize(previous);
        }

        /// <summary>
        /// Maps a virtual path to a physical path on the server.
        /// </summary>
        /// <param name="virtualPath">The virtual path (absolute or relative). </param>
        /// <returns>The physical path on the server specified by virtualPath.</returns>
        public static string MapPath(string virtualPath)
        {
            var separator = new string(System.IO.Path.DirectorySeparatorChar, 1);

            // Normalize the path
            string path = System.Text.RegularExpressions.Regex.Replace(virtualPath, @"(/+)", "/");

            // Get the index of the end of the last directory name
            string resourcePath;
            string resourceIdentifier;

            int endIndex = path.LastIndexOf("/");
            if (endIndex < 0)
            {
                resourcePath = "/";
                resourceIdentifier = path;
            }
            else
            {
                resourcePath = (endIndex == 0) ? string.Empty : path.Substring(virtualPath.StartsWith("/") ? 1 : 0, endIndex - 1);
                resourceIdentifier = path.Substring(endIndex + 1);
            }

            ServerConfig webServerConfig = ServerConfig.GetConfig();

            // make sure all slashes on all OSes use the proper directory separator
            string rootPath = webServerConfig.DocumentRoot.Replace('/', System.IO.Path.DirectorySeparatorChar).Replace('\\', System.IO.Path.DirectorySeparatorChar);
            rootPath = (rootPath.EndsWith(separator) ? rootPath : String.Format("{0}{1}", rootPath, separator));
            StringBuilder physicalPath = new StringBuilder(rootPath);

            string[] directories = resourcePath.Split('/');
            foreach (string directory in directories)
            {
                if (String.IsNullOrEmpty(directory))
                {
                    break;
                }

                if (UrlPath.IsVirtualDirectory(directory))
                {
                    physicalPath = new StringBuilder(String.Format("{0}{1}", ServerConfig.GetConfig().VirtualDirectories.GetVirtualDirectory(directory).PhysicalDirectory, System.IO.Path.DirectorySeparatorChar));
                }
                else
                {
                    physicalPath.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}", directory, System.IO.Path.DirectorySeparatorChar);
                }
            }

            physicalPath.Append(resourceIdentifier);

            return physicalPath.ToString().Replace('/', System.IO.Path.DirectorySeparatorChar).Replace('\\', System.IO.Path.DirectorySeparatorChar);
        }

        private static AssemblyName m_assemblyName; 

        /// <summary>
        /// Gets the physical path on disk to the application's directory.
        /// </summary>
        public static string ApplicationPhysicalPath
        {
            get
            {
                if(m_assemblyName == null)
                {
                    m_assemblyName = Assembly.GetExecutingAssembly().GetName();
                }

                return Path.GetDirectoryName(m_assemblyName.CodeBase);
            }
        }

        /// <summary>
        /// Gets the version of the Padarn server assembly
        /// </summary>
        public static Version ApplicationVersion
        {
            get
            {
                if (m_assemblyName == null)
                {
                    m_assemblyName = Assembly.GetExecutingAssembly().GetName();
                }

                return m_assemblyName.Version;
            }
        }
    }
}
