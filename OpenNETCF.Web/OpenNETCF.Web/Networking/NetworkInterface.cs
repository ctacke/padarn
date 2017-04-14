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
using System.Collections;
using System.Text;
using Microsoft.Win32;
using System.Net;
using System.Runtime.InteropServices;
using System.Net.Sockets;

namespace OpenNETCF.Net.NetworkInformation
{

    /// <summary>
    /// Provides configuration and statistical information for a network interface.
    /// </summary>
    internal partial class NetworkInterface : INetworkInterface
    {
        private IP_ADAPTER_INFO m_adapterInfo;

        /// <summary>
        /// Creates a NetworkInterface instance
        /// </summary>
        /// <param name="index"></param>
        /// <param name="interfaceName"></param>
        internal NetworkInterface(int index, string interfaceName)
        {
            Index = index;
            Name = interfaceName;
            m_adapterInfo = null;
        }

        internal int Index { get; private set; }

        /// <summary>
        /// Gets the name of the network adapter.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a System.String that describes this interface.
        /// </summary>
        public string Description
        {
            get
            {
                // get the adapter info if it's not already been done
                if (m_adapterInfo == null)
                {
                    m_adapterInfo = GetAdapterInfo(Index);
                }

                return m_adapterInfo.Description;
            }
        }

        /// <summary>
        /// Gets the identifier of the network adapter.
        /// </summary>
        public string Id
        {
            get { return Index.ToString(); }
        }

        /// <summary>
        /// Returns the Media Access Control (MAC) address for this adapter.
        /// </summary>
        /// <returns>
        /// A System.Net.NetworkInformation.PhysicalAddress object that contains the
        /// physical address.
        /// </returns>
        public PhysicalAddress GetPhysicalAddress()
        {
            // get the adapter info if it's not already been done
            if (m_adapterInfo == null)
            {
                m_adapterInfo = GetAdapterInfo(Index);
            }
            return m_adapterInfo.PhysicalAddress;
        }

        /// <summary>
        /// The currently active IP address of the adapter.
        /// </summary>
        /// <remarks>After Setting this property, you must Rebind the adapter for it to take effect</remarks>
        public IPAddress CurrentIpAddress
        {
            get
            {
                // get the adapter info if it's not already been done
                if (m_adapterInfo == null)
                {
                    m_adapterInfo = GetAdapterInfo(Index);
                }
                return m_adapterInfo.CurrentIpAddress;
            }
        }

    }
}
