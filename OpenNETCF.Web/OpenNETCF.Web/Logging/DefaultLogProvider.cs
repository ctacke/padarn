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
using System.IO;
using System.Diagnostics;
using OpenNETCF.Web.Configuration;

namespace OpenNETCF.Web.Logging
{
    [Flags]
    public enum ZoneFlags : uint
    {
        None = 0,
        SSL = 1,
        RequestListener = 2,
        WorkerRequest = 4,
        Startup = 8,
        All = 0xffffffff
    }

    /// <summary>
    /// The default log provider for Padarn.  This provider is used for logging if a specialized logging provider is not specified
    /// </summary>
    public class DefaultLogProvider : ILogProvider
    {
        private static DateTime m_lastLogDay = DateTime.MinValue;
        private static string m_currentFileName;

        /// <summary>
        /// The Padarn configuration state at the time the Provider was created
        /// </summary>
        /// <remarks>
        /// Padarn injects the configuration when the provider is created at startup
        /// </remarks>
        public ServerConfig ServerConfiguration { get; set; }

        /// <summary>
        /// Called whenever padarn get a request to access a page
        /// </summary>
        /// <param name="dataItem">Data related to the access call</param>
        public virtual void LogPageAccess(LogDataItem dataItem)
        {
            string page = dataItem.PageName;

            // do nothing if logging is not enabled
            if (!dataItem.ServerConfiguration.LoggingEnabled) return;

            // see if extension filtering is on, if so see if we should get filtered
            if (dataItem.ServerConfiguration.LogExtensions.Count > 0)
            {
                string ext = Path.GetExtension(page);
                if (!dataItem.ServerConfiguration.LogExtensions.Contains(ext))
                {
                    return;
                }
            }

            try
            {
                DateTime now = DateTime.Now;

                // if we're not the first run, we need to do some checking
                if (m_lastLogDay != DateTime.MinValue)
                {
                    // check to see if we're in a new day
                    if ((now.Day != m_lastLogDay.Day) || ((TimeSpan)(now - m_lastLogDay)).Days > 0)
                    {
                        // new day, new file
                        m_currentFileName = string.Format("LOG_{0}-{1}-{2}.txt", now.Year, now.Month, now.Day);
                    }
                }
                else
                {
                    m_currentFileName = string.Format("LOG_{0}-{1}-{2}.txt", now.Year, now.Month, now.Day);
                }

                m_currentFileName = Path.Combine(dataItem.ServerConfiguration.LogFileFolder, m_currentFileName);

                // // create the log file if it doesn't exist
                if (!File.Exists(m_currentFileName))
                {
                    using (StreamWriter writer = File.CreateText(m_currentFileName))
                    {
                        // do nothing - just create the file
                    }
                }

                page = page.Replace(dataItem.ServerConfiguration.DocumentRoot, "");
                string ip = dataItem.RemoteClientIP.ToString();

                using (StreamWriter writer = File.AppendText(m_currentFileName))
                {
                    // log here
                    // format is as follows:
                    //
                    // [ISO date] page <client ip> client browser
                    //
                    writer.WriteLine(
                        string.Format("[{0}] {1} <{2}> {3}",
                            now.ToString("yyyy-MM-dd hh:mm:ss"),
                            page,
                            ip.Substring(0, ip.IndexOf(':')),
                            dataItem.Headers["user-agent"])
                    );
                }
            }
            catch
            {
                // swallow errors to prevent blowing up on log failures (like out of space)
            }
        }

        /// <summary>
        /// Called whenever Padarn encounters an internal error
        /// </summary>
        /// <param name="errorInfo">Information about the specific error</param>
        public virtual void LogPadarnError(string errorInfo, LogDataItem dataItem)
        {
            Debug.WriteLine(errorInfo);
            
            try
            {
                if (ServerConfiguration == null) return; // the configuration is null, so we have no output path
                if (!ServerConfiguration.LoggingEnabled) return;
                if (dataItem == null) return;

                if (!dataItem.ServerConfiguration.LoggingEnabled) return;

                using (TextWriter writer = File.AppendText( Path.Combine(ServerConfiguration.LogFileFolder, "PadarnErrors.txt")))
                {
                    writer.WriteLine(errorInfo);
                    writer.Flush();
                }
            }
            catch
            {
                // swallow errors to prevent blowing up on log failures (like out of space)
            }
        }

        /// <summary>
        /// Called during certain runtime operations.  Primarily used for debugging Padarn's internal systems.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="zoneMask">The zone associated with the incoming information</param>
        public virtual void LogRuntimeInfo(ZoneFlags zoneMask, string info)
        {
            Debug.WriteLine(string.Format("Zone {0} : {1}", zoneMask.ToString(), info));
        }
    }
}
