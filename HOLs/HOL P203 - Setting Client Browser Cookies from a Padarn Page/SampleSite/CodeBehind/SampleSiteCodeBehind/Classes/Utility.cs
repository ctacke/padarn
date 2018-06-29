using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web.Html;

namespace SampleSite
{
    internal static class Utility
    {
        public static void AddPadarnHeaderToDocument(Document doc, bool showMainPageLink, string sourcename)
        {
            Div headerImageDiv = new Div("header");
            headerImageDiv.Elements.Add(new Image("/images/header.png", "OpenNETCF Consulting, LLC."));
            if(showMainPageLink)
            {
                headerImageDiv.Elements.Add(Generator.LineBreak);
                headerImageDiv.Elements.Add(new Paragraph(new Hyperlink("Back to Main Page", "/default.aspx")));
            }

            doc.Body.Elements.Add(headerImageDiv);
            doc.Body.Elements.Add(Generator.LineBreak);

        }

        public static void AddETFooterToDocument(Document doc, int startTime)
        {
            int et = Environment.TickCount - startTime;

            Paragraph loadTimeParagraph = new Paragraph(string.Format("Page loaded in {0:0.000} sec", (float)((float)et / 1000f)));

            // some elements allow setting element-specific styles
            loadTimeParagraph.Styles.Add(new ElementStyle("font-size", "10px"));
            loadTimeParagraph.Styles.Add(new ElementStyle("text-align", "left"));
            doc.Body.Elements.Add(loadTimeParagraph);
        }
    }
}
