using System;
using OpenNETCF.Web.UI;
using OpenNETCF.Web.Html;
using System.IO;

namespace SampleSite
{
	public class Default : Page
	{
		public const string VIRTUAL_DIRECTORY_LOCAL = @"\Windows\DumpSite\";
		private const string CLASS_NAME = "DIRECTORY_LISTING";

		protected override void Page_Load(object sender, EventArgs e)
		{
			//Create the document
			Document doc = new Document();

			//Add a header to the document
			doc.Head = new DocumentHead("OpenNETCF Dump File Directory Listing");

			Div menuContainer = new Div();
			menuContainer.ClassName = "centeredContainer";

			Div menuItemContainer = new Div();
			menuItemContainer.ClassName = "siteMenu";

			Paragraph paragraph = new Paragraph();
			paragraph.Elements.Add(new FormattedText("Listing of Dump Virtual Folder Contents:", TextFormat.Bold, "12"));
			paragraph.Elements.Add(new LineBreak());

			Table table = new Table(CLASS_NAME, null, Align.Left);

			//Create header row corresponding to file details
			Row row = new Row();
			row.Cells.Add(new RawText("Num #"));
			row.Cells.Add(new RawText("Full Path"));
			row.Cells.Add(new RawText("File Size (kb)"));
			row.Cells.Add(new RawText("Upload date"));
			table.Headers.Add(row);

			int fileCount = 0;

			try
			{
				if (Directory.Exists(VIRTUAL_DIRECTORY_LOCAL))
				{
					foreach (string fileName in Directory.GetFiles(VIRTUAL_DIRECTORY_LOCAL))
					{
						if (!fileName.ToLower().EndsWith(".aspx"))
						{
							//Create one row per file
							DateTime dCreate = File.GetCreationTime(fileName);
							FileInfo fiFile = new FileInfo(fileName);

							row = new Row();
							//Show file number in directory, file name, file size in kB, and timestamp
							row.Cells.Add(new RawText(fileCount.ToString()));
							row.Cells.Add(new RawText(fiFile.Name));
							row.Cells.Add(new RawText((fiFile.Length / 1024).ToString()));
							row.Cells.Add(new RawText(fiFile.CreationTime.ToString()));
							table.Rows.Add(row);

							fileCount++;
						}
					}
				}

				if (fileCount == 0)
				{
					paragraph.Elements.Add(new LineBreak());
					paragraph.Elements.Add(new RawText("- Empty Directory -"));
					paragraph.Elements.Add(new LineBreak());
				}
			}
			catch
			{
				paragraph.Elements.Add(new LineBreak());
				paragraph.Elements.Add(new RawText("Error reading Dump File Directory Listing"));
				paragraph.Elements.Add(new LineBreak());
			}

			menuItemContainer.Elements.Add(paragraph);
			menuContainer.Elements.Add(menuItemContainer);
			doc.Body.Elements.Add(menuContainer);

			if (fileCount != 0)
			{
				//Add content table if there were files encountered
				menuContainer.Add(table);
			}

			//Send the document html to the Response object
			Response.Write(doc.OuterHtml);

			//Flush the response
			Response.Flush();
		}
	}
}
