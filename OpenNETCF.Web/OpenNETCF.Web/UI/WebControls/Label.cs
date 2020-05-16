using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.UI.WebControls
{
    /// <summary>
    /// Represents a label control, which displays text on a Web page.
    /// </summary>
    public class Label : WebControl, ITextControl
    {
        /// <summary>
        /// Gets or sets the text caption displayed in the Label control.
        /// </summary>
        public string Text { get; set; }
        
        protected internal override void Render(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("span");
            if (Parameters.Contains("id"))
            {
                writer.WriteAttributeString("id", Parameters["id"].ToString());
            }

            if (!string.IsNullOrEmpty(Content))
            {
                writer.WriteValue(Content);
            }
            else if (Parameters.Contains("text"))
            {
                writer.WriteValue(Parameters["text"].ToString());
            }
            
            writer.WriteEndElement();
        }
    }
}
