using System;
using OpenNETCF.Web.UI;
using OpenNETCF.Web.Html;

namespace SampleSite
{
	public class Default : Page
	{
		protected override void Page_Load(object sender, EventArgs e)
		{

			// create the document
			Document doc = new Document();

			// add a header to the document
			doc.Head = new DocumentHead("OpenNETCF Padarn Web Server");

			#region --- other site links ---
			Div menuContainer = new Div();
			menuContainer.ClassName = "centeredContainer";

			Div menuItemContainer = new Div();
			menuItemContainer.ClassName = "siteMenu";

			Paragraph paragraph = new Paragraph();
			paragraph.Elements.Add(new FormattedText("Hello World Padarn!", TextFormat.Bold, "20"));
			paragraph.Elements.Add(new LineBreak());
			paragraph.Elements.Add(new FormattedText("If you are viewing this page, your credentials " + 
				"were accepted (though this connection is not encrypted)", TextFormat.Bold));

			menuItemContainer.Elements.Add(paragraph);
			menuContainer.Elements.Add(menuItemContainer);
			doc.Body.Elements.Add(menuContainer);
			#endregion

			// send the document html to the Response object
			Response.Write(doc.OuterHtml);

			// flush
			Response.Flush();
		}
	}
}
