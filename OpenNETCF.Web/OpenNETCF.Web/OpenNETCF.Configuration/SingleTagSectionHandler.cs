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
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// 
	/// </summary>
    internal class SingleTagSectionHandler : IConfigurationSectionHandler
	{
		/// <summary>
		/// Returns a collection of configuration section values.
		/// </summary>
		/// <param name="parent">The configuration settings in a corresponding parent configuration section.</param>
		/// <param name="context">This parameter is reserved and is null.</param>
		/// <param name="section">An <see cref="System.Xml.XmlNode"/> that contains configuration information from the configuration file.
		/// Provides direct access to the XML contents of the configuration section.</param>
		/// <returns>A <see cref="Hashtable"/> containing configuration section directives.</returns>
		public virtual object Create(object parent, object context, XmlNode section)
		{
			Hashtable result;

			// start result off as a shallow clone of the parent
			if (parent == null)
			{
				result = new Hashtable();
			}
			else
			{
				result = new Hashtable((Hashtable)parent);
			}

			// Check for child nodes
			HandlerBase.CheckForChildNodes(section);
			
			foreach(XmlNode attribute in section.Attributes)
			{
				result[attribute.Name] = attribute.Value;
			}

			return result;
		}
	}
}
