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
	public class CacheExample : Page
	{
		#region Constants

		#endregion

		#region Page Load

		protected override void Page_Load(object sender, EventArgs e)
		{
			// 1. Start timer
			int startTime = Environment.TickCount;

			// 2. Create an HTML document with our three test images
			Document page = new Document();

			Div headerImageDiv = new Div("header");
			headerImageDiv.Elements.Add(new Image("/images/testgif.gif", "Test GIF"));
			headerImageDiv.Elements.Add(Generator.LineBreak);
			headerImageDiv.Elements.Add(new Image("/images/testjpg.jpg", "Test JPG"));
			headerImageDiv.Elements.Add(Generator.LineBreak);
			headerImageDiv.Elements.Add(new Image("/images/testpng.png", "Test PNG"));

			page.Body.Elements.Add(headerImageDiv);
			page.Body.Elements.Add(Generator.LineBreak);

			// 3. Stop counter
			int et = Environment.TickCount - startTime;

			Paragraph loadTimeParagraph = new Paragraph(string.Format("Page loaded in {0:0.000} sec", 
				(float)((float)et / 1000f)));

			loadTimeParagraph.Styles.Add(new ElementStyle("font-size", "10px"));
			loadTimeParagraph.Styles.Add(new ElementStyle("text-align", "left"));
			page.Body.Elements.Add(loadTimeParagraph);

			Response.Write(page.OuterHtml);
			Response.Flush();
		}

		#endregion
	}
}

