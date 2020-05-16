using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Represents a virtual directory setup on the web server
    /// </summary>
    internal class VirtualDirectory
    {
        private string m_alias;
        private string m_physicalPath;
        private bool m_authenticationEnabled;

        /// <summary>
        /// Determins if authentication is enabled for the virtual directory.  
        /// </summary>
        public bool AuthenticationEnabled
        {
            get { return m_authenticationEnabled; }
            internal set { m_authenticationEnabled = value; }
        }
	
        /// <summary>
        /// Gets the physical path of the virtual directory
        /// </summary>
        public string PhysicalPath
        {
            get { return m_physicalPath; }
            internal set { m_physicalPath = value; }
        }
	
        /// <summary>
        /// Gets the alias that was given to the virtual directory
        /// </summary>
        public string Alias
        {
            get { return m_alias; }
            internal set { m_alias = value; }
        }

        /// <summary>
        /// Creates an isntance of a VirtualDirectory with no internal values set
        /// </summary>
        public VirtualDirectory()
        {
        }

        /// <summary>
        /// Creates an instance of a VirtualDirectory
        /// </summary>
        public VirtualDirectory(string alias, string physicalPath, bool authenticationEnabled)
        {
            m_alias = alias;
            m_physicalPath = physicalPath;
            m_authenticationEnabled = authenticationEnabled;
        }
    }
}
