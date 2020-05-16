using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.UI.WebControls
{
    /// <summary>
    /// Displays a text box control for user input.
    /// </summary>
    public class TextBox : WebControl //, IButtonControl, IPostBackEventHandler
    {
        /// <summary>
        /// Occurs when the content of the text box changes between posts to the server.
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// Gets or sets the text caption displayed in the TextBox control.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the behavior mode (single-line, multiline, or password) of the TextBox control.
        /// </summary>
        public virtual TextBoxMode TextMode { get; set; }

        /// <summary>
        /// Raises the TextChanged event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTextChanged(EventArgs e)
        {
            var handler = TextChanged;

            if (handler == null) return;

            handler(this, null);
        }

        /// <summary>
        /// Sends server control content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer"></param>
        protected internal override void Render(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("input");

            if (Parameters.Contains("id"))
            {
                writer.WriteAttributeString("id", Parameters["id"].ToString());
                writer.WriteAttributeString("name", Parameters["id"].ToString());
            }

            if (Parameters.Contains("textmode"))
            {
                var m = Parameters["textmode"];
                writer.WriteAttributeString("type", m.ToString());
            }
            else
            {
                writer.WriteAttributeString("type", "text");
            }

            if (!string.IsNullOrEmpty(Content))
            {
                writer.WriteAttributeString("value", Content);
            }
            else if (Parameters.Contains("text"))
            {
                writer.WriteAttributeString("value", Parameters["text"].ToString());
            }

            writer.WriteEndElement();
        }

        protected internal override void SetParameters(System.Collections.IDictionary parms)
        {
            base.SetParameters(parms);

            if (parms.Contains("textmode"))
            {
                switch (((string)parms["textmode"]).ToLower())
                {
                    case "password":
                        TextMode = TextBoxMode.Password;
                        break;
                    case "multiline":
                        TextMode = TextBoxMode.MultiLine;
                        break;
                    default:
                        TextMode = TextBoxMode.SingleLine;
                        break;
                }
            }
        }
    }
}
