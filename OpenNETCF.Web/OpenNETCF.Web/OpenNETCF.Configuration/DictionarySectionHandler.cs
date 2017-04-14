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
using System.Collections;
using System.Globalization;
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// Reads key-value pair configuration information for a configuration section.
	/// </summary>
	/// <example>
	/// <code>
	/// &lt;add key="name" value="text"> - sets key=text
	/// &lt;remove key="name"> - removes the definition of key
	/// &lt;clear/> - removes all definitions
	/// </code>
	/// </example>
    internal class DictionarySectionHandler : IConfigurationSectionHandler
	{
		/// <summary>
		/// Make the name of the key attribute configurable by derived classes.
		/// </summary>
		protected virtual string KeyAttributeName
		{
			get { return "key"; }
		}

		/// <summary>
		/// Make the name of the value attribute configurable by derived classes.
		/// </summary>
		protected virtual string ValueAttributeName
		{
			get { return "value"; }
		}

		internal virtual bool ValueRequired
		{
			get { return false; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="context"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public virtual object Create(object parent, object context, XmlNode section)
		{
			Hashtable result;

			// Create a shallow clone of the parent
			if (parent == null)
			{
                result = new Hashtable(StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				result = (Hashtable)((Hashtable)parent).Clone();
			}
			// Process XML
			HandlerBase.CheckForUnrecognizedAttributes(section);

			foreach(XmlNode child in section.ChildNodes)
			{
				// Skip whitespace and comments; throw exception if non-element
				if(HandlerBase.IsIgnorableAlsoCheckForNonElement(child))
				{
					continue;
				}

				// Handle <add>, <remove>, and <clear> tags
				if(child.Name == "add")
				{
					string key = HandlerBase.RemoveRequiredAttribute(child, KeyAttributeName);
					string value = HandlerBase.RemoveAttribute(child, ValueAttributeName);
					HandlerBase.CheckForUnrecognizedAttributes(child);

					if(value == null)
					{
						value = string.Empty;
					}

					result[key] = value;
				}
				else if(child.Name == "remove")
				{
					string key = HandlerBase.RemoveRequiredAttribute(child, KeyAttributeName);
					HandlerBase.CheckForUnrecognizedAttributes(child);

					result.Remove(key);
				}
				else if(child.Name == "clear")
				{
					HandlerBase.CheckForUnrecognizedAttributes(child);
					result.Clear();
				}
				else
				{
					HandlerBase.ThrowUnrecognizedElement(child);
				}
			}
			return result;
		}
	}
}

