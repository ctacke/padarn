using System.Collections.Generic;

namespace OpenNETCF.Web.Configuration
{
    /// <summary>
    /// Descriptions the web server's authentication configuration
    /// </summary>
    public sealed class AuthenticationConfiguration
    {
        private string m_mode;
        private bool m_enabled;
        private string m_realm;
        private UserCollection m_users;

        /// <summary>
        /// Gets the authentication mode. Either Basic, Digest or Forms.
        /// </summary>
        public string Mode
        {
            get { return m_mode; }
            internal set { m_mode = value; }
        }

        /// <summary>
        /// Gets the authentication realm for HTTP Authentication.
        /// </summary>
        public string Realm
        {
            get { return m_realm; }
            internal set { m_realm = value; }
        }

        /// <summary>
        /// Gets a boolean value indicating whether authentication is enabled or not. 
        /// </summary>
        public bool Enabled
        {
            get { return m_enabled; }
            internal set { m_enabled = value; }
        }

        /// <summary>
        /// Describes the users configuration for authentication.
        /// </summary>
        public UserCollection Users
        {
            get { return m_users; }
        }

        public AuthenticationCallbackHandler AuthenticationCallback { get; set; }

        /// <summary>
        /// Creates an instance of the AuthneticationConfiguration class. 
        /// </summary>
        public AuthenticationConfiguration()
        {
            m_users = new UserCollection();
        }
    }
}
