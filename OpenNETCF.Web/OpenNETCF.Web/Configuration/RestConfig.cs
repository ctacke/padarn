using System;

using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Diagnostics;
using System.IO;

namespace OpenNETCF.Web.Configuration
{
    internal class RestConfig : List<Assembly>
    {
        internal RestConfig(XmlNode section)
        {
            ParseSection(section);
        }

        private void ParseSection(XmlNode section)
        {
            XmlNode assemblies = section.SelectSingleNode("Assemblies");
            if (assemblies != null)
            {
                ParseAssemblies(assemblies);
            }
        }

        private void ParseAssemblies(XmlNode assemblies)
        {
            XmlNodeList adds = assemblies.SelectNodes("add");
            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            foreach (XmlNode n in adds)
            {
                string fileName = n.Attributes["name"].Value;
                if (!File.Exists(fileName))
                {
                    // local path?
                    fileName = Path.Combine(Path.GetDirectoryName(currentAssembly.GetName().CodeBase),
                        fileName);

                    if(!File.Exists(fileName))
                    {
                        throw new FileNotFoundException(string.Format("Cannot find REST service assembly '{0}'", n.Attributes["name"].Value));
                    }
                }

                Assembly asm;
                try
                {
                    if (string.Compare(AppDomain.CurrentDomain.FriendlyName, Path.GetFileName(fileName), true) == 0)
                    {
                        asm = currentAssembly;
                    }
                    else
                    {
                        asm = Assembly.Load(fileName);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Cannot load REST service assembly '{0}'", n.Attributes["name"].Value), ex);
                }
                this.Add(asm);
            }
        }
    }
}
