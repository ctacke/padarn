using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web.UI;
using OpenNETCF.Web.Html;
using OpenNETCF.Web.Html.Meta;
using System.Runtime.InteropServices;
using System.IO;
using SampleLogging;

namespace SampleSite
{
	/// <summary>
	/// This code-behind logic presents the user with a searchable listing of all events
	/// captured by the Padarn server between instance resets.  A calendar control is used
	/// to help the user pick the date of concern.
	/// </summary>
    public class AdminLogView : Page
    {
		private const string CLASS_NAME = "ERROR_LISTING";

        protected override void Page_Load(object sender, EventArgs e)
        {
			//User-selected time comes in here
            string time = Request.Form["data"];

			if (time == null)
			{
				//Page version 1 - User views admin page for first time; display buttons to launch calendar and submit

				Document doc = new Document();
				doc.Head = new DocumentHead("OpenNETCF Padarn Web Server", new StyleInfo("css/SampleSite.css"));

				//Add the calendar scripts
				doc.Head.Scripts.Add(new Script("script/calendar.js"));
				doc.Head.Scripts.Add(new Script("script/calendar-en.js"));
				doc.Head.Scripts.Add(new Script("script/calendar-setup.js"));

				Utility.AddPadarnHeaderToDocument(doc, true, "AdminLogView");

				Div timeDiv = new Div();

				//Add the calendar elements
				Form calendarForm = new Form("AdminLogView.aspx", FormMethod.Post);
				calendarForm.Add(new RawText("<input type=\"text\" id=\"data\" name=\"data\" />"));
				calendarForm.Add(new RawText("<button id=\"trigger\">...</button>"));
				calendarForm.Add(new Button(new ButtonInfo(ButtonType.Submit, "Search")));
				timeDiv.Add(calendarForm);

				timeDiv.Add(new RawText("<script type=\"text/javascript\">\r\n" +
										   "Calendar.setup(\r\n" +
											"{\r\n" +
											"  inputField : \"data\",\r\n" +
											"  ifFormat : \"%m-%d-%y %H:%M\",\r\n" +
											"  button : \"trigger\",\r\n" +
											"  showsTime : \"true\"\r\n" +
											"}\r\n" +
											");\r\n" +
											"</script>\r\n"
											));

				doc.Body.Add(timeDiv);

				//Send the document html to the Response object
				Response.Write(doc.OuterHtml);

				//Flush the response
				Response.Flush();
			}

			else
			{
				//Page version 2 - User selected a valid date from the JavaScript calendar control

				//What Date was passed in?
				DateTime selectedDate = DateTime.Parse(time).Date;

				//Create the document
				Document doc = new Document();

				//Add a header to the document
				doc.Head = new DocumentHead("OpenNETCF LogProvider Sample Output", new StyleInfo("css/SampleSite.css"));

				Div menuContainer = new Div();
				menuContainer.ClassName = "centeredContainer";

				Div menuItemContainer = new Div();
				menuItemContainer.ClassName = "siteMenu";

				Paragraph paragraph = new Paragraph();
				paragraph.Elements.Add(new FormattedText("Listing of Event Log for " 
					+ selectedDate.ToShortDateString() + " :", TextFormat.Bold, "6"));
				paragraph.Elements.Add(new LineBreak());

				Table table = new Table(CLASS_NAME, "1000", Align.Center);

				//Create header row corresponding to file details
				Row row = new Row();
				row.Cells.Add(new RawText("Num #"));
				row.Cells.Add(new RawText("Page"));
				row.Cells.Add(new RawText("Client IP"));
				row.Cells.Add(new RawText("Cookies"));
				row.Cells.Add(new RawText("SSL"));
				row.Cells.Add(new RawText("Browser"));
				row.Cells.Add(new RawText("Event ID"));
				row.Cells.Add(new RawText("Event Time"));
				table.Headers.Add(row);

				//Retrieve records for selected day
				List<DataItemHelper.ClientRequestInfo> listEntries = DataItemHelper.FindRecordsOnDay(selectedDate);

				int recordCount = 0;

				foreach (DataItemHelper.ClientRequestInfo data in listEntries)
				{
					//For each record, show page (or error), client IP, cookie enablement, SSL enablement, eventID, and time
					row = new Row();
					row.Cells.Add(new FormattedText((++recordCount).ToString(), TextFormat.None, "2"));
					row.Cells.Add(new FormattedText(data.PageRequested, TextFormat.None, "2"));
					row.Cells.Add(new FormattedText(data.ClientIP, TextFormat.None, "2"));
					row.Cells.Add(new FormattedText(data.ClientBringsCookies ? "YES" : "NO", TextFormat.None, "2"));
					row.Cells.Add(new FormattedText(data.UsingSsl ? "ON" : "OFF", TextFormat.None, "2"));
					row.Cells.Add(new FormattedText(data.Browser.ToString(), TextFormat.None, "2"));
					row.Cells.Add(new FormattedText(data.EventID.ToString(), TextFormat.None, "2"));
					row.Cells.Add(new FormattedText(data.RequestDate.ToLongTimeString(), TextFormat.None, "2"));
					table.Rows.Add(row);
				}

				if (listEntries.Count == 0)
				{
					//If no records can be found, display some boilerplate
					paragraph.Elements.Add(new LineBreak());
					paragraph.Elements.Add(new FormattedText("- No records found for " + selectedDate.ToShortDateString() + " -", TextFormat.None, "6"));
					paragraph.Elements.Add(new LineBreak());
				}

				menuItemContainer.Elements.Add(paragraph);
				menuContainer.Elements.Add(menuItemContainer);
				doc.Body.Elements.Add(menuContainer);

				if (recordCount != 0)
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
}
