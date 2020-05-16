//                                                                   
// Copyright (c) 2007-2009 OpenNETCF Consulting, LLC                        
//                                                                     
using OpenNETCF.Web.Configuration;
using OpenNETCF.Web.Logging;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace OpenNETCF.Web.Server
{
    internal interface IRequestListener : IDisposable
    {
        event ListenerStateChange OnStateChange;
        void StartListening();
        void Shutdown();
        bool ProcessingRequest { get; }
    }

    /// <summary>
    /// The Web Server.
    /// </summary>
    public sealed partial class WebServer : MarshalByRefObject
    {
        private Thread m_listeningThread;
        private IRequestListener m_listener;
        private ILogProvider m_logProvider;

        /// <summary>
        /// Creates an instance of the WebServer type.
        /// </summary>
        /// <param name="config">The configuration to use in place of that provided in the app.config file</param>
        public WebServer(ServerConfig config)
        {
            ServerConfig.SetConfig(config);
            HttpRuntimeConfig.SetConfig(new HttpRuntimeConfig());
            LoadLogProvider();
        }

        /// <summary>
        /// Creates an instance of the WebServer type.
        /// </summary>
        public WebServer()
        {
            // create the one and only log provider.  This gets injected into anyone who needs it.
            LoadLogProvider();
        }

        private void LoadLogProvider()
        {
            Exception e = null;
            string providerPath = string.Empty;
            try
            {
                // see if a LogProvider is set in the config
                providerPath = ServerConfig.GetConfig(false).LogProvider;

                if (providerPath == null) return;
                if (!File.Exists(providerPath))
                {
                    // look locally
                    providerPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), providerPath);
                    if (!File.Exists(providerPath))
                    {
                        return;
                    }
                }

                Assembly providerAssembly = Assembly.LoadFrom(providerPath);

                foreach (Module m in providerAssembly.GetModules())
                {
                    foreach (Type t in m.GetTypes())
                    {
                        if (System.TypeExtensions.Implements<ILogProvider>(t))
                        {
                            m_logProvider = (ILogProvider)Activator.CreateInstance(t);
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // swallow any errors - pass them to the default provider
                e = ex;
            }
            finally
            {
                if (m_logProvider == null)
                {
                    m_logProvider = new DefaultLogProvider();
                }

                // log any error loading the log
                if (e != null)
                {
                    m_logProvider.LogPadarnError(string.Format("Error loading custom log provider '{0}' : {1}", providerPath, e.Message), null);
                }

                m_logProvider.ServerConfiguration = ServerConfig.GetConfig(m_logProvider, true);
            }
        }

        /// <summary>
        /// Returns the physical path of the Web Application folder.
        /// </summary>
        /// <remarks>Obsolete as of version 1.1.70.  Throws an error as of version 1.1.90</remarks>
        // TODO: obsolete as of 1.1.70 - mark to throw error in future version
        [Obsolete("Consider using Configuration.DocumentRoot", true)]
        public static string PhysicalPath
        {
            get
            {
                return ServerConfig.GetConfig().DocumentRoot;
            }
        }

        /// <summary>
        /// Gets the current server configuration information
        /// </summary>
        public ServerConfig Configuration
        {
            get { return ServerConfig.GetConfig(m_logProvider, true); }
        }

        /// <summary>
        /// Listens for incoming requests on a separate thread. 
        /// </summary>
        public void Start()
        {
            m_logProvider.LogRuntimeInfo(ZoneFlags.Startup, "Starting WebServer");

            if (Running) throw new ApplicationException("Server is already running");

            // Initialize Virtual Path Providers
            var config = Configuration;

            if (config.VirtualPathProviders != null)
            {
                m_logProvider.LogRuntimeInfo(ZoneFlags.Startup, "Loading Virtual Path Providers");
                config.VirtualPathProviders.Initialize();
            }
            else
            {
                m_logProvider.LogRuntimeInfo(ZoneFlags.Startup, "No defined Virtual Path Providers");
            }

            IPAddress bindAddress = config.LocalIP;
            int bindPort = config.Port;
            int connections = config.MaxConnections;

            m_listener = new HttpRequestListener(bindAddress, bindPort, connections, m_logProvider);

            m_listener.OnStateChange += new ListenerStateChange(listener_OnStateChange);
            m_listeningThread = new Thread(m_listener.StartListening);
            m_listeningThread.IsBackground = true;
            m_listeningThread.Start();
            m_listeningThread.Name = "HttpRequestListener";
            Running = true;
        }

        /// <summary>
        /// Returns <b>true</b> if the server is currently running, otherwise <b>false</b>.
        /// </summary>
        public bool Running { get; private set; }

        private void listener_OnStateChange(bool listening)
        {
            Running = listening;
        }

        /// <summary>
        /// Stops listening for incoming requests.
        /// </summary>
        public void Stop()
        {
            if (!Running) return;

            m_listener.Shutdown();

            int timeout = 0;
            while (m_listener.ProcessingRequest)
            {
                Thread.Sleep(10);
                if (timeout++ > 500) // 5 seconds
                    break;
            }

            m_listener.Dispose();
        }
    }
}
