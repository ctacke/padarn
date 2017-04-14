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
using System.Xml;
using System.Collections;

namespace OpenNETCF.Web.UI
{
    /// <summary>
    /// Defines the properties, methods, and events that are shared by all Padarn server controls.
    /// </summary>
    public class Control : IDisposable
    {
        /// <summary>
        /// Occurs when the server control is loaded into the <see cref="Page"/> object.
        /// </summary>
        public event EventHandler Load;

        /// <summary>
        /// Occurs when the server control is initialized, which is the first step in its lifecycle.
        /// </summary>
        public event EventHandler Init;

        /// <summary>
        /// Initializes a new instance of the <see cref="Control" /> class.
        /// </summary>
        public Control()
        {
            Controls = new ControlCollection();
            Visible = true;

            if (Load != null)
            {
                Load(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the Init event.
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnInit(EventArgs e)
        {
            var handler = Init;

            if (handler == null) return;
            handler(this, null);
        }

        /// <summary>
        /// Raises the Load event.
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnLoad(EventArgs e)
        {
            var handler = Load;

            if (handler == null) return;
            handler(this, null);
        }

        /// <summary>
        /// Gets or sets the programmatic identifier assigned to the server control.
        /// </summary>
        public virtual string ID { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether a server control is rendered as UI on the page.
        /// </summary>
        public virtual bool Visible { get; set; }

        /// <summary>
        /// Gets a reference to the Page instance that contains the server control.
        /// </summary>
        public virtual Page Page { get; internal set; }

        /// <summary>
        /// Sends server control content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer"></param>
        protected internal virtual void Render(HtmlTextWriter writer)
        {
        }

        protected internal virtual void Render(XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a ControlCollection object that represents the child controls for a specified server control in the UI hierarchy.
        /// </summary>
        public ControlCollection Controls
        {
            get;
            internal set;
        }

        public virtual void Dispose()
        {
        }

        protected internal virtual void SetParameters(IDictionary parms)
        {
            Parameters = parms;

            // parse for fields

            if (parms.Contains("id"))
            {
                ID = (string)parms["id"];
            }
            if (parms.Contains("visible"))
            {
                Visible = bool.Parse((string)parms["visible"]);
            }
        }

        protected internal IDictionary Parameters { get; private set; }
    }
}
