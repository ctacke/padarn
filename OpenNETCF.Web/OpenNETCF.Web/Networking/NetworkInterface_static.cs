using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Provides configuration and statistical information for a network interface.
    /// </summary>
    internal partial class NetworkInterface
    {
        /// <summary>
        /// Returns objects that describe the network interfaces on the local computer.
        /// </summary>
        /// <returns>
        /// A System.Net.NetworkInformation.NetworkInterface array that contains objects
        /// that describe the available network interfaces, or an empty array if no interfaces
        /// are detected.
        /// </returns>
        public unsafe static INetworkInterface[] GetAllNetworkInterfaces()
        {

            NetworkInterface[] interfaceList;
            uint size;

            // get buffer size requirement
            NativeMethods.GetInterfaceInfo(null, out size);

            byte[] ifTable = new byte[size];
            // pin the table buffer
            fixed (byte* pifTable = ifTable)
            {
                byte* p = pifTable;

                /* table looks like this:
                    typedef struct _IP_INTERFACE_INFO {
                      LONG NumAdapters; 
                      IP_ADAPTER_INDEX_MAP Adapter[1]; 
                    } IP_INTERFACE_INFO, *PIP_INTERFACE_INFO;
                
                    typedef struct _IP_ADAPTER_INDEX_MAP {
                      ULONG Index; 
                      WCHAR Name [MAX_ADAPTER_NAME]; 
                    } IP_ADAPTER_INDEX_MAP, *PIP_ADAPTER_INDEX_MAP;
                 */

                // get the table data
                NativeMethods.GetInterfaceInfo(pifTable, out size);

                // get interface count
                int interfaceCount = *p;

                interfaceList = new NetworkInterface[interfaceCount];

                p += 4;

                // get each interface
                for (int i = 0; i < interfaceCount; i++)
                {
                    // get interface index
                    int index = (int)*((int*)p);
                    p += 4;

                    // get interface name
                    byte[] nameBytes = new byte[256];
                    Marshal.Copy(new IntPtr(p), nameBytes, 0, nameBytes.Length);
                    string name = Encoding.Unicode.GetString(nameBytes, 0, nameBytes.Length);
                    int nullIndex = name.IndexOf('\0');
                    if (nullIndex > 0)
                    {
                        name = name.Substring(0, nullIndex);
                    }
                    p += 256;

                    interfaceList[i] = new NetworkInterface(index, name);
                }
            }

            return interfaceList;
        }

        internal unsafe static IP_ADAPTER_INFO GetAdapterInfo(int adapterIndex)
        {
            return GetAdapterInfo(ref adapterIndex, null);
        }

        internal unsafe static IP_ADAPTER_INFO GetAdapterInfo(ref int adapterIndex, string adapterName)
        {
            uint size = 0;

            // get buffer size requirement
            NativeMethods.GetAdaptersInfo(null, ref size);

            byte[] info = new byte[size];

            // get the data
            int errorCode = NativeMethods.GetAdaptersInfo(info, ref size);
            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new Exception(errorCode.ToString());
            }

            // walk the list looking fo the requested index
            fixed (byte* pInfo = info)
            {
                // get the index for this adapter
                byte* p = pInfo;
                uint pNext = 0;

                do
                {
                    // get the pointer to the next adapter
                    // C# is screwy - we have to cast the pointer type before getting the data
                    pNext = (uint)*((uint*)p);
                    byte* pThis = p;

                    p += 8;
                    string name = Marshal2.PtrToStringAnsi((IntPtr)p, 256);

                    p += 404;
                    int index = (int)*((int*)p);

                    if (index == adapterIndex)
                    {
                        return new IP_ADAPTER_INFO(pThis);
                    }
                    else if ((adapterName != null) && (name == adapterName))
                    {
                        // the index has changed (Rebind may do this) so update the index
                        adapterIndex = index;
                        return new IP_ADAPTER_INFO(pThis);
                    }
                    else if (adapterIndex == -1)
                    {
                        // looking for localhost
                    }

                    uint offset = pNext - (uint)p;
                    p += offset;
                } while (pNext != 0);

                // if we get here, the index is not found
                return null;

                /*

                #define MAX_ADAPTER_DESCRIPTION_LENGTH  128 // arb.
                #define MAX_ADAPTER_NAME_LENGTH         256 // arb.
                #define MAX_ADAPTER_ADDRESS_LENGTH      8   // arb.

                    typedef struct _IP_ADAPTER_INFO {
                000      struct _IP_ADAPTER_INFO* Next;
                004      DWORD ComboIndex;
                008      Char AdapterName[MAX_ADAPTER_NAME_LENGTH + 4];
                268      char Description[MAX_ADAPTER_DESCRIPTION_LENGTH + 4];
                400      UINT AddressLength;
                404      BYTE Address[MAX_ADAPTER_ADDRESS_LENGTH];
                412      DWORD Index;
                416      UINT Type;
                418      UINT DhcpEnabled;
                      PIP_ADDR_STRING CurrentIpAddress;
                      IP_ADDR_STRING IpAddressList;
                      IP_ADDR_STRING GatewayList;
                      IP_ADDR_STRING DhcpServer;
                      BOOL HaveWins;
                      IP_ADDR_STRING PrimaryWinsServer;
                      IP_ADDR_STRING SecondaryWinsServer;
                      time_t LeaseObtained;
                      time_t LeaseExpires;
                    } IP_ADAPTER_INFO, *PIP_ADAPTER_INFO;
                */
            }
        }

        //internal static MibIfRow GetMibIfRow(int adapterIndex)
        //{
        //    MibIfRow row = new MibIfRow();
        //    row.Index = adapterIndex;
        //    int errorCode = NativeMethods.GetIfEntry(row);

        //    if (errorCode != NativeMethods.NO_ERROR)
        //    {
        //        throw new NetworkInformationException(errorCode);
        //    }

        //    return row;
        //}
    }

    internal class Marshal2
    {
        public static string PtrToStringAnsi(IntPtr ptr, int len)
        {
            int cb = len;
            byte[] data = new byte[cb];
            Marshal.Copy(new IntPtr(ptr.ToInt32()), data, 0, cb);

            string s = Encoding.ASCII.GetString(data, 0, len);

            int nullpos = s.IndexOf('\0');
            if (nullpos > -1)
                s = s.Substring(0, nullpos);

            return s;
        }
    }
}
