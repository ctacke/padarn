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

using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// Defines the contract that all configuration section handlers must implement in order to participate in the resolution of configuration settings.
	/// Reads key-value pair configuration information for a configuration section.
	/// </summary>
    internal interface IConfigurationSectionHandler
	{
		/// <summary>
		/// Implemented by all configuration section handlers to parse the XML of the configuration section. The 
		/// returned object is added to the configuration collection and is accessed by 
		/// System.Configuration.ConfigurationSettings.GetConfig(System.String).
		/// </summary>
		/// <param name="parent">The configuration settings in a corresponding parent configuration section.</param>
		/// <param name="configContext">An System.Web.Configuration.HttpConfigurationContext when 
		/// System.Configuration.IConfigurationSectionHandler.Create(System.Object,System.Object,System.Xml.XmlNode) 
		/// is called from the ASP.NET configuration system. Otherwise, this parameter is reserved and is null.</param>
		/// <param name="section">The System.Xml.XmlNode that contains the configuration information from the 
		/// configuration file. Provides direct access to the XML contents of the configuration section.</param>
		/// <returns>A configuration object.</returns>
		object Create(object parent, object configContext, XmlNode section);
	}
}
