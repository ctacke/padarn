using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using OpenNETCF.Web.Hosting;

namespace OpenNETCF.Web.Configuration
{

    public class VirtualPathProviders : List<String>
    {
        public string ProviderPath { get; internal set; }

        internal VirtualPathProviders()
        {
        }

        internal void Initialize()
        {
            foreach (string provider in this)
            {
                string[] assemblyNameParts = provider.Split(',');
                if (assemblyNameParts.Length < 2)
                {
                    return;
                }

                string className = assemblyNameParts[0];
                string assemblyName = assemblyNameParts[1].Trim(' ');

                string binPath = String.IsNullOrEmpty(ProviderPath) ? Path.Combine(ServerConfig.GetConfig().DocumentRoot, "bin") : ProviderPath;
                string assemblyPath = Path.Combine(binPath, String.Format("{0}.dll",assemblyName));

                try
                {
                    Assembly providerDll = Assembly.LoadFrom(assemblyPath);
                    Type providerType = providerDll.GetType(className);

                    if (providerType != null)
                    {
                        VirtualPathProvider providerInst = Activator.CreateInstance(providerType) as VirtualPathProvider;
                        HostingEnvironment.RegisterVirtualPathProvider(providerInst);
                    }
                }
                catch (FileNotFoundException)
                {
                    // Should we log this?
                }
            }
        }



        private static void InvokeAppInit(Type provider)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Static;
            MethodInfo appInit = provider.GetMethod("AppInitialize", flags);
            if (appInit == null)
                return;

            appInit.Invoke(null, null);
        }
    }
}
