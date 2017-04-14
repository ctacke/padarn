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

namespace OpenNETCF.Web.UI
{
    /// <summary>
    /// Specifies the HTML attributes that an HtmlTextWriter or Html32TextWriter object writes to the opening tag of an HTML element when a Web request is processed.
    /// </summary>
    public enum HtmlTextWriterAttribute
    {
        /// <summary>
        /// Specifies that the HTML align attribute be written to the tag.  
        /// </summary>
        Align,        
        /// <summary>
        /// Specifies that the HTML alt attribute be written to the tag. 
        /// </summary>
        Alt,
        /// <summary>
        /// Specifies that the HTML border attribute be written to the tag. 
        /// </summary>
        Border,        
        /// <summary>
        /// Specifies that the HTML checked attribute be written to the tag. 
        /// </summary>
        Checked,
        /// <summary>
        /// Specifies that the HTML class attribute be written to the tag. 
        /// </summary>
        Class,
        /// <summary>
        /// Specifies that the HTML cols attribute be written to the tag. 
        /// </summary>
        Cols,
        /// <summary>
        /// Specifies that the HTML colspan attribute be written to the tag. 
        /// </summary>
        Colspan,
        /// <summary>
        /// Specifies that the HTML content attribute be written to the tag. 
        /// </summary>
        Content,        
        /// <summary>
        /// Specifies that the HTML disabled attribute be written to the tag. 
        /// </summary>
        Disabled,
        /// <summary>
        /// Specifies that the HTML height attribute be written to the tag. 
        /// </summary>
        Height,
        /// <summary>
        /// Specifies that the HTML href attribute be written to the tag.
        /// </summary>
        Href,
        /// <summary>
        /// Specifies that the HTML id attribute be written to the tag. 
        /// </summary>
        Id,
        /// <summary>
        /// Specifies that the HTML maxlength attribute be written to the tag. 
        /// </summary>
        Maxlength,        
        /// <summary>
        /// Specifies that the HTML multiple attribute be written to the tag. 
        /// </summary>
        Multiple,        
        /// <summary>
        /// Specifies that the HTML name attribute be written to the tag. 
        /// </summary>
        Name,
        /// <summary>
        /// Specifies that the HTML nowrap attribute be written to the tag. 
        /// </summary>
        Nowrap,        
        /// <summary>
        /// Specifies that the HTML onchange attribute be written to the tag. 
        /// </summary>
        Onchange,
        /// <summary>
        /// Specifies that the HTML onclick attribute be written to the tag. 
        /// </summary>    
        Onclick,
        /// <summary>
        /// Specifies that the HTML readonly attribute be written to the tag.
        /// </summary>
        ReadOnly,        
        /// <summary>
        /// Specifies that the HTML rel attribute be written to the tag. 
        /// </summary>
        Rel,
        /// <summary>
        /// Specifies that the HTML rows attribute be written to the tag. 
        /// </summary>
        Rows,
        /// <summary>
        /// Specifies that the HTML rowspan attribute be written to the tag. 
        /// </summary>
        Rowspan,
        /// <summary>
        /// Specifies that the HTML rules attribute be written to the tag. 
        /// </summary>
        Rules,        
        /// <summary>
        /// Specifies that the HTML scope attribute be written to the tag. 
        /// </summary>
        Scope,
        /// <summary>
        /// Specifies that the HTML selected attribute be written to the tag. 
        /// </summary>
        Selected,
        /// <summary>
        /// Specifies that the HTML size attribute be written to the tag. 
        /// </summary>
        Size,        
        /// <summary>
        /// Specifies that the HTML src attribute be written to the tag.
        /// </summary>
        Src,
        /// <summary>
        /// Specifies that the HTML style attribute be written to the tag. 
        /// </summary>
        Style,
        /// <summary>
        /// Specifies that the HTML tabindex attribute be written to the tag. 
        /// </summary>
        Tabindex,
        /// <summary>
        /// Specifies that the HTML target attribute be written to the tag. 
        /// </summary>
        Target,
        /// <summary>
        /// Specifies that the HTML title attribute be written to the tag. 
        /// </summary>
        Title,        
        /// <summary>
        /// Specifies that the HTML type attribute be written to the tag. 
        /// </summary>
        Type,
        /// <summary>
        /// Specifies that the HTML value attribute be written to the tag. 
        /// </summary>
        Value,
        /// <summary>
        /// Specifies that the HTML width attribute be written to the tag. 
        /// </summary>
        Width,
        /// <summary>
        /// Specifies that the HTML wrap attribute be written to the tag. 
        /// </summary>
        Wrap,



        //Accesskey Specifies that the HTML accesskey attribute be written to the tag.  
        //Background Specifies that the HTML background attribute be written to the tag. 
        //Bgcolor Specifies that the HTML bgcolor attribute be written to the tag. 
        //Bordercolor Specifies that the HTML bordercolor attribute be written to the tag. 
        //Cellpadding Specifies that the HTML cellpadding attribute be written to the tag. 
        //Cellspacing Specifies that the HTML cellspacing attribute be written to the tag. 
        //For Specifies that the HTML for attribute be written to the tag. 
        //Valign Specifies that the HTML valign attribute be written to the tag. 
        //Abbr Specifies that the HTML abbr attribute be written to the tag. 
        //AutoComplete Specifies that the HTML autocomplete attribute be written to the tag. 
        //Axis Specifies that the HTML axis attribute be written to the tag. 
        //Coords Specifies that the HTML coords attribute be written to the tag. 
        //DesignerRegion Specifies that the HTML designerregion attribute be written to the tag. 
        //Dir Specifies that the HTML dir attribute be written to the tag. 
        //Headers Specifies that the HTML headers attribute be written to the tag. 
        //Longdesc Specifies that the HTML longdesc attribute be written to the tag. 
        //Shape Specifies that the HTML shape attribute be written to the tag. 
        //Usemap Specifies that the HTML usemap attribute be written to the tag. 
        //VCardName Specifies that the HTML vcardname attribute be written to the tag. 
    }
}
