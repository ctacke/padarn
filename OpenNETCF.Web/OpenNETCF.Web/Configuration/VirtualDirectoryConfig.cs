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
