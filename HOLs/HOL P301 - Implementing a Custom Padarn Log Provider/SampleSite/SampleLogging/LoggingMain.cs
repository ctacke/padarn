using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web.Logging;
using OpenNETCF.Web.Configuration;
using System.Diagnostics;

namespace SampleLogging
{
	public class LoggingSupport : ILogProvider
	{
		#region Implemented Interfaces

		/// <summary>
		/// Gets or sets configuration data pertaining to running web server
		/// </summary>
		public ServerConfig ServerConfiguration { get; set; }

		/// <summary>
		/// Implementation for Error Logging Event
		/// </summary>
		/// <param name="errorInfo">String representing error text</param>
		/// <param name="dataItem">Contains data about a Padarn page access</param>
		public void LogPadarnError(string errorInfo, LogDataItem dataItem)
		{
			//Store away this error.  LogDataItem may or may not be null
			DataItemHelper.AddErrorItem(errorInfo, dataItem, ServerConfiguration);
		}

		/// <summary>
		/// Implementation for normal page access event
		/// </summary>
		/// <param name="dataItem">Contains data about a Padarn page access</param>
		public void LogPageAccess(LogDataItem dataItem)
		{
			//Store away this entry item
			DataItemHelper.AddLogDataItem(dataItem, ServerConfiguration);
		}

		/// <summary>
		/// Called during certain runtime operations.  Primarily used for debugging Padarn's internal systems.
		/// </summary>
		/// <param name="zoneMask">Origin of operation</param>
		/// <param name="info">Any information Padarn wishes to pass along (ID of event)</param>
		/// <remarks>This function should be treated as an unsupported API used for debugging only</remarks>
		public void LogRuntimeInfo(ZoneFlags zoneMask, string info)
		{
			Debug.WriteLine(string.Format(" zone '{0}' : {1}", zoneMask.ToString(), info)); 
		}

		#endregion

		#region Other Functions

		#endregion

	}
}
