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
using System.Collections.Specialized;
using System.Globalization;
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// Provides name-value pair configuration information from a configuration section.
	/// </summary>
    internal class NameValueSectionHandler : IConfigurationSectionHandler
	{
		private const string defaultKeyAttribute = "key";
		private const string defaultValueAttribute = "value";

		/// <summary>
		/// 
		/// </summary>
		protected virtual string KeyAttributeName
		{
			get { return "key"; }
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual string ValueAttributeName
		{
			get { return "value"; }
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
			return CreateStatic(parent, section, KeyAttributeName, ValueAttributeName);
		}

		internal static object CreateStatic(object parent, XmlNode section)
		{
			return CreateStatic(parent, section, "key", "value");
		}

		internal static object CreateStatic(object parent, XmlNode section, string keyAttriuteName, string valueAttributeName)
		{
			ReadOnlyNameValueCollection result;

			if (parent == null)
			{
                result = new ReadOnlyNameValueCollection(StringComparer.OrdinalIgnoreCase);

			}
			else
			{
				result = new ReadOnlyNameValueCollection((ReadOnlyNameValueCollection)parent);
			}
			HandlerBase.CheckForUnrecognizedAttributes(section);

			foreach(XmlNode child in section.ChildNodes)
			{
				if(HandlerBase.IsIgnorableAlsoCheckForNonElement(child))
					continue;

				if (child.Name == "add")
				{
					string key = HandlerBase.RemoveRequiredAttribute(child, keyAttriuteName);
					string val = HandlerBase.RemoveRequiredAttribute(child, valueAttributeName, true);
					HandlerBase.CheckForUnrecognizedAttributes(child);
					result[key] = val;
				}
				else if (child.Name == "remove")
				{
					string key = HandlerBase.RemoveRequiredAttribute(child, keyAttriuteName);
					HandlerBase.CheckForUnrecognizedAttributes(child);
					result.Remove(key);
				}
				else if (child.Name.Equals("clear"))
				{
					HandlerBase.CheckForUnrecognizedAttributes(child);
					result.Clear();
				}
				else
				{
					HandlerBase.ThrowUnrecognizedElement(child);
				}
			}

			result.SetReadOnly();
			return result;
		}
	}
}
