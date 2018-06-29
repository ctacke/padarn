using System;

using System.Collections.Generic;
using System.Text;

namespace SampleSite
{
	partial class Library
	{
		/// <summary>
		/// This method will query a disparate datastore to fill in the book entries
		/// </summary>
		private void BookFetch()
		{
#if VS2008
			_bookCollection = new List<Book>() { 
					new Book("The Northern Clemency (Hardcover)", "Philip Hensher", 1400044480),
					new Book("Hurry Down Sunshine", "Michael Greenberg", 1590511913),
					new Book("Nixonland: The Rise of a President and the Fracturing of America", "Rick Perlstein", 0743243021),
					new Book("The Forever War", "Dexter Filkins", 0307266397),
					new Book("The Story of Edgar Sawtelle: A Novel", "David Wroblewski", 0061768065),
					new Book("The Mulberry Empire: A Novel", "Philip Hensher", 1400030897),
				};
#else
			_bookCollection = new List<Book>();
			_bookCollection.Add(new Book("The Northern Clemency (Hardcover)", "Philip Hensher", 1400044480));
			_bookCollection.Add(new Book("Hurry Down Sunshine", "Michael Greenberg", 1590511913));
			_bookCollection.Add(new Book("Nixonland: The Rise of a President and the Fracturing of America", "Rick Perlstein", 0743243021));
			_bookCollection.Add(new Book("The Forever War", "Dexter Filkins", 0307266397));
			_bookCollection.Add(new Book("The Story of Edgar Sawtelle: A Novel", "David Wroblewski", 0061768065));
			_bookCollection.Add(new Book("The Mulberry Empire: A Novel", "Philip Hensher", 1400030897));
#endif
		}

		/// <summary>
		/// This method will query a disparate datastore to obtain valid credentials
		/// In production systems, every time a user attempts to authenticate, a live
		/// directory store query would take place.  We would rely on that server to 
		/// perform appropriate internal caching to aide in performance
		/// </summary>
		private void CredsFetch()
		{
#if VS2008
			_credentialStore = new List<Creds> { new Creds("TestUser01", "TestPW01") };
#else
			_credentialStore = new List<Creds>();
			_credentialStore.Add(new Creds("TestUser01", "TestPW01"));
#endif
		}
	}
}
