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
//-----------------------------------------------------------------------
// <copyright company="OpenNETCF" file="VirtualDirectoryConfig.cs">
// Copyright (c) 2008 OpenNETCF Consulting, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace OpenNETCF.Web.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// Mappings for Padarn virtual directories
    /// </summary>
    public sealed class VirtualDirectoryMapping
    {
        private readonly string physicalDirectory;
        private readonly string virtualDirectory;
        private bool requiresAuth;

        /// <summary>
        /// Physical directory path
        /// </summary>
        public string PhysicalDirectory
        {
            get { return this.physicalDirectory; }
        }

        /// <summary>
        /// Virtual directory name
        /// </summary>
        public string VirtualDirectory
        {
            get { return this.virtualDirectory; }
        }

        /// <summary>
        /// True if the virtual directory requires authentication, otherwise false
        /// </summary>
        public bool RequiresAuthentication
        {
            get { return this.requiresAuth; }
            internal set { this.requiresAuth = value; }
        }

        /// <summary>
        /// VirtualDirectoryMapping constructor
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="phyiscalPath"></param>
        public VirtualDirectoryMapping(string virtualPath, string phyiscalPath)
        {
            this.virtualDirectory = virtualPath;
            this.physicalDirectory = phyiscalPath;

            this.requiresAuth = false;
        }
    }

    internal class VirtualDirectoryMappingCollection : List<VirtualDirectoryMapping>
    {
        public VirtualDirectoryMapping GetVirtualDirectory(string virtualPath)
        {
            return Find(mapping => string.Compare(mapping.VirtualDirectory, virtualPath, true) == 0);
        }

        public VirtualDirectoryMapping this[string virtualPath]
        {
            get { return Find(mapping => string.Compare(mapping.VirtualDirectory, virtualPath, true) == 0); }
        }

        public string FindPhysicalDirectoryForVirtualUrlPath(string url)
        {
            foreach (VirtualDirectoryMapping mapping in this)
            {
                if (url.IndexOf(mapping.VirtualDirectory, StringComparison.InvariantCultureIgnoreCase) > 0)
                {
                    return mapping.PhysicalDirectory;
                }
            }

            return null;
        }
    }
}
