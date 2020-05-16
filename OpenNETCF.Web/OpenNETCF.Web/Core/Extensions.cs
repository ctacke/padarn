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
