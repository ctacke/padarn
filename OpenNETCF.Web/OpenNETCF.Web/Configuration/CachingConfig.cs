using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Configuration
{
    /// <summary>
    /// Caching configuration for the server
    /// </summary>
    public sealed class CachingConfig
    {
        private List<CachingProfile> m_profiles = new List<CachingProfile>();

        internal CachingConfig()
        {
        }

        internal void AddProfile(CachingProfile profile)
        {
            m_profiles.Add(profile);
        }

        /// <summary>
        /// The currently set caching profiles for the server
        /// </summary>
        public CachingProfile[] Profiles 
        {
            get { return m_profiles.ToArray(); } 
        }
    }
}
