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
