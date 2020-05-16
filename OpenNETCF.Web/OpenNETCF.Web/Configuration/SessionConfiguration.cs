using System;

using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenNETCF.Web.Configuration
{
    public class SessionConfiguration
    {
        public const bool DefaultAllowSessionState = true;
        public const int DefaultMaxSessions = 10;
        public const int DefaultTimeout = 20;

        internal SessionConfiguration()
        {
            AllowSessionState = DefaultAllowSessionState;
            MaxSessions = DefaultMaxSessions;
            Timeout = DefaultTimeout;
        }

        internal static SessionConfiguration FromXml(XmlNode sessionNode)
        {
            SessionConfiguration config = new SessionConfiguration();

            var attrib = sessionNode.Attributes["allowSessionState"];
            if (attrib == null)
            {
                config.AllowSessionState = DefaultAllowSessionState;
            }
            else
            {
                try
                {
                    config.AllowSessionState = bool.Parse(attrib.Value);
                }
                catch
                {
                    config.AllowSessionState = DefaultAllowSessionState;
                }
            }

            attrib = sessionNode.Attributes["max"];
            if (attrib == null)
            {
                config.MaxSessions = DefaultMaxSessions;
            }
            else
            {
                try
                {
                    config.MaxSessions = int.Parse(attrib.Value);
                }
                catch
                {
                    config.MaxSessions = DefaultMaxSessions;
                }
            }

            attrib = sessionNode.Attributes["timeout"];
            if (attrib == null)
            {
                config.Timeout = DefaultTimeout;
            }
            else
            {
                try
                {
                    config.Timeout = (int)TimeSpan.Parse(attrib.Value).TotalMinutes;
                }
                catch
                {
                    config.Timeout = DefaultTimeout;
                }
            }

            if (config.Timeout < 1)
            {
                config.Timeout = DefaultTimeout;
            }

            return config;
        }

        /// <summary>
        /// Timeout in minutes
        /// </summary>
        public int Timeout { get; private set; }

        /// <summary>
        /// Specifies the maximum number of concurrent sessions.
        /// </summary>
        public int MaxSessions { get; private set; }

        /// <summary>
        /// Specifies whether session state persistence for the Padarn server is enabled. 
        /// </summary>
        public bool AllowSessionState { get; private set; }
    }
}
