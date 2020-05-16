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
