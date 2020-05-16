﻿using System;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Provides utility methods for common virtual path operations. 
    /// </summary>
    public static class VirtualPathUtility
    {
        // Required for ReST ------------------------------------------------

        /// <summary>
        /// Returns the directory portion of a virtual path
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>The directory referenced in the virtual path.</returns>
        public static string GetDirectory(string virtualPath)
        {
            VirtualPath parent = VirtualPath.CreateNonRelative(virtualPath).Parent;
            if (parent == null)
            {
                return null;
            }
            return parent.VirtualPathStringWhicheverAvailable;
        }

        /// <summary>
        /// Retrieves the extension of the file that is referenced in the virtual path. 
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>The file name extension string literal, including the period (.), nullNothingnullptra null reference (Nothing in Visual Basic), or an empty string ("").</returns>
        public static string GetExtension(string virtualPath)
        {
            return VirtualPath.Create(virtualPath).Extension;
        }

        /// <summary>
        /// Retrieves the file name of the file that is referenced in the virtual path.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>The file name literal after the last directory character in virtualPath; otherwise, an empty string (""), if the last character of virtualPath is a directory or volume separator character.</returns>
        public static string GetFileName(string virtualPath)
        {
            return VirtualPath.CreateNonRelative(virtualPath).FileName;
        }

        public static string ToAppRelative(string virtualPath)
        {
            return VirtualPath.CreateNonRelative(virtualPath).AppRelativeVirtualPathString;
        }

        /// <summary>
        /// Combines a base path and a relative path.
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string Combine(string basePath, string relativePath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a virtual path to an application absolute path using the specified application path.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="applicationPath"></param>
        /// <returns></returns>
        public static string ToAbsolute(string virtualPath, string applicationPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the relative virtual path from one virtual path containing the root operator (the tilde [~]) to another.
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        /// <returns></returns>
        public static string MakeRelative(string fromPath, string toPath)
        {
            throw new NotImplementedException();
        }

    }
}
