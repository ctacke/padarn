#region License
// Copyright ©2017 Tacke Consulting (dba OpenNETCF)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or 
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
// ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion
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
