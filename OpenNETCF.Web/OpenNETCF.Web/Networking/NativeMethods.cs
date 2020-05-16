using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Net.Sockets;

namespace OpenNETCF.Net.NetworkInformation
{
    internal static class NativeMethods
    {
        internal const int NO_ERROR = 0;

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetInterfaceInfo(byte* pIfTable, out uint dwOutBufLen);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public static extern int GetAdaptersInfo(byte[] pAdapterInfo, ref uint pOutBufLen);

    }
}
