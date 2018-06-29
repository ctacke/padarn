using System;

using System.Collections.Generic;
using System.Text;
using System.Net;

namespace SampleSite
{
	/// <summary>
	/// Structure to hold credentials for Library user
	/// </summary>
	public struct Creds
	{
		/// <summary> User name and password pair </summary>
		internal NetworkCredential credentials;
		/// <summary> User authentication session </summary>
		internal Guid sessionID;

		public Creds(string userNameIn, string userPWIn)
		{
			credentials = new NetworkCredential(userNameIn, userPWIn);
			sessionID = Guid.Empty;
		}
	}
}
