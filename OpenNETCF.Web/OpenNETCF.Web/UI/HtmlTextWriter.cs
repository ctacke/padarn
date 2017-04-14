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
using System.IO;
using OpenNETCF.Web.Core;
using System.Net;

namespace OpenNETCF.Web.UI
{
    internal class HtmlElement
    {
        private string m_contents;
        internal string TagName { get; private set; }
        internal bool IsEmpty { get; set; }
        internal bool StartWritten { get; set; }
        internal bool ContentsWritten { get; set; }
        internal Dictionary<string, string> Attributes { get; private set; }

        internal HtmlElement(string tagName)
        {
            TagName = tagName;
            IsEmpty = true;
            StartWritten = false;
            ContentsWritten = false;

            Attributes = new Dictionary<string, string>();
        }

        internal string StartTag 
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<");
                sb.Append(TagName);

                foreach (var attrib in Attributes)
                {
                    if (attrib.Value == null)
                    {
                        sb.Append(string.Format(" {0}", attrib.Key));
                    }
                    else
                    {
                        sb.Append(string.Format(" {0}=\"{1}\"", attrib.Key, attrib.Value));
                    }
                }

                //if(IsEmpty)
                //{
                //    sb.Append(" /");
                //}
                sb.Append(">");

                return sb.ToString();
            }
        }

        internal string EndTag 
        {
            get
            {
//                if (IsEmpty) return string.Empty;

                return string.Format("</{0}>", TagName);                    
            }
        }

        internal string Contents
        {
            get { return m_contents; }
            set
            {
                m_contents = value;
                IsEmpty = (string.IsNullOrEmpty(Contents));
                ContentsWritten = false;
            }
        }
    }

    /// <summary>
    /// Writes markup characters and text to a Padarn server control output stream. This class provides formatting capabilities that Padarn pages use when rendering markup to clients.
    /// </summary>
    public class HtmlTextWriter : TextWriter
    {
        private TextWriter m_writer;
        private Stack<HtmlElement> m_elementStack = new Stack<HtmlElement>();
        private Dictionary<string, string> m_NextElementAttributes = new Dictionary<string, string>();

        private HtmlElement m_currentElement;
        private int m_indentLevel = 0;
        private string m_tabString = new string(' ', 2);

        /// <summary>
        /// Initializes a new instance of the HtmlTextWriter class that uses a default tab string.
        /// </summary>
        /// <param name="writer"></param>
        public HtmlTextWriter(TextWriter writer)
        {
            m_writer = writer;
            m_currentElement = null;
        }

        /// <summary>
        /// Writes the opening tag of the markup element associated with the specified HtmlTextWriterTag enumeration value to the output stream.
        /// </summary>
        /// <param name="tagKey"></param>
        public virtual void RenderBeginTag(HtmlTextWriterTag tagKey)
        {
            string tag = tagKey.AsText();
            RenderBeginTag(tag);
        }

        /// <summary>
        /// Writes the opening tag of the specified markup element to the output stream.
        /// </summary>
        /// <param name="tagName"></param>
        public virtual void RenderBeginTag(string tagName)
        {
            if (m_currentElement != null)
            {
                // we called Begin while another element is open
                m_currentElement.IsEmpty = false;
                if (!m_currentElement.StartWritten)
                {
                    m_writer.Write("\r\n");
                    // indent
                    for (int i = 0; i < m_indentLevel; i++)
                    {
                        m_writer.Write(m_tabString);
                    }

                    m_writer.Write(m_currentElement.StartTag);
                    m_currentElement.StartWritten = true;
                }

                if (!m_currentElement.ContentsWritten)
                {
                    if (!string.IsNullOrEmpty(m_currentElement.Contents))
                    {
                        m_writer.Write(m_currentElement.Contents);
                        m_currentElement.ContentsWritten = true;
                        m_currentElement.Contents = null;
                    }
                }

                m_elementStack.Push(m_currentElement);
            }
            m_currentElement = new HtmlElement(tagName);

            foreach (var attrib in m_NextElementAttributes)
            {
                m_currentElement.Attributes.Add(attrib.Key, attrib.Value);
            }
            m_NextElementAttributes.Clear();
            m_indentLevel++;
        }

        /// <summary>
        /// Writes the end tag of a markup element to the output stream.
        /// </summary>
        public virtual void RenderEndTag()
        {
            if (m_currentElement == null) throw new WebException("RenderEndTag called when no open tag exists.");

            if (!m_currentElement.StartWritten)
            {
                // indent
                for (int i = 0; i < m_indentLevel; i++)
                {
                    m_writer.Write(m_tabString);
                }

                m_writer.Write(m_currentElement.StartTag);
            }
            if (!m_currentElement.ContentsWritten)
            {
                m_writer.Write(m_currentElement.Contents);
            }

            m_writer.Write(m_currentElement.EndTag);
            m_writer.Write("\r\n");

            if (m_elementStack.Count > 0)
            {
                m_currentElement = m_elementStack.Pop();
            }
            else
            {
                m_currentElement = null;
            }
            m_NextElementAttributes.Clear();
            m_indentLevel--;
        }

        /// <summary>
        /// Adds the specified markup attribute and value to the opening tag of the element that the HtmlTextWriter object creates with a subsequent call to the RenderBeginTag method.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void AddAttribute(HtmlTextWriterAttribute key, string value)
        {
            AddAttribute(key.AsText(), value);
        }

        /// <summary>
        /// Adds the specified markup attribute and value to the opening tag of the element that the HtmlTextWriter object creates with a subsequent call to the RenderBeginTag method.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public virtual void AddAttribute(string name, string value)
        {
            if (m_NextElementAttributes.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("Attribute '{0}' already exists for the current tag"));
            }

            m_NextElementAttributes.Add(name, value);
        }

        /// <summary>
        /// Writes the specified string to the output stream, along with any pending tab spacing.
        /// </summary>
        /// <param name="value"></param>
        public override void Write(string value)
        {
            if (m_currentElement == null)
            {
                m_writer.Write(value);
                return;
            }

            m_currentElement.Contents += value;
        }

        internal void WriteRaw(string value)
        {
            if ((m_currentElement == null) || (string.IsNullOrEmpty(m_currentElement.Contents)))
            {
                m_writer.Write(value);
            }
            else
            {
                m_currentElement.Contents += value;
            }
        }

        /// <summary>
        /// Writes the text representation of a Boolean value to the output stream, along with any pending tab spacing.
        /// </summary>
        /// <param name="value"></param>
        public override void Write(bool value)
        {
            Write(value.ToString());
        }

        /// <summary>
        /// Writes the text representation of an array of Unicode characters to the output stream, along with any pending tab spacing.
        /// </summary>
        /// <param name="buffer"></param>
        public override void Write(char[] buffer)
        {
            Write(new string(buffer));
        }

        /// <summary>
        /// Writes the text representation of a decimal value to the text string or stream.
        /// </summary>
        /// <param name="value"></param>
        public override void Write(decimal value)
        {
            Write(value.ToString());
        }

        /// <summary>
        /// Writes the text representation of a double-precision floating-point number to the output stream, along with any pending tab spacing. 
        /// </summary>
        /// <param name="value"></param>
        public override void Write(double value)
        {
            Write(value.ToString());
        }

        /// <summary>
        /// Writes the text representation of a single-precision floating-point number to the output stream, along with any pending tab spacing.
        /// </summary>
        /// <param name="value"></param>
        public override void Write(float value)
        {
            Write(value.ToString());
        }

        /// <summary>
        /// Writes the text representation of a 32-byte signed integer to the output stream, along with any pending tab spacing.
        /// </summary>
        /// <param name="value"></param>
        public override void Write(int value)
        {
            Write(value.ToString());
        }

        /// <summary>
        /// Writes the text representation of a 64-byte signed integer to the output stream, along with any pending tab spacing.
        /// </summary>
        /// <param name="value"></param>
        public override void Write(long value)
        {
            Write(value.ToString());
        }

        /// <summary>
        /// Writes the text representation of an object to the output stream, along with any pending tab spacing.
        /// </summary>
        /// <param name="value"></param>
        public override void Write(object value)
        {
            Write(value.ToString());
        }

        /// <summary>
        /// Writes the text representation of a Unicode character to the output stream, along with any pending tab spacing.
        /// </summary>
        /// <param name="value"></param>
        public override void Write(char value)
        {
            string s = new string(new char[] { value });
            Write(s);
        }

        /// <summary>
        /// Writes a <br /> markup element to the output stream. 
        /// </summary>
        public void WriteBreak()
        {
            Write("<br />");
        }

        /// <summary>
        /// Writes any tab spacing and the closing tag of the specified markup element.
        /// </summary>
        public void WriteEndTag(string tagName)
        {
            Write(string.Format("</{0}>", tagName));
        }

        /// <summary>
        /// Gets the encoding that the HtmlTextWriter object uses to write content to the page. 
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
