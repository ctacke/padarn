using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web.Logging;
using OpenNETCF.Web.Configuration;

namespace SampleLogging
{
	/// <summary>
	/// Class exists to store LogDataItem information from Padarn ILogProvider in a meaningful format
	/// </summary>
	/// <remarks>This class should write-through to disk on the Padarn server</remarks>
	public class DataItemHelper
	{
		#region Fields

		private static List<ClientRequestInfo> _logRecords;

		#endregion

		#region Constants

		private const string NO_CLIENT_IP = "No client IP";
		private const string USER_AGENT = "HTTP_USER_AGENT";

		#endregion

		#region Enumerations

		/// <summary>
		/// Enumeration describing event having taken place
		/// </summary>
		public enum Events
		{
			/// <summary> Unknown </summary>
			Unknown,
			/// <summary> Page Load has occurred </summary>
			PageLoad,
			/// <summary> ArgumentException has occurred 
			/// See <see cref="ArgumentException"/> for details
			/// </summary>	
			ErrorArgumentException,
			/// <summary> ArgumentNullException has occurred 
			/// See <see cref="ArgumentNullException"/> for details
			/// </summary>	
			ErrorArgumentNullException,
			/// <summary> ArgumentOutOfRangeException has occurred 
			/// See <see cref="ArgumentOutOfRangeException"/> for details
			/// </summary>	
			ErrorArgumentOutOfRangeException,
			/// <summary> ConfigurationException has occurred 
			/// See <see cref="OpenNETCF.Configuration.ConfigurationException"/> for details
			/// </summary>	
			ErrorConfigurationException,
			/// <summary> CryptographicException has occurred 
			/// See <see cref="System.Security.Cryptography.CryptographicException"/> for details
			/// </summary>	
			ErrorCryptographicException,
			/// <summary> FileNotFoundException has occurred 
			/// See <see cref="FileNotFoundException"/> for details
			/// </summary>	
			ErrorFileNotFoundException,
			/// <summary> HttpException has occurred 
			/// See <see cref="OpenNETCF.Web.HttpException"/> for details
			/// </summary>	
			ErrorHttpException,
			/// <summary> InvalidOperationException has occurred 
			/// See <see cref="InvalidOperationException"/> for details
			/// </summary>	
			ErrorInvalidOperationException,
			/// <summary> IOException has occurred 
			/// See <see cref="IOException"/> for details
			/// </summary>	
			ErrorIOException,
			/// <summary> NotSupportedException has occurred 
			/// See <see cref="NotSupportedException"/> for details
			/// </summary>	
			ErrorNotSupportedException,
			/// <summary> NullReference has occurred 
			/// See <see cref="NullReference"/> for details
			/// </summary>	
			ErrorNullReference,
			/// <summary> PlatformNotSupportedException has occurred 
			/// See <see cref="PlatformNotSupportedException"/> for details
			/// </summary>	
			ErrorPlatformNotSupportedException,
			/// <summary> SecurityException has occurred 
			/// See <see cref="SecurityException"/> for details
			/// </summary>	
			ErrorSecurityException,
			/// <summary> Non-specific exception </summary>
			ErrorOther
		}

		#endregion

		#region Structures

		/// <summary>
		/// Class that represents either a valid or invalid request to Padarn
		/// </summary>
		public struct ClientRequestInfo
		{
			string _pageRequested;
			string _clientIP;
			bool _clientBringsCookies;
			bool _usingSsl;
			GetBrowserType.BrowserType _browser;
			Events _eventID;
			DateTime _UTCDate;

			internal ClientRequestInfo(string pageRequested, string clientIP, bool clientBringsCookies, bool isUsingSSL,
				GetBrowserType.BrowserType browserUsed, Events eventID, DateTime timeOfEvent)
			{
				_pageRequested = pageRequested;
				_clientIP = clientIP;
				_clientBringsCookies = clientBringsCookies;
				_usingSsl = isUsingSSL;
				_browser = browserUsed;
				_eventID = eventID;
				_UTCDate = timeOfEvent;
			}

			/// <summary>
			/// Specified page where event occurred
			/// </summary>
			public string PageRequested
			{
				get { return _pageRequested; }
			}

			/// <summary>
			/// IP address of requesting client
			/// </summary>
			public string ClientIP
			{
				get { return !String.IsNullOrEmpty(_clientIP) ? _clientIP : NO_CLIENT_IP; }
			}

			/// <summary>
			/// Is the client bringing cookies for this domain?
			/// </summary>
			public bool ClientBringsCookies
			{
				get { return _clientBringsCookies; }
			}

			/// <summary>
			/// Indicates whether SSL is enabled during request
			/// </summary>
			public bool UsingSsl
			{
				get { return _usingSsl; }
			}

			/// <summary>
			/// Returns browser type
			/// </summary>
			/// <seealso cref="GetBrowserType.BrowserType"/>
			public GetBrowserType.BrowserType Browser
			{
				get { return _browser; }
			}

			/// <summary>
			/// Event ID if error occured 
			/// </summary>
			public Events EventID
			{
				get { return _eventID; }
			}

