using System;

using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using OpenNETCF.Web;
using OpenNETCF.Web.Hosting;
using OpenNETCF.Web.Html;
using System.Collections.ObjectModel;

namespace SampleSite.Providers
{
    class LibraryVirtualFile : VirtualFile
    {
        private enum Format { Xhtml, Xml, Text }
		private enum Action { Show, Destroy, Update, Create }
        private Format renderFormat;
		private Action requestedAction;
        private bool m_exists;
        private readonly string m_virtualPath;
        private readonly string m_extension;
        private readonly string m_fileName;

        public LibraryVirtualFile(string virtualPath) : base(virtualPath)
        {
            m_virtualPath = virtualPath;
            m_extension = VirtualPathUtility.GetExtension(virtualPath).ToLower();
            m_fileName = VirtualPathUtility.GetFileName(m_virtualPath).ToLower();

            // If no extension exists, set Exists to false.
            m_exists = !String.IsNullOrEmpty(m_extension);
        }

        public override System.IO.Stream Open()
        {
			// This is the start point for a virtual path provider
			// After parsing the incoming request (with format IP/controller/action/parameters.format),
			// we create an appropriate response.  Here, we support xml and xhtml output

            HttpContext.Current.Response.ContentType = "text/xml";

            MemoryStream stream = null;

            // Based on the extension, determine the output format
            switch (m_extension)
            {
				case ".html":
					HttpContext.Current.Response.ContentType = "text/html";
					renderFormat = Format.Xhtml;
					break;
                case ".xml":
                    HttpContext.Current.Response.ContentType = "text/xml";
                    renderFormat = Format.Xml;
                    break;
                default:
                    HttpContext.Current.Response.ContentType = "text/html";
                    renderFormat = Format.Xhtml;
                    break;
            }

			// We're only working with destroy and show actions
			// Out of scope are create and update

			if (m_virtualPath.IndexOf("destroy") != -1)
				requestedAction = Action.Destroy;
			else
				requestedAction = Action.Show;

            // Split the file name on the '.' character.
            string[] fileNameArray = m_fileName.Split('.');

			// We need to decode the filename because it will come in with HTML characters
			string bookTitle = m_fileName.StartsWith("all") ? String.Empty : OpenNETCF.Web.HttpUtility.UrlDecode(fileNameArray[0]);

			// Perform delete action if requested by URL
			if (requestedAction == Action.Destroy && !String.IsNullOrEmpty(bookTitle))
			{
				LibraryManager.GetLibrary(null).RemoveMatchingBooks(bookTitle, null, null);
				bookTitle = String.Empty;
			}

            // Build the virtual content
            switch (renderFormat)
            {
                case Format.Xhtml:
					stream = GetLibraryInfoAsHtml(bookTitle);
                    break;
				case Format.Xml:
					stream = GetLibraryInfoAsXml(bookTitle);
					break;
            }

            return stream;
        }

        /// <summary>
        /// Gets a value indicating whether the Virtual File exists.
        /// </summary>
        public bool Exists
        {
            get { return true; }
        }

		/// <summary>
		/// This method will create an HTML document, using the format specified by the LibrarySvc resource
		/// </summary>
		/// <param name="title">Title of book (partial or complete) to search on</param>
		/// <returns>Resultant book collection in HTML format defined by template</returns>
        private MemoryStream GetLibraryInfoAsHtml(string title)
        {
            const string CssClass = "data_query";

            Table dataTable = new Table(CssClass);

			// We expect a read-only collection of books.  We'll either get no books, one book, or all books in the library
			ReadOnlyCollection<Book> foundBooks = (String.IsNullOrEmpty(title) ? 
				LibraryManager.GetLibrary(null).RetrieveAllBooks() : LibraryManager.GetLibrary(null).RetrieveMatchingBooks(title, null, null));

			Row header = new Row(CssClass);
			header.Cells.Add(new Cell(new RawText("Title"), CssClass));
			header.Cells.Add(new Cell(new RawText("Author"), CssClass));

			// Only show ISBN if this is a details click
			if (!String.IsNullOrEmpty(title))
				header.Cells.Add(new Cell(new RawText("ISBN"), CssClass));

			dataTable.Headers.Add(header);

			// Go through each book and fill a single table row
            foreach (Book b in foundBooks)
            {
                Row dataRow = new Row(CssClass);
				Hyperlink href = new Hyperlink(b._title, String.Format("/library/show/{0}/", b._title));
                dataRow.Cells.Add(href);
                dataRow.Cells.Add(new RawText(b._author.ToString()));

				// Only show ISBN if this is a details click
				if (!String.IsNullOrEmpty(title))
					dataRow.Cells.Add(new RawText(b._ISBN.ToString()));

                dataTable.Rows.Add(dataRow);
            }

            string htmlString = String.Format(LibrarySvc.HTML_TEMPLATE, dataTable.OuterHtml);

            MemoryStream stream = new MemoryStream();
            byte[] buffer = Encoding.ASCII.GetBytes(htmlString);
            stream.Write(buffer, 0, buffer.Length);
            stream.Position = 0;

            return stream;
        }

		/// <summary>
		/// This method will create an XML output for any books matching the inputted title
		/// </summary>
		/// <param name="title">Title of book (partial or complete) to search on</param>
		/// <returns>Resultant book collection in XML format defined by template</returns>
		private MemoryStream GetLibraryInfoAsXml(string title)
		{
			StringBuilder items = new StringBuilder();

			// We expect a read-only collection of books.  We'll either get no books, one book, or all books in the library
			ReadOnlyCollection<Book> foundBooks = (String.IsNullOrEmpty(title) ?
				LibraryManager.GetLibrary(null).RetrieveAllBooks() : LibraryManager.GetLibrary(null).RetrieveMatchingBooks(title, null, null));

			// Go through each book and fill the basic XML element template
			foreach (Book b in foundBooks)
			{
				items.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, LibrarySvc.XML_ITEM_TEMPLATE,
								   b._title, b._author, b._ISBN);
			}

			// Fill in the XML document template
			string xmlString = String.Format(LibrarySvc.XML_TEMPLATE, items);

			MemoryStream stream = new MemoryStream();
			byte[] buffer = Encoding.ASCII.GetBytes(xmlString);
			stream.Write(buffer, 0, buffer.Length);
			stream.Position = 0;

			return stream;
		}
    }
}
