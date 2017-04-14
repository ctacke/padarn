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

namespace OpenNETCF.Web.UI
{
    /// <summary>
    /// Supports the page parser in building a control and the child controls it contains.
    /// </summary>
    public class ControlBuilder
    {
        /// <summary>
        /// Initializes a new instance of the ControlBuilder class.
        /// </summary>
        public ControlBuilder()
        {
        }

        /// <summary>
        /// Gets the TemplateParser responsible for parsing the control.
        /// </summary>
        protected internal TemplateParser Parser { get; private set;}

        /// <summary>
        /// The Type for the control to be created.
        /// </summary>
        public Type ControlType { get; protected internal set; }

        /// <summary>
        /// Gets the tag name for the control to be built.
        /// </summary>
        public string TagName { get; protected internal set; }

        /// <summary>
        /// Obtains the Type of the control type corresponding to a child tag. This method is called by the Padarn page framework. 
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="attribs"></param>
        /// <returns></returns>
        public virtual Type GetChildControlType(string tagName, IDictionary attribs)
        {
            return null;
        }
    }
}