			/// <summary>
			/// Date during which request took place
			/// </summary>
			public DateTime RequestDate
			{
				get { return _UTCDate; }
			}
		}

		#endregion

		#region External Functions

		/// <summary>
		/// Returns list of ClientData items for a particular day
		/// </summary>
		/// <param name="specifiedDay">Day of interest in UTC Date</param>
		public static List<ClientRequestInfo> FindRecordsOnDay(DateTime specifiedDay)
		{
			List<ClientRequestInfo> records = new List<ClientRequestInfo>();

			for (int i = 0; i < _logRecords.Count; i++)
			{
				//Go through each item and determine if it occurred during target day
				if (_logRecords[i].RequestDate.Date == specifiedDay.Date)
				{
					records.Add(_logRecords[i]);
				}
			}

			return records;
		}

		/// <summary>
		/// Function to add LogDataItem to our internal store
		/// </summary>
		/// <param name="logData">Contains data about a Padarn page access</param>
		/// <remarks>This function logs everything; it'd be advisable to define rules as to what to log</remarks>
		internal static void AddLogDataItem(LogDataItem logData, ServerConfig serverConfig)
		{
			if (logData != null && serverConfig != null)
			{
				string userAgentString =  logData.Headers[USER_AGENT];
				GetBrowserType.BrowserType browser = GetBrowserType.DetermineBrowserType(userAgentString);

				//Add record to data structure
				_logRecords.Add(new ClientRequestInfo(logData.PageName, logData.RemoteClientIP,
					serverConfig.Cookies.Domain != null, serverConfig.UseSsl, browser, Events.PageLoad, DateTime.Now));
			}
		}

		/// <summary>
		/// Method to store away error record
		/// </summary>
		/// <param name="errorInfo">String representing error text</param>
		/// <param name="logData">Contains data about a Padarn page access</param>
		/// <param name="serverConfig">ServerConfiguration of Padarn instance</param>
		/// <remarks>logData will likely be null</remarks>
		internal static void AddErrorItem(string errorInfo, LogDataItem logData, ServerConfig serverConfig)
		{
			string pageName = logData != null ? logData.PageName : errorInfo;
			string remoteIP = logData != null ? logData.RemoteClientIP : String.Empty;
			GetBrowserType.BrowserType browser = GetBrowserType.BrowserType.Unknown;

			if (logData != null)
			{
				string userAgentString = logData.Headers[USER_AGENT];
				browser = GetBrowserType.DetermineBrowserType(userAgentString);
			}

			_logRecords.Add(new ClientRequestInfo(pageName, remoteIP, serverConfig.Cookies.Domain != null,
				serverConfig.UseSsl, browser, ParseErrorCode(errorInfo), DateTime.Now));
		}

		#endregion

		#region Private Functions

		/// <summary>
		/// Static initializer of DataItemHelperClass (implicitly called)
		/// </summary>
		static DataItemHelper()
		{
			//Make sure our "data store" is initialized
			_logRecords = new List<ClientRequestInfo>();
		}

		/// <summary>
		/// Method that parses internal Padarn errors and tries to provide a useful
		/// error code to store in the ClientRequestInfo
		/// </summary>
		/// <param name="errorInfo">Incoming error string</param>
		/// <returns>Enumeration describing error</returns>
		private static Events ParseErrorCode(string errorInfo)
		{
			Events retVal = Events.Unknown;

			if (errorInfo.IndexOf("ArgumentException") != -1)
				retVal = Events.ErrorArgumentException;
			else if (errorInfo.IndexOf("ArgumentNullException") != -1)
				retVal = Events.ErrorArgumentNullException;
			else if (errorInfo.IndexOf("ArgumentOutOfRangeException") != -1)
				retVal = Events.ErrorArgumentOutOfRangeException;
			else if (errorInfo.IndexOf("ConfigurationException") != -1)
				retVal = Events.ErrorConfigurationException;
			else if (errorInfo.IndexOf("CryptographicException") != -1)
				retVal = Events.ErrorCryptographicException;
			else if (errorInfo.IndexOf("FileNotFoundException") != -1)
				retVal = Events.ErrorFileNotFoundException;
			else if (errorInfo.IndexOf("HttpException") != -1)
				retVal = Events.ErrorHttpException;
			else if (errorInfo.IndexOf("InvalidOperationException") != -1)
				retVal = Events.ErrorInvalidOperationException;
			else if (errorInfo.IndexOf("IOException") != -1)
				retVal = Events.ErrorIOException;
			else if (errorInfo.IndexOf("NotSupportedException") != -1)
				retVal = Events.ErrorNotSupportedException;
			else if (errorInfo.IndexOf("NullReference") != -1)
				retVal = Events.ErrorNullReference;
			else if (errorInfo.IndexOf("PlatformNotSupportedException") != -1)
				retVal = Events.ErrorPlatformNotSupportedException;
			else if (errorInfo.IndexOf("SecurityException") != -1)
				retVal = Events.ErrorSecurityException;
			else
				retVal = Events.ErrorOther;

			return retVal;
		}

		#endregion
	}
}