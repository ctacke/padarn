using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.UI.WebControls
{
    /// <summary>
    /// Displays a hyperlink-style button control on a Web page.
    /// </summary>
    public class LinkButton : WebControl, IPostBackEventHandler //, IButtonControl, 
    {
        /// <summary>
        /// Occurs when the LinkButton control is clicked.
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// Gets or sets the text caption displayed in the Button control.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Raises the Click event of the LinkButton control.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClick(EventArgs e)
        {
            var handler = Click;

            if (handler == null) return;

            handler(this, e);
        }

        // <a id="LinkButton1" href="javascript:__doPostBack('LinkButton1','')">LinkButton</a>
        protected internal override void Render(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("a");

            string id = "unknown";

            if (Parameters.Contains("id"))
            {
                id = Parameters["id"].ToString();
                writer.WriteAttributeString("id", id);
            }

            writer.WriteAttributeString("href", string.Format("javascript:__doPostBack('{0}','')", id));

            if (!string.IsNullOrEmpty(Content))
            {
                writer.WriteValue(Content);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Raises events for the LinkButton control when it posts back to the server.
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
        }
    }
}
