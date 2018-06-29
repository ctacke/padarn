using System;

using System.Collections.Generic;
using System.Text;

namespace SampleSite
{
	/// <summary>
	/// Class that defines basic Book properties
	/// </summary>
	public class Book
	{
		internal string _title;
		internal string _author;
		internal int _ISBN;

		public Book(string title, string author, int ISBN)
		{
			//Remark: Would want to perform any format validation here
			_title = title;
			_author = author;
			_ISBN = ISBN;
		}

		public override bool Equals(object obj)
		{
			bool retVal = false;
			if (obj is Book)
				retVal = this._ISBN == ((Book)obj)._ISBN;

			return retVal;
		}

		public override string ToString()
		{
			return String.Format("Book title: {0} written by {1}", _title, _author);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
