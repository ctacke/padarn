using System;

using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace OpenNETCF.Web.UI.WebControls
{
    /// <summary>
    /// Serves as the base class that defines the methods, properties and events common to all controls in the OpenNETCF.Web.UI.WebControls namespace.
    /// </summary>
    public class WebControl : Control
    {
        /// <summary>
        /// Initializes a new instance of the WebControl class that represents a Span HTML tag.
        /// </summary>
        protected WebControl()
        {
            TagName = "span";
        }

        /// <summary>
        /// Initializes a new instance of the WebControl class using the specified HTML tag.
        /// </summary>
        /// <param name="tag"></param>
        protected WebControl(string tag)
        {
            TagName = tag.ToLower();
        }

        /// <summary>
        /// Initializes a new instance of the WebControl class using the specified HTML tag.
        /// </summary>
        /// <param name="tag"></param>
        public WebControl(HtmlTextWriterTag tag)
        {
            TagName = tag.ToString().ToLower();
        }

        /// <summary>
        /// Gets the name of the control tag. This property is used primarily by control developers.
        /// </summary>
        protected virtual string TagName { get; private set; }

        protected internal string Content { get; set; }
    }
}
