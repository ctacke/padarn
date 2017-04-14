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
using System.Collections.Specialized;
using System.Net;
using OpenNETCF.Web.Configuration;

namespace OpenNETCF.Web.Logging
{
    /// <summary>
    /// Interface for Padarn Log Providers
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// The Padarn configuration state at the time the Provider was created
        /// </summary>
        /// <remarks>
        /// Padarn injects the configuration when the provider is created at startup
        /// </remarks>
        ServerConfig ServerConfiguration { get; set; }
        /// <summary>
        /// Called on page access
        /// </summary>
        /// <param name="dataItem"></param>
        void LogPageAccess(LogDataItem dataItem);
        /// <summary>
        /// Called on error conditions
        /// </summary>
        /// <param name="errorInfo"></param>
        /// <param name="dataItem">Log data for the current error, if it exists</param>
        void LogPadarnError(string errorInfo, LogDataItem dataItem);
        /// <summary>
        /// Called during certain runtime operations.  Primarily used for debugging Padarn's internal systems.
        /// </summary>
        /// <param name="info"></param>
        void LogRuntimeInfo(ZoneFlags zoneMask, string info);
    }
}
