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
using System.IO;
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// 
	/// </summary>
    internal class NameValueFileSectionHandler : IConfigurationSectionHandler
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public virtual object Create(object parent, object configContext, XmlNode section)
		{
			object result = parent;
			XmlNode fileAttribute = section.Attributes.RemoveNamedItem("file");
			result = NameValueSectionHandler.CreateStatic(result, section);
			if (fileAttribute != null && fileAttribute.Value.Length != 0)
			{
				string sectionName = fileAttribute.Value;
				IConfigXmlNode configXmlNode = fileAttribute as IConfigXmlNode;
				if (configXmlNode == null)
				{
					return null;
				}
				string sourceFileFullPath = Path.Combine(Path.GetDirectoryName(configXmlNode.Filename), sectionName);
				if (File.Exists(sourceFileFullPath))
				{
					ConfigXmlDocument configXmlDocument = new ConfigXmlDocument();
					try
					{
						configXmlDocument.Load(sourceFileFullPath);
					}
					catch (XmlException e)
					{
						throw new ConfigurationException(e.Message, e, sourceFileFullPath, e.LineNumber);
					}
					if (section.Name != configXmlDocument.DocumentElement.Name)
					{
						throw new ConfigurationException("Config NameValueFile Section: Invalid root", configXmlDocument.DocumentElement);
					}
					result = NameValueSectionHandler.CreateStatic(result, configXmlDocument.DocumentElement);
				}
			}
			return result;
		}
	}

}
