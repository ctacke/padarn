using System;

using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace OpenNETCF.Web.UI
{
    public abstract class PageParserFilter
    {
        /// <summary>
        /// Gets the virtual path to the page currently being parsed.
        /// </summary>
        protected abstract string VirtualPath { get; }

        /// <summary>
        /// Gets a value indicating whether a Padarn parser filter permits code on the page. 
        /// </summary>
        /// <remarks>
        /// For Padarn, this is always <b>false</b>.
        /// </remarks>
        public abstract bool AllowCode { get; }

        /// <summary>
        /// Allows the page parser filter to preprocess page directives. 
        /// </summary>
        /// <param name="directiveName"></param>
        /// <param name="attributes"></param>
        /// <remarks>The page parser calls the PreprocessDirective method for each directive encountered during parsing.</remarks>
        public virtual void PreprocessDirective(string directiveName, IDictionary attributes)
        {
        }

        public virtual void ParseComplete(ControlBuilder rootBuilder)
        {
        }
    }
}
