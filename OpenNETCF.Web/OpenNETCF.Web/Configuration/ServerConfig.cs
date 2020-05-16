#if WindowsCE
  using OpenNETCF.Configuration;
#else
  using System.Configuration;
#endif

using System.IO;
using System.Collections.Generic;
using System;
using System.Net;
using System.Reflection;
using System.Xml;
using OpenNETCF.Web.Logging;
using System.Diagnostics;

// disable warnings about obsoletes
#pragma warning disable 612, 618

  namespace OpenNETCF.Web.Configuration
{

    /// <summary>
    /// Holds configuration information for the Web Server
    /// </summary>
    public sealed class ServerConfig
    {
        private string m_documentRoot;
        private bool m_loggingEnabled;
        private string m_logFolder;
        private bool m_logFolderError = false;
        private static ServerConfig m_instance;
        private string m_tempRoot;
        private string m_errorFolder;
        private string m_licenseFile;

        private int DefaultPort = 80;
        private int DefaultMaxConnections = 20;
        private List<Assembly> m_loadedAssemblies;

        public string HostLocation { get; private set; }

        internal ServerConfig()
        {
            DefaultDocuments = new List<string>();
            LogExtensions = new List<string>();
            LocalIP = IPAddress.Any;
            Authentication = new AuthenticationConfiguration();
            Caching = new CachingConfig();
            Cookies = new CookiesConfiguration();
            Session = new SessionConfiguration();
            Security = new SecurityConfig();
            this.HttpHandlers = new List<HttpHandler>();
            this.AssembliesToLoad = new List<string>();
            m_loadedAssemblies = new List<Assembly>();

            HostLocation = Path.GetDirectoryName(new Uri(Assembly.GetCallingAssembly().GetName().CodeBase).LocalPath);

            SetDefaults();

            ServerVersion = Assembly.GetExecutingAssembly().GetName().Version;
        }

