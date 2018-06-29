using System;

using System.Collections.Generic;
using System.Text;

namespace SampleSite
{
	/// <summary>
	/// Class encapsulates basic credential checking routine to ensure user is legit
	/// </summary>
	public class Authentication
	{
		/// <summary>
		/// Attempts to login specified Library user
		/// </summary>
		/// <param name="creds"></param>
		/// <param name="userName"></param>
		/// <param name="userPW"></param>
		/// <returns>Empty string if invalid credentials; otherwise, returns usable token ID</returns>
		internal static string Login(ref List<Creds> credsStore, string userName, string userPW)
		{
			string retVal = String.Empty;

			Creds foundCredentials = credsStore.Find(delegate(Creds storedCreds)
			{
				return (storedCreds.credentials.UserName.Equals(userName) && storedCreds.credentials.Password.Equals(userPW));
			});

			if (foundCredentials.credentials != null && !String.IsNullOrEmpty(foundCredentials.credentials.UserName))
			{
				if (foundCredentials.sessionID == Guid.Empty)
				{
					credsStore.Remove(foundCredentials);
					foundCredentials.sessionID = Guid.NewGuid();
					credsStore.Insert(0, foundCredentials);
				}

				retVal = foundCredentials.sessionID.ToString();
			}

			return retVal;
		}

		/// <summary>
		/// Checks to ensure passed in authentication token matches live user
		/// </summary>
		/// <param name="guidInput"></param>
		/// <returns></returns>
		/// <remarks>This function would be ordinarily extended to return a user role</remarks>
		internal static bool IsValidAuthenticationToken(List<Creds> credsStore, String guidInput)
		{
			bool retVal = false;

			if (!String.IsNullOrEmpty(guidInput))
			{
				foreach (Creds c in credsStore)
				{
					if (c.sessionID.Equals(new Guid(guidInput)))
					{
						retVal = true;
						break;
					}
				}
			}

			return retVal;
		}
	}
}
