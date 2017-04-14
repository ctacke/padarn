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
using OpenNETCF.Web.UI;

namespace OpenNETCF.Web.Core
{
    internal static class Extensions
    {
        public static bool Implements<T>(this Type t) where T:class
        {
            foreach (Type type in t.GetInterfaces())
            {
                if (type == typeof(T)) return true;
            }

            return false;
        }

        public static string AsText(this HtmlTextWriterTag tag)
        {
            return tag.ToString().ToLower();
            //switch (tag)
            //{
            //    case HtmlTextWriterTag.A: return "a";
            //    case HtmlTextWriterTag.Body: return "body";
            //    case HtmlTextWriterTag.Br: return "br";
            //    case HtmlTextWriterTag.Button: return "button";
            //    case HtmlTextWriterTag.Caption: return "caption";
            //    case HtmlTextWriterTag.Col: return "col";
            //    case HtmlTextWriterTag.Colgroup: return "colgroup";
            //    case HtmlTextWriterTag.Div: return "div";
            //    case HtmlTextWriterTag.Form: return "form";
            //    case HtmlTextWriterTag.H1: return "h1";
            //    case HtmlTextWriterTag.H2: return "h2";
            //    case HtmlTextWriterTag.H3: return "h3";
            //    case HtmlTextWriterTag.H4: return "h4";
            //    case HtmlTextWriterTag.H5: return "h5";
            //    case HtmlTextWriterTag.H6: return "h6";
            //    case HtmlTextWriterTag.Head: return "head";
            //    case HtmlTextWriterTag.Html: return "html";
            //    case HtmlTextWriterTag.Img: return "img";
            //    case HtmlTextWriterTag.Input: return "input";
            //    case HtmlTextWriterTag.Li: return "li";
            //    case HtmlTextWriterTag.Link: return "link";
            //    case HtmlTextWriterTag.P: return "p";
            //    case HtmlTextWriterTag.Span: return "span";
            //    case HtmlTextWriterTag.Table: return "table";
            //    case HtmlTextWriterTag.Tbody: return "tbody";
            //    case HtmlTextWriterTag.Td: return "td";
            //    case HtmlTextWriterTag.Th: return "th";
            //    case HtmlTextWriterTag.Thead: return "thead";
            //    case HtmlTextWriterTag.Tr: return "tr";
            //    case HtmlTextWriterTag.Ul: return "ul";
            //    case HtmlTextWriterTag.Unknown: return "unknown";

            //    default: throw new NotSupportedException("Unsupported HtmlTextWriterTag.  Use a string instead");
            //}
        }

        public static string AsText(this HtmlTextWriterAttribute attrib)
        {
            return attrib.ToString().ToLower();
            //switch (attrib)
            //{
            //    case HtmlTextWriterAttribute.Class: return "class";
            //    case HtmlTextWriterAttribute.Href: return "href";
            //    case HtmlTextWriterAttribute.Id: return "id";
            //    case HtmlTextWriterAttribute.Name: return "name";
            //    case HtmlTextWriterAttribute.Rel: return "rel";
            //    case HtmlTextWriterAttribute.Src: return "src";
            //    case HtmlTextWriterAttribute.Type: return "type";

            //    default: throw new NotSupportedException("Unsupported HtmlTextWriterAttribute.  Use a string instead");
            //}
        }
    }
}
