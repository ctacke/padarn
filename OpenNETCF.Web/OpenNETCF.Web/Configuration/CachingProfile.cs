using System;

using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OpenNETCF.Web.Configuration
{
    public sealed class CachingProfile
    {
        internal CachingProfile(XmlNode descriptor)
        {
            Duration = new TimeSpan(0, 0, 30);
            Location = CacheLocation.Client;

            foreach(XmlAttribute attrib in descriptor.Attributes)
            {
                switch(attrib.Name.ToLower())
                {
                    case "extension":
                        // make all lower here to make comparisons later easier
                        Extension = attrib.Value.ToLower();
                        break;
                    case "duration":
                        Duration = TimeSpan.Parse(attrib.Value);
                        break;
                    case "location":
                        Location = (CacheLocation)Enum.Parse(typeof(CacheLocation), attrib.Value, true);
                        break;
                }
            }

            if (this.Extension == null) throw new Exception("Extension is required");
        }

        /// <summary>
        /// Specifies the time that the page or user control is cached . The default is 00:00:30.
        /// </summary>
        public TimeSpan Duration { get; internal set; }
        /// <summary>
        /// Specifies the file name extension for the files you want to cache.
        /// </summary>
        public string Extension { get; internal set; }
        /// <summary>
        /// Specifies the valid values for controlling the location of the output-cached HTTP response for a resource. 
        /// </summary>
        /// <remarks>This will default to <b>Client</b></remarks>
        public CacheLocation Location { get; internal set; }
    }
}
