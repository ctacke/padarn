using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.UI.WebControls
{
    /// <summary>
    /// Displays a push button control on the Web page.
    /// </summary>
    public class Button : WebControl, IPostBackEventHandler //, IButtonControl, 
    {
        /// <summary>
        /// Occurs when the Button control is clicked.
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// Gets or sets the text caption displayed in the Button control.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Raises the Click event of the Button control.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClick(EventArgs e)
        {
            var handler = Click;

            if (handler == null) return;

            handler(this, null);
        }

        //<input type="submit" name="Button1" value="Button" id="Button1" />
        protected internal override void Render(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("input");
            writer.WriteAttributeString("type", "submit");

            if (Parameters.Contains("text"))
            {
                writer.WriteAttributeString("value", Parameters["text"].ToString());
            }
            if (Parameters.Contains("id"))
            {
                writer.WriteAttributeString("id", Parameters["id"].ToString());
                writer.WriteAttributeString("name", Parameters["id"].ToString());
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Raises events for the Button control when it posts back to the server.
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
        }
    }
}
