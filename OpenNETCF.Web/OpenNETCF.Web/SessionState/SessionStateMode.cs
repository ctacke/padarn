using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.SessionState
{
    /// <summary>
    /// Specifies the session-state mode.
    /// </summary>
    public enum SessionStateMode
    {
        /// <summary>
        /// Session state is disabled.
        /// </summary>
        Off,
        /// <summary>
        /// Session state is in process with the Padarn host process.
        /// </summary>
        InProc,
        /// <summary>
        /// Session state is in process with an Padarn worker process.
        /// </summary>
        StateServer,
        /// <summary>
        /// Session state is using an out-of-process SQL Server database to store state information.
        /// </summary>
        SQLServer,
        /// <summary>
        /// Session state is using a custom data store to store session-state information.
        /// </summary>
        Custom
    }
}
