using System;
using System.IO;
using System.Text;
using System.Xml;
using OpenNETCF.Web.UI;
using System.Collections.Generic;
using System.Net;
using OpenNETCF.Web.Html;
using OpenNETCF.Web.Html.Meta;

namespace SampleSite
{
	public class RedirectExample : Page
	{
		protected override void Page_Load(object sender, EventArgs e)
		{
			// 1. Create a simple input form with textbox, radio buttons, and sumbit button
			Document page = new Document();
			page.Head = new DocumentHead("OpenNETCF Padarn Web Server - Redirect Test", new StyleInfo("css/SampleSite.css"));
			Utility.AddPadarnHeaderToDocument(page, true, "Default");

			// 2. Add some text
			Paragraph paragraph = new Paragraph(new FormattedText("Redirected!", TextFormat.Bold, "4"), Align.Center);

			// 3. Display to user
			page.Body.Elements.Add(paragraph);
			Response.Write(page.OuterHtml);
			Response.Flush();
		}
	}
}