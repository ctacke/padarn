using System;

using System.Collections.Generic;
using System.Text;

namespace SampleLogging
{
	/// <summary>
	/// Helper class that makes sense of User Agent Strings for logging display purposes
	/// </summary>
	/// <remarks>User Agent Strings courtesy of UserAgentString.com</remarks>
	public class GetBrowserType
	{
		/// <summary>
		/// Encapsulation of current major browser series
		/// </summary>
		public enum BrowserType
		{
			/// <summary> Unknown browser type </summary>
			Unknown,
			/// <summary> Mozilla Firefox v2.x </summary>
			Firefox_2,
			/// <summary> Mozilla Firefox v3.x </summary>
			Firefox_3,
			/// <summary> Microsoft Internet Explorer v6.x </summary>
			IE_6,
			/// <summary> Microsoft Internet Explorer v7.x </summary>
			IE_7,
			/// <summary> Microsoft Internet Explorer v8.x </summary>
			IE_8,
			/// <summary> Apple Safari v3.x </summary>
			Safari_3,
			/// <summary> Apple Safari v4.x </summary>
			Safari_4,
			/// <summary> Google Chrome v1.x </summary>
			Chrome_1,
			/// <summary> Google Chrome v2.x </summary>
			Chrome_2
		}

		/// <summary>
		/// Attempts to determine browser type based upon agent string
		/// </summary>
		/// <param name="userAgentString">Agent string as reported by user</param>
		/// <returns>Browser series used by user</returns>
		public static BrowserType DetermineBrowserType(string userAgentString)
		{
			BrowserType retVal = BrowserType.Unknown;

			if (!String.IsNullOrEmpty(userAgentString))
			{
				//Chrome 
				if (userAgentString.IndexOf("Chrome/2.") != -1)
					retVal = BrowserType.Chrome_2;
				else if (userAgentString.IndexOf("Chrome/1.") != -1)
				{
					retVal = BrowserType.Chrome_1;
				}
				//Firefox
				else if (userAgentString.IndexOf("Firefox/2.") != -1)
				{
					retVal = BrowserType.Firefox_2;
				}
				else if (userAgentString.IndexOf("Firefox/3.") != -1)
				{
					retVal = BrowserType.Firefox_3;
				}
				//MSIE
				else if (userAgentString.IndexOf("MSIE 6.") != -1)
				{
					retVal = BrowserType.IE_6;
				}
				else if (userAgentString.IndexOf("MSIE 7.") != -1)
				{
					retVal = BrowserType.IE_7;
				}
				else if (userAgentString.IndexOf("MSIE 8.") != -1)
				{
					retVal = BrowserType.IE_8;
				}
				//Safari
				else if (userAgentString.IndexOf("Version/3") != -1 && userAgentString.IndexOf("Safari") != -1)
				{
					retVal = BrowserType.Safari_3;
				}
				else if (userAgentString.IndexOf("Version/4") != -1 && userAgentString.IndexOf("Safari") != -1)
				{
					retVal = BrowserType.Safari_4;
				}
			}

			return retVal;
		}
	}
}
