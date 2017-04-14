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
