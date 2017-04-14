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
