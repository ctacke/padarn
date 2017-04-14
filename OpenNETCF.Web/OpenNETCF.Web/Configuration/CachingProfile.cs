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
