using System;

using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

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
		public void SetupLibrary()
		{
			CredsFetch();
			//TODO: BookFetch should take in a library identifier
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
		/// Returns properties for all books in library
		/// </summary>
		/// <param name="title"></param>
		/// <param name="author"></param>
		/// <param name="ISBN"></param>
		/// <returns></returns>
		/// <remarks>Can return multiple hits</remarks>
		public ReadOnlyCollection<Book> RetrieveAllBooks()
		{
			return _bookCollection.AsReadOnly();
		}

		/// <summary>
		/// Perform a search on any of the three basic Book properties
		/// </summary>
		/// <param name="title"></param>
		/// <param name="author"></param>
		/// <param name="ISBN"></param>
		/// <returns></returns>
		/// <remarks>Can return multiple hits</remarks>
		public ReadOnlyCollection<Book> RetrieveMatchingBooks(string title, string author, string ISBN)
		{
			List<Book> bookList = new List<Book>();

			// Search through each book in the library's collection and do partial matching
			foreach (Book b in _bookCollection)
			{
				if (!String.IsNullOrEmpty(title) && b._title.IndexOf(title, StringComparison.CurrentCultureIgnoreCase) != -1
					|| !String.IsNullOrEmpty(author) && b._author.IndexOf(author, StringComparison.CurrentCultureIgnoreCase) != -1
					|| !String.IsNullOrEmpty(ISBN) && b._ISBN == Int32.Parse(ISBN))
				{
					bookList.Add(b);
				}
			}

			return bookList.AsReadOnly();
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

			// Search through each book in the library's collection and do partial matching
			foreach (Book b in _bookCollection)
			{
				if (!String.IsNullOrEmpty(title) && b._title.IndexOf(title, StringComparison.CurrentCultureIgnoreCase) != -1
					|| !String.IsNullOrEmpty(author) && b._author.IndexOf(author, StringComparison.CurrentCultureIgnoreCase) != -1
					|| !String.IsNullOrEmpty(ISBN) && b._ISBN == Int32.Parse(ISBN))
				{
					retVal.Append(String.Format("Title={0};Author={1};ISBN={2}\r\n", b._title, b._author, b._ISBN));
				}
			}

			return retVal.ToString();
		}

		/// <summary>
		/// Removes specified book from library
		/// </summary>
		public ReadOnlyCollection<Book> RemoveMatchingBooks(string title, string author, string ISBN)
		{
			// Search through each book in the library's collection and do partial matching
			for (int i = _bookCollection.Count - 1; i >= 0; i--)
			{
				Book b = _bookCollection[i];

				if (!String.IsNullOrEmpty(title) && b._title.IndexOf(title, StringComparison.CurrentCultureIgnoreCase) != -1
					|| !String.IsNullOrEmpty(author) && b._author.IndexOf(author, StringComparison.CurrentCultureIgnoreCase) != -1
					|| !String.IsNullOrEmpty(ISBN) && b._ISBN == Int32.Parse(ISBN))
				{
					_bookCollection.Remove(b);
				}
			}

			return _bookCollection.AsReadOnly();
		}
	}
}
