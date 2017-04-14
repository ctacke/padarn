using System;
using System.Collections.Generic;
using OpenNETCF.Web;

namespace OpenNETCF.Web.UI
{
    /// <summary>
    /// Provides extension methods for the HtmlTextWriterClass.
    /// </summary>
    public static class HtmlTextWriterExtensions
    {
        /// <summary>
        /// Renders a start tag, the tag is closed by a call to the EndTag-method.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="tag">The tag to render the start tag of.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Tag(this HtmlTextWriter writer, HtmlTextWriterTag tag)
        {
            writer.RenderBeginTag(tag);

            return writer;
        }

        public static HtmlTextWriter Tag(this HtmlTextWriter writer, bool condition, HtmlTextWriterTag tag)
        {
            if (!condition) return writer;

            writer.RenderBeginTag(tag);

            return writer;
        }

        /// <summary>
        /// Renders a start tag, the tag is closed by a call to the EndTag-method.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="tag">The tag to render the start tag of.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Tag(this HtmlTextWriter writer, string tag)
        {
            writer.RenderBeginTag(tag);

            return writer;
        }

        /// <summary>
        /// Renders a start tag, the tag is closed by a call to the EndTag-method.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="tag">The tag to render the start tag of.</param>
        /// <param name="appender">A delegate that takes in an HtmlAttributeManager for appending
        /// attributes to the start tag.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Tag(this HtmlTextWriter writer, HtmlTextWriterTag tag, Func<HtmlAttributeManager, HtmlAttributeManager> appender)
        {
            Validate.Begin()
                .IsNotNull(appender)
                .Check();

            var manager = new HtmlAttributeManager(writer);
            appender(manager);

            writer.RenderBeginTag(tag);

            return writer;
        }

        public static HtmlTextWriter Tag(this HtmlTextWriter writer, bool condition, HtmlTextWriterTag tag, Func<HtmlAttributeManager, HtmlAttributeManager> appender)
        {
            if (!condition) return writer;

            Validate.Begin()
                .IsNotNull(appender)
                .Check();

            var manager = new HtmlAttributeManager(writer);
            appender(manager);

            writer.RenderBeginTag(tag);

            return writer;
        }

        /// <summary>
        /// Renders a start tag, the tag is closed by a call to the EndTag-method.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="tag">The tag to render the start tag of.</param>
        /// <param name="appender">A delegate that takes in an HtmlAttributeManager for appending
        /// attributes to the start tag.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Tag(this HtmlTextWriter writer, string tag, Func<HtmlAttributeManager, HtmlAttributeManager> appender)
        {
            Validate.Begin()
                .IsNotNull(appender)
                .Check();

            var manager = new HtmlAttributeManager(writer);
            appender(manager);

            writer.RenderBeginTag(tag);

            return writer;
        }

        /// <summary>
        /// Renders a Div start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Div(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Div);
        }
        
