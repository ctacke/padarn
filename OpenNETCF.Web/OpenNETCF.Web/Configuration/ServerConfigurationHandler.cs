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
using System.Xml;
using System.Net;
using System.Reflection;
using OpenNETCF.Configuration;
using OpenNETCF.Web.Security;

#if !WindowsCE
using System.Configuration;
#endif

// disable warnings about obsoletes
#pragma warning disable 612, 618

namespace OpenNETCF.Web.Configuration
{

    /// <summary>
    /// Represents the WebServer section in the app.config file
    /// </summary>
    public sealed class ServerConfigurationHandler
#if WindowsCE
 : OpenNETCF.Configuration.IConfigurationSectionHandler
#else
     : System.Configuration.IConfigurationSectionHandler
#endif
    {
        /// <summary>
        /// Creates an instance of ServerConfig from the information in the app.config file
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            ServerConfig cfg = new ServerConfig();

            foreach (XmlAttribute attribute in section.Attributes)
            {
                if (attribute.NodeType == XmlNodeType.Attribute)
                {
                    switch (attribute.Name)
                    {
                        case "LocalIP":
                            try
                            {
                                cfg.LocalIP = IPAddress.Parse(attribute.Value);
                            }
                            catch
                            {
                                // TODO: log this
                                cfg.LocalIP = IPAddress.Any;
                            }
                            break;
                        case "DefaultPort":
                            cfg.Port = Int32.Parse(attribute.Value);
                            break;
                        case "DocumentRoot":
                            string docRoot = attribute.Value;
                            cfg.DocumentRoot = !docRoot.EndsWith("\\") ? string.Concat(docRoot, "\\") : docRoot;
                            break;
                        case "TempRoot":
                            cfg.TempRoot = attribute.Value;
                            break;
                        case "MaxConnections":
                            cfg.MaxConnections = Int32.Parse(attribute.Value);
                            break;
                        case "Logging":
                            cfg.LoggingEnabled = bool.Parse(attribute.Value);
                            break;
                        case "LogFolder":
                            cfg.LogFileFolder = attribute.Value;
                            break;
                        case "LogExtensions":
                            cfg.SetLogExtensions(attribute.Value);
                            break;
                        case "LogProvider":
                            cfg.LogProvider = attribute.Value;
                            break;
                        case "BrowserDefinitions":
                            cfg.BrowserDefinitions = attribute.Value;
                            break;
                        case "AuthenticationEnabled":
                            cfg.AuthenticationEnabled = bool.Parse(attribute.Value);
                            break;
                        case "CertificateName":
                            cfg.CertificateName = attribute.Value;
                            break;
                        case "CertificatePassword":
                            cfg.CertificatePassword = attribute.Value;
                            break;
                        case "UseSsl":
                            cfg.UseSsl = bool.Parse(attribute.Value);
                            break;
                        case "SSLLicenseKey":
                            cfg.SSLLicenseKey = attribute.Value;
                            break;
                        case "CustomErrorFolder":
                            cfg.CustomErrorFolder = attribute.Value;
                            break;
                        case "LicensePath":
                            cfg.LicensePath = attribute.Value;
                            break;
                        default:
                            HandleInvalidAttributes();
                            break;
                    }
                }
            }

            //Do the child nodes
            foreach (XmlNode node in section.ChildNodes)
            {
                try
                {
                    switch (node.Name)
                    {
                        case "DefaultDocuments":
                            foreach (XmlNode docNode in node.ChildNodes)
                            {
                                cfg.DefaultDocuments.Add(docNode.InnerText);
                            }
                            break;

                        case "Authentication": // for backward compatibility
                        case "authentication":
                            cfg.Authentication = ReadAuthenticationConfig(node);
                            break;
                        case "authorization":
                            ParseFormsAuthorization(node);
                            break;
                        case "VirtualDirectories":
                            if (cfg.VirtualDirectories == null)
                            {
                                cfg.VirtualDirectories = new VirtualDirectoryMappingCollection();
                            }
                            ReadVirtualDirectoriesConfig(cfg, node);
                            break;
                        case "Cookies":
                            CookiesConfiguration cookieConfig = new CookiesConfiguration();
                            XmlAttribute domainAttrib = node.Attributes["Domain"];
                            if (domainAttrib == null) break;

                            cookieConfig.Domain = domainAttrib.Value;

                            if (node.Attributes["RequireSSL"] != null)
                            {
                                cookieConfig.RequireSSL = Convert.ToBoolean(node.Attributes["RequireSSL"].Value);
                            }

                            if (node.Attributes["HttpOnlyCookies"] != null)
                            {
                                cookieConfig.HttpOnlyCookies = Convert.ToBoolean(node.Attributes["HttpOnlyCookies"].Value);
                            }

                            cfg.Cookies = cookieConfig;
                            break;
                        case "Caching":
                            CachingConfig cachingConfig = new CachingConfig();

                            foreach (XmlNode subnode in node.ChildNodes)
                            {
                                if (subnode.Name.ToLower() == "profiles")
                                {
                                    foreach (XmlNode profileNode in subnode.ChildNodes)
                                    {
                                        if (profileNode.Name.ToLower() == "add")
                                        {
                                            CachingProfile profile = new CachingProfile(profileNode);
                                            cachingConfig.AddProfile(profile);
                                        }
                                    }
                                }
                            }

                            cfg.Caching = cachingConfig;
                            break;
                        case "VirtualPathProviders":
                            VirtualPathProviders providers = new VirtualPathProviders();

                            if (node.Attributes.Count > 0)
                            {
                                if (node.Attributes["ProviderPath"] != null)
                                {
                                    string val = node.Attributes["ProviderPath"].Value;
                                    if (String.IsNullOrEmpty(val)) { val = null; }
                                    if (val != null)
                                    {
                                        providers.ProviderPath = val;
                                    }
                                }
                            }

                            foreach (XmlNode subnode in node.ChildNodes)
                            {
                                if (subnode.Name.Equals("Provider"))
                                {
                                    if (subnode.Attributes.Count > 0)
                                    {
                                        if (subnode.Attributes["Type"] != null)
                                        {
                                            string provider = subnode.Attributes["Type"].Value;
                                            providers.Add(provider);
                                        }
                                    }
                                }
                            }

                            cfg.VirtualPathProviders = providers;
                            break;
                        case "httpHandlers":
                            var h = new HttpHandlersConfigSection(node);
                            foreach (var assemblyName in h.AssemblyNames)
                            {
                                cfg.AssembliesToLoad.Add(assemblyName);
                            }

                            cfg.HttpHandlers.AddRange(h);
                            break;
                        case "session":
                            cfg.Session = SessionConfiguration.FromXml(node);
                            break;
                        case "Security":
                            cfg.Security = new SecurityConfig();

                            foreach (XmlNode subnode in node.ChildNodes)
                            {
                                bool enabled = false;
                                
                                if (subnode.Attributes.Count > 0 && subnode.Attributes["Enabled"] != null)
                                {
                                    enabled = bool.Parse(subnode.Attributes["Enabled"].Value);
                                }

                                switch (subnode.Name)
                                {
                                    case "TLS10":
                                        cfg.Security.Tls10 = enabled;
                                        break;

                                    case "TLS11":
                                        cfg.Security.Tls11 = enabled;
                                        break;

                                    case "TLS12":
                                        cfg.Security.Tls12 = enabled;
                                        break;

                                    case "ResumeSession":
                                        cfg.Security.ResumeSession = enabled;
                                        break;

                                    case "CipherList":
                                        try
                                        {
                                            var list = subnode.InnerText;
                                            if (!string.IsNullOrEmpty(list))
                                            {
                                                var items = list.Split(new char[] { ',' });

                                                foreach (var item in items)
                                                {
                                                    if (string.IsNullOrEmpty(item))
                                                    {
                                                        continue;
                                                    }

                                                    try
                                                    {
                                                        var cipher = short.Parse(item.Trim());
                                                        cfg.Security.CipherList.Add(cipher);
                                                    }
                                                    catch
                                                    {
                                                        // ignore non-numerics
                                                    }
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            // just use the defaults
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException(string.Format("Error parsing '{0}' configuration node in '{1}' section", node.Name, section.Name), ex);
                }
            }

            if (cfg.DefaultDocuments.Count == 0)
            {
                cfg.DefaultDocuments.Add("default.aspx");
            }

            return cfg;
        }

        private static void ReadVirtualDirectoriesConfig(ServerConfig cfg, XmlNode node)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (HandlerBase.IsIgnorableAlsoCheckForNonElement(childNode))
                    {
                        continue;
                    }

                    if (childNode.Name == "Directory")
                    {
                        XmlNode directoryNode = childNode;

                        string virtualPath = HandlerBase.RemoveRequiredAttribute(directoryNode, "VirtualPath",
                                                                                 false);
                        string physicalPath = HandlerBase.RemoveRequiredAttribute(directoryNode,
                                                                                  "PhysicalPath",
                                                                                  false);

                        if (physicalPath.EndsWith("\\"))
                        {
                            physicalPath = physicalPath.Substring(0, physicalPath.Length - 1);
                        }

                        VirtualDirectoryMapping virtualDir = new VirtualDirectoryMapping(virtualPath,
                                                                                         physicalPath);

                        string requiresAuthValue = HandlerBase.RemoveAttribute(directoryNode,
                                                                               "RequireAuthentication");
                        virtualDir.RequiresAuthentication = String.IsNullOrEmpty(requiresAuthValue)
                                                                ? false
                                                                : Convert.ToBoolean(requiresAuthValue);

                        HandlerBase.CheckForUnrecognizedAttributes(directoryNode);

                        cfg.VirtualDirectories.Add(virtualDir);
                        continue;
                    }
                }
            }
        }

        // TODO:
        private static void ParseFormsAuthorization(XmlNode node)
        {
            // see http://support.microsoft.com/kb/316871
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (string.Compare(child.Name, "allow", true) == 0)
                    {
                    }
                    if (string.Compare(child.Name, "deny", true) == 0)
                    {
                    }
                }
            }
        }

        private static AuthenticationConfiguration ReadAuthenticationConfig(XmlNode node)
        {
            AuthenticationConfiguration auth = new AuthenticationConfiguration();

            auth.Mode = node.Attributes["Mode"].Value;

            if (string.Compare(auth.Mode, "forms", true) == 0)
            {
                FormsAuthentication.IsEnabled = true;
            }

            var attr = node.Attributes["Enabled"];
            auth.Enabled = false;
            if (attr != null)
            {
                try
                {
                    auth.Enabled = bool.Parse(attr.Value);
                }
                catch { }
            }

            attr = node.Attributes["Realm"];
            auth.Realm = "Padarn";
            if (attr != null)
            {
                try
                {
                    auth.Realm = attr.Value;
                }
                catch { }
            }

            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (string.Compare(child.Name, "users", true) == 0)
                    {
                        foreach (XmlNode userNode in child.ChildNodes)
                        {
                            User user = new User();

                            XmlAttribute nameAttribute = userNode.Attributes["Name"];
                            user.Name = (nameAttribute == null) ? String.Empty : nameAttribute.Value;

                            XmlAttribute passwordAttribute = userNode.Attributes["Password"];
                            user.Password = (passwordAttribute == null) ? String.Empty : passwordAttribute.Value;

                            if (userNode.HasChildNodes)
                            {
                                foreach (XmlNode roleNode in userNode.ChildNodes)
                                {
                                    Role role = new Role();
                                    XmlAttribute roleNameAttribute = roleNode.Attributes["Name"];
                                    role.Name = (roleNameAttribute == null)
                                                    ? String.Empty
                                                    : roleNameAttribute.Value;
                                    user.Roles.Add(role);
                                }
                            }

                            auth.Users.Add(user);
                        }
                    }
                    if (string.Compare(child.Name, "forms", true) == 0)
                    {
                        //loginUrl="login.aspx"
                        //name=".PADARNAUTH"
                        //domain="testdomain"
                        //defaultUrl="default.aspx"
                        //path="/"
                        //timeout="30"

                        //slidingExpiration="true"
                        //requireSSL="false"
                        //protection="None"
                        //enableCrossAppRedirects="false"
                        //cookieless="UseCookies"    

                        attr = child.Attributes["loginUrl"];
                        if (attr != null)
                        {
                            FormsAuthentication.LoginUrl = attr.Value;
                        }

                        attr = child.Attributes["loginUrl"];
                        if (attr != null)
                        {
                            FormsAuthentication.LoginUrl = attr.Value;
                        }
                    }
                }

                //if (node.FirstChild.HasChildNodes)
                //{
                //    foreach (XmlNode userNode in node.FirstChild.ChildNodes)
                //    {
                //        User user = new User();

                //        XmlAttribute nameAttribute = userNode.Attributes["Name"];
                //        user.Name = (nameAttribute == null) ? String.Empty : nameAttribute.Value;

                //        XmlAttribute passwordAttribute = userNode.Attributes["Password"];
                //        user.Password = (passwordAttribute == null) ? String.Empty : passwordAttribute.Value;

                //        if (userNode.HasChildNodes)
                //        {
                //            foreach (XmlNode roleNode in userNode.ChildNodes)
                //            {
                //                Role role = new Role();
                //                XmlAttribute roleNameAttribute = roleNode.Attributes["Name"];
                //                role.Name = (roleNameAttribute == null)
                //                                ? String.Empty
                //                                : roleNameAttribute.Value;
                //                user.Roles.Add(role);
                //            }
                //        }

                //        auth.Users.Add(user);
                //    }
                //}
            }
            return auth;
        }

        private static void HandleInvalidAttributes()
        {
            // Just ignore these attributes
        }
    }
}

#pragma warning restore 612, 618
