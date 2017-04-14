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
using System.Reflection;
using System.Threading;
using OpenNETCF.Web.Configuration;
using System.Xml;

namespace OpenNETCF.Configuration
{
#if MONO
    using Android.Content.Res;
    
    class DefaultConfigurationSystem : IConfigurationSystem
    {
        private OpenNETCF.Web.Configuration.ServerConfig m_config;

        public object GetConfig(string configKey, object context)
        {
            return m_config;
        }

        public void Init()
        {
            // use a default set
            m_config = new Web.Configuration.ServerConfig();

            m_config.DocumentRoot = Environment.CurrentDirectory;
            m_config.LocalIP = System.Net.IPAddress.Parse("0.0.0.0");
            m_config.Port = 8080;
            m_config.DefaultDocuments.Add("default.aspx");

        }
    }
#else
    /// <summary>
	/// Summary description for DefaultConfigurationSystem.
	/// </summary>
	class DefaultConfigurationSystem : IConfigurationSystem 
	{
		private const string ConfigExtension = ".config";
        private const string UnitTestExtension = ".unittest";
        private const string MachineConfigFilename = "machine.config";
		private const string MachineConfigSubdirectory = "config";
		private ConfigurationRecord _application;
		
		internal DefaultConfigurationSystem()
		{
		}

        [Obsolete("There is no need to call this constructor anymore.", false)]
		public DefaultConfigurationSystem(string CodeBase)
		{
		}

        object IConfigurationSystem.GetConfig(string configKey, object context)
        {
            if (_application != null)
            {
                return _application.GetConfig(configKey, context);
            }
            else
            {
                throw new InvalidOperationException("Client config init error");
            }
        }
        
        void IConfigurationSystem.Init() 
		{
			lock(this) 
			{
				if(_application == null) 
				{
                    ConfigurationRecord machineConfig = null;
                    bool machineConfigExists = false;
                                        
					string machineConfigFilename = MachineConfigurationFilePath;
					_application = machineConfig = new ConfigurationRecord();
					machineConfigExists = machineConfig.Load(machineConfigFilename);

                    if (!machineConfigExists)
                    {
                        // Load machine.config from embedded resource
//                        string machineConfigXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"
//                                                + "<configuration>"
//                                                + "  <configSections>"
//                                                + "     <section name=\"appSettings\" type=\"OpenNETCF.Configuration.NameValueFileSectionHandler, OpenNETCF.Configuration\" allowLocation=\"false\" />"
//                                                + "     <section name=\"opennetcf.diagnostics\" type=\"OpenNETCF.Diagnostics.DiagnosticsConfigurationHandler, OpenNETCF\"/>"
//                                                + "  </configSections>"
//                                                + "</configuration>";
#if(WindowsCE)
                        string machineConfigXml = OpenNETCF.Web.Resources.machine_config;
#else
                        string machineConfigXml = OpenNETCF.Web.Properties.Resources.machine_config;
#endif
                        bool machineConfigLoaded = machineConfig.LoadXml(machineConfigXml);
                    }

					Uri appConfigFilename = AppConfigPath;

                    // test to see if a unit test config exists first
                    string unitTestConfig = appConfigFilename.LocalPath + UnitTestExtension;

                    if(File.Exists(unitTestConfig))
                    {
                        appConfigFilename = new Uri("file://" + unitTestConfig);
                    }

					// Only load the app.config if machine.config exists
					if(appConfigFilename != null) 
					{
						_application = new ConfigurationRecord(machineConfig);

                        string fileName = appConfigFilename.LocalPath;

                        if (!File.Exists(fileName))
                        {
                            throw new FileNotFoundException(string.Format("Cannot locate configuration file '{0}'", fileName));
                        }
                        
                        if (!_application.Load(fileName))
						{
							throw new ConfigurationException("Unable to load application configuration file");
						}
					}
				}
			}            
		}

		internal static string MsCorLibDirectory 
		{
			get 
			{ 
				string corCodeBase = typeof(object).Assembly.GetName().CodeBase;
				int separatorIndex = corCodeBase.IndexOf("\\", 2) + 1;
				
				string filename = corCodeBase.Substring(0, separatorIndex);//.Replace('/','\\');
				filename = filename.Replace("/","\\");
				return Path.GetDirectoryName(filename);
			}
		}

		internal static string MachineConfigurationFilePath 
		{
			get 
			{
				return Path.Combine(Path.Combine(MsCorLibDirectory, MachineConfigSubdirectory), MachineConfigFilename);
			}
		}

		internal static Uri AppConfigPath 
		{
			get 
			{                
				try 
				{
					string appBase = ApplicationBase();
                
					// we need to ensure AppBase ends in an '/'.                                  
					if (appBase.Length > 0) 
					{
						char lastChar = appBase[appBase.Length - 1];
						if (lastChar != '/' && lastChar != '\\') 
						{
							appBase += '\\';
						}
					}
					Uri uri = new Uri(appBase);
					string config = ConfigurationFile();
					if (config != null && config.Length > 0) 
					{
						uri = new Uri(uri, config);
						return uri;
					}
				}
				finally 
				{
                    
				}
				return null;
			}
		}

		private static string GetCodeBase() 
		{
			return OpenNETCF.Reflection.Assembly2.GetEntryAssembly().GetName().CodeBase; 
		}

		private static string ApplicationBase() 
		{
			string codeBase = GetCodeBase();
            // neilco: Bug #330: UriFormatException when reading app.config from root directory
			return codeBase.Substring(0,codeBase.LastIndexOf("\\") + 1);
		}

		private static string ConfigurationFile() 
		{
			return GetCodeBase() + ConfigExtension;
		}
    }
#endif
}
