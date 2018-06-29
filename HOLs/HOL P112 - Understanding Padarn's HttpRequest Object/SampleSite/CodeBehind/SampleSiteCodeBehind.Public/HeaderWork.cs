using System;
using OpenNETCF.Web.UI;
using OpenNETCF.Web.Html;
using System.Reflection;

namespace SampleSite
{
	public class HeaderWork : Page
	{
		private const string CLASS_NAME = "HEADER_LISTING";

		protected override void Page_Load(object sender, EventArgs e)
		{
			#region Header

			// When did transaction start?
			DateTime startLoad = DateTime.Now;

			// Create the document
			Document doc = new Document();

			// Add a header to the document
			doc.Head = new DocumentHead("OpenNETCF Padarn Web Server - Header Details", new StyleInfo("css/SampleSite.css"));

			Utility.AddPadarnHeaderToDocument(doc, true, "Default");

			Div menuContainer = new Div();
			menuContainer.ClassName = "centeredContainer";

			Div menuItemContainer = new Div();
			menuItemContainer.ClassName = "siteMenu";

			#endregion

			#region HTTP Header Details

			Table tableHeader = new Table(CLASS_NAME, "1000", Align.Center);

			// Details section #1: What's in the header?
			Row rowHeader = new Row();
			rowHeader.Cells.Add(new RawText("Key"));
			rowHeader.Cells.Add(new RawText("Value"));
			tableHeader.Headers.Add(rowHeader);

			// Go through entire NameValueCollection that makes up the request header
			foreach (string key in Request.Headers.Keys)
			{
				rowHeader = new Row();
				rowHeader.Cells.Add(new FormattedText(key, TextFormat.None, "2"));
				rowHeader.Cells.Add(new FormattedText(Request.Headers[key], 
					TextFormat.None, "2"));
				tableHeader.Rows.Add(rowHeader);
			}

			#endregion

			#region Browser Details

			Table tableBrowser = new Table(CLASS_NAME, "1000", Align.Center);

			// Details section #2: What can the browser handle?
			Row rowBrowser = new Row();
			rowBrowser.Cells.Add(new RawText("Browser Capability"));
			rowBrowser.Cells.Add(new RawText("Value"));
			tableBrowser.Headers.Add(rowBrowser);

			rowBrowser = new Row();

			Type myBrowserCapabilities = (typeof(OpenNETCF.Web.HttpBrowserCapabilities));

			// Get the public methods
			MethodInfo[] myArrayMethodInfo = myBrowserCapabilities.GetMethods();

			foreach (MethodInfo method in myArrayMethodInfo)
			{
				if (method.ReturnType == typeof(bool) && method.Name.StartsWith("get_"))
				{
					// Using reflection, pull out any boolean properties
					try
					{
						bool valueBool = (bool)method.Invoke(Request.Browser, null);

						rowBrowser = new Row();
						rowBrowser.Cells.Add(new FormattedText(method.Name.Replace("get_", String.Empty), TextFormat.None, "2"));
						rowBrowser.Cells.Add(new FormattedText(valueBool ? "YES" : "NO", TextFormat.None, "2"));
						tableBrowser.Rows.Add(rowBrowser);
					}
					catch
					{
						//Do not care
					}
				}
				else if (method.ReturnType == typeof(string))
				{
					// Using reflection, pull out any string properties
					try
					{
						string valueString = method.Invoke(Request.Browser, null) as string;

						if (!String.IsNullOrEmpty(valueString))
						{
							rowBrowser = new Row();
							rowBrowser.Cells.Add(new FormattedText(method.Name.Replace("get_", String.Empty), TextFormat.None, "2"));
							rowBrowser.Cells.Add(new FormattedText(valueString, TextFormat.None, "2"));
							tableBrowser.Rows.Add(rowBrowser);
						}
					}
					catch
					{
						//Do not care
					}
				}
				else if (method.ReturnType == typeof(Version))
				{
					// Using reflection, pull out any Version properties
					try
					{
						Version valueVersion = method.Invoke(Request.Browser, null) as Version;

						if (valueVersion != null)
						{
							rowBrowser = new Row();
							rowBrowser.Cells.Add(new FormattedText(method.Name.Replace("get_", String.Empty), TextFormat.None, "2"));
							rowBrowser.Cells.Add(new FormattedText(valueVersion.ToString(), TextFormat.None, "2"));
							tableBrowser.Rows.Add(rowBrowser);
						}
					}
					catch
					{
						//Do not care
					}
				}
			}
			#endregion

			#region Prepare Document

			double timeElapsed = DateTime.Now.Subtract(startLoad).TotalMilliseconds;

			// Add splash text
			Paragraph paragraph = new Paragraph(new FormattedText(String.Format("Header details for request made on {0} by {1} and took {2} ms",
				startLoad.ToString(), Request.UserHostAddress, timeElapsed), TextFormat.Bold, "4"), Align.Center);
			paragraph.Elements.Add(new LineBreak());

			menuItemContainer.Add(paragraph);
			menuItemContainer.Add(new LineBreak());
			menuItemContainer.Add(new LineBreak());

			// Add header details table to document
			menuItemContainer.Add(tableHeader);
			menuItemContainer.Add(new LineBreak());
			menuItemContainer.Add(new LineBreak());

			// Add browser table to document
			menuItemContainer.Add(tableBrowser);
			menuItemContainer.Add(new LineBreak());
			menuItemContainer.Add(new LineBreak());

			menuContainer.Elements.Add(menuItemContainer);
			doc.Body.Elements.Add(menuContainer);

			// Send the document html to the Response object
			Response.Write(doc.OuterHtml);

			// Flush the response
			Response.Flush();

			#endregion
		}
	}
}
