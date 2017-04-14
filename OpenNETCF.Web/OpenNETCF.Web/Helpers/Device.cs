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
//                                                                   
// Copyright (c) 2007-2009 OpenNETCF Consulting, LLC                        
//                        
#if WINDOWSCE                                             
namespace OpenNETCF.WindowsCE
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Describes the type of platform the device is using
    /// </summary>
    public enum PlatformType
    {
        /// <summary>
        /// Windows CE
        /// </summary>
        WindowsCE,
        /// <summary>
        /// Windows Mobile
        /// </summary>
        WindowsMobile,
    }

    /// <summary>
    /// Describes the edition of the Windows Mobile the device is running
    /// </summary>
    public enum WindowsMobileEdition
    {
        /// <summary>
        /// Windows Mobile Classic (Smartphone)
        /// </summary>
        Classic,
        /// <summary>
        /// Windows Mobile Standard (Pocket PC)
        /// </summary>
        Standard,
        /// <summary>
        /// Windows Mobile Professional (Pocket PC Phone Edition)
        /// </summary>
        Professional,
    }

    /// <summary>
    /// Provides platform information about the device the application is running on. 
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Determines if the device is a generic Windows CE device.
        /// </summary>
        public static bool IsGenericWindowsCE
        {
            get { return (GetPlatformType() == PlatformType.WindowsCE); }
        }

        /// <summary>
        /// Determines if the device is a Windows Mobile device.
        /// </summary>
        public static bool IsWindowsMobile
        {
            get { return (GetPlatformType() == PlatformType.WindowsMobile); }
        }

        /// <summary>
        /// Gets the <see cref="PlatformType"/> of the device.
        /// </summary>
        public static PlatformType GetPlatformType()
        {
            string platform = NativeMethods.GetSystemParameter(NativeMethods.SPI_GETPLATFORMTYPE);
            switch (platform)
            {
                case "SmartPhone": return PlatformType.WindowsMobile;
                case "PocketPC": return PlatformType.WindowsMobile;
                default: return PlatformType.WindowsCE;
            }
        }

        /// <summary>
        /// Gets the <see cref="WindowsMobileEdition"/> of the device.
        /// </summary>
        public static WindowsMobileEdition GetWindowsMobileEdition()
        {
            string platform = NativeMethods.GetSystemParameter(NativeMethods.SPI_GETPLATFORMTYPE);
            switch (platform)
            {
                case "SmartPhone":
                    return WindowsMobileEdition.Classic;
                case "PocketPC":
                    if (!HasPhone())
                        return WindowsMobileEdition.Standard;
                    else
                        return WindowsMobileEdition.Professional;
                default:
                    return WindowsMobileEdition.Classic;
            }
        }

        /// <summary>
        /// Determines whether the device has a phone stack 
        /// </summary>
        /// <returns>True: is the device has a phone stack, otherwise False</returns>
        public static bool HasPhone()
        {
            return (File.Exists("\\Windows\\Phone.dll"));
        }

        internal class NativeMethods
        {
            internal const uint SPI_GETPLATFORMTYPE = 257;
            internal const uint SPI_GETOEMINFO = 258;

            [DllImport("coredll.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
            public static extern int SystemParametersInfo(uint uiAction, uint uiParam, StringBuilder pvParam, uint fWiniIni);

            public static string GetSystemParameter(uint uiParam)
            {
                StringBuilder sb = new StringBuilder(128);
                if (SystemParametersInfo(uiParam, (uint)sb.Capacity, sb, 0) == 0)
                    throw new ApplicationException("Failed to get system parameter");
                return sb.ToString();
            }

            public static string GetOEMInfo()
            {
                return GetSystemParameter(SPI_GETOEMINFO);
            }
        }
    }
}
#endif