using System;

using System.Collections.Generic;
using System.Text;

namespace SampleSite
{
	/// <summary>
	/// Current sample only assumes one library.  A more realistic demo would have a collection
	/// of libraries, each with its own book repository and credential set
	/// </summary>
	public class LibraryManager
	{
		static Dictionary<string, Library> m_libraryList = new Dictionary<string, Library>();

		/// <summary>
		/// Returns a Library instance and ensures it's initialized properly
		/// </summary>
		/// <param name="name">Name of library to be obtained</param>
		/// <remarks>This implementation ignored library name; you might wish to define
		/// multiple libraries with unique string identification</remarks>
		public static Library GetLibrary(string name)
		{
			if (String.IsNullOrEmpty(name))
			{
				name = "Default";
			}

			if (!String.IsNullOrEmpty(name) && !m_libraryList.ContainsKey(name))
			{
				Library newLib = new Library();
				newLib.SetupLibrary();
				m_libraryList.Add(name, newLib);
			}

			return m_libraryList[name];
		}
	}
}
