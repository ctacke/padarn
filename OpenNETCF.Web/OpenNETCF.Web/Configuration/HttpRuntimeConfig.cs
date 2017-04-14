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
using System.IO;
using System.Collections.Generic;
using System;
using System.Xml;

#if WindowsCE
using OpenNETCF.Configuration;
#else
using System.Configuration;
#endif

// disable warnings about obsoletes
#pragma warning disable 612, 618

namespace OpenNETCF.Web.Configuration
{

    /// <summary>
    /// Holds configuration information for the HTTP run-time settings that determine how to process a request for an ASP.NET application.
    /// </summary>
    public sealed class HttpRuntimeConfig
    {
        private int maxRequestLength = 4096;
        private int requestLengthDiskThreshold = 256;
        private int maxRequestLengthBytes = -1;
        private int requestLengthDiskThresholdBytes = -1;
        
        private static HttpRuntimeConfig config;

        internal HttpRuntimeConfig()
        {
        }

        internal static void SetConfig(HttpRuntimeConfig cfg)
        {
            config = cfg;
        }

        /// <summary>
        /// Reads the HttpRuntime configuration settings.
        /// </summary>
        /// <returns>An instance of HttpRuntimeConfig</returns>
        public static HttpRuntimeConfig GetConfig()
        {
            if (config == null)
            {
                config = ConfigurationSettings.GetConfig("httpRuntime") as HttpRuntimeConfig;

                if (config == null)
                {
                    // set up some defaults
                    config = new HttpRuntimeConfig();
                }
            }


            return config;
        }

        /// <summary>
        /// Reloads the httpRuntime configuration section from the App.Config
        /// </summary>
        public void Reload()
        {
            config = null;
            config = GetConfig();
        }

        /// <summary>
        /// Specifies the limit for the input stream buffering threshold, in KB. This limit can be used to prevent denial of service attacks that are caused, for example, by users posting large files to the server. 
        /// </summary>
        public int MaxRequestLength
        {
            get { return maxRequestLength; }
            internal set { maxRequestLength = value; }
        }

        /// <summary>
        /// Specifies the limit for the input stream buffering threshold, in bytes. This value should not exceed the maxRequestLength attribute.
        /// </summary>
        public int RequestLengthDiskThreshold
        {
            get { return requestLengthDiskThreshold; }
            internal set
            {
                requestLengthDiskThreshold = value;
                if (requestLengthDiskThreshold > maxRequestLength)
                    throw new ArgumentException("requestLengthDiskThreshold cannot exceed maxRequestLength");
            }
        }

        internal int RequestLengthDiskThresholdBytes
        {
            get
            {
                if (this.requestLengthDiskThresholdBytes < 0)
                {
                    this.requestLengthDiskThresholdBytes = this.BytesFromKilobytes(this.RequestLengthDiskThreshold);
                }
                return this.requestLengthDiskThresholdBytes;
            }
        }

        internal int MaxRequestLengthBytes
        {
            get
            {
                if (this.maxRequestLengthBytes < 0)
                {
                    this.maxRequestLengthBytes = this.BytesFromKilobytes(this.MaxRequestLength);
                }
                return this.maxRequestLengthBytes;
            }
        }
 


        private int BytesFromKilobytes(int kilobytes)
        {
            long num = kilobytes * 0x400L;
            if (num >= 0x7fffffffL)
            {
                return 0x7fffffff;
            }
            return (int)num;
        }

 

 

    }


    /// <summary>
    /// Represents the WebServer section in the app.config file
    /// </summary>
    public sealed class HttpRuntimeConfigurationHandler : IConfigurationSectionHandler
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
            HttpRuntimeConfig cfg = new HttpRuntimeConfig();

            foreach (XmlAttribute attribute in section.Attributes)
            {
                if (attribute.NodeType == XmlNodeType.Attribute)
                {
                    switch (attribute.Name)
                    {
                        case "requestLengthDiskThreshold":
                            cfg.RequestLengthDiskThreshold = Int32.Parse(attribute.Value);
                            break;
                        case "maxRequestLength":
                            cfg.MaxRequestLength = Int32.Parse(attribute.Value);
                            break;
                        case "xmlns":
                            break;
                        default:
                            HandleInvalidAttributes();
                            break;
                    }
                }
            }
            return cfg;
        }

        private static void HandleInvalidAttributes()
        {
            throw new Exception();
        }
    }
}

#pragma warning restore 612, 618