        public string LicensePath
        {
            get { return m_licenseFile; }
            set
            {
                var temp = value;

                if(temp != null)
                {
                    temp = temp.Replace("%APPLICATIONDATA%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                    temp = temp.Replace("%PERSONAL%", Environment.GetFolderPath(Environment.SpecialFolder.Personal));
#if !WindowsCE
                    temp = temp.Replace("%PROGRAMFILES%", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
                    temp = temp.Replace("%MYDOCUMENTS%", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                    temp = temp.Replace("%PROGRAMDATA%", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
#endif
                }

                m_licenseFile = temp;
            }
        }

        private void SetDefaults()
        {
            Port = DefaultPort;
            MaxConnections = DefaultMaxConnections;
            UseSsl = false;
            AuthenticationEnabled = false;
            LoggingEnabled = false;
            CustomErrorFolder = Path.Combine(HostLocation, "errors");
            DocumentRoot = Path.Combine(HostLocation, "inetpub");
            LogFileFolder = Path.Combine(HostLocation, "logs");
            TempRoot = Path.Combine(HostLocation, "temp");
        }

        public static ServerConfig Create(int port, string documentRoot)
        {
            var doc = new XmlDocument();
            var configNode = doc.CreateNode(XmlNodeType.Element, "WebServer", null);

            var attr = doc.CreateAttribute("DefaultPort");
            attr.Value = port.ToString();
            configNode.Attributes.Append(attr);

            attr = doc.CreateAttribute("DocumentRoot");
            attr.Value = documentRoot;
            configNode.Attributes.Append(attr);

            return Create(configNode);
        }

        public static ServerConfig Create(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            foreach (XmlNode n in doc.ChildNodes)
            {
                // find the configuration node
                if (n.Name == "configuration")
                {
                    // find the WebServer node
                    foreach (XmlNode child in n.ChildNodes)
                    {
                        if (string.Compare(child.Name, "WebServer", true) == 0)
                        {
                            return Create(child);
                        }
                    }
                    break;
                }
            }

            throw new ArgumentException("Invalid configuration XML");
        }

        internal static ServerConfig Create(XmlNode serverConfigNode)
        {
            if ((serverConfigNode == null) || (serverConfigNode.Name != "WebServer"))
            {
                throw new ArgumentException("Invalid configuration XML");
            }

            var handler = new ServerConfigurationHandler();
            return (ServerConfig)handler.Create(null, null, serverConfigNode);
        }

        internal static void SetConfig(ServerConfig config)
        {
            m_instance = config;
        }

        /// <summary>
        /// Reads the Server configuration from the application configuration file. 
        /// </summary>
        /// <returns>An instance of ServerConfig</returns>
        public static ServerConfig GetConfig()
        {
            return GetConfig(null, true);
        }

        internal static ServerConfig GetConfig(bool cache)
        {
            return GetConfig(null, cache);
        }

        internal static ServerConfig GetConfig(ILogProvider provider, bool cache)
        {
            ServerConfig config = null;

            if (m_instance == null)
            {
                if (provider != null)
                {
                    provider.LogRuntimeInfo(ZoneFlags.Startup, "Loading WebServer config node");
                }

                config = ConfigurationSettings.GetConfig("WebServer") as ServerConfig;

                if (config == null)
                {
                    if (provider != null)
                    {
                        provider.LogRuntimeInfo(ZoneFlags.Startup, "Unable to locate WebServer config node");
                    }
                    throw new ConfigurationException("Unable to load 'WebServer' configuration node");
                }

                if (provider != null)
                {
                    provider.LogRuntimeInfo(ZoneFlags.Startup, "Config.DocumentRoot: " + config.DocumentRoot);
                }

                if (string.IsNullOrEmpty(config.DocumentRoot))
                {
                    throw new ConfigurationException("Empty DocumentRoot is invalid");
                }

                if (!System.IO.Directory.Exists(config.DocumentRoot))
                {
                    if (provider != null)
                    {
                        provider.LogRuntimeInfo(ZoneFlags.Startup, "Creating DocumentRoot at " + config.DocumentRoot);
                    }
                    System.IO.Directory.CreateDirectory(config.DocumentRoot);
                    // TODO: Should we dump in a standard "default" web page (pulled from an embedded resource), or is the 404 going to be enough?
                    // if we have a page that at least says "the server is installed an running" I think that would be useful
                }

                foreach (var assemblyName in config.AssembliesToLoad)
                {
                    string newName = Path.Combine(Path.Combine(config.DocumentRoot, "bin"), assemblyName);

                    if (provider != null)
                    {
                        provider.LogRuntimeInfo(ZoneFlags.Startup, "Loading Assembly: " + newName);
                    }

                    try
                    {
                        // try loading from the docs folder first
                        if (File.Exists(newName))
                        {
                            var a = Assembly.LoadFrom(newName);
                            config.m_loadedAssemblies.Add(a);
                            var t = a.GetTypes();

                        }
                        else
                        {
                            // try the loader path (exception will throw if it's not there)
                            Assembly.LoadFrom(assemblyName);
                        }
                    }
                    catch (Exception ex)
                    {
                        var msg = string.Format("Unable to load requested assembly '{0}': {1}", assemblyName, ex.Message);
                        Debug.WriteLine(msg);
                        if (provider != null)
                        {
                            provider.LogPadarnError(msg, null);
                        }
                        
                    }
                }
                if (cache)
                {
                    m_instance = config;
                }
            }
            else
            {
                if (m_instance.LogFileFolder == null)
                {
                    m_instance.LogFileFolder = "\\Temp\\PadarnLogs";
                }

                config = m_instance;
            }

            return config;
        }

        internal Type GetType(string typeName)
        {
            Type t = null;

            foreach (var a in m_loadedAssemblies)
            {
                t = a.GetType(typeName.Substring(0, typeName.IndexOf(',')));

                if (t != null) break;
            }

            return t;
        }

#if WindowsCE
        /// <summary>
        /// Forces the server to reload the configuration settings from the Padarn configuration settings XML file
        /// </summary>
        public void Reload()
        {
            ConfigurationSettings.Reload();
            m_instance = ConfigurationSettings.GetConfig("WebServer") as ServerConfig;
        }
#endif

        /// <summary>
        /// Optional path to a custom log provider assembly
        /// </summary>
        public string LogProvider { get; internal set; }

        /// <summary>
        /// The default port to listen on for incoming requests.
        /// </summary>
        public int Port { get; internal set; }

        /// <summary>
        /// Contains a list of default documents to display when a document is not specified in a request.
        /// </summary>
        public List<string> DefaultDocuments { get; internal set; }

        /// <summary>
        /// Gets or sets the value of the local IP address to which Padarn will bind.
        /// </summary>
        /// <remarks>
        /// The Address 0.0.0.0 will be returned if no specific IP address has been configured.  In this case the Padarn server will attach to the first valid IP address it finds.  This is equivalent to the IIS setting of "Unassigned"
        /// </remarks>
        public IPAddress LocalIP { get; set; }

        /// <summary>
        /// Gets a value indicating if authentication is enabled for the server
        /// </summary>
        public bool AuthenticationEnabled { get; internal set; }

        /// <summary>
        /// Gets the authentication configuration for the server
        /// </summary>
        public AuthenticationConfiguration Authentication { get; internal set; }

        /// <summary>
        /// Gets the session configuration for the server
        /// </summary>
        public SessionConfiguration Session { get; internal set; }

        /// <summary>
        /// The SSL LicenseKey for the server when running in secure mode (from SecureBlackBox)
        /// </summary>
        public string SSLLicenseKey { get; internal set; }

        
        /// <summary>
        /// 
        /// </summary>
        internal VirtualDirectoryMappingCollection VirtualDirectories { get; set; }

        /// <summary>
        ///
        /// </summary>
        public CookiesConfiguration Cookies { get; internal set; }

        /// <summary>
        ///
        /// </summary>
        public CachingConfig Caching { get; internal set; }


        /// <summary>
        /// The security configuration of the WebServer
        /// </summary>
        public SecurityConfig Security { get; internal set; }
        /// <summary>
        ///
        /// </summary>
        public VirtualPathProviders VirtualPathProviders { get; internal set; }

        internal List<HttpHandler> HttpHandlers { get; private set; }
        internal List<string> AssembliesToLoad { get; private set; }

        /// <summary>
        /// The location of the Web Server pages.
        /// </summary>
        public string DocumentRoot
        {
            get { return m_documentRoot; }
            internal set
            {
                // ensure that it's not '\' terminated
                if (value[value.Length - 1] == '\\')
                {
                    m_documentRoot = value.Substring(0, value.Length - 1);
                }
                else
                {
                    m_documentRoot = value;
                }
            }
        }

        /// <summary>
        /// The location where Padarn will create its internally-generated temp files (such as for file uploads)
        /// </summary>
        public string TempRoot
        {
            get
            {
                try
                {
                    // see if the folder exists
                    if (!Directory.Exists(m_tempRoot))
                    {
                        Directory.CreateDirectory(m_tempRoot);
                    }
                }
                catch
                {
                    // TODO: log this

                    // default it to the document root temp
                    m_tempRoot = null;
                }

                if (m_tempRoot == null)
                {
                    m_tempRoot = Path.Combine(DocumentRoot, "temp");
                }

                return m_tempRoot;
            }
            set
            {
                m_tempRoot = value;
            }
        }

        /// <summary>
        /// The maximum number of concurrent connections the Web Server will allow. 
        /// </summary>
        public int MaxConnections { get; internal set; }

        /// <summary>
        /// Determines whether server logging is turned on or not
        /// </summary>
        public bool LoggingEnabled
        {
            get { return m_loggingEnabled; }
            internal set
            {
                // don't enable logging if the log folder can't be created for some reason
                if (m_logFolderError)
                {
                    m_loggingEnabled = false;
                }
                else
                {
                    m_loggingEnabled = value;
                }
            }
        }

        /// <summary>
        /// The location in the server's file system where log files are stored when LoggingEnabled is true.  This folder will be created if it doesn't exist
        /// </summary>
        public string LogFileFolder
        {
            get { return m_logFolder; }
            internal set
            {
                m_logFolderError = false;
                m_logFolder = value;

                try
                {
                    if (LoggingEnabled)
                    {
                        if (!Directory.Exists(m_logFolder = value))
                        {
                            Directory.CreateDirectory(m_logFolder);
                        }
                    }
                }
                catch
                {
                    LoggingEnabled = false;
                    m_logFolderError = true;
                }

            }
        }

        /// <summary>
        /// The list of file extensions that access to will be logged
        /// </summary>
        public List<string> LogExtensions { get; private set; }

        /// <summary>
        /// Gets the Browser definition file directory.
        /// </summary>
        public string BrowserDefinitions { get; internal set; }

        /// <summary>
        /// Fully qualified path to the SSL certificate file
        /// </summary>
        public string CertificateName { get; internal set; }

        /// <summary>
        /// Provides the version of the currently running Padarn server
        /// </summary>
        public Version ServerVersion { get; private set; }

        /// <summary>
        /// Password for the SSL certificate file
        /// </summary>
        public string CertificatePassword { get; internal set; }

        /// <summary>
        /// Determines if the server instance uses secure sockets for client connections
        /// </summary>
        public bool UseSsl { get; internal set; }

        /// <summary>
        /// Gets the path for custom HTML error pages
        /// </summary>
        public string CustomErrorFolder
        {
            get 
            {
                try
                {
                    if (!Directory.Exists(m_errorFolder))
                    {
                        Directory.CreateDirectory(m_errorFolder);
                    }
                }
                catch
                {
                    m_errorFolder = "\\";
                }

                return m_errorFolder; 
            }
            internal set { m_errorFolder = value; }
        }

        internal void SetLogExtensions(string extensionList)
        {
            string[] extensions = extensionList.Split(';');

            foreach (string extension in extensions)
            {
                string e = extension.Trim();

                if (e.Length > 0)
                {
                    if (e[0] != '.')
                    {
                        LogExtensions.Add(string.Format(".{0}", e));
                    }
                    else
                    {
                        LogExtensions.Add(e);
                    }
                }
            }
        }
    }
}

  // disable warnings about obsoletes
#pragma warning restore 612, 618
