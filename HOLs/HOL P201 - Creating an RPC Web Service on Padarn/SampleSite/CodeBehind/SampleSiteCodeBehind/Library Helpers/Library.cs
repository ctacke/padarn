using System;

using System.Collections.Generic;
using System.Text;

namespace SampleSite
{
	/// <summary>
	/// Class encapsulates a library with authorized users and a collection of books
	/// </summary>
	public partial class Library
	{
		//Simulated collection of credentials.  Would ordinarily come from encrypted SQL database or additional
		//web service query
		private List<Creds> _credentialStore;

		//Simulated collection of books.  Would ordinarily come from SQL or file-based datastore
		private List<Book> _bookCollection;

		//Create a list of users for library as well as a collection of books
		public Library()
		{
			CredsFetch();
			BookFetch();
		}

		/// <summary>
		/// Attempts to login specified Library user
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="userPW"></param>
		/// <returns>Empty string if invalid credentials; otherwise, returns usable token ID</returns>
		public string Login(string userName, string userPW)
		{
			return Authentication.Login(ref _credentialStore, userName, userPW);
		}

		/// <summary>
		/// Checks to ensure passed in authentication token matches live user
		/// </summary>
		/// <param name="guidInput"></param>
		/// <returns></returns>
		/// <remarks>This function would be ordinarily extended to return a user role</remarks>
		public bool IsValidAuthenticationToken(String guidInput)
		{
			return Authentication.IsValidAuthenticationToken(_credentialStore, guidInput);
		}

		/// <summary>
		/// Perform a search on any of the three basic Book properties
		/// </summary>
		/// <param name="title"></param>
		/// <param name="author"></param>
		/// <param name="ISBN"></param>
		/// <returns></returns>
		/// <remarks>Can return multiple hits</remarks>
		public string RetrieveBookProperties(string title, string author, string ISBN)
		{
			StringBuilder retVal = new StringBuilder();

			foreach (Book b in _bookCollection)
			{
				if (!String.IsNullOrEmpty(title) && b._title.IndexOf(title) != -1
					|| !String.IsNullOrEmpty(author) && b._author.IndexOf(author) != -1
					|| !String.IsNullOrEmpty(ISBN) && b._ISBN == Int32.Parse(ISBN))
				{
					retVal.Append(String.Format("Title={0};Author={1};ISBN={2}\r\n", b._title, b._author, b._ISBN));
				}
			}

			return retVal.ToString();
		}
	}
}
