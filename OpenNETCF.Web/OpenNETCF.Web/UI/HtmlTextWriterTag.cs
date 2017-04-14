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
    /// Specifies the HTML tags that can be passed to an HtmlTextWriter object output stream.
    /// </summary>
    public enum HtmlTextWriterTag
    {
        /// <summary>
        /// The string passed as an HTML tag is not recognized.  
        /// </summary>
        Unknown,
        /// <summary>
        /// The HTML a element.  
        /// </summary>
        A,
        /// <summary>
        /// The HTML address element.  
        /// </summary>
        Address,
        /// <summary>
        /// The HTML area element.  
        /// </summary>
        Area,        
        /// <summary>
        /// The HTML blockquote element.  
        /// </summary>
        Blockquote,        
        /// <summary>
        ///  The HTML body element.  
        /// </summary>
        Body,
        /// <summary>
        /// The HTML br element.  
        /// </summary>
        Br,       
        /// <summary>
        /// The HTML button element.  
        /// </summary>
        Button,
        /// <summary>
        /// The HTML caption element.  
        /// </summary>
        Caption,
        /// <summary>
        /// The HTML code element.  
        /// </summary>
        Code,        
        /// <summary>
        /// The HTML col element.
        /// </summary>
        Col,
        /// <summary>
        /// The HTML colgroup element.  
        /// </summary>  
        Colgroup,        
        /// <summary>
        /// The HTML div element.  
        /// </summary>
        Div,
        /// <summary>
        /// The HTML fieldset element.  
        /// </summary>
        Fieldset,        
        /// <summary>
        /// The HTML form element.  
        /// </summary>
        Form,        
        /// <summary>
        /// The HTML H1 element.  
        /// </summary>
        H1,
        /// <summary>
        /// The HTML H2 element.  
        /// </summary>
        H2,
        /// <summary>
        /// The HTML H3 element.  
        /// </summary>
        H3,
        /// <summary>
        /// The HTML H4 element.  
        /// </summary>
        H4,
        /// <summary>
        /// The HTML H5 element.  
        /// </summary>
        H5,
        /// <summary>
        /// The HTML H6 element.  
        /// </summary>
        H6,
        /// <summary>
        /// The HTML head element.
        /// </summary>
        Head,          
        /// <summary>
        /// The HTML html element.  
        /// </summary>
        Html,
        /// <summary>
        /// The HTML hr element.
        /// </summary>
        Hr,
        /// <summary>
        /// The HTML i element.
        /// </summary>
        I,
        /// <summary>
        /// The HTML iframe element.
        /// </summary>
        Iframe,
        /// <summary>
        /// The HTML img element.  
        /// </summary>
        Img,
        /// <summary>
        /// The HTML input element.  
        /// </summary>
        Input,
        /// <summary>
        /// The HTML label element.
        /// </summary>
        Label,
        /// <summary>
        /// The HTML legend element.  
        /// </summary>
        Legend,
        /// <summary>
        /// The HTML li element.  
        /// </summary>
        Li,
        /// <summary>
        /// The HTML link element.  
        /// </summary>
        Link,
        /// <summary>
        /// The HTML meta element.
        /// </summary>
        Meta,
        /// <summary>
        /// The HTML object element.
        /// </summary>
        Object,
        /// <summary>
        /// The HTML ol element.  
        /// </summary>
        Ol,
        /// <summary>
        /// The HTML option element.  
        /// </summary>
        Option,        
        /// <summary>
        /// The HTML p element.   
        /// </summary>        
        P,
        /// <summary>
        /// The HTML param element.  
        /// </summary>
        Param,
        /// <summary>
        /// The HTML pre element.
        /// </summary>
        Pre,
        /// <summary>
        /// The HTML script element.  
        /// </summary>
        Script,        
        /// <summary>
        /// The HTML select element.  
        /// </summary>
        Select,
        /// <summary>
        /// The HTML span element.  
        /// </summary>
        Span,
        /// <summary>
        /// The HTML strike element.  
        /// </summary>
        Strike,
        /// <summary>
        /// The HTML strong element.  
        /// </summary>
        Strong, 
        /// <summary>
        /// The HTML style element.
        /// </summary>
        Style,        
        /// <summary>
        /// The HTML table element.  
        /// </summary>
        Table,
        /// <summary>
        /// The HTML tbody element.  
        /// </summary>
        Tbody,
        /// <summary>
        /// The HTML td element.  
        /// </summary>
        Td,
        /// <summary>
        /// The HTML textarea element.  
        /// </summary>
        Textarea,        
        /// <summary>
        /// The HTML th element.  
        /// </summary>
        Th,
        /// <summary>
        /// The HTML thead element.  
        /// </summary>
        Thead,
        /// <summary>
        /// The HTML title element.
        /// </summary>
        Title,        
        /// <summary>
        /// The HTML tr element.  
        /// </summary>
        Tr,        
        /// <summary>
        /// The HTML ul element.  
        /// </summary>
        Ul,
        


        //Acronym The HTML acronym element.  
        //B The HTML b element.  
        //Base The HTML base element.  
        //Basefont The HTML basefont element.  
        //Bdo The HTML bdo element.  
        //Bgsound The HTML bgsound element.  
        //Big The HTML big element.  
        //Center The HTML center element.  
        //Cite The HTML cite element.  
        //Dd The HTML dd element.  
        //Del The HTML del element.  
        //Dfn The HTML dfn element.  
        //Dir The HTML dir element.  
        //Dl The HTML dl element.  
        //Dt The HTML dt element.  
        //Em The HTML em element.  
        //Embed The HTML embed element.  
        //Font The HTML font element.  
        //Frame The HTML frame element.  
        //Frameset The HTML frameset element.  
          
        //I The HTML i element.  
        //Ins The HTML ins element.  
        //Isindex The HTML isindex element.  
        //Kbd The HTML kbd element.  
        //Map The HTML map element.  
        //Marquee The HTML marquee element.  
        //Menu The HTML menu element.  
          
        //Nobr The HTML nobr element.  
        //Noframes The HTML noframes element.  
        //Noscript The HTML noscript element.  
        //Q The HTML q element.  
        //Rt The DHTML rt element, which specifies text for the ruby element.  
        //Ruby The DHTML ruby element.  
        //S The HTML s element.  
        //Samp The HTML samp element.  
        //Small The HTML small element.  
          
        //Sub The HTML sub element.  
        //Sup The HTML sup element.  
        //Tfoot The HTML tfoot element.  
          
        //Tt The HTML tt element.  
        //U The HTML u element.  
        //Var The HTML var element.  
        //Wbr The HTML wbr element.  
        //Xml 
    }
}
