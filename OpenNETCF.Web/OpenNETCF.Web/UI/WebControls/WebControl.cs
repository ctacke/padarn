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
