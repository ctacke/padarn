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
using System.Xml;

namespace OpenNETCF.Web
{
    /// <summary>
    /// A list of virtual directories available on the webserver
    /// </summary>
    internal class VirtualDirectories : List<VirtualDirectory>
    {
        private readonly string VIRTUAL_DIRECTORY_FILE = "VirtualDirectories.xml";

        private static VirtualDirectories m_instance;
        public static VirtualDirectories Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new VirtualDirectories();
                return m_instance;
            }
        }

        private VirtualDirectories()
        {
            LoadVirtualDirectories();
        }

        private new void Add(VirtualDirectory item) { return; }
        private new void AddRange(IEnumerable<VirtualDirectory> collection) { return; }
        private new void Insert(int index, VirtualDirectory item) { return; }
        private new void InsertRange(int index, IEnumerable<VirtualDirectory> collection) { return; }
        private new bool Remove(VirtualDirectory item) { return false; }
        private new int RemoveAll(Predicate<VirtualDirectory> match) { return -1; }
        private new void RemoveAt(int index) { return; }
        private new void RemoveRange(int index, int count) { return; }

        private void LoadVirtualDirectories()
        {
            string path = Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName),
                VIRTUAL_DIRECTORY_FILE);
            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                XmlTextReader reader = new XmlTextReader(fs);

                try
                {
                    //Move directlky to the content
                    reader.MoveToContent();

                    //check for virtualDirectories node which should be the first
                    if (reader.NodeType != XmlNodeType.Element || reader.Name != "VirtualDirectories")
                        throw new Exception("VirtualDirectories.xml is not valid!");

                    //Read all the virtual directory items
                    VirtualDirectory vd;
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "VirtualDirectory")
                        {
                            //Found a virtual directory so get the attributes
                            vd = new VirtualDirectory();
                            ValidateVirtualDirectoryAttributesExists(reader, vd);
                            //if we get here then there was no exception so add to the list
                            base.Add(vd);
                            //TODO add support for users and groups that are allowed to access the VirtualDir
                        }
                    }
                }
                finally
                {
                    //close the reader
                    fs.Close();
                    reader.Close();
                }
            }
        }

        private void ValidateVirtualDirectoryAttributesExists(XmlTextReader reader, VirtualDirectory vd)
        {
            //Check for the appropriate attributes Alias, PhysicalPath and AuthenticationEnabled
            string[] att = new string[] {"Alias", "PhysicalPath", "AuthenticationEnabled"};
            string value;
            for(int x=0;x<att.Length;x++)
            {
                value = reader.GetAttribute(att[x]);
                if (value == null)
                    throw new Exception(Resources.VirtualDir_AttributeNotFound);
                switch(att[x])
                {
                    case "Alias":
                        vd.Alias = value;
                        break;
                    case "PhysicalPath":
                        // neilco -- Normalize the Physical Path by trimming the trailing slash
                        vd.PhysicalPath = NormalizePath(value);
                        break;
                    case "AuthenticationEnabled":
                        vd.AuthenticationEnabled = bool.Parse(value);
                        break;
                }
            }
        }

        public bool AliasExists(string alias)
        {
            bool ret = false;
            if (base.Count > 0)
            {
                ret = base.Exists(new Predicate<VirtualDirectory>(delegate(VirtualDirectory vd)
                {
                    return vd.Alias.Equals(alias,StringComparison.OrdinalIgnoreCase);
                }));
            }

            return ret;
        }

        internal VirtualDirectory GetVirtualDirectory(string alias)
        {
            return base.Find((new Predicate<VirtualDirectory>(delegate(VirtualDirectory vd)
                {
                    return vd.Alias.Equals(alias, StringComparison.OrdinalIgnoreCase);
                })));
        }

        private static string NormalizePath(string path)
        {
            return path.EndsWith("\\") ? path.Substring(0, path.LastIndexOf('\\')) : path;
        }
    }
}
