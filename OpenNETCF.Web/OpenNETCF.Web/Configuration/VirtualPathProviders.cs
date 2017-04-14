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
