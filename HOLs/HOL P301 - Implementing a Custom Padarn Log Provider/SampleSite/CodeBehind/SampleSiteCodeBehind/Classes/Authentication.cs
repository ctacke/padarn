using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SampleSite
{
	class Authentication
	{
		public struct UserObject
		{
			string _userName;
			string _userPW;
			Guid _GUID;
			DateTime _GUIDExpire;

			/// <summary>
			/// User name of account
			/// </summary>
			public string UserName
			{
				get { return _userName; }
			}

			/// <summary>
			/// User password of account
			/// </summary>
			public string UserPW
			{
				get { return _userPW; }
			}

			/// <summary>
			/// Unique identifier associated with authentication session
			/// </summary>
			public Guid UserGuid
			{
				get { return _GUID; }
			}

			/// <returns>Is this object null</returns>
			public bool Null
			{
				get { return String.IsNullOrEmpty(_userName); }
			}

			/// <returns>Has the user's authentication session expired?</returns>
			public bool IsGuidExpired()
			{
				return DateTime.Now > this._GUIDExpire;
			}

			/// <summary>
			/// Method to expire user's authentication session for logout
			/// </summary>
			public void SetGuidExpired()
			{
				_GUIDExpire = DateTime.Now.AddDays(-1);
			}

			/// <summary>
			/// Creates a new authentication Guid for user
			/// </summary>
			/// <returns></returns>
			public Guid SetGuid()
			{
				this._GUIDExpire = DateTime.Now.AddDays(1);
				this._GUID = System.Guid.NewGuid();
				return this._GUID;
			}

			/// <summary>
			/// Public constructor for UserObject
			/// </summary>
			/// <param name="userName">User name</param>
			/// <param name="userPW">User password</param>
			public UserObject(string userName, string userPW)//, Guid inGUID)
			{
				_userName = userName;
				_userPW = userPW;
				_GUID = Guid.Empty;
				_GUIDExpire = DateTime.MinValue;
			}
		}

		//We will store credentials as userName
		private static Dictionary<string, UserObject> userCredentialStore = new Dictionary<string, UserObject>();

		//Static constructor pre-loads values
		static Authentication()
		{
			userCredentialStore.Add("TestUser01", new UserObject("TestUser01", "TestPW01"));
			userCredentialStore.Add("TestUser02", new UserObject("TestUser02", "TestPW02"));
			userCredentialStore.Add("TestUser03", new UserObject("TestUser03", "TestPW03"));
			userCredentialStore.Add("TestUser04", new UserObject("TestUser04", "TestPW04"));
		}

		/// <summary>
		/// Function that attempts to query credential store
		/// </summary>
		/// <param name="userName">User Name</param>
		/// <param name="userPW">Password</param>
		/// <returns>Valid Guid for future use</returns>
		public static Guid ValidateUser(string userName, string userPW)
		{
			Guid retVal = Guid.Empty;

			if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(userPW))
			{
				UserObject uoTarget = userCredentialStore.ContainsKey(userName) ? userCredentialStore[userName] : new UserObject();
				if (!uoTarget.Null && userCredentialStore[userName].UserPW.Equals(userPW))
				{
					//If a GUID already exists and hasn't expired, return it
					if (uoTarget.UserGuid != Guid.Empty && !uoTarget.IsGuidExpired())
						retVal = userCredentialStore[userName].UserGuid;
					else
					{
						uoTarget.SetGuid();
						userCredentialStore.Remove(userName);
						userCredentialStore.Add(userName, uoTarget);
						retVal = userCredentialStore[userName].UserGuid;
					}
				}
			}

			return retVal;
		}

		/// <summary>
		/// Function that attempts to query credential store
		/// </summary>
		/// <param name="inGuid">Guid against which we're checking</param>
		/// <returns>Valid Guid for future use</returns>
		public static UserObject ValidateUser(Guid inGuid)
		{
			UserObject retVal = new UserObject();

			foreach (UserObject uo in userCredentialStore.Values)
			{
				if (uo.UserGuid == inGuid)
				{
					retVal = uo;
					break;
				}
			}

			return retVal;

		}

		/// <summary>
		/// Method to remove GUID and last access time for particular user name
		/// </summary>
		/// <param name="UID"></param>
		internal static void LogOff(string UID)
		{
			if (!String.IsNullOrEmpty(UID))
			{
				UserObject uoTarget = userCredentialStore.ContainsKey(UID) ? userCredentialStore[UID] : new UserObject();
				if (!uoTarget.Null)
				{
					uoTarget.SetGuidExpired();
					userCredentialStore.Remove(UID);
					userCredentialStore.Add(UID, uoTarget);
				}
			}			
		}
	}
}