        /// <summary>
        /// Renders a Div start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="appender">A delegate that takes in an HtmlAttributeManager for appending
        /// attributes to the start tag.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Div(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Div, attributes);
        }

        /// <summary>
        /// Renders a Body start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Body(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Body);
        }

        /// <summary>
        /// Renders a Body start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="appender">A delegate that takes in an HtmlAttributeManager for appending
        /// attributes to the start tag.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Body(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Body, attributes);
        }

        /// <summary>
        /// Renders a Html start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Html(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Html);
        }

        /// <summary>
        /// Renders a Html start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="appender">A delegate that takes in an HtmlAttributeManager for appending
        /// attributes to the start tag.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Html(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Html, attributes);
        }

        /// <summary>
        /// Renders a Span start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Span(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Span);
        }

        /// <summary>
        /// Renders a Span start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="attributes">A delegate that takes in an HtmlAttributeManager for appending
        /// attributes to the start tag.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Span(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Span, attributes);
        }

        /// <summary>
        /// Renders an anchor tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="href">The url of the hyperlink.</param>
        /// <param name="title">The title of the hyperlink.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter A(this HtmlTextWriter writer, string href, string title)
        {
            Validate.Begin()
                .IsNotNullOrEmpty(href)
                .IsNotNullOrEmpty(title)
                .Check();

            return writer.Tag(HtmlTextWriterTag.A, e => e[HtmlTextWriterAttribute.Href, href][HtmlTextWriterAttribute.Title, title]);
        }

        /// <summary>
        /// Renders an anchor tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter A(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.A);
        }

        /// <summary>
        /// Renders an &lt;i&gt; tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter I(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.I, attributes);
        }

        /// <summary>
        /// Closes the latest started tag.
        /// </summary>
        /// <param name="writer">The writer to render the end tag to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter EndTag(this HtmlTextWriter writer)
        {
            writer.RenderEndTag();
            return writer;
        }

        /// <summary>
        /// Closes the latest started tag.
        /// </summary>
        /// <param name="writer">The writer to render the end tag to.</param>
        /// <param name="ignored">The tag this call closes, only specified for readability,
        /// this parameter is ignored.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter EndTag(this HtmlTextWriter writer, HtmlTextWriterTag ignored)
        {
            return writer.EndTag();
        }

        /// <summary>
        /// Renders a text literal to the writer.
        /// </summary>
        /// <param name="writer">The writer to render the text to.</param>
        /// <param name="text">The text to render.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Text(this HtmlTextWriter writer, string text)
        {
            return writer.Text(text, false);
        }

        /// <summary>
        /// Renders a text literal to the writer.
        /// </summary>
        /// <param name="writer">The writer to render the text to.</param>
        /// <param name="text">The text to render.</param>
        /// <param name="htmlEncode">If set to true the text will be html-encoded.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Text(this HtmlTextWriter writer, string text, bool htmlEncode)
        {
            if (htmlEncode)
            {
                writer.Write(HttpUtility.HtmlEncode(text));
            }
            else
            {
                writer.Write(text);
            }

            return writer;
        }

        /// <summary>
        /// Renders a text literal to the writer.
        /// </summary>
        /// <param name="writer">The writer to render the text to.</param>
        /// <param name="value">An object that represents the text to be written.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Text(this HtmlTextWriter writer, object value)
        {
            if (value != null)
            {
                IFormattable formattable = value as IFormattable;
                if (formattable != null)
                {
                    writer.Text(formattable.ToString(null, null));
                }
                else
                {
                        writer.Text(value.ToString());
                }
            }

            return writer;
        }

        public static HtmlTextWriter Label(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer
                .Tag(HtmlTextWriterTag.Label, attributes);
        }

        public static HtmlTextWriter Label(this HtmlTextWriter writer)
        {
            return writer
                .Tag(HtmlTextWriterTag.Label);
        }

        /// <summary>
        /// Repeats over the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="collection">The collection to repeat over.</param>
        /// <param name="binder">A function that will be called for each of the elements
        /// in the collection, the first parameter is the item in the collection, the second
        /// parameter the index of the item in the collection, and the third is the writer
        /// to render to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Bind<T>(this HtmlTextWriter writer,
            IEnumerable<T> collection, Func<T, int, HtmlTextWriter, HtmlTextWriter> binder)
        {
            Validate.Begin()
                .IsNotNull(collection)
                .IsNotNull(binder)
                .Check();

            int index = 0;
            foreach (var item in collection)
            {
                binder(item, index++, writer);
            }

            return writer;
        }

        /// <summary>
        /// Repeats the specified number of times.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="times">The number of times to repeat.</param>
        /// <param name="binder">A function that will be called the specified number of times,
        /// the first parameter is the number of the call (starting with one), the
        /// second parameter is the writer to render to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Repeat(this HtmlTextWriter writer, int times, 
            Func<int, HtmlTextWriter, HtmlTextWriter> binder)
        {
            Validate.Begin()
                .IsNotNull(binder)
                .Check();

            if (times < 0) throw new ArgumentOutOfRangeException("times");

            for (var i = 1; i <= times; i++)
            {
                binder(i, writer);
            }

            return writer;
        }

        /// <summary>
        /// Renders a title tag
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static HtmlTextWriter Title(this HtmlTextWriter writer, string title)
        {
            return writer
                .Tag(HtmlTextWriterTag.Title)
                .Text(title)
                .EndTag();
        }

        public static HtmlTextWriter JavascriptImport(this HtmlTextWriter writer, string source)
        {
            return writer
                    .Tag(HtmlTextWriterTag.Script, t => t
                        [HtmlTextWriterAttribute.Src, source]
                        [HtmlTextWriterAttribute.Type, "text/javascript"])
                    .Text(" ")
                    .EndTag();
        }

        public static HtmlTextWriter StylesheetLink(this HtmlTextWriter writer, string linkHref)
        {
            return writer
                    .Tag(HtmlTextWriterTag.Link, a => a
                        [HtmlTextWriterAttribute.Rel, "stylesheet"]
                        [HtmlTextWriterAttribute.Type, "text/css"]
                        [HtmlTextWriterAttribute.Href, linkHref])
                    .EndTag();
        }

        private static class DoctypeTags
        {
            public const string HTML4 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">";
            public const string XHTML1 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
        }

        public static HtmlTextWriter DocType(this HtmlTextWriter writer, DocumentType docType)
        {
            switch(docType)
            {
                case DocumentType.HTML401Transitional:
                    writer.Write(DoctypeTags.HTML4);
                    break;
                case DocumentType.XHTML10Transitional:
                    writer.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    writer.Write(DoctypeTags.XHTML1);
                    break;
            }

            return writer;
        }

        /// <summary>
        /// Renders a Head start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Head(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Head);
        }

        /// <summary>
        /// Renders a P start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter P(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.P);
        }

        public static HtmlTextWriter P(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.P, attributes);
        }

        public static HtmlTextWriter Strong(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Strong);
        }

        public static HtmlTextWriter Strong(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Strong, attributes);
        }

        /// <summary>
        /// Renders a td start tag.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <returns>The writer.</returns>
        public static HtmlTextWriter Td(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Td);
        }

        public static HtmlTextWriter Td(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Td, attributes);
        }

        /// <summary>
        /// Renders a complete TD element (start and end tag) with the specified text
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static HtmlTextWriter Td(this HtmlTextWriter writer, string text)
        {
            return writer
                .Tag(HtmlTextWriterTag.Td)
                .Text(text)
                .EndTag();
        }

        public static HtmlTextWriter H4(this HtmlTextWriter writer, string text)
        {
            return writer
                .Tag(HtmlTextWriterTag.H4)
                .Text(text);
        }

        public static HtmlTextWriter H4(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer
                .Tag(HtmlTextWriterTag.H4, attributes);
        }

        public static HtmlTextWriter H4(this HtmlTextWriter writer)
        {
            return writer
                .Tag(HtmlTextWriterTag.H4);
        }

        public static HtmlTextWriter H3(this HtmlTextWriter writer, string text)
        {
            return writer
                .Tag(HtmlTextWriterTag.H3)
                .Text(text);
        }

        public static HtmlTextWriter H3(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer
                .Tag(HtmlTextWriterTag.H3, attributes);
        }

        public static HtmlTextWriter H3(this HtmlTextWriter writer)
        {
            return writer
                .Tag(HtmlTextWriterTag.H3);
        }

        /// <summary>
        /// Renders a complete H2 element (start and end tag) with the specified text
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static HtmlTextWriter H2(this HtmlTextWriter writer, string text)
        {
            return writer
                .Tag(HtmlTextWriterTag.H2)
                .Text(text);
        }

        public static HtmlTextWriter H1(this HtmlTextWriter writer)
        {
            return writer
                .Tag(HtmlTextWriterTag.H1);
        }

        public static HtmlTextWriter H1(this HtmlTextWriter writer, string text)
        {
            return writer
                .Tag(HtmlTextWriterTag.H1)
                .Text(text);
        }

        public static HtmlTextWriter H1(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer
                .Tag(HtmlTextWriterTag.H1, attributes);
        }

        public static HtmlTextWriter SubmitButton(this HtmlTextWriter writer, string name, string value)
        {
            return writer
                        .Tag(HtmlTextWriterTag.Input, t => t
                            [HtmlTextWriterAttribute.Type, "submit"]
                            [HtmlTextWriterAttribute.Name, name]
                            [HtmlTextWriterAttribute.Value, value])
                        .EndTag(); // input
        }

        public static HtmlTextWriter SubmitButton(this HtmlTextWriter writer, string id, string name, string value)
        {
            return writer
                        .Tag(HtmlTextWriterTag.Input, t => t
                            [HtmlTextWriterAttribute.Type, "submit"]
                            [HtmlTextWriterAttribute.Name, name]
                            [HtmlTextWriterAttribute.Id, id]
                            [HtmlTextWriterAttribute.Value, value])
                        .EndTag(); // input
        }

        public static HtmlTextWriter Hidden(this HtmlTextWriter writer, string name, string value)
        {
            return writer
                        .Tag(HtmlTextWriterTag.Input, t => t
                            [HtmlTextWriterAttribute.Type, "hidden"]
                            [HtmlTextWriterAttribute.Name, name]
                            [HtmlTextWriterAttribute.Value, value])
                        .EndTag(); // input
        }

        public static HtmlTextWriter Tr(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Tr);
        }

        public static HtmlTextWriter Tr(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Tr, attributes);
        }

        public static HtmlTextWriter Br(this HtmlTextWriter writer)
        {
            writer.WriteBreak();
            return writer;
        }

        public static HtmlTextWriter Option(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Option);
        }

        public static HtmlTextWriter Option(this HtmlTextWriter writer, string name, string value, string text)
        {
            return writer
                .Tag(HtmlTextWriterTag.Option, t => t
                    [HtmlTextWriterAttribute.Value, value]
                    [HtmlTextWriterAttribute.Name, name])
                .Text(text);
        }

        public static HtmlTextWriter Option(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Option, attributes);
        }

        public static HtmlTextWriter Thead(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Thead);
        }

        public static HtmlTextWriter Tbody(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Tbody);
        }

        public static HtmlTextWriter Tbody(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Tbody, attributes);
        }

        public static HtmlTextWriter Table(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Table);
        }

        public static HtmlTextWriter Table(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Table, attributes);
        }

        public static HtmlTextWriter Colgroup(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Colgroup, attributes);
        }

        public static HtmlTextWriter Colgroup(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Colgroup);
        }

        public static HtmlTextWriter Col(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Col, attributes);
        }

        public static HtmlTextWriter Ul(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Ul, attributes);
        }

        public static HtmlTextWriter Ul(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Ul);
        }

        public static HtmlTextWriter Fieldset(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Fieldset, attributes);
        }

        public static HtmlTextWriter Fieldset(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Fieldset);
        }

        public static HtmlTextWriter Legend(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Legend, attributes);
        }

        public static HtmlTextWriter Legend(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Legend);
        }

        public static HtmlTextWriter Li(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Li, attributes);
        }

        public static HtmlTextWriter Li(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Li);
        }

        public static HtmlTextWriter Script(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Script, attributes);
        }

        public static HtmlTextWriter JavaScript(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Script, t=>t[HtmlTextWriterAttribute.Type, "text/javascript"]);
        }

        public static HtmlTextWriter Form(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Form);
        }

        public static HtmlTextWriter Form(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Form, attributes);
        }

        public static HtmlTextWriter Th(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Th);
        }

        public static HtmlTextWriter Th(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Th, attributes);
        }

        public static HtmlTextWriter A(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.A, attributes);
        }

        public static HtmlTextWriter Select(this HtmlTextWriter writer)
        {
            return writer.Tag(HtmlTextWriterTag.Select);
        }

        public static HtmlTextWriter Select(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Select, attributes);
        }

        public static HtmlTextWriter Button(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Button, attributes);
        }

        public static HtmlTextWriter Input(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Input, attributes);
        }

        public static HtmlTextWriter TextArea(this HtmlTextWriter writer, string name, string id, int rows, int columns)
        {
            return writer.Tag(HtmlTextWriterTag.Textarea, t => t
                [HtmlTextWriterAttribute.Name, name]
                [HtmlTextWriterAttribute.Id, id]
                [HtmlTextWriterAttribute.Rows, rows.ToString()]
                [HtmlTextWriterAttribute.Cols, columns.ToString()]);
        }
        public static HtmlTextWriter TextArea(this HtmlTextWriter writer, string id, int rows, int columns)
        {
            return writer.Tag(HtmlTextWriterTag.Textarea, t=>t
                [HtmlTextWriterAttribute.Id, id]
                [HtmlTextWriterAttribute.Rows, rows.ToString()]
                [HtmlTextWriterAttribute.Cols, columns.ToString()]);
        }

        public static HtmlTextWriter Meta(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Meta, attributes);
        }

        public static HtmlTextWriter Img(this HtmlTextWriter writer, Func<HtmlAttributeManager, HtmlAttributeManager> attributes)
        {
            return writer.Tag(HtmlTextWriterTag.Img, attributes);
        }

        public static HtmlTextWriter ValidatedXhtmlParagraph(this HtmlTextWriter writer)
        {
            return writer
                .P()
                .A("http://validator.w3.org/check?uri=referer", "Validated XHTML")
                .Img(t => t[HtmlTextWriterAttribute.Src, "http://www.w3.org/Icons/valid-xhtml10"]
                    [HtmlTextWriterAttribute.Alt, "Valid XHTML 1.0 Transitional"]
                    [HtmlTextWriterAttribute.Height, "31"]
                    [HtmlTextWriterAttribute.Width, "88"])
                .EndTag() // img
                .EndTag() // href
                .EndTag(); //p

        }
    }
}
