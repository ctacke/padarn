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
