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
